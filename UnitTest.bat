MSTest.exe /testcontainer:AgateLib.Tests\UnitTests\bin\Debug\AgateLib.UnitTests.dll
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
