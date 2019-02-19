using Microsoft.AspNetCore.Identity;
using NotesMVC.Data;
using System.Threading.Tasks;

namespace NotesMVC.ViewModels.Validation {
    public class UserViewModelValidator {

        private readonly UserManager<User> _userMng;
        private readonly IModelsFactory _modelsFactory;

        public UserViewModelValidator(UserManager<User> userMng, IModelsFactory modelsFac) {
            _userMng = userMng;
            _modelsFactory = modelsFac;
        }

        public interface ILoginResultValidation : IValidationResult {
            User UserToLogin { get; set; }
        }

        public interface IRegisterResultValidation : IValidationResult {
            User UserToRegister { get; set; }
        }

        public class LoginResultValidation : ValidationResult, ILoginResultValidation {
            public User UserToLogin { get; set; }
        }

        public class RegisterResultValidation : ValidationResult, IRegisterResultValidation {
            public User UserToRegister { get; set; }
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

            } else {

                result.UserToRegister = _modelsFactory.CreateUser();
                result.UserToRegister.UserName = registerModel.User;

            }

            return result;

        }

    }
}
