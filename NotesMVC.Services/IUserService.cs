using NotesMVC.Data;
using System.Threading.Tasks;

namespace NotesMVC.Services {

    public interface IUserService {
        Task<User> Register(User registerModel, string pwd);
    }
}
