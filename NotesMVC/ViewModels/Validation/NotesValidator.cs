using Microsoft.EntityFrameworkCore;
using NotesMVC.Data;
using NotesMVC.DomainServices;
using NotesMVC.Services.Encrypter;
using System.Threading.Tasks;

namespace NotesMVC.ViewModels.Validation {
    public class NotesViewModelValidator {

        public interface IEditNoteResultValidate : IValidationResult {
            Note NoteAfterEdit { get; set; }
        }

        public interface IDeleteNoteResulValidate : IValidationResult {
            Note NoteForRemove { get; set; }
        }

        public interface IAddNoteResultValidate : IValidationResult {
            Note NoteToAdd { get; set; }
        }

        public class EditNoteResultValidate : ValidationResult, IEditNoteResultValidate {
            public Note NoteAfterEdit { get; set; }
        }

        public class DeleteNoteResulValidate : ValidationResult, IDeleteNoteResulValidate {
            public Note NoteForRemove { get; set; }
        }

        public class AddNoteResultValidate : ValidationResult, IAddNoteResultValidate {
            public Note NoteToAdd { get; set; }
        }

        private readonly DefaultContext _dbContext;
        private readonly CryptographManager _cryptoMng;
        private readonly IModelsFactory _modelsFactory;

        public NotesViewModelValidator(DefaultContext dbCtx, CryptographManager cryptoMng, IModelsFactory modelsFactory) {
            _dbContext = dbCtx;
            _cryptoMng = cryptoMng;
            _modelsFactory = modelsFactory;
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

            result.NoteToAdd = noteToAdd.ToNote(_cryptoMng, userWhoAdd, _modelsFactory);

            return result;

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

                noteToEdit.EditNote(note, userWhoEdit, _cryptoMng);
                result.NoteAfterEdit = note;

            }

            return result;

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

    }
}
