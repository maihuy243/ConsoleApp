using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using System;

namespace MyApps.Chrome
{
  public class ChromeManager
  {
    public class ChromeConfig
    {
      public ChromeWindowConfig chrome { get; set; }

      public class ChromeWindowConfig
      {
        public int width { get; set; }
        public int height { get; set; }
        public float scale { get; set; }
        public bool isHideChrome { get; set; }

      }
    }
    public IWebDriver CreateDriver(int instanceIndex)
    {
      ChromeConfig config = LoadConfig();
      ChromeOptions options = new ChromeOptions();

      int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
      int screenHeight = Screen.PrimaryScreen.Bounds.Height;

      // Calculate maxCols and maxRows
      int maxCols = screenWidth / config.chrome.width;
      int maxRows = screenHeight / config.chrome.height;

      // Calculate X and Y positions based on the index
      int x = (instanceIndex % maxCols) * config.chrome.width; // X position
      int y = (instanceIndex / maxCols) * config.chrome.height; // Y position


      Console.ForegroundColor = ConsoleColor.Green;
      if (config.chrome.isHideChrome)
      {
        options.AddArgument("--headless"); // Chạy Chrome ở chế độ không giao diện
      }
      options.AddArgument("--no-sandbox"); // Không sử dụng sandbox
      options.AddArgument("--disable-dev-shm-usage"); // Để giảm lỗi khi thiếu tài nguyên
      options.AddArgument("--disable-gpu"); // Tắt GPU
      options.AddArgument($"--window-size={config.chrome.width},{config.chrome.height}");
      options.AddArgument($"--window-position={x},{y}");
      options.AddArgument("--disable-infobars");
      options.AddArgument("--disable-notifications");
      options.AddArgument("--mute-audio");
      options.AddArgument("--log-level=3");
      options.AddArgument("--disable-webgl"); // Tắt WebGL
      options.AddArgument("--disable-software-rasterizer"); // Tắt rasterizer phần mềm


      var service = ChromeDriverService.CreateDefaultService();
      service.EnableVerboseLogging = false; // Tắt log chi tiết
      service.SuppressInitialDiagnosticInformation = true; // Tắt thông báo ban đầu
      service.HideCommandPromptWindow = true;


      IWebDriver driver = new ChromeDriver(service, options);

      return driver;
    }

    public ChromeConfig LoadConfig()
    {
      string settingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Configs", "settings.json");
      string json = File.ReadAllText(settingsFilePath);
      ChromeConfig config = JsonConvert.DeserializeObject<ChromeConfig>(json);
      return config;
    }

    public int CalculateMaxChromeInstances()
    {
      ChromeConfig config = LoadConfig();

      int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
      int screenHeight = Screen.PrimaryScreen.Bounds.Height;

      int maxCols = screenWidth / config.chrome.width;
      int maxRows = screenHeight / config.chrome.height;
      int maxChromeInstances = maxCols * maxRows;

      return maxChromeInstances;
    }
  }

}
