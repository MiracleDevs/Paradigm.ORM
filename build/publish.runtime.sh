#!/bin/bash
. _include.sh

#------------------------------------------------
# INIT VARS
#------------------------------------------------

tool=$1
toolProject=$2
runtime=$3
index=$4
noclsArg=$5
deployDir=../.deploy/$tool
distDir=../dist
ubuntu16=ubuntu.16.04-x64
zipFile="$tool.$runtime.zip"

if [[ "$tool" == "" ]]; then error "Missing tool name." ; buildFailed ; fi
if [[ "$toolProject" == "" ]]; then error "Missing tool path." ; buildFailed ; fi
if [[ "$runtime" == "" ]]; then error "Missing runtime version." ; buildFailed ; fi
if [[ "$index" == "" ]]; 	then index="1";fi
if [[ "$noclsArg" == "" ]]; then clear;fi
if [[ "$runtime" == "" ]]; 	then buildFailed;fi
if [ ! -d "$distDir" ]; 	then mkdir $distDir;fi

block "$index - PUBLISH AND ZIP $tool FOR $C_CYAN $runtime $C_TRANSPARENT"

execute "rm -rf $deployDir/$runtime/"

execute "dotnet publish $toolProject -c Release -r $runtime -o ../$deployDir/$runtime/ -v q"

execute "rm -rf $distDir/$zipFile"

pushd "$deployDir/$runtime/"
execute "zip -q ../../$distDir/$zipFile ./*" "zip $distDir/$zipFile"
popd

buildSuccessfully