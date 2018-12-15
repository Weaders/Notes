using Newtonsoft.Json;
using NotesMVC.Models;
using NotesMVC.Services.Encrypter;

namespace NotesMVC.ViewModels {
    public class NoteEdit {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("secretKey")]
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
