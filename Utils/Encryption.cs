using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyApps.Utils
{
    public class EncryptionManager
    {
        private static string key = "CodeByMSH!"; // Khóa AES

        // Mã hóa UUID
        public static string Encrypt(string uuid)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(uuid);
                    }

                    byte[] encryptedData = ms.ToArray();
                    byte[] result = new byte[iv.Length + encryptedData.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(encryptedData, 0, result, iv.Length, encryptedData.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }

        // Mã hóa và lưu UUID vào permission.dat
        public static void EncryptAndSaveUuid(string uuid)
        {
            string encryptedUuid = Encrypt(uuid);
            File.AppendAllText("Configs\\permissions.dat", encryptedUuid + Environment.NewLine);
            Console.WriteLine("UUID encrypted and saved.");
        }

        // Mã hóa tất cả UUID từ file data.dat và lưu vào permissions.dat
        public static void EncryptFromFileAndSave()
        {
            string dataFilePath = "Configs\\data.dat";

            if (!File.Exists(dataFilePath))
            {
                Console.WriteLine("File data.dat not found.");
                return;
            }

            // Đọc các UUID từ file data.dat
            string[] uuids = File.ReadAllLines(dataFilePath);

            // Mã hóa mỗi UUID và lưu vào permission.dat
            foreach (var uuid in uuids)
            {
                EncryptAndSaveUuid(uuid);
            }

            Console.WriteLine("All UUIDs from data.dat have been encrypted and saved to permissions.dat.");
        }

        // Giải mã UUID
        public static string Decrypt(string encryptedText)
        {
            try
            {
                byte[] cipherData = Convert.FromBase64String(encryptedText);
                byte[] iv = new byte[16];
                byte[] encryptedBytes = new byte[cipherData.Length - iv.Length];

                Buffer.BlockCopy(cipherData, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(cipherData, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                using (var aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor())
                    using (var ms = new MemoryStream(encryptedBytes))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption error: " + ex.Message);
                return null;
            }
        }
    }
}
