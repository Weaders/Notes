using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NotesMVC.Models;
using NotesMVC.Output;
using NotesMVC.ViewModels;
using System.Threading.Tasks;

namespace NotesMVC.Controllers {
    public class UserController : Controller {

        private readonly UserManager<User> usersManager;
        private readonly SignInManager<User> signInManager;

        public UserController(SignInManager<User> _signInManager, UserManager<User> _usersManager) {

            signInManager = _signInManager;
            usersManager = _usersManager;

        }

        public IActionResult LoginForm() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginModel model) {

            if (ModelState.IsValid) {

                var user = await usersManager.FindByNameAsync(model.User);

                if (user != null) {

                    if (await usersManager.CheckPasswordAsync(user, model.Password)) {

                        await signInManager.SignInAsync(user, true);

                        return Json(new UserForOutput(user));

                    } else {
                        ModelState.AddModelError("No user pwd and login", "No user pwd and login");
                    }

                } else {
                    ModelState.AddModelError("No user pwd and login", "No user pwd and login");
                }

            }

            return new JsonFailResult(ModelState);

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterModel model) {

            if (ModelState.IsValid) {

                var user = await usersManager.FindByNameAsync(model.User);

                if (user == null) {

                    var newUser = new User {
                        UserName = model.User
                    };

                    newUser.PasswordHash = usersManager.PasswordHasher.HashPassword(newUser, model.Password);

                    var result = await usersManager.CreateAsync(newUser);

                    if (result.Succeeded) {

                        await signInManager.SignInAsync(newUser, true);
                        return Json(new UserForOutput(newUser));

                    } else {

                        foreach (var error in result.Errors) {
                            ModelState.AddModelError(error.Code, error.Description);
                        }

                    }

                } else {
                    ModelState.AddModelError("exists_user", "User already exists");
                }

            }

            return new JsonFailResult(ModelState);

        }

        [HttpGet]
        [Authorize]
        [Route("[controller]/current")]
        public async Task<IActionResult> GetCurrentUser() {

            if (signInManager.IsSignedIn(User)) {
                return Json(new UserForOutput(await usersManager.GetUserAsync(User)));
            } else {
                return Json(null);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout() {

            await HttpContext.SignOutAsync();
            await signInManager.SignOutAsync();

            return Json("Ok!");

        }


    }
}