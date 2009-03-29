#!/bin/bash

mono Prebuild/Prebuild.exe /target nant /file AgateLib.xml 
mono Prebuild/Prebuild.exe /target nant /file Tests.xml
mono Prebuild/Prebuild.exe /target nant /file AgateTools.xml

# This environment variable allows mono's resgen
# to use backslashes as path separators, making it
# compatible with resx files generated with Visual Studio.
export MONO_IOMAP=all

nant -buildfile:AgateLib.build
nant -buildfile:Tests.build
nant -buildfile:AgateTools.build

