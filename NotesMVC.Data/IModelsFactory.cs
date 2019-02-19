namespace NotesMVC.Data {
    public interface IModelsFactory {
        User CreateUser();
        Note CreateNote();
    }
}
