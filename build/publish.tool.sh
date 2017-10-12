#!/bin/bash
. _include.sh

#------------------------------------------------
# INIT VARS
#------------------------------------------------
tool=$1
toolProject=$2
index=$3
noclsArg=$4
win86="win-x86"
win64="win-x64"
linux="linux-x64"
osx="osx-x64"

if [[ "$tool" == "" ]]; then error "Missing tool name." ; buildFailed ; fi
if [[ "$toolProject" == "" ]]; then error "Missing tool path." ; buildFailed ; fi
if [[ "$index" == "" ]]; then index="1"; fi
if [[ "$noclsArg" == "" ]]; then clear; fi

block "$index - PUBLISH $C_CYAN $tool"

#------------------------------------------------
# PUBLISH TOOL
#------------------------------------------------
execute "bash ./publish.runtime.sh $tool $toolProject $win86 $index.1 nocls"
execute "bash ./publish.runtime.sh $tool $toolProject $win64 $index.2 nocls"
execute "bash ./publish.runtime.sh $tool $toolProject $linux $index.3 nocls"
execute "bash ./publish.runtime.sh $tool $toolProject $osx $index.4 nocls"

buildSuccessfully