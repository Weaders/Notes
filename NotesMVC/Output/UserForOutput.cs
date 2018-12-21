﻿using Newtonsoft.Json;
using NotesMVC.Models;

namespace NotesMVC.Output {

    public class UserForOutput {

        public UserForOutput(User user) {

            Id = user.Id;
            UserName = user.UserName;


        } 

        [JsonProperty("id")]
        public string Id;

        [JsonProperty("username")]
        public string UserName;

    }
}
