﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotesMVC.Data;

namespace NotesMVC.DomainServices {

    public class DefaultContext : IdentityDbContext<User> {

        public virtual DbSet<Note> Notes { get; set; }

        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {

            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UsersLoginHistory");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UsersClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");
            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");
            builder.Entity<IdentityUserToken<string>>().ToTable("UsersTokens");

            builder.Entity<IdentityRole>().HasData(new[] {

                new IdentityRole() {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole() {
                    Name = "Member",
                    NormalizedName = "MEMBER"
                }

            });

        }

    }

}
