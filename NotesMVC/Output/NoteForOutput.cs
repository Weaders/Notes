using Newtonsoft.Json;
using NotesMVC.Data;
using NotesMVC.Services.Encrypter;
using System;
using System.Text;

namespace NotesMVC.Output {

    [Serializable]
    public class NoteForOutput {

        public NoteForOutput(Note note, CryptographManager manager, string secretCode) {

            Id = note.Id;

            var cryptograph = manager.Get(CryptographType.Get(note.CryptoName));

            try {

                Text = cryptograph.Decrypt(note.Text, secretCode);
                Title = cryptograph.Decrypt(note.Title, secretCode);

            } catch {

                Text = Encoding.ASCII.GetString(note.Text);
                Title = Encoding.ASCII.GetString(note.Title);

            }

        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

    }

}
