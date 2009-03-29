@rem Generates a solution (.sln) and a set of project files (.csproj, .vbproj, etc.)
@rem for Microsoft Visual Studio .NET 2008

Prebuild\Prebuild.exe /target vs2008 /file AgateLib.xml 
Prebuild\Prebuild.exe /target vs2008 /file AgateLib-Windows.xml 
Prebuild\Prebuild.exe /target vs2008 /file AgateTools.xml

Prebuild\Prebuild.exe /target vs2008 /file Tests.xml

@pause
