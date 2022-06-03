using System;
using System.IO;
using System.Security.Cryptography;

namespace cryptsoft
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // args[0] fichier a crypter
            // args[1] dossier emplacement .enc
            // args[2] clé
            if (args.Length != 0 && args.Length == 3)
            {
                try
                {
                    Crypt chiff = new Crypt((string)args[0].Replace("?"," "), (string)args[1].Replace("?", " "), (string)args[2]);
                    chiff.EncryptFile();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            } else
            {
                Crypt chiff = new Crypt(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
                chiff.EncryptFile();
            }
        }
    }

    public class Crypt
    {
        private readonly CspParameters _cspp = new CspParameters();
        private RSACryptoServiceProvider _rsa;

        private string RepFolder;
        private FileInfo TargetFile;
        private string KeyName;

        public Crypt(string pathFileSource,string pathFileTarget, string newKey)
        {
            TargetFile = new FileInfo(pathFileSource);
            RepFolder = pathFileTarget;
            KeyName = newKey;
            _rsa = CreateKeys();
        }

        public RSACryptoServiceProvider CreateKeys()
        {
            _cspp.KeyContainerName = KeyName;
            return new RSACryptoServiceProvider(_cspp)
            {
                PersistKeyInCsp = true
            };
        }

        public void EncryptFile()
        {
            Aes aes = Aes.Create();
            byte[] keyEncrypted = _rsa.Encrypt(aes.Key, false); 

            // Create file with ".enc" extension
            string file = Path.Combine(RepFolder, Path.ChangeExtension(TargetFile.Name, ".enc"));

            //write file
            using (var outFile = new FileStream(file, FileMode.Create))
            {
                outFile.Write(BitConverter.GetBytes(keyEncrypted.Length), 0, 4);
                outFile.Write(BitConverter.GetBytes(aes.IV.Length), 0, 4);
                outFile.Write(keyEncrypted, 0, keyEncrypted.Length);
                outFile.Write(aes.IV, 0, aes.IV.Length);

                using (var encryptedContent = new CryptoStream(outFile, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    int count = 0, bytesRead = 0, offset = 0;
                    byte[] bytes = new byte[aes.BlockSize / 8];


                    using (var fileStream = new FileStream(TargetFile.FullName, FileMode.Open))
                    {
                        do
                        {
                            count = fileStream.Read(bytes, 0, aes.BlockSize / 8);
                            offset += count;
                            encryptedContent.Write(bytes, 0, count);
                            bytesRead += aes.BlockSize / 8;
                        } while (count > 0);
                    }
                    encryptedContent.FlushFinalBlock();
                }
            }
        }
    }
}
