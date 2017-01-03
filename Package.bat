
nuget pack AgateLib/AgateLib.csproj
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.AgateSDL/AgateLib.SDL.csproj
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.OpenGL/AgateLib.OpenGL.csproj
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.Platform.WinForms/AgateLib.Platform.WinForms.csproj
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.Platform.Test/AgateLib.Platform.Test.csproj
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.Platform.IntegrationTest/AgateLib.Platform.IntegrationTest.csproj
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
