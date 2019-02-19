using Microsoft.AspNetCore.Identity;
using NotesMVC.Data;
using System.Threading.Tasks;

namespace NotesMVC.Services {
    public class UserService : IUserService {

        private readonly UserManager<User> _userMng;

        public UserService(UserManager<User> userMng) {
            _userMng = userMng;
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="registerModel"></param>
        /// <param name="userToReg"></param>
        /// <returns></returns>
        public async Task<User> Register(User userToRegister, string pwd) {

            var userCreate = await _userMng.CreateAsync(userToRegister, pwd);

            if (userCreate.Succeeded) {
                return userToRegister;
            }

            return null;

        }

    }
}
