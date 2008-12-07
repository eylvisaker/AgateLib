#!/bin/bash

mono Prebuild/Prebuild.exe /target nant /file AgateLib.xml 
mono Prebuild/Prebuild.exe /target nant /file AgateTests.xml
mono Prebuild/Prebuild.exe /target nant /file AgateTools.xml

# This environment variable allows mono's resgen
# to use backslashes as path separators, making it
# compatible with resx files generated with Visual Studio.
export MONO_IOMAP=all

nant -buildfile:AgateLib.build
nant -buildfile:AgateTests.build
nant -buildfile:AgateTools.build

nant --version

echo ""
echo "**********************************************************************"
echo "NAnt version 0.85 appears to have a bug when it comes to generating"
echo "resource files from resx files.  If you have NAnt version 0.85, either"
echo "upgrade to 0.86 or manually run resgen on affected resx files."

