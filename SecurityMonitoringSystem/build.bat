@echo off
echo Building SecurityMonitoringSystem...
"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" SecurityMonitoringSystem.csproj /t:Build /p:Configuration=Debug /v:m
pause
