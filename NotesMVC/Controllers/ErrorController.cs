using Microsoft.AspNetCore.Mvc;

namespace NotesMVC.Controllers {
    public class ErrorController : Controller {
        [Route("/NotThere")]
        public IActionResult Index() {
            return StatusCode(404);
        }
    }
}