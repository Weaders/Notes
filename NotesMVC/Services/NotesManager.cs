using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotesMVC.Models;
using NotesMVC.Services.Encrypter;
using NotesMVC.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace NotesMVC.Services {

    public class NotesManager : INotesManager {

        public class EditNoteResultValidate : ValidationResult, IEditNoteResultValidate {
            public Note NoteForEdit { get; set; }
        }

        public class DeleteNoteResulValidate : ValidationResult, IDeleteNoteResulValidate {
            public Note NoteForRemove { get; set; }
        }

        public class AddNoteResultValidate : ValidationResult, IAddNoteResultValidate { }

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
        /// Validate add note form.
        /// </summary>
        /// <param name="noteToAdd"></param>
        /// <param name="userWhoAdd"></param>
        /// <param name="stateValid"></param>
        /// <returns></returns>
        public IAddNoteResultValidate ValidateAddNote(NoteAdd noteToAdd, User userWhoAdd) {

            var result = new AddNoteResultValidate();

            if (userWhoAdd == null) {

                result.Errors.Add("Bad user", "Bad user");
                result.IsSuccess = false;

            }

            return result;

        }

        /// <summary>
        /// Add note
        /// </summary>
        /// <param name="noteToAdd"></param>
        /// <param name="userWhoAdd"></param>
        /// <returns></returns>
        public async Task<Note> AddNote(NoteAdd noteToAdd, User userWhoAdd) {

            var note = noteToAdd.ToNote(_cryptoMng, userWhoAdd, _modelsFactory);

            _dbContext.Notes.Add(note);

            await _dbContext.SaveChangesAsync();

            return note;

        }

        /// <summary>
        /// Validate and process note edit form
        /// </summary>
        /// <param name="noteToEdit"></param>
        /// <param name="userWhoEdit"></param>
        /// <param name="stateValid"></param>
        /// <returns></returns>
        public async Task<IEditNoteResultValidate> ValidateEditNote(NoteEdit noteToEdit, User userWhoEdit) {

            var result = new EditNoteResultValidate();

            if (userWhoEdit == null) {

                result.IsSuccess = false;
                result.Errors.Add("Bad user", "Bad user");

            }

            var note = await _dbContext.Notes.FirstOrDefaultAsync((n) => n.Id == noteToEdit.Id && n.User == userWhoEdit);

            if (note == null) {

                result.IsSuccess = false;
                result.Errors.Add("There no note with this id", "There no note with this id");

            } else {

                result.NoteForEdit = note;

            }

            return result;

        }

        /// <summary>
        /// Edit note.
        /// </summary>
        /// <param name="noteToEdit"></param>
        /// <param name="noteEdit"></param>
        /// <param name="userWhoEdit"></param>
        /// <returns></returns>
        public async Task<Note> EditeNote(Note noteToEdit, NoteEdit noteEdit, User userWhoEdit) {

            var newNote = noteEdit.ToNote(_cryptoMng, userWhoEdit, _modelsFactory);

            noteToEdit.Text = newNote.Text;
            noteToEdit.Title = newNote.Title;
            noteToEdit.CryptoName = newNote.CryptoName;

            _dbContext.Update(noteToEdit);

            await _dbContext.SaveChangesAsync();

            return noteToEdit;

        }

        /// <summary>
        /// Validate form for delete note.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userWhoDelete"></param>
        /// <returns></returns>
        public async Task<IDeleteNoteResulValidate> ValidateDeleteNote(int id, User userWhoDelete) {

            var result = new DeleteNoteResulValidate();

            if (userWhoDelete == null) {

                result.IsSuccess = false;
                result.Errors.Add("Bad user", "Bad user");

            }

            var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.Id == id && n.User == userWhoDelete);

            if (note == null) {

                result.IsSuccess = false;
                result.Errors.Add("There no note with this id", "There no note with this id");

            } else {
                result.NoteForRemove = note;
            }

            return result;

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
