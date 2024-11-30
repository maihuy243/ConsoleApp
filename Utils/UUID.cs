using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace MyApps.Utils
{
    class UUID
    {
        public static string GetMachineUUID()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT UUID FROM Win32_ComputerSystemProduct"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return obj["UUID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving UUID: {ex.Message}");
            }
            return string.Empty;
        }
    }
}
