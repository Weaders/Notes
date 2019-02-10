namespace NotesMVC.Models {
    public interface IModelsFactory {
        User CreateUser();
        Note CreateNote();
    }
}
