namespace NotesMVC.Services.Encrypter {
    public interface ICryptograph {

        byte[] Encrypt(string text, string secretKey);
        string Decrypt(byte[] encodedText, string secretKey);
        string GenerateSecretKey();

    }
}
