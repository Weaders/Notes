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

        [Required]
        public string AlgorithmName { get; set; } = CryptographType.AES.Type;

        public Note ToNote(CryptographManager manager, User user) {

            if (AlgorithmName == null) {
                AlgorithmName = CryptographType.AES.Type;
            }

            var encoder = manager.Get(CryptographType.Get(AlgorithmName));

            return new Note() {
                Title = encoder.Encrypt(Title, SecretKey),
                Text = encoder.Encrypt(Text, SecretKey),
                User = user,
                CryptoName = AlgorithmName
            };

        }

    }
}
