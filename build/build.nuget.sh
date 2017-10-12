#!/bin/bash
. _include.sh

#------------------------------------------------
# INIT VARS
#------------------------------------------------
index=$1
noclsArg=$2

if [[ "$noclsArg" == "" ]]; then clear; fi
if [[ "$index"    == "" ]]; then index="1"; fi

block "$index - Build Solution"

#------------------------------------------------
# CLEAR NUGET PACKAGES
#------------------------------------------------
execute "bash ./clear.nuget.sh $index.1 nocls"

#------------------------------------------------
# BUILD SOLUTION
#------------------------------------------------
execute "bash ./build.solution.sh $index.2 nocls"

#------------------------------------------------
# COPY NEW NUGETS
#------------------------------------------------
execute "bash ./copy.nuget.sh $index.3 nocls"

buildSuccessfully