using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesMVC.JsonResults;
using NotesMVC.Models;
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

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginModel model) {

            if (ModelState.IsValid) {

                var user = await _usersManager.FindByNameAsync(model.User);                

                if (user != null) {

                    if (await _usersManager.CheckPasswordAsync(user, model.Password)) {

                        await _signInManager.SignInAsync(user, true);
                        return Json("OK");

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

                    await _usersManager.CreateAsync(newUser);

                    return Json("Ok");

                } else {
                    ModelState.AddModelError("exists_user", "Пользователь уже существуетs");
                }

            }

            return new JsonFailResult(ModelState);

        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser() {

            if (User != null) {
                return Json(await _usersManager.GetUserAsync(User));
            } else {
                return Json(null);
            }

        }

    }
}