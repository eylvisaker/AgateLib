OpenCover.Console.exe "-target:MSTest.exe" -targetargs:/testcontainer:AgateLib.Tests\UnitTests\bin\Debug\AgateLib.UnitTests.dll -targetargs:/noisolation -excludebyfile:*\*.Designer.cs -output:Coverage.xml -register:user
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
