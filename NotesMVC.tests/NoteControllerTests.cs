using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NotesMVC.Controllers;
using NotesMVC.Data;
using NotesMVC.Output;
using NotesMVC.Services;
using NotesMVC.Services.Encrypter;
using NotesMVC.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;
using System.Linq;
using NotesMVC.DomainServices;
using NotesMVC.ViewModels.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace NotesMVC.tests {

    public class NoteControllerTests {

        /// <summary>
        /// Get notes
        /// </summary>
        [Fact]
        public async void GetNotes() {

            // Arrange
            var opts = new DbContextOptions<DefaultContext>();
            var db = new Mock<DefaultContext>(opts);

            var user = new User() {
                Id = "user-id-test",
                UserName = "UserNameTest"
            };

            var notesData = new Note[] {
                new Note() {
                    Id = 1,
                    Text = new byte[] { 0x0019, 0x0032 },
                    Title = new byte[] { 0x0056, 0x00C1 },
                    User = user,
                    CryptoName = CryptographType.AES.Type
                }
            };

            db.Setup(d => d.Notes).Returns(MockHelper.GetDbSet(notesData).Object);

            var ctrl = this.GetNoteController(db, user).Object;

            ////Act
            var jsonResult = await ctrl.List("test") as JsonResult;

            ////Assert
            Assert.Null(jsonResult.StatusCode);
            Assert.Equal((jsonResult.Value as IEnumerable<NoteForOutput>).Count(), notesData.Length);

        }

        /// <summary>
        /// Try add note with good data.
        /// </summary>
        [Fact]
        public async void AddGoodNote() {

            // Arrange
            var opts = new DbContextOptions<DefaultContext>();
            var db = new Mock<DefaultContext>(opts);

            var user = new User() {
                Id = "user-id-test",
                UserName = "UserNameTest"
            };

            var notesData = new Note[] {
                new Note() {
                    Id = 1,
                    Text = new byte[] { 0x0019, 0x0032 },
                    Title = new byte[] { 0x0056, 0x00C1 },
                    User = user
                }
            };

            db.Setup(d => d.Notes).Returns(MockHelper.GetDbSet(notesData).Object);

            var ctrl = this.GetNoteController(db, user).Object;

            var noteAdd = new NoteAdd() {
                SecretKey = "test",
                Text = "TextTest",
                Title = "TitleTest"
            };

            //Act
            var result = await ctrl.AddNote(noteAdd) as JsonResult;

            //Assert
            Assert.Null(result.StatusCode);
            Assert.IsType<NoteForOutput>(result.Value);
            Assert.Equal((result.Value as NoteForOutput).Text, noteAdd.Text);
            Assert.Equal((result.Value as NoteForOutput).Title, noteAdd.Title);

        }

        /// <summary>
        /// Try add note, with bad data. Empty text and title
        /// </summary>
        [Fact]
        public async void AddBadNote() {

            // Arrange
            var opts = new DbContextOptions<DefaultContext>();
            var db = new Mock<DefaultContext>(opts);

            var user = new User() {
                Id = "user-id-test",
                UserName = "UserNameTest"
            };

            var notesData = new Note[] {
                new Note() {
                    Id = 1,
                    Text = new byte[] { 0x0019, 0x0032 },
                    Title = new byte[] { 0x0056, 0x00C1 },
                    User = user
                }
            };

            db.Setup(d => d.Notes).Returns(MockHelper.GetDbSet(notesData).Object);

            var ctrl = this.GetNoteController(db, user);

            var noteAdd = new NoteAdd() {
                SecretKey = "test",
                Text = "",
                Title = ""
            };

            ctrl.Setup(ct => ct.TryValidateModel(It.IsIn(noteAdd))).Callback(() => {
                ctrl.Object.ModelState.AddModelError("Text field empty", "Text field empty");
                ctrl.Object.ModelState.AddModelError("Title field empty", "Title field empty");
            });

            //Act
            ctrl.Object.TryValidateModel(noteAdd);
            var result = await ctrl.Object.AddNote(noteAdd);

            //Assert
            Assert.IsType<JsonFailResult>(result);

            var failResult = result as JsonFailResult;

            Assert.Equal(400, failResult.StatusCode);
            Assert.IsType<ErrorsHandler>(failResult.Value);
            Assert.Equal(2, (failResult.Value as ErrorsHandler).Errors.Count);

        }

        /// <summary>
        /// Edit note, with good data
        /// </summary>
        [Fact]
        public async void EditGoodNote() {

            // Arrange
            var opts = new DbContextOptions<DefaultContext>();
            var db = new Mock<DefaultContext>(opts);

            var user = new User() {
                Id = "user-id-test",
                UserName = "UserNameTest"
            };

            var notesData = new Note[] {
                new Note() {
                    Id = 1,
                    Text = new byte[] { 0x0019, 0x0032 },
                    Title = new byte[] { 0x0056, 0x00C1 },
                    User = user,
                    CryptoName = CryptographType.AES.Type
                },
                new Note() {
                    Id = 2,
                    Text = new byte[] { 0x01, 0x12 },
                    Title = new byte[] { 0x02, 0x03 },
                    User = null,
                    CryptoName = CryptographType.AES.Type
                }
            };

            db.Setup(d => d.Notes).Returns(MockHelper.GetDbSet(notesData).Object);

            var ctrl = this.GetNoteController(db, user);
            var ctrlObj = ctrl.Object;

            var noteEdit = new NoteEdit() {
                SecretKey = "test",
                Text = "NewText",
                Title = "NewTitle",
                Id = 1
            };

            //Act
            var result = await ctrlObj.EditNote(noteEdit);

            //Assert
            Assert.IsType<JsonResult>(result);

            var goodResult = result as JsonResult;

            Assert.Null(goodResult.StatusCode);
            Assert.IsType<NoteForOutput>(goodResult.Value);

            var noteForOut = goodResult.Value as NoteForOutput;

            Assert.Equal(noteForOut.Id, noteEdit.Id);
            Assert.Equal(noteForOut.Text, noteEdit.Text);
            Assert.Equal(noteForOut.Title, noteEdit.Title);

        }

        /// <summary>
        /// Try edit note, that not user not owner.
        /// </summary>
        [Fact]
        public async void EditBadOwnerNote() {

            // Arrange
            var opts = new DbContextOptions<DefaultContext>();
            var db = new Mock<DefaultContext>(opts);

            var user = new User() {
                Id = "user-id-test",
                UserName = "UserNameTest"
            };

            var notesData = new Note[] {
                new Note() {
                    Id = 1,
                    Text = new byte[] { 0x0019, 0x0032 },
                    Title = new byte[] { 0x0056, 0x00C1 },
                    User = user
                },
                new Note() {
                    Id = 2,
                    Text = new byte[] { 0x01, 0x12 },
                    Title = new byte[] { 0x02, 0x03 },
                    User = null
                }
            };

            db.Setup(d => d.Notes).Returns(MockHelper.GetDbSet(notesData).Object);

            var ctrl = this.GetNoteController(db, user);
            var ctrlObj = ctrl.Object;

            var noteEdit = new NoteEdit() {
                SecretKey = "test",
                Text = "NewText",
                Title = "NewTitle",
                Id = 2
            };

            ctrl.Setup(ct => ct.TryValidateModel(It.IsAny<object>())).Callback(() => {
                ctrlObj.ModelState.AddModelError("Now onwer", "Not owner");
            });

            //Act
            ctrlObj.TryValidateModel(noteEdit);
            var result = await ctrlObj.EditNote(noteEdit);

            //Assert
            Assert.IsType<JsonFailResult>(result);

            var badResult = result as JsonFailResult;

            Assert.Equal(400, badResult.StatusCode);
            Assert.IsType<ErrorsHandler>(badResult.Value);

            var errors = badResult.Value as ErrorsHandler;

            Assert.Single(errors.Errors);

        }

        /// <summary>
        /// Edit bad note data
        /// </summary>
        [Fact]
        public async void EditBadNoteData() {

            // Arrange
            var opts = new DbContextOptions<DefaultContext>();
            var db = new Mock<DefaultContext>(opts);

            var user = new User() {
                Id = "user-id-test",
                UserName = "UserNameTest"
            };

            var notesData = new Note[] {
                new Note() {
                    Id = 1,
                    Text = new byte[] { 0x0019, 0x0032 },
                    Title = new byte[] { 0x0056, 0x00C1 },
                    User = user
                },
                new Note() {
                    Id = 2,
                    Text = new byte[] { 0x01, 0x12 },
                    Title = new byte[] { 0x02, 0x03 },
                    User = null
                }
            };

            db.Setup(d => d.Notes).Returns(MockHelper.GetDbSet(notesData).Object);

            var ctrl = this.GetNoteController(db, user);

            var noteEdit = new NoteEdit() {
                SecretKey = "",
                Text = "",
                Title = "",
                Id = 1
            };

            ctrl.Setup(ct => ct.TryValidateModel(It.IsAny<object>())).Callback(() => {

                ctrl.Object.ModelState.AddModelError("Secret key", "Secret key empty");
                ctrl.Object.ModelState.AddModelError("Text", "Text empty");
                ctrl.Object.ModelState.AddModelError("Title", "Title empty");

            });

            var ctrlObj = ctrl.Object;

            //Act
            ctrlObj.TryValidateModel(noteEdit);
            var result = await ctrlObj.EditNote(noteEdit);

            //Assert
            Assert.IsType<JsonFailResult>(result);

            var badResult = result as JsonFailResult;

            Assert.Equal(400, badResult.StatusCode);
            Assert.IsType<ErrorsHandler>(badResult.Value);

            var errors = badResult.Value as ErrorsHandler;

            Assert.Equal(3, errors.Errors.Count);

        }

        /// <summary>
        /// Remove note
        /// </summary>
        [Fact]
        public async void RemoveNote() {

            // Arrange
            var opts = new DbContextOptions<DefaultContext>();
            var db = new Mock<DefaultContext>(opts);

            var user = new User() {
                Id = "user-id-test",
                UserName = "UserNameTest"
            };

            var notesData = new Note[] {
                new Note() {
                    Id = 1,
                    Text = new byte[] { 0x0019, 0x0032 },
                    Title = new byte[] { 0x0056, 0x00C1 },
                    User = user
                },
                new Note() {
                    Id = 2,
                    Text = new byte[] { 0x01, 0x12 },
                    Title = new byte[] { 0x02, 0x03 },
                    User = null
                }
            };

            db.Setup(d => d.Notes).Returns(MockHelper.GetDbSet(notesData).Object);

            var ctrl = this.GetNoteController(db, user).Object;

            //Act
            var result = await ctrl.DeleteNote(1);

            //Assert
            Assert.IsType<JsonResult>(result);

            var goodResult = result as JsonResult;

            Assert.Null(goodResult.StatusCode);
            Assert.IsType<string>(goodResult.Value);

            Assert.Equal("Ok", goodResult.Value);


        }

        /// <summary>
        /// Remove note, that not exists
        /// </summary>
        [Fact]
        public async void RemoveNoteNotExists() {

            // Arrange
            var opts = new DbContextOptions<DefaultContext>();
            var db = new Mock<DefaultContext>(opts);

            var user = new User() {
                Id = "user-id-test",
                UserName = "UserNameTest"
            };

            var notesData = new Note[] {
                new Note() {
                    Id = 1,
                    Text = new byte[] { 0x0019, 0x0032 },
                    Title = new byte[] { 0x0056, 0x00C1 },
                    User = user
                },
                new Note() {
                    Id = 2,
                    Text = new byte[] { 0x01, 0x12 },
                    Title = new byte[] { 0x02, 0x03 },
                    User = null
                }
            };

            db.Setup(d => d.Notes).Returns(MockHelper.GetDbSet(notesData).Object);

            var ctrl = this.GetNoteController(db, user).Object;

            //Act
            var result = await ctrl.DeleteNote(3);

            //Assert
            Assert.IsType<JsonFailResult>(result);

            var badResult = result as JsonFailResult;

            Assert.Equal(400, badResult.StatusCode);
            Assert.IsType<ErrorsHandler>(badResult.Value);

            var errors = badResult.Value as ErrorsHandler;

            Assert.Single(errors.Errors);

        }

        /// <summary>
        /// Remove note, that user not owner
        /// </summary>
        [Fact]
        public async void RemoveNoteNotOwner() {

            // Arrange
            var opts = new DbContextOptions<DefaultContext>();
            var db = new Mock<DefaultContext>(opts);

            var user = new User() {
                Id = "user-id-test",
                UserName = "UserNameTest"
            };

            var notesData = new Note[] {
                new Note() {
                    Id = 1,
                    Text = new byte[] { 0x0019, 0x0032 },
                    Title = new byte[] { 0x0056, 0x00C1 },
                    User = user
                },
                new Note() {
                    Id = 2,
                    Text = new byte[] { 0x01, 0x12 },
                    Title = new byte[] { 0x02, 0x03 },
                    User = null
                }
            };

            db.Setup(d => d.Notes).Returns(MockHelper.GetDbSet(notesData).Object);

            var ctrl = this.GetNoteController(db, user).Object;

            //Act
            var result = await ctrl.DeleteNote(2);

            //Assert
            Assert.IsType<JsonFailResult>(result);

            var badResult = result as JsonFailResult;

            Assert.Equal(400, badResult.StatusCode);
            Assert.IsType<ErrorsHandler>(badResult.Value);

            var errors = badResult.Value as ErrorsHandler;

            Assert.Single(errors.Errors);

        }

        private Mock<NotesController> GetNoteController(Mock<DefaultContext> db, User user) {

            var httpContext = MockHelper.GetHttpContextWithUserId(user.Id);
            var userManager = MockHelper.GetUserManagerWithUser(db.Object);

            userManager.Setup(um => um.GetUserAsync(It.Is<ClaimsPrincipal>(t => true))).ReturnsAsync(user);

            var signInManager = new Mock<SignInManager<User>>(userManager.Object,
                new HttpContextAccessor { HttpContext = httpContext.Object },
                new Mock<IUserClaimsPrincipalFactory<User>>().Object, null, null, null
            );

            Func<object, JsonResult> toJson = (data) => {
                return new JsonResult(data);
            };

            var manager = new CryptographManager();
            var modelsFactory = new ModelsFactory();
            var outputFactory = new OutputFactory(manager);
            var notesManager = new NotesManager(db.Object, manager, modelsFactory, userManager.Object);
            var notesValidator = new NotesViewModelValidator(db.Object, manager, modelsFactory);

            var ctrlNotes = new Mock<NotesController>(userManager.Object, outputFactory, notesManager, notesValidator);

            ctrlNotes.Setup(ctrl => ctrl.Json(It.IsAny<object>())).Returns(toJson);

            ctrlNotes.Object.ControllerContext = new ControllerContext() { HttpContext = httpContext.Object };

            return ctrlNotes;
            
        }
    }
}