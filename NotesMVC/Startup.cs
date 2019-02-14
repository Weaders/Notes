using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotesMVC.Middleware;
using NotesMVC.Models;
using NotesMVC.Output;
using NotesMVC.Services;
using NotesMVC.Services.Encrypter;
using React.AspNet;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NotesMVC {

    public class Startup {

        public Startup(IConfiguration configuration, ILogger<Startup> _logger) {
            Configuration = configuration;
            logger = _logger;
        }

        public ILogger<Startup> logger;

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services) {

            services.Configure<CookiePolicyOptions>(options => {

                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });

            var connectionStr = this.Configuration.GetConnectionString("DefaultConnection");

            services.AddMvc();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IOutputFactory, OutputFactory>();
            services.AddSingleton<IModelsFactory, ModelsFactory>();
            services.AddSingleton<CryptographManager>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<INotesManager, NotesManager>();

            services.AddReact();

            services.AddIdentity<User, IdentityRole>(o => {
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 1;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<DefaultContext>();

            services.AddDbContext<DefaultContext>(options => options.UseSqlServer(connectionStr));

            services.ConfigureApplicationCookie(opts => {

                opts.Events.OnRedirectToLogin = ((ctx) => { // Don't redirect on fail authorize

                    ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;

                });

                opts.Events.OnRedirectToAccessDenied = ((ctx) => { // Don't redirect on access denied

                    ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Task.CompletedTask;

                });

            });

            return services.BuildServiceProvider();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {

            if (env.IsDevelopment()) {

                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware();

            } else {

                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();

            }

            app.UseReact(config => { });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMiddleware<ReqTimer>();

            CreateDefaultUser(app.ApplicationServices);

            app.UseMvc(routes => {

                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}/{action=LoginForm}"
                );

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller}/{action}"
                );

            });

        }

        private async void CreateDefaultUser(IServiceProvider services) {

            logger.LogInformation("Start create default user");

            var userMng = services.GetService<UserManager<User>>();
            var userCnfgSection = Configuration.GetSection("UserSettings");

            var userData = new User {
                UserName = userCnfgSection["Login"],
                Email = userCnfgSection["Email"]
            };

            if ((await userMng.FindByEmailAsync(userData.Email)) == null) {

                var userCreate = await userMng.CreateAsync(userData, userCnfgSection["Pwd"]);

                if (userCreate.Succeeded) {
                    await userMng.AddToRoleAsync(userData, "Admin");
                } else {
                    logger.LogWarning("Can not create default user");
                }

            } else {
                logger.LogInformation("Default user already exists");
            }

        }

    }
}
