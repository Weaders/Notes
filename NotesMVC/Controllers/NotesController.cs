using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesMVC.JsonResults;
using NotesMVC.Models;
using NotesMVC.Services.Encrypter;
using NotesMVC.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace NotesMVC.Controllers {

    public class NotesController : Controller {

        private readonly DefaultContext _db;
        private readonly CryptographManager _manager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public NotesController(DefaultContext db, CryptographManager manager, UserManager<User> userManager, SignInManager<User> signInManager) {

            _userManager = userManager;
            _db = db;
            _manager = manager;
            _signInManager = signInManager;

        }

        /// <summary>
        /// List notes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> List() {

            var cryptographer = _manager.Get(CryptographType.AES);
            var user = await _userManager.GetUserAsync(User);

            var notesRdy = _db.Notes
                .Where(n => n.User == user)
                .Select(n => new NoteForOutput(n, cryptographer, Request.Query["key"]));

            return Json(notesRdy);

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
                return Json(ModelState);
            }

            var cryptographer = _manager.Get(CryptographType.AES);
            var user = await _userManager.GetUserAsync(User);

            if (user == null) {

                await this._signInManager.SignOutAsync();
                return new JsonFailResult("Bad user.");

            }

            var note = noteModel.ToNote(cryptographer, user);

            await _db.AddAsync(note);
            await _db.SaveChangesAsync();

            return Json(new NoteForOutput(note, cryptographer, noteModel.SecretKey));

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

            var user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return new JsonFailResult("Bad user!");
            }

            var note = await this._db.Notes.FirstOrDefaultAsync((n) => n.Id == noteForm.Id && n.User == user);

            if (note == null) {
                return new JsonFailResult("There no note with this id");
            } else {

                var cryptograph = _manager.Get(CryptographType.AES);

                var newNote = noteForm.ToNote(cryptograph, user);

                note.Text = newNote.Text;
                note.Title = newNote.Title;

                _db.Notes.Update(note);
                await _db.SaveChangesAsync();

                return Json(new NoteForOutput(note, cryptograph, noteForm.SecretKey));
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

            var user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return new JsonFailResult("Bad user!");
            }

            var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.User == user);

            if (note == null) {
                return new JsonFailResult("User dont have note with that id.");
            }

            _db.Notes.Remove(note);

            await _db.SaveChangesAsync();

            return Json("Ok");

        }

    }

}