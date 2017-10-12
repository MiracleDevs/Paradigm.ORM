#!/bin/bash

C_BLACK='\033[0;30m'
C_RED='\033[0;31m'
C_GREEN='\033[0;32m'
C_ORANGE='\033[0;33m'
C_BLUE='\033[0;34m'
C_PURPLE='\033[0;35m'
C_CYAN='\033[0;36m'
C_LIGHT_GRAY='\033[0;37m'
C_DARK_GRAY='\033[1;30m'
C_LIGHT_RED='\033[1;31m'
C_LIGHT_GREEN='\033[1;32m'
C_YELLOW='\033[1;33m'
C_LIGHT_BLUE='\033[1;34m'
C_LIGHT_PURPLE='\033[1;35m'
C_LIGHT_CYAN='\033[1;36m'
C_WHITE='\033[1;37m'
C_TRANSPARENT='\033[0m'

function block()
{
	color=$2
	if [[ $color == "" ]]; then color="$C_GREEN"; fi

	echo -e "$color------------------------------------------------------------------------------------------"
	echo -e "$color $1"
	echo -e "$color------------------------------------------------------------------------------------------"
}

function error()
{
	echo -e "$C_RED ERROR: $1"
}

function buildSuccessfully()
{
	block "Process COMPLETED"
	exit 0
}

function buildFailed()
{
	block "Process FAILED" $C_RED
	exit 1
}


function execute()
{
	text=$2
	if [[ $text == "" ]]; then text="$1"; fi

	echo -e ${C_DARK_GRAY} $text ${C_TRANSPARENT}
	$1
	if [[ $? -ne 0 ]]; then buildFailed; fi
}