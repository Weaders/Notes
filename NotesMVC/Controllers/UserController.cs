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

        private readonly UserManager<User> usersManager;
        private readonly SignInManager<User> signInManager;
        private readonly IOutputFactory outputFactory;
        private readonly IModelsFactory modelsFactory;

        public UserController(SignInManager<User> _signInManager, UserManager<User> _usersManager, IOutputFactory _outputFactory, IModelsFactory _modelsFactory) {

            signInManager = _signInManager;
            usersManager = _usersManager;
            outputFactory = _outputFactory;
            modelsFactory = _modelsFactory;

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

                        return Json(outputFactory.CreateUser(user));

                    } else {
                        ModelState.AddModelError("No user pwd and login", "No user pwd and login");
                    }

                } else {
                    ModelState.AddModelError("No user pwd and login", "No user pwd and login");
                }

            }

            return outputFactory.CreateJsonFail(ModelState);

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterModel model) {

            if (ModelState.IsValid) {

                var user = await usersManager.FindByNameAsync(model.User);

                if (user == null) {

                    var newUser = modelsFactory.CreateUser();
                    newUser.UserName = model.User;

                    var result = await usersManager.CreateAsync(newUser, model.Password);

                    if (result.Succeeded) {

                        await signInManager.SignInAsync(newUser, true);
                        return Json(outputFactory.CreateUser(newUser));

                    } else {

                        foreach (var error in result.Errors) {
                            ModelState.AddModelError(error.Code, error.Description);
                        }

                    }

                } else {
                    ModelState.AddModelError("User already exists", "User already exists");
                }

            }

            return outputFactory.CreateJsonFail(ModelState);

        }

        [HttpGet]
        [Authorize]
        [Route("[controller]/current")]
        public async Task<IActionResult> GetCurrentUser() {

            if (signInManager.IsSignedIn(User)) {
                return Json(outputFactory.CreateUser(await usersManager.GetUserAsync(User)));
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