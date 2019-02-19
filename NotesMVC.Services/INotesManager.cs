using NotesMVC.Data;
using System.Threading.Tasks;

namespace NotesMVC.Services {

    public interface INotesManager {

        Task<Note> AddNote(Note noteToAdd);
        Task<Note> EditeNote(Note noteToEdit);
        Task<Note> RemoveNote(Note noteForRemove);

        Task<Note[]> NotesForUser(User user);

    }
}
