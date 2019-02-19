using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotesMVC.Data;
using NotesMVC.DomainServices;
using NotesMVC.Services.Encrypter;
using System.Linq;
using System.Threading.Tasks;

namespace NotesMVC.Services {

    public class NotesManager : INotesManager {

        private readonly DefaultContext _dbContext;
        private readonly CryptographManager _cryptoMng;
        private readonly IModelsFactory _modelsFactory;
        private readonly UserManager<User> _userMng;

        public NotesManager(DefaultContext context, CryptographManager cryptoMng, IModelsFactory modelsFactory, UserManager<User> userMng) {
            _dbContext = context;
            _cryptoMng = cryptoMng;
            _modelsFactory = modelsFactory;
            _userMng = userMng;
        }


        /// <summary>
        /// Add note
        /// </summary>
        /// <param name="noteToAdd"></param>
        /// <param name="userWhoAdd"></param>
        /// <returns></returns>
        public async Task<Note> AddNote(Note noteToAdd) {

            _dbContext.Notes.Add(noteToAdd);

            await _dbContext.SaveChangesAsync();

            return noteToAdd;

        }

        /// <summary>
        /// Edit note.
        /// </summary>
        /// <param name="noteToEdit"></param>
        /// <param name="noteEdit"></param>
        /// <param name="userWhoEdit"></param>
        /// <returns></returns>
        public async Task<Note> EditeNote(Note noteToEdit) {

            _dbContext.Update(noteToEdit);

            await _dbContext.SaveChangesAsync();

            return noteToEdit;

        }

        /// <summary>
        /// Remove note.
        /// </summary>
        /// <param name="noteForRemove"></param>
        /// <returns></returns>
        public async Task<Note> RemoveNote(Note noteForRemove) {

            _dbContext.Notes.Remove(noteForRemove);
            await _dbContext.SaveChangesAsync();

            return noteForRemove;

        }

        /// <summary>
        /// Get notes for user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Note[]> NotesForUser(User user) {

            return await _dbContext.Notes
                .Where(n => n.User == user)
                .ToArrayAsync();

        }

    }
}
