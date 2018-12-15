using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotesMVC.Controllers;
using NotesMVC.Models;
using NotesMVC.Services.Encrypter;
using Xunit;

namespace NotesMVC.tests {

    public class NoteControllerTests {

        [Fact]
        public async void GetNotes() {

            //Arrange
            //var db = new Mock<DefaultContext>().Setup(db => db.);
            //var manager = new CryptographManager();

            //var userManager = GetUserManager();
            //var signInManager = GetSigninManager(userManager.Object);

            //var ctrl = new NotesController(db.Object, manager, userManager.Object, signInManager.Object);

            ////Act
            //var jsonResult = await ctrl.List() as JsonResult;

            ////Assert
            //Assert.Equal(jsonResult.StatusCode, 300);

        }

        protected Mock<UserManager<User>> GetUserManager() {

            var userStore = new Mock<IUserStore<User>>();

            return new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
        }

        protected Mock<SignInManager<User>> GetSigninManager(UserManager<User> userManager) {
            return new Mock<SignInManager<User>>(userManager, null, null, null, null, null);
        }

    }
}
