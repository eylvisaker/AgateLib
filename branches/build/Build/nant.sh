#!/bin/bash

mono Prebuild/Prebuild.exe /target nant /file AgateLib.xml 
mono Prebuild/Prebuild.exe /target nant /file AgateTests.xml
mono Prebuild/Prebuild.exe /target nant /file AgateTools.xml

nant -buildfile:AgateLib.build
nant -buildfile:AgateTests.build
nant -buildfile:AgateTools.build

