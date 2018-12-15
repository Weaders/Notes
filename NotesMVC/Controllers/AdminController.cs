using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesMVC.Models;
using System.Linq;

namespace NotesMVC.Controllers {

    public class AdminController : Controller {

        private readonly UserManager<User> _manager;

        public AdminController(UserManager<User> manager) {
            _manager = manager;
        }

        public IActionResult Index() {
            return View(_manager.Users.ToArray());
        }
    }
}