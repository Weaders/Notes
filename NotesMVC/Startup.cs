using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotesMVC.Models;
using NotesMVC.Services.Encrypter;
using React.AspNet;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NotesMVC {

    public class Startup {

        public Startup(IConfiguration configuration, ILogger<Startup> logger) {
            Configuration = configuration;
            _logger = logger;
        }

        public ILogger<Startup> _logger;

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services) {

            services.Configure<CookiePolicyOptions>(options => {

                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });

            var connectionStr = this.Configuration.GetConnectionString("DefaultConnection");

            services.AddMvc();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddReact();

            services.AddSingleton<CryptographManager>();

            services.AddIdentity<User, IdentityRole>(o => {
                o.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<DefaultContext>();

            services.AddDbContext<DefaultContext>(options => options.UseSqlServer(connectionStr));

            services.ConfigureApplicationCookie(opts => {

                opts.Events.OnRedirectToLogin = ((ctx) => { // Don't redirect on fail authorize

                    ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
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

            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller=User}/{action=LoginForm}");
            });
        }
    }
}
