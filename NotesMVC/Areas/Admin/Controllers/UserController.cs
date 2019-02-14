using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesMVC.Models;
using System.Linq;

namespace NotesMVC.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class UserController : Controller {

        private readonly DefaultContext context;

        public UserController(DefaultContext _context) {
            context = _context;
        }

        public IActionResult List() {

            var users = context.Users.ToArray();

            return Json(users);

        }

    }
}