using System;
using System.IO;
using System.Threading;
using MyApps;
using MyApps.Utils;

try
{

  Console.WriteLine("Checking permissions...");
  Thread.Sleep(2000);

  if (!PermissionManager.CheckPermission())
  {
    Console.WriteLine("Access denied! Your machine is not authorized to run this application, please contact to Admin !");
    return;
  }

  Console.WriteLine("Access granted!");
  RunTask.Execute("10");

  //if (args.Length > 0)
  //{
  //    string choice = args[0];
  //    RunTask.Execute(choice);
  //}
}
catch (Exception ex)
{
  Console.WriteLine("An error occurred: " + ex.Message);
}
finally
{
  Console.WriteLine("Press any key to exit...");
  Console.ReadKey();  // Đảm bảo ứng dụng không đóng ngay lập tức
}
