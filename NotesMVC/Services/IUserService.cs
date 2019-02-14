using Microsoft.AspNetCore.Identity;
using NotesMVC.Models;
using NotesMVC.ViewModels;
using System.Threading.Tasks;

namespace NotesMVC.Services {

    public interface ILoginResultValidation: IValidationResult {
        User UserToLogin { get; set; }
    }

    public interface IRegisterResultValidation: IValidationResult {
        User UserToRegister { get; set; }
    }

    public interface IUserService {

        Task<ILoginResultValidation> LoginValidate(LoginModel model);
        Task Login(User user);
        Task<IRegisterResultValidation> RegisterValidate(RegisterModel registerModel);
        Task<User> Register(RegisterModel registerModel);

    }
}
