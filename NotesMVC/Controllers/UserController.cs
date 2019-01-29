using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesMVC.Models;
using NotesMVC.Output;
using NotesMVC.ViewModels;
using System.Threading.Tasks;

namespace NotesMVC.Controllers {
    public class UserController : Controller {

        private readonly UserManager<User> _usersManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(SignInManager<User> signInManager, UserManager<User> usersManager) {

            _signInManager = signInManager;
            _usersManager = usersManager;

        }

        public IActionResult LoginForm() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginModel model) {

            if (ModelState.IsValid) {

                var user = await _usersManager.FindByNameAsync(model.User);

                if (user != null) {

                    if (await _usersManager.CheckPasswordAsync(user, model.Password)) {

                        await _signInManager.SignInAsync(user, true);

                        return Json(new UserForOutput(user));

                    } else {
                        ModelState.AddModelError("bad_login", "Bad login or password.");
                    }

                } else {
                    ModelState.AddModelError("bad_login", "Bad login or password.");
                }

            }

            return new JsonFailResult(ModelState);

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterModel model) {

            if (ModelState.IsValid) {

                var user = await _usersManager.FindByNameAsync(model.User);

                if (user == null) {

                    var newUser = new User {
                        UserName = model.User
                    };

                    newUser.PasswordHash = _usersManager.PasswordHasher.HashPassword(newUser, model.Password);

                    var result = await _usersManager.CreateAsync(newUser);

                    if (result.Succeeded) {
                        return Json(new UserForOutput(newUser));
                    } else {

                        foreach (var error in result.Errors) {
                            ModelState.AddModelError(error.Code, error.Description);
                        }

                    }

                } else {
                    ModelState.AddModelError("exists_user", "Пользователь уже существует");
                }

            }

            return new JsonFailResult(ModelState);

        }

        [HttpGet]
        [Authorize]
        [Route("[controller]/current")]
        public async Task<IActionResult> GetCurrentUser() {

            if (_signInManager.IsSignedIn(User)) {
                return Json(new UserForOutput(await _usersManager.GetUserAsync(User)));
            } else {
                return Json(null);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout() {

            await HttpContext.SignOutAsync();
            await _signInManager.SignOutAsync();

            return Json("Ok!");

        }

    }
}