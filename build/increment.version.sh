#!/bin/bash
. _include.sh

#------------------------------------------------
# INIT VARS
#------------------------------------------------
oldVersion=$1
newVersion=$2

if [[ "$oldVersion" == "" ]]; then error "Missing old version number." ; buildFailed ; fi
if [[ "$newVersion" == "" ]]; then error "Missing new version number." ; buildFailed ; fi

findVersion="<Version>$oldVersion<\/Version>"
replaceVersion="<Version>$newVersion<\/Version>"

block "$index - Incrementing solution version from $oldVersion to $newVersion"
execute "sed -i -e s/$findVersion/$replaceVersion/g ../src/**/*.csproj"