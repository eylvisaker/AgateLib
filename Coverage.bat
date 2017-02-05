OpenCover.Console.exe "-target:VSTest.Console.exe" -targetargs:AgateLib.Tests.UnitTests\bin\Debug\AgateLib.UnitTests.dll -excludebyfile:*\*.Designer.cs -output:Coverage.xml -register:user -returntargetcode
@if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
