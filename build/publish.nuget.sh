#!/bin/bash
. _include.sh

#------------------------------------------------
# INIT VARS
#------------------------------------------------
nugetKey=$1
index=$2
noclsArg=$3
nugetDir=../.nuget/
nugetExt=*.nupkg
nugetUrl=https://www.nuget.org/api/v2/package

if [[ "$nugetKey" == "" ]]; then error "Missing nuget key." ; buildFailed ; fi
if [[ "$index" == "" ]]; 	then index="1"; fi
if [[ "$noclsArg" == "" ]]; then clear; fi

block "$index - Publish Nuget Packages"

#------------------------------------------------
# BUILD NUGET PACKAGES
#------------------------------------------------
execute "bash ./build.nuget.sh $index.1 nocls"

#------------------------------------------------
# PUBLISHING NUGET PACKAGES
#------------------------------------------------
for f in $(find $nugetDir -type f -name "$nugetExt"); do
	echo "PUBLISHING: $f"
	./nuget.exe push $f $nugetKey -source $nugetUrl
	echo ""
done