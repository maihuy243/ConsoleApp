using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyApps.Chrome;
using MyApps.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MyApps.Functions.LoginFacebook
{
  public class LoginFacebook
  {

    public static void ExecuteLoginFacebook()
    {
      string filePath = "Functions\\LoginFacebook\\data.txt";
      ReadFileTxt dataReader = new ReadFileTxt(filePath);
      List<string> dataList = dataReader.ReadData();

      if (dataList.Count > 0)
      {
        ChromeInitial.RunMultipleChromeDrivers(dataList, ExcuteCallback);
      }

    }

    public static void ExcuteCallback(IWebDriver driver, string account)
    {
      if (account != null)
      {

        //string isAuthenticationUrl = "two_step_verification/authentication";
        string isTwoFactorUrl = "two_step_verification/two_factor";
        //Format is Username|pass|2fa|v.v
        string[] accountSplit = account.Split('|');
        string uid = accountSplit.Length > 0 ? accountSplit[0] : null;
        string pass = accountSplit.Length > 1 ? accountSplit[1] : null;
        string twoFa = accountSplit.Length > 2 ? accountSplit[2] : null;
        string hotmail = accountSplit.Length > 3 ? accountSplit[3] : null;
        string passHotMail = accountSplit.Length > 4 ? accountSplit[4] : null;
        string mailGetCode = accountSplit.Length > 5 ? accountSplit[5] : null;

        Console.WriteLine($"[ {uid} ]: Bat dau dang nhap !");

        driver.Navigate().GoToUrl("https://www.facebook.com/");

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
        var usernameE = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='email']")));
        var passwordE = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='pass']")));
        var submitE = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@type='submit']")));

        usernameE.SendKeys(uid);
        passwordE.SendKeys(pass);
        Task.Delay(2000).Wait();
        submitE.Click();

        string currentUrl = driver.Url;

        // Xác thực 2Fa
        if (currentUrl.Contains(isTwoFactorUrl))
        {
          Console.WriteLine($"[ {uid} ]: Dang lay ma 2Fa ");
          driver.ExecuteJavaScript("window.open('https://gauth.apps.gbraad.nl');");
          var windows = driver.WindowHandles;
          driver.SwitchTo().Window(windows[1]);
          driver.Navigate().GoToUrl("https://gauth.apps.gbraad.nl");

          var buttonAdd2Fa = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@id='addButton']")));
          buttonAdd2Fa.Click();
          Task.Delay(2000).Wait();


          // Fill data to get code 

          var keyAccount = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='keyAccount']")));
          var keySecreat = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='keySecret']")));
          var addKeyButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@id='addKeyButton']")));
          Task.Delay(2000).Wait();
          keyAccount.SendKeys(uid);
          keySecreat.SendKeys(twoFa);
          addKeyButton.Click();



          var twoFaCodeElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//span[text()='{uid}']//h3")));
          string code2Fa = twoFaCodeElement.Text;
          driver.SwitchTo().Window(windows[0]);
          Task.Delay(2000).Wait();

          var input2Fa = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@type='text']")));
          input2Fa.SendKeys(code2Fa);

          try
          {
            var form = driver.FindElement(By.TagName("form"));
            form.Submit();
          }
          catch
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ {uid} ]: Loi submit 2FaCode ");
            Console.ForegroundColor = ConsoleColor.Green;
          }


          // Click Home button 
          try
          {
            var homeIcon = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@href='/']")));
            homeIcon.Click();
            Console.WriteLine($"[ {uid} ]: Login thanh cong ✔ ");

          }
          catch
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ {uid} ]: Loi click home ");
            Console.ForegroundColor = ConsoleColor.Green;
          }
        }


        Task.Delay(100000).Wait();


        driver.Quit();

      }

    }


  }
}
