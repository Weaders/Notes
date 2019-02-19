using Newtonsoft.Json;
using NotesMVC.Data;
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
        /// Edit existed note.
        /// </summary>
        /// <param name="noteForEdit"></param>
        /// <param name="user"></param>
        /// <param name="cryptoManager"></param>
        public void EditNote(Note noteForEdit, User user, CryptographManager cryptoManager) {

            var encoder = cryptoManager.Get(CryptographType.Get(AlgorithName));

            noteForEdit.Text = encoder.Encrypt(Text, SecretKey);
            noteForEdit.Title = encoder.Encrypt(Title, SecretKey);
            noteForEdit.User = user;
            noteForEdit.CryptoName = AlgorithName;

        }

    }
}
