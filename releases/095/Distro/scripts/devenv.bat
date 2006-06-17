@echo off
REM Environment variables to make life easier when writing scripts

REM Change this to where your POL directory really is
set POLROOT=c:\pol

set PATH=%PATH%;%POLROOT%\scripts

set ECOMPILE_PATH_EM=%POLROOT%\scripts
set ECOMPILE_PATH_INC=%POLROOT%\scripts

echo POL development environment set to %POLROOT%
