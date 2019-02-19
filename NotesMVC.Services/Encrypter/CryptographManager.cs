namespace NotesMVC.Services.Encrypter {

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
