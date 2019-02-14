using Microsoft.AspNetCore.Identity;
using NotesMVC.Models;
using NotesMVC.ViewModels;
using System.Threading.Tasks;

namespace NotesMVC.Services {
    public class UserService : IUserService {

        public class LoginResultValidation : ValidationResult, ILoginResultValidation {
            public User UserToLogin { get; set; }
        }

        public class RegisterResultValidation : ValidationResult, IRegisterResultValidation {
            public User UserToRegister { get; set; }
        }

        private readonly UserManager<User> _userMng;
        private readonly SignInManager<User> _signInMng;

        public UserService(UserManager<User> userMng, SignInManager<User> signInMng) {

            _userMng = userMng;
            _signInMng = signInMng;

        }

        /// <summary>
        /// Check is can user login.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ILoginResultValidation> LoginValidate(LoginModel model) {

            var result = new LoginResultValidation {
                UserToLogin = await _userMng.FindByNameAsync(model.User)
            };

            if (result.UserToLogin != null) {

                if (!await _userMng.CheckPasswordAsync(result.UserToLogin, model.Password)) {

                    result.IsSuccess = false;
                    result.Errors.Add("No user pwd and login", "No user pwd and login");

                }

            } else {

                result.IsSuccess = false;
                result.Errors.Add("No user pwd and login", "No user pwd and login");

            }

            return result;

        }

        /// <summary>
        /// Login to user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task Login(User user) {
            return _signInMng.SignInAsync(user, true);
        }

        /// <summary>
        /// Register validate
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        public async Task<IRegisterResultValidation> RegisterValidate(RegisterModel registerModel) {

            var result = new RegisterResultValidation() {
                UserToRegister = await _userMng.FindByNameAsync(registerModel.User)
            };

            if (result.UserToRegister != null) {

                result.IsSuccess = false;
                result.Errors.Add("User already exists", "User already exists");

            }

            return result;

        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="registerModel"></param>
        /// <param name="userToReg"></param>
        /// <returns></returns>
        public async Task<User> Register(RegisterModel registerModel) {

            var user = new User() {
                UserName = registerModel.User
            };

            var userCreate = await _userMng.CreateAsync(user, registerModel.Password);

            if (userCreate.Succeeded) {
                return user;
            }

            return null;

        }

    }
}
