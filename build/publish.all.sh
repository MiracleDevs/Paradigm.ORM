#!/bin/bash
. _include.sh

#------------------------------------------------
# INIT VARS
#------------------------------------------------
index=$1
noclsArg=$2
win86="win-x86"
win64="win-x64"
linux="linux-x64"
osx="osx-x64"

if [[ "$index" == "" ]]; then index="1"; fi
if [[ "$noclsArg" == "" ]]; then clear; fi

block "$index - PUBLISH ALL TOOLS"

#------------------------------------------------
# BUILD SOLUTION
#------------------------------------------------
execute "bash ./build.solution.sh $index.1 nocls"

#------------------------------------------------
# PUBLISH ALL TOOLS
#------------------------------------------------
execute "bash ./publish.tool.sh dbfirst ../src/Paradigm.ORM.DbFirst/Paradigm.ORM.DbFirst.csproj $index.2 nocls"
execute "bash ./publish.tool.sh dbpublisher ../src/Paradigm.ORM.DbPublisher/Paradigm.ORM.DbPublisher.csproj $index.3 nocls"
execute "bash ./publish.tool.sh dataexport  ../src/Paradigm.ORM.DataExport/Paradigm.ORM.DataExport.csproj $index.4 nocls"

