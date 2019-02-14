using Microsoft.AspNetCore.Mvc.ModelBinding;
using NotesMVC.Models;
using NotesMVC.Services.Encrypter;

namespace NotesMVC.Output {

    public class OutputFactory : IOutputFactory {

        private readonly CryptographManager _cryptoManager;

        public OutputFactory(CryptographManager manager) {
            _cryptoManager = manager;
        }

        public UserForOutput CreateUser(User user) {
            return new UserForOutput(user);
        }

        public NoteForOutput CreateNote(Note note, string secretCode) {
            return new NoteForOutput(note, _cryptoManager, secretCode);
        }

        public JsonFailResult CreateJsonFail(string error) {
            return new JsonFailResult(error);
        }

        public JsonFailResult CreateJsonFail(ModelStateDictionary modelState) {
            return new JsonFailResult(modelState);
        }

    }

}
