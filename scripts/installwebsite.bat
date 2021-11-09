%SystemRoot%\sysnative\WindowsPowerShell\v1.0\powershell.exe -command "Set-ExecutionPolicy Unrestricted -Force"
IF EXIST C:\webApps\MVCWebApplication rmdir C:\webApps\CW1-WebAppUI-8408
mkdir C:\webApps\CW1-WebAppUI-8408

cd c:\temp

%SystemRoot%\sysnative\WindowsPowerShell\v1.0\powershell.exe -command ".\installwebsite.ps1"