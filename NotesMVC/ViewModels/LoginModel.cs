using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NotesMVC.ViewModels {
    public class LoginModel {

        [Required(ErrorMessage = "User Required")]
        [JsonProperty("user")]
        public string User { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [JsonProperty("password")]
        public string Password { get; set; }

    }
}
