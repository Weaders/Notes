namespace NotesMVC.Services.Encrypter {

    public class CryptographType {

        public readonly string Type;

        private CryptographType(string type) {
            this.Type = type;
        }

        public static CryptographType AES = new CryptographType("aes");

        public static CryptographType Get(string str) {

            if (str == AES.Type) {
                return AES;
            }

            throw new System.Exception("Get type of cryptographer, that not implemented.");

        }

    }

}