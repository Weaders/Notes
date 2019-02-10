namespace NotesMVC.Models {

    public class ModelsFactory: IModelsFactory {

        public Note CreateNote() {
            return new Note();
        }

        public User CreateUser() {
            return new User();
        }

    }

}
