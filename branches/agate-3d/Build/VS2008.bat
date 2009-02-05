@rem Generates a solution (.sln) and a set of project files (.csproj, .vbproj, etc.)
@rem for Microsoft Visual Studio .NET 2008

Prebuild\Prebuild.exe /target vs2008 /file AgateLib.xml 
Prebuild\Prebuild.exe /target vs2008 /file AgateLib-Windows.xml 
Prebuild\Prebuild.exe /target vs2008 /file AgateTools.xml

Prebuild\Prebuild.exe /target vs2008 /file AllTests.xml
Prebuild\Prebuild.exe /target vs2008 /file DisplayTests.xml
Prebuild\Prebuild.exe /target vs2008 /file Display3DTests.xml
Prebuild\Prebuild.exe /target vs2008 /file AudioTests.xml
Prebuild\Prebuild.exe /target vs2008 /file CoreTests.xml
Prebuild\Prebuild.exe /target vs2008 /file InputTests.xml
Prebuild\Prebuild.exe /target vs2008 /file FontTests.xml

@pause
