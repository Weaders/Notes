using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NotesMVC.Controllers;
using NotesMVC.Models;
using NotesMVC.ViewModels;
using Xunit;

namespace NotesMVC.tests {
    public class UserControllerTests {

        [Fact]
        public void Signin() {

            // Arrange
            var pwd = "pwd";

            var users = new User[] {
                new User() {
                    Id = "IdUserName",
                    UserName = "NewUserName"
                },
                new User() {
                    Id = "TestUserName",
                    UserName = "TestUserName"
                }
            };

            var db = new Mock<DefaultContext>(new DbContextOptions<DefaultContext>());

            db.Object.Users = MockHelper.GetDbSet(users).Object;

            var set = db.Object.Set<User>();

            var userManager = MockHelper.GetUserManagerWithUser(db.Object);

            var ctrl = this.GetUserController(users[0], userManager.Object);
            var pwdHash = userManager.Object.PasswordHasher.HashPassword(users[0], pwd);

            users[0].PasswordHash = pwdHash;

            var loginModel = new LoginModel() {
                User = users[0].UserName,
                Password = pwd
            };

            //Act
            var result = ctrl.Login(loginModel);

            //Assert
            Assert.IsType<JsonResult>(result);


        }

        /// <summary>
        /// Get user controller
        /// </summary>
        /// <param name="db">Database context</param>
        /// <param name="user">User</param>
        /// <returns></returns>
        private UserController GetUserController(User user, UserManager<User> userManager) {

            var httpContext = MockHelper.GetHttpContextWithUserId(user.Id);

            var signInManager = new Mock<SignInManager<User>>(userManager,
                new HttpContextAccessor { HttpContext = httpContext.Object },
                new Mock<IUserClaimsPrincipalFactory<User>>().Object, null, null, null);

            return new UserController(signInManager.Object, userManager) {
                ControllerContext = new ControllerContext() { HttpContext = httpContext.Object },
                ObjectValidator = MockHelper.GetObjectValidator()
            };


        }

    }
}
