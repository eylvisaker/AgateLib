set sln=AgateLib.sln

nuget restore %sln%
@if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

msbuild %sln% /t:rebuild /p:Configuration=%1 /p:Platform="Any CPU"
@if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
