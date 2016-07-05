using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kryptonite
{
    internal sealed class Kryptons
    {
        private const int BufferSize = 8192;
        private const string salt = "ph2DoYmgABfXlyD8JExHjdgfcUBaspNooUoCwtMolIi9jS8eWFedWTW77Sr3WIV";


        internal static byte[] EncryptStringToBytes(string text, byte[] key, byte[] iv)
        {
            byte[] encrypted;

            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            MessageBox.Show("Testo criptato", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return encrypted;

        }

        internal static string DecryptStringFromBytes(byte[] encryptedByteText, byte[] key, byte[] iv)
        {
            string plaintext = null;

            try
            {
                using (var rijAlg = new RijndaelManaged())
                {
                    rijAlg.Key = key;
                    rijAlg.IV = iv;

                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                    using (var msDecrypt = new MemoryStream(encryptedByteText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }
            }
            catch
            {
                MessageBox.Show("Errore durante la decrittazione, la chiave potrebbe essere sbagliata oppure il file potrebbe essere non valido!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return plaintext;
        }

        internal static void EncryptFile(string inputPath, string outputPath, string key)
        {
            var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            var output = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);

            var algorithm = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
            var hashedKey = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(salt));

            algorithm.Key = hashedKey.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = hashedKey.GetBytes(algorithm.BlockSize / 8);

            using (var encryptedStream = new CryptoStream(output, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
            {
                CopyStream(input, encryptedStream);
                MessageBox.Show("File criptato", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                File.Delete(inputPath);

            }
        }

        internal static void DecryptFile(string inputPath, string outputPath, string key)
        {
            var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            var output = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);

            var algorithm = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
            var hashedKey = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(salt));

            algorithm.Key = hashedKey.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = hashedKey.GetBytes(algorithm.BlockSize / 8);

            try
            {
                using (var decryptedStream = new CryptoStream(output, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    CopyStream(input, decryptedStream);
                    MessageBox.Show("File decriptato", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch
            {
                MessageBox.Show("Errore durante la decrittazione, la chiave potrebbe essere sbagliata oppure il file potrebbe essere non valido!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            using (output)
            using (input)
            {
                byte[] buffer = new byte[BufferSize];
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, read);
                }
            }
        }
    }
}
