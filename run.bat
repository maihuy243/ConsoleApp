@echo off
cls
echo ==========================
echo MyApp - Permission Checker
echo ==========================
echo 1. Encrypt Permission
echo 2. Login Facebook
echo 3. Exit
set /p choice="Choose an option: "

if "%choice%"=="1" (
    echo Running MyApp.exe...
    start "" "D:\Code\CodeByMSH\MyApps\bin\Debug\net9.0-windows\MyApps.exe" encrypt_uuid
)else if "%choice%"=="2" (
    echo Running MyApp.exe...
    start "" "D:\Code\CodeByMSH\MyApps\bin\Debug\net9.0-windows\MyApps.exe" login_facebook
) else (
    echo Exiting...
)
