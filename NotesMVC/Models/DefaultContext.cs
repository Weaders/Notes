using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NotesMVC.Models {

    public class DefaultContext : IdentityDbContext<User> {

        public virtual DbSet<Note> Notes { get; set; }

        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder) {

            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UsersLoginHistory");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UsersClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");
            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");
            builder.Entity<IdentityUserToken<string>>().ToTable("UsersTokens");

        }

    }

}
