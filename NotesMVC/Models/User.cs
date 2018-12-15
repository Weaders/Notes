using Microsoft.AspNetCore.Identity;

namespace NotesMVC.Models {

    public class User : IdentityUser {
        public string Hash { get; set; }
    }

}
