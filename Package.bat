
nuget pack AgateLib/AgateLib.csproj -IncludeReferencedProjects -properties Configuration=Release
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.Physics/AgateLib.Physics.csproj -IncludeReferencedProjects -properties Configuration=Release
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.SDL/AgateLib.SDL.csproj -IncludeReferencedProjects -properties Configuration=Release
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.OpenGL/AgateLib.OpenGL.csproj -IncludeReferencedProjects -properties Configuration=Release
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.Platform.WinForms/AgateLib.Platform.WinForms.csproj -IncludeReferencedProjects -properties Configuration=Release
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.Platform.Test/AgateLib.Platform.Test.csproj -IncludeReferencedProjects -properties Configuration=Release
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

nuget pack AgateLib.Platform.IntegrationTest/AgateLib.Platform.IntegrationTest.csproj -IncludeReferencedProjects -properties Configuration=Release
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
