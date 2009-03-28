#!/bin/bash

mono Prebuild/Prebuild.exe /target monodev /file AgateLib.xml 
mono Prebuild/Prebuild.exe /target monodev /file AgateTools.xml

mono Prebuild/Prebuild.exe /target monodev /file AllTests.xml
mono Prebuild/Prebuild.exe /target monodev /file DisplayTests.xml
mono Prebuild/Prebuild.exe /target monodev /file AudioTests.xml
mono Prebuild/Prebuild.exe /target monodev /file CoreTests.xml
mono Prebuild/Prebuild.exe /target monodev /file InputTests.xml
mono Prebuild/Prebuild.exe /target monodev /file FontTests.xml
mono Prebuild/Prebuild.exe /target monodev /file GuiTests.xml

