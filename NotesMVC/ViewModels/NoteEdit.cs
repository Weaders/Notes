using Newtonsoft.Json;
using NotesMVC.Models;
using NotesMVC.Services.Encrypter;
using System.ComponentModel.DataAnnotations;

namespace NotesMVC.ViewModels {
    public class NoteEdit {

        [JsonProperty("id")]
        [Required]
        public int Id { get; set; }

        [JsonProperty("text")]
        [Required]
        public string Text { get; set; }

        [JsonProperty("title")]
        [Required]
        public string Title { get; set; }

        [JsonProperty("secretKey")]
        [Required]
        public string SecretKey { get; set; }

        [JsonProperty("algorithName")]
        [Required]
        public string AlgorithName { get; set; } = CryptographType.AES.Type;

        /// <summary>
        /// Return note from current class.
        /// </summary>
        /// <param name="manager">For get Cryptographer</param>
        /// <param name="user">Owner of note</param>
        /// <returns></returns>
        public Note ToNote(CryptographManager manager, User user) {

            var encoder = manager.Get(CryptographType.Get(AlgorithName));

            return new Note() {
                Title = encoder.Encrypt(Title, SecretKey),
                Text = encoder.Encrypt(Text, SecretKey),
                User = user,
                CryptoName = AlgorithName
            };

        }

    }
}
