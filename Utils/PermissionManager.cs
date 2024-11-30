using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyApps.Utils
{
    public class PermissionManager
    {
        

        // Kiểm tra quyền truy cập
        public static bool CheckPermission()
        {
            string uuidEncrypt = "sxaflJlB0eRdhJKJFzFAbdQoiL1VPWX7L3BxeEOyxwpOQTIU2OLcrD15ddWtMlkylZIsHeLx2po7gDmF2C3SUA==";
            string uuidCurrent = UUID.GetMachineUUID();
            string permissionsFilePath = "Configs\\permissions.dat";


            string decryptedAdmin = EncryptionManager.Decrypt(uuidEncrypt);

            if (decryptedAdmin == uuidCurrent)
            {
                return true;
            }

            if (!File.Exists(permissionsFilePath))
            {
                Console.WriteLine("Permission file not found.");
                return false;
            }

            // Đọc danh sách UUID đã mã hóa từ permission.dat
            string[] encryptedUuids = File.ReadAllLines(permissionsFilePath);

            foreach (var encryptedUuid in encryptedUuids)
            {
                string decryptedUuid = EncryptionManager.Decrypt(encryptedUuid);
                if (decryptedUuid == uuidCurrent)
                {
                    return true; 
                }
            }

            return false; 
        }
    }
}
