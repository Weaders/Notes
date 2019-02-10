using Newtonsoft.Json;
using NotesMVC.Models;
using NotesMVC.Services.Encrypter;
using System.ComponentModel.DataAnnotations;

namespace NotesMVC.ViewModels {
    public class NoteEdit {

        [JsonProperty("id")]
        [Required(ErrorMessage = "Id Required")]
        public int Id { get; set; }

        [JsonProperty("text")]
        [Required(ErrorMessage = "Text Required")]
        public string Text { get; set; }

        [JsonProperty("title")]
        [Required(ErrorMessage = "Title Required")]
        public string Title { get; set; }

        [JsonProperty("secretKey")]
        [Required(ErrorMessage = "Secret key required")]
        public string SecretKey { get; set; }

        [JsonProperty("algorithName")]
        [Required(ErrorMessage = "Algorithm name required")]
        public string AlgorithName { get; set; } = CryptographType.AES.Type;

        /// <summary>
        /// Return note from current class.
        /// </summary>
        /// <param name="manager">For get Cryptographer</param>
        /// <param name="user">Owner of note</param>
        /// <returns></returns>
        public Note ToNote(CryptographManager manager, User user, IModelsFactory modelsFactory) {

            var encoder = manager.Get(CryptographType.Get(AlgorithName));

            var note = modelsFactory.CreateNote();

            note.Title = encoder.Encrypt(Title, SecretKey);
            note.Text = encoder.Encrypt(Text, SecretKey);
            note.User = user;
            note.CryptoName = AlgorithName;

            return note;

        }

    }
}
