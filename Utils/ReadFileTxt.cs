using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApps.Utils
{
    public class ReadFileTxt
    {
        private readonly string _filePath;

        public ReadFileTxt(string filePath)
        {
            _filePath = filePath;
        }

        public List<string> ReadData()
        {
            List<string> dataList = new List<string>();

            try
            {
                // Đọc tất cả các dòng trong file và thêm vào list
                string[] lines = File.ReadAllLines(_filePath);

                string[] nonEmptyLines = lines
           .Where(line => !string.IsNullOrWhiteSpace(line))
           .ToArray();
                dataList.AddRange(nonEmptyLines);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Read file error: " + ex.Message);
            }

            return dataList;
        }
    }
}
