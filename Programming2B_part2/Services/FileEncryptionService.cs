﻿using System.Security.Cryptography;
using System.Text;

namespace Programming2B_part2.Services
{
    public class FileEncryptionService
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("MySecretKey12345MySecretKey12345");

        private static readonly byte[] IV = Encoding.UTF8.GetBytes("MyInitVector16b!");

        public async Task EncryptFileAsync(Stream input, string outputPath)
        {

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.Key = IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);


                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create))
                using (CryptoStream cryptoStream = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write))
                {
                    await input.CopyToAsync(cryptoStream);
                }
            }

        }


        public async Task<MemoryStream> DecryptFileStream(String encryptedFilePath)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform decrypter = aes.CreateDecryptor(aes.Key, aes.IV);


                using (FileStream fileStream = new FileStream(encryptedFilePath, FileMode.Open))
                using (CryptoStream cryptoStream = new CryptoStream(fileStream, decrypter, CryptoStreamMode.Read))
                {
                    MemoryStream decryptStream = new MemoryStream();
                    await cryptoStream.CopyToAsync(decryptStream);
                    decryptStream.Position = 0;
                    return decryptStream;

                }
            }
        }
    }

}
