using Microsoft.AspNetCore.Mvc.ModelBinding;
using NotesMVC.Models;
using NotesMVC.Services.Encrypter;

namespace NotesMVC.Output {

    public class OutputFactory : IOutputFactory {

        public UserForOutput CreateUser(User user) {
            return new UserForOutput(user);
        }

        public NoteForOutput CreateNote(Note note, CryptographManager manager, string secretCode) {
            return new NoteForOutput(note, manager, secretCode);
        }

        public JsonFailResult CreateJsonFail(string error) {
            return new JsonFailResult(error);
        }

        public JsonFailResult CreateJsonFail(ModelStateDictionary modelState) {
            return new JsonFailResult(modelState);
        }

    }

}
