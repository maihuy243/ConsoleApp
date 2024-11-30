using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApps.Functions.LoginFacebook;
using MyApps.Utils;

namespace MyApps
{
  public class RunTask
  {
    public static void Execute(string choice)
    {

      Console.WriteLine("Executing the main task...", choice);
      switch (choice)
      {
        case "login_facebook":
          LoginFacebook.ExecuteLoginFacebook();
          break;
        case "encrypt_uuid":
          EncryptionManager.EncryptFromFileAndSave();
          break;
        default:
          Console.WriteLine("Invalid choice.");
          LoginFacebook.ExecuteLoginFacebook();
          break;
      }
      Console.WriteLine("Task completed!");
    }
  }
}
