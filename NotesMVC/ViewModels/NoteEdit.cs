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

        [Required]
        [JsonProperty("algorithName")]
        public string AlgorithName { get; set; } = CryptographType.AES.Type;

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
