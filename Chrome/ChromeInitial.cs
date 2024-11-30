using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using static MyApps.Chrome.ChromeManager;

namespace MyApps.Chrome
{
  public class ChromeInitial
  {
    // Hàm chạy đa nhiệm Chrome Driver
    public static void RunMultipleChromeDrivers(List<string> dataList, Action<IWebDriver, string> callback)
    {
      ChromeManager manager = new ChromeManager();

      int maxChromeInstances = manager.CalculateMaxChromeInstances();

      // Tạo các driver và xử lý callback
      List<Task> tasks = new List<Task>();

      // Danh sách các driver đã được tạo
      List<IWebDriver> activeDrivers = new List<IWebDriver>();

      // Danh sách các vị trí trống
      List<int> availablePositions = new List<int>();

      // Semaphore để giới hạn số lượng driver chạy cùng lúc
      SemaphoreSlim semaphore = new SemaphoreSlim(maxChromeInstances);

      for (int i = 0; i < dataList.Count; i++)
      {
        // Đợi khi có chỗ trống
        semaphore.Wait();
        int currentIndex = i;

        // Khởi tạo Chrome Driver trong Task mới
        tasks.Add(Task.Run(() =>
        {
          try
          {
            // Kiểm tra xem có vị trí trống nào không
            int driverIndex;
            if (availablePositions.Count > 0)
            {
              // Nếu có vị trí trống, lấy một vị trí trống từ danh sách
              driverIndex = availablePositions[0];
              availablePositions.RemoveAt(0);  // Loại bỏ vị trí đã dùng
            }
            else
            {
              // Nếu không có vị trí trống, gán chỉ số hiện tại
              driverIndex = currentIndex;
            }

            var driver = manager.CreateDriver(driverIndex);
            try
            {
              if (driverIndex >= 0 && driverIndex < activeDrivers.Count)
              {
                activeDrivers.Insert(driverIndex, driver);
              }
              else
              {
                // Nếu không hợp lệ, thêm vào cuối danh sách
                activeDrivers.Add(driver);
              }
              callback(driver, dataList[currentIndex]);
              Task.Delay(1500).Wait();
            }
            finally
            {
              driver.Quit();
            }
          }
          finally
          {
            // Thả semaphore khi driver kết thúc
            semaphore.Release();
            // Nếu driver bị tắt, thêm vị trí vào danh sách availablePositions
            if (activeDrivers.Count > 0)
            {
              int finishedDriverIndex = activeDrivers.Count - 1 < 0 ? 0 : activeDrivers.Count - 1; // Ví dụ: sử dụng vị trí cuối cùng
              availablePositions.Add(finishedDriverIndex);
              activeDrivers.RemoveAt(finishedDriverIndex);
            }
          }
        }));
      }
      // Đợi tất cả các task hoàn thành
      Task.WhenAll(tasks).Wait();
    }
  }
}
