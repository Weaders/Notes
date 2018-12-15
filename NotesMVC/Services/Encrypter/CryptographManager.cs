namespace NotesMVC.Services.Encrypter {

    public enum CryptographType {
        AES
    }

    public class CryptographManager {

        /// <summary>
        /// Return new cryptographer by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ICryptograph Get(CryptographType type) {

            switch (type) {

                case CryptographType.AES:
                    return new AesCryptograph();
                default:
                    throw new System.Exception("Get type of cryptographer, that not implemented.");
                
            }

        }

    }
}
