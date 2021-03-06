﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NotesMVC.Controllers;
using NotesMVC.Data;
using NotesMVC.DomainServices;
using NotesMVC.Output;
using NotesMVC.Services;
using NotesMVC.Services.Encrypter;
using NotesMVC.ViewModels;
using NotesMVC.ViewModels.Validation;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NotesMVC.tests {
    public class UserControllerTests {

        [Fact]
        public async void SigninTest() {

            // Arrange
            var pwd = "pwd";
            var userToLogin = new User() {
                Id = "IdUserName",
                UserName = "NewUserName",
            };

            var db = new Mock<DefaultContext>(new DbContextOptions<DefaultContext>());

            var userManager = MockHelper.GetUserManagerWithUser(db.Object);
            var pwdHash = userManager.Object.PasswordHasher.HashPassword(userToLogin, pwd);

            userToLogin.PasswordHash = pwdHash;

            var users = new[] {
                userToLogin,
                new User() {
                    Id = "TestUserName",
                    UserName = "TestUserName"
                }
            };

            db.Object.Users = MockHelper.GetDbSet(users).Object;

            userManager.Setup(u => u.FindByNameAsync(It.Is<string>(str => str == users[0].UserName))).Returns(Task.FromResult(users[0]));
            userManager.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.Is<string>(str => str == pwd))).Returns(Task.FromResult(true));

            var ctrl = this.GetUserController(users[0], userManager.Object);
            var ctrlObj = ctrl.Object;
            
            users[0].PasswordHash = pwdHash;

            var loginModelSuccess = new LoginModel() {
                User = users[0].UserName,
                Password = pwd
            };

            var loginModelWrongPwd = new LoginModel() {
                User = users[0].UserName,
                Password = pwd + "a"
            };

            var loginModelWrongName = new LoginModel() {
                User = users[0].UserName + "a",
                Password = pwd
            };

            //Act
            //ctrl.TryValidateModel(loginModelSuccess);
            var resultSuccess = await ctrlObj.Login(loginModelSuccess);
            var resultFailPwd = await ctrlObj.Login(loginModelWrongPwd);
            var resultFailUserName = await ctrlObj.Login(loginModelWrongName);

            //Assert
            Assert.IsType<JsonResult>(resultSuccess);
            Assert.IsType<JsonFailResult>(resultFailPwd);
            Assert.IsType<JsonFailResult>(resultFailUserName);

            var pwdErrors = ((JsonFailResult)resultFailPwd).Value as ErrorsHandler;
            var pwdUserName = ((JsonFailResult)resultFailUserName).Value as ErrorsHandler;

            Assert.True(pwdErrors.Errors.ContainsKey("No user pwd and login"));
            Assert.True(pwdUserName.Errors.ContainsKey("No user pwd and login"));

        }

        [Fact]
        public async void SingupTest() {

            // Arrange
            var pwd = "pwd";
            var users = new[] {
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

            var userManager = MockHelper.GetUserManagerWithUser(db.Object);
            db.Object.Users = MockHelper.GetDbSet(users).Object;

            userManager.Setup(u => u.FindByNameAsync(It.Is<string>(str => str == users[0].UserName))).Returns(Task.FromResult(users[0]));
            userManager.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.Is<string>(str => str == pwd))).Returns(Task.FromResult(true));
            userManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

            var ctrl = this.GetUserController(null, userManager.Object);
            var ctrlObj = ctrl.Object;

            var userToCreateSuccess = new RegisterModel() {
                User = "test",
                Password = pwd
            };

            var userToCreateFailExists = new RegisterModel() {
                User = users[0].UserName,
                Password = pwd
            };

            var userToCreateEmpty = new RegisterModel() {
                User = "",
                Password = ""
            };

            //Act
            var resultSuccess = await ctrlObj.Register(userToCreateSuccess);
            var resultFailExists = await ctrlObj.Register(userToCreateFailExists);

            ctrlObj.TryValidateModel(userToCreateEmpty);
            var resultFailEmpty = await ctrlObj.Register(userToCreateEmpty);

            //Assert
            Assert.IsType<JsonResult>(resultSuccess);
            Assert.IsType<JsonFailResult>(resultFailExists);
            Assert.IsType<JsonFailResult>(resultFailEmpty);

            var existsUserError = ((JsonFailResult)resultFailExists).Value as ErrorsHandler;

            Assert.True(existsUserError.Errors.ContainsKey("User already exists"));

        }

        /// <summary>
        /// Get user controller
        /// </summary>
        /// <param name="db">Database context</param>
        /// <param name="user">User</param>
        /// <returns></returns>
        private Mock<UserController> GetUserController(User user, UserManager<User> userManager) {

            var httpContext = MockHelper.GetHttpContextWithUserId(user != null ? user.Id : "");

            var signInManager = new Mock<SignInManager<User>>(userManager,
                new HttpContextAccessor { HttpContext = httpContext.Object },
                new Mock<IUserClaimsPrincipalFactory<User>>().Object, null, null, null);

            Func<object, JsonResult> toJson = (data) => {
                return new JsonResult(data);
            };

            var manager = new CryptographManager();
            var outputFactory = new OutputFactory(manager);
            var modelsFactory = new ModelsFactory();
            var userService = new UserService(userManager);
            var usersValidator = new UserViewModelValidator(userManager, new ModelsFactory());

            var userCtrl = new Mock<UserController>(signInManager.Object, userManager, outputFactory, userService, usersValidator);

            userCtrl.Object.ControllerContext = new ControllerContext() { HttpContext = httpContext.Object };
            userCtrl.Setup(uCtrl => uCtrl.Json(It.IsAny<object>())).Returns(toJson);

            return userCtrl;

        }

    }
}
