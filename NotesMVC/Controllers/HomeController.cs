using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesMVC.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NotesMVC.Controllers {

    public class HomeController : Controller {

        private readonly DefaultContext userDb;

        public HomeController(DefaultContext userContext) {
            this.userDb = userContext;
        }

        public IActionResult Index() {
            return View();

        }

    }

}
