using NotesMVC.Models;
using NotesMVC.ViewModels;
using System.Threading.Tasks;

namespace NotesMVC.Services {

    public interface IEditNoteResultValidate : IValidationResult {
        Note NoteForEdit { get; set; }
    }

    public interface IDeleteNoteResulValidate : IValidationResult {
        Note NoteForRemove { get; set; }
    }

    public interface IAddNoteResultValidate : IValidationResult { }

    public interface INotesManager {

        IAddNoteResultValidate ValidateAddNote(NoteAdd noteToAdd, User userWhoAdd);
        Task<Note> AddNote(NoteAdd noteToAdd, User userWhoAdd);

        Task<IEditNoteResultValidate> ValidateEditNote(NoteEdit noteToEdit, User userWhoEdit);
        Task<Note> EditeNote(Note noteToEdit, NoteEdit noteEdit, User userWhoEdit);

        Task<IDeleteNoteResulValidate> ValidateDeleteNote(int id, User userWhoDelete);
        Task<Note> RemoveNote(Note noteForRemove);

        Task<Note[]> NotesForUser(User user);

    }
}
