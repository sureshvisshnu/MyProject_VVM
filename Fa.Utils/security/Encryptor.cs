using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace fa.api.security
{
    public class Encryptor
    {
        private string EncKey="AB2AEabcdEFg01234g";

        public string EncryptText(string Input)
        {
            // Get the bytes of the string
            byte[] BytesToBeEncrypted = Encoding.UTF8.GetBytes(Input);
            byte[] PasswordBytes = Encoding.UTF8.GetBytes(EncKey);

            // Hash the password with SHA256
            PasswordBytes = SHA256.Create().ComputeHash(PasswordBytes);

            byte[] BytesEncrypted = AES_Encrypt(BytesToBeEncrypted, PasswordBytes);

            string Result = Convert.ToBase64String(BytesEncrypted);

            return Result;
        }

        public string DecryptText(string Input, string Password)
        {
            // Get the bytes of the string
            byte[] BytesToBeDecrypted = Convert.FromBase64String(Input);
            byte[] PasswordBytes = Encoding.UTF8.GetBytes(EncKey);
            PasswordBytes = SHA256.Create().ComputeHash(PasswordBytes);

            byte[] BytesDecrypted = AES_Decrypt(BytesToBeDecrypted, PasswordBytes);

            string Result = Encoding.UTF8.GetString(BytesDecrypted);

            return Result;
        }

        public void EncryptFile()
        {
            string file = "C:\\SampleFile.DLL";
            byte[] BytesToBeEncrypted = File.ReadAllBytes(file);
            byte[] PasswordBytes = Encoding.UTF8.GetBytes(EncKey);

            // Hash the password with SHA256
            PasswordBytes = SHA256.Create().ComputeHash(PasswordBytes);

            byte[] BytesEncrypted = AES_Encrypt(BytesToBeEncrypted, PasswordBytes);

            string FileEncrypted = "C:\\SampleFileEncrypted.DLL";

            File.WriteAllBytes(FileEncrypted, BytesEncrypted);
        }


        public void DecryptFile()
        {
            string fileEncrypted = "C:\\SampleFileEncrypted.DLL";
            byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(EncKey);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string file = "C:\\SampleFile.DLL";
            File.WriteAllBytes(file, bytesDecrypted);
        }

        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}
