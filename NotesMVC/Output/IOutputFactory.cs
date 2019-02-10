using Microsoft.AspNetCore.Mvc.ModelBinding;
using NotesMVC.Models;
using NotesMVC.Services.Encrypter;

namespace NotesMVC.Output {
    public interface IOutputFactory {

        UserForOutput CreateUser(User user);

        NoteForOutput CreateNote(Note note, CryptographManager manager, string secretCode);

        JsonFailResult CreateJsonFail(string error);

        JsonFailResult CreateJsonFail(ModelStateDictionary modelState);

    }
}
