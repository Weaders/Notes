using NotesMVC.Models;
using NotesMVC.Services.Encrypter;
using System.ComponentModel.DataAnnotations;

namespace NotesMVC.ViewModels {
    public class NoteAdd {

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string SecretKey { get; set; }

        public Note ToNote(ICryptograph encoder, User user) {

            return new Note() {
                Title = encoder.Encrypt(Title, SecretKey),
                Text = encoder.Encrypt(Text, SecretKey),
                User = user
            };

        }

    }
}
