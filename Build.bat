set sln=AgateLib-Desktop.sln

nuget restore %sln%
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

msbuild %sln% /T:rebuild /P:Configuration=%1
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
