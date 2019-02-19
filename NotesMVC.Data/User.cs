using Microsoft.AspNetCore.Identity;

namespace NotesMVC.Data {

    public class User : IdentityUser {
        public string Hash { get; set; }
    }

}
