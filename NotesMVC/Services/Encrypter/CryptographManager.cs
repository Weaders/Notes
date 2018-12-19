namespace NotesMVC.Services.Encrypter {

    //public enum CryptographType {
    //    AES
    //}

    public class CryptographType {

        public readonly string Type;

        private CryptographType(string type) {
            this.Type = type;
        }

        public static  CryptographType AES = new CryptographType("aes");

        public static CryptographType Get(string str) {

            if (str == AES.Type) {
                return AES;
            }

            throw new System.Exception("Get type of cryptographer, that not implemented.");

        }

    }


    public class CryptographManager {

        /// <summary>
        /// Return new cryptographer by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ICryptograph Get(CryptographType type) {

            if (type == CryptographType.AES) {
                return new AesCryptograph();
            }

            throw new System.Exception("Get type of cryptographer, that not implemented.");

        }

    }
}
