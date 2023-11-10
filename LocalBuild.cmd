@echo off
set argDestination=%1
if "%1" == "" set argDestination=PublishedExample

echo Destination is %argDestination%
echo ...
echo Beginning publish
echo ...
dotnet publish CompressedStaticFiles.Example/CompressedStaticFiles.Example.csproj --configuration Debug --output %argDestination%
echo ...
echo Build results
echo ...
dir "%argDestination%" /s
echo ...
echo Executing published example
echo ...
cd %argDestination%
compressedstaticfiles.example.exe"
cd ..
rd %argDestination% /s /q
pause
