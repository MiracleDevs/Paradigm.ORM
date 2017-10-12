#!/bin/bash
. _include.sh

#------------------------------------------------
# INIT VARS
#------------------------------------------------
index=$1
noclsArg=$2
srcDir=../src/
nugetDir=../.nuget/
nugetExt=*.nupkg

if [[ "$noclsArg" == "" ]]; then clear; fi
if [[ "$index" == "" ]]; 	then index="1"; fi
if [ ! -d "$nugetDir" ]; 	then mkdir $nugetDir; fi

block "$index - Remove Old NuGet Packages"

#------------------------------------------------
# REMOVE OLD MESSAGES
#------------------------------------------------
execute "find $srcDir -name $nugetExt -exec echo - Removing {} ; -exec rm -rf {} ; -exec echo - File Removed ;"
execute "find $nugetDir -name $nugetExt -exec echo - Removing {} ; -exec rm -rf {} ; -exec echo - File Removed ;"

buildSuccessfully