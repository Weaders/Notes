using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NotesMVC.Data;
using NotesMVC.tests.DbSetClasses;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace NotesMVC.tests {
    public static class MockHelper {

        public static Mock<HttpContext> GetHttpContextWithUserId(string userId) {

            var httpContext = new Mock<HttpContext>();

            var claimsPrincipal = new ClaimsPrincipal();
            var claimIdentity = new ClaimsIdentity();

            claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));

            claimsPrincipal.AddIdentity(claimIdentity);

            httpContext.Setup(h => h.User).Returns(claimsPrincipal);

            return httpContext;

        }

        public static Mock<DbSet<T>> GetDbSet<T>(T[] vals) where T : class {

            var valsQueryble = vals.AsQueryable();

            var context = new Mock<DbSet<T>>();

            context.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(valsQueryble.GetEnumerator()));

            context.As<IQueryable<T>>()
                 .Setup(m => m.Provider)
                 .Returns(new TestDbAsyncQueryProvider<T>(valsQueryble.Provider));

            context.As<IQueryable<T>>().Setup(m => m.Expression).Returns(valsQueryble.Expression);
            context.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(valsQueryble.ElementType);
            context.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(valsQueryble.GetEnumerator());

            return context;

        }

        public static Mock<UserManager<User>> GetUserManagerWithUser(DbContext dbContext) {

            var userStore = new UserStore<User>(dbContext);

            var pwdHasher = new PasswordHasher<User>();

            var userManager = new Mock<UserManager<User>>(userStore, null, pwdHasher, null, null, null, null, null, null);

            return userManager;

        }

    }
}
