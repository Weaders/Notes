namespace NotesMVC.Models {
    public class Note {

        public int Id { get; set; }

        public byte[] Title { get; set; }
        public byte[] Text { get; set; }

        public string CryptoName { get; set; }

        public User User { get; set; }

    }
}