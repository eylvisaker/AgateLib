#!/bin/bash

mono Prebuild/Prebuild.exe /target vs2008 /file AgateLib.xml 
mono Prebuild/Prebuild.exe /target vs2008 /file AgateTools.xml
mono Prebuild/Prebuild.exe /target vs2008 /file Tests.xml
