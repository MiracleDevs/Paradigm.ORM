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

block "$index - Copy NuGet Packages"

#------------------------------------------------
# BUILD SOLUTION
#------------------------------------------------
execute "find $srcDir -name $nugetExt -exec echo - Copying {} ; -exec cp {} $nugetDir ; -exec echo - File Copied ;"

buildSuccessfully
