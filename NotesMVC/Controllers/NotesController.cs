using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesMVC.Data;
using NotesMVC.Output;
using NotesMVC.Services;
using NotesMVC.ViewModels;
using NotesMVC.ViewModels.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace NotesMVC.Controllers {

    public class NotesController : Controller {

        private readonly UserManager<User> _userManager;
        private readonly IOutputFactory _outputFactory;
        private readonly INotesManager _notesMng;
        private readonly NotesViewModelValidator _notesValidator;

        public NotesController(UserManager<User> userManager, IOutputFactory outputFactory, INotesManager notesMng, NotesViewModelValidator notesValidator) {

            _userManager = userManager;
            _outputFactory = outputFactory;
            _notesMng = notesMng;
            _notesValidator = notesValidator;

        }

        /// <summary>
        /// List notes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> List([FromQuery]string key) {

            var user = await _userManager.GetUserAsync(User);
            var notes = await _notesMng.NotesForUser(user);

            return Json(notes.Select(n => _outputFactory.CreateNote(n, key)));

        }

        /// <summary>
        /// Add note 
        /// </summary>
        /// <param name="noteModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("notes/add")]
        public async Task<IActionResult> AddNote([FromBody]NoteAdd noteModel) {

            if (!ModelState.IsValid) {
                return _outputFactory.CreateJsonFail(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            var addValid = _notesValidator.ValidateAddNote(noteModel, user);

            if (addValid.IsSuccess) {

                var note = await _notesMng.AddNote(addValid.NoteToAdd);
                return Json(_outputFactory.CreateNote(note, noteModel.SecretKey));

            } else {

                addValid.ErrorsToModelState(ModelState);
                return Json(_outputFactory.CreateJsonFail(ModelState));
            }

        }

        /// <summary>
        /// Edit note
        /// </summary>
        /// <param name="noteForm"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("notes/{id:int}/edit")]
        public async Task<IActionResult> EditNote([FromBody]NoteEdit noteForm) {

            if (!ModelState.IsValid) {
                return _outputFactory.CreateJsonFail(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            var editValid = await _notesValidator.ValidateEditNote(noteForm, user);

            if (editValid.IsSuccess) {

                var editedNote = await _notesMng.EditeNote(editValid.NoteAfterEdit);
                return Json(_outputFactory.CreateNote(editedNote, noteForm.SecretKey));

            } else {

                editValid.ErrorsToModelState(ModelState);
                return _outputFactory.CreateJsonFail(ModelState);

            }

        }

        /// <summary>
        /// Delete note
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("notes/{id:int}")]
        public async Task<IActionResult> DeleteNote(int id) {

            if (!ModelState.IsValid) {
                return _outputFactory.CreateJsonFail(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            var delValid = await _notesValidator.ValidateDeleteNote(id, user);

            if (delValid.IsSuccess) {

                var deleted = await _notesMng.RemoveNote(delValid.NoteForRemove);
                return Json("Ok");

            } else {

                delValid.ErrorsToModelState(ModelState);
                return _outputFactory.CreateJsonFail(ModelState);

            }

        }

    }

}