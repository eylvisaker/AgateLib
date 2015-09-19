"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\MSTest.exe" -targetargs:/testcontainer:AgateLib.Tests\UnitTests\bin\Debug\AgateLib.UnitTests.dll
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
