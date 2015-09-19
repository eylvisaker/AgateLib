set sln=AgateLib-Desktop.sln

nuget restore %sln%
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

msbuild %sln% /T:rebuild /P:Configuration=Debug %*
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

msbuild %sln% /T:rebuild /P:Configuration=Release %*
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
