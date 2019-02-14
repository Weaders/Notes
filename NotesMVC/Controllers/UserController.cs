using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesMVC.Models;
using NotesMVC.Output;
using NotesMVC.Services;
using NotesMVC.ViewModels;
using System.Threading.Tasks;

namespace NotesMVC.Controllers {

    public class UserController : Controller {
        
        private readonly UserManager<User> _usersManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IOutputFactory _outputFactory;
        private readonly IUserService _usersService;

        public UserController(SignInManager<User> signInManager, UserManager<User> usersManager, IOutputFactory outputFactory, IUserService usersService) {

            _signInManager = signInManager;
            _usersManager = usersManager;
            _outputFactory = outputFactory;
            _usersService = usersService;

        }

        /// <summary>
        /// Home page for application
        /// </summary>
        /// <returns></returns>
        public IActionResult LoginForm() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginModel model) {

            if (ModelState.IsValid) {

                var result = await _usersService.LoginValidate(model);

                if (result.IsSuccess) {

                    await _usersService.Login(result.UserToLogin);
                    return Json(_outputFactory.CreateUser(result.UserToLogin));

                } else {
                    result.ErrorsToModelState(ModelState);
                }

            }

            return _outputFactory.CreateJsonFail(ModelState);

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterModel model) {

            if (ModelState.IsValid) {

                var result = await _usersService.RegisterValidate(model);

                if (result.IsSuccess) {

                    var userCreated = await _usersService.Register(model);
                    return Json(_outputFactory.CreateUser(userCreated));

                } else {
                    result.ErrorsToModelState(ModelState);
                }

            }

            return _outputFactory.CreateJsonFail(ModelState);

        }

        [HttpGet]
        [Authorize]
        [Route("[controller]/current")]
        public async Task<IActionResult> GetCurrentUser() {
            return Json(_outputFactory.CreateUser(await _usersManager.GetUserAsync(User)));
        }

        [HttpPost]
        public async Task<IActionResult> Logout() {

            await HttpContext.SignOutAsync();
            await _signInManager.SignOutAsync();

            return Json("Ok!");

        }

    }

}