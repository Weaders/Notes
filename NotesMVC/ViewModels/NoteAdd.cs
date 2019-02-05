using NotesMVC.Models;
using NotesMVC.Services.Encrypter;
using System.ComponentModel.DataAnnotations;

namespace NotesMVC.ViewModels {
    public class NoteAdd {

        [Required(ErrorMessage = "Title Required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Text Required")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Secret key required")]
        public string SecretKey { get; set; }

        [Required(ErrorMessage = "Algorithm name required")]
        public string AlgorithmName { get; set; } = CryptographType.AES.Type;

        /// <summary>
        /// Return note, based on data from current class.
        /// </summary>
        /// <param name="manager">For get cryptographer</param>
        /// <param name="user">Owner of note</param>
        /// <returns></returns>
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
