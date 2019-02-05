﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NotesMVC.Models;
using NotesMVC.Output;
using NotesMVC.Services.Encrypter;
using NotesMVC.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace NotesMVC.Controllers {

    public class NotesController : Controller {

        private readonly DefaultContext db;
        private readonly CryptographManager manager;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IStringLocalizer<SharedResources> sharedLocalizer;

        public NotesController(DefaultContext _db, CryptographManager _manager, UserManager<User> _userManager, SignInManager<User> _signInManager, IStringLocalizer<SharedResources> _sharedLocalizer) {

            userManager = _userManager;
            db = _db;
            manager = _manager;
            signInManager = _signInManager;
            sharedLocalizer = _sharedLocalizer;

        }

        /// <summary>
        /// List notes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> List([FromQuery]string key) {

            var cryptographer = manager.Get(CryptographType.AES);
            var user = await userManager.GetUserAsync(User);

            var notesRdy = db.Notes
                .Where(n => n.User == user)
                .Select(n => new NoteForOutput(n, manager, key))
                .ToArray();

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
                return new JsonFailResult(ModelState);
            }

            var user = await userManager.GetUserAsync(User);

            if (user == null) {

                await this.signInManager.SignOutAsync();
                return new JsonFailResult(sharedLocalizer.GetString("Bad user"));

            }

            var note = noteModel.ToNote(manager, user);

            await db.AddAsync(note);
            await db.SaveChangesAsync();

            return Json(new NoteForOutput(note, manager, noteModel.SecretKey));

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
                return new JsonFailResult(ModelState);
            }

            var user = await userManager.GetUserAsync(User);

            if (user == null) {
                return new JsonFailResult(sharedLocalizer.GetString("Bad user"));
            }

            var note = await this.db.Notes.FirstOrDefaultAsync((n) => n.Id == noteForm.Id && n.User == user);

            if (note == null) {
                return new JsonFailResult(sharedLocalizer.GetString("There no note with this id"));
            } else {

                var newNote = noteForm.ToNote(manager, user);

                note.Text = newNote.Text;
                note.Title = newNote.Title;
                note.CryptoName = newNote.CryptoName;

                db.Notes.Update(note);
                await db.SaveChangesAsync();

                return Json(new NoteForOutput(note, manager, noteForm.SecretKey));
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

            var user = await userManager.GetUserAsync(User);

            if (user == null) {
                return new JsonFailResult(sharedLocalizer.GetString("Bad user"));
            }

            var note = await db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.User == user);

            if (note == null) {
                return new JsonFailResult(sharedLocalizer.GetString("There no note with this id"));
            }

            db.Notes.Remove(note);

            await db.SaveChangesAsync();

            return Json("Ok");

        }

    }

}