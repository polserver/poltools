@ECHO OFF

REM - $Id$
REM - Designed to be used with scheduled task for automated
REM - maintenance on the distro server.
REM - EXAMPLE:
REM - Every friday its set to do:
REM -  automate.bat svnupdate recompile datawipe svnupdate killpol
REM - Everyday at 7am it does
REM -  automate.bat svnupdate recompile

REM -- CONFIGURE GENERAL PATHS BELOW --
SET SVN_BIN_PATH=D:\Tortoise\SVN\bin
SET SVN_UPDATE_PATH=E:\UOL\Distros
SET POL_PATH=E:\UOL\Distros\releases\097\Distro
REM -----------------------------------
REM -- CONFIGURE PATH BELOW IF USING WIN 2000
REM -- http://www.sysinternals.com/Utilities/PsTools.html
SET PS_TOOLS_PATH=D:\PsTools
REM -----------------------------------

ECHO.
IF NOT "%1" == "" GOTO :ARGS_PARSE();
GOTO :SHOW_SYNTAX()

REM -- SHOW_SYNTAX() FUNCTION
:SHOW_SYNTAX()
ECHO Syntax (Can use one or more of the following in any order)
ECHO     SVNUPDATE  - Will perform an SVN file update.
ECHO     RECOMPILE  - Will recompile all POL scripts.
ECHO     DATAWIPE   - Will remove the POL data directory.
ECHO     KILLPOL    - Will terminate the POL process.
GOTO QUIT();

REM -- ARGS_PARSE() FUNCTION
:ARGS_PARSE()
SET CMD=%1
SHIFT /1
IF /i "%CMD%" == "" GOTO :QUIT()
IF /i "%CMD%" == "SVNUPDATE" GOTO :SVN_UPDATE()
IF /i "%CMD%" == "RECOMPILE" GOTO :RECOMPILE_SCRIPTS()
IF /i "%CMD%" == "DATAWIPE" GOTO :DATAWIPE()
IF /i "%CMD%" == "KILLPOL" GOTO :KILLPOL()
ECHO WARNING: INVALID COMMAND "%CMD%"
GOTO :ARGS_PARSE()

REM -- SVN_UPDATE() FUNCTION
:SVN_UPDATE()
%SVN_BIN_PATH%\TortoiseProc.exe /command:update /path:"%SVN_UPDATE_PATH%" /notempfile /closeonend:1
GOTO :ARGS_PARSE()

REM -- RECOMPILE_SCRIPTS() FUNCTION
:RECOMPILE_SCRIPTS()
START "RECOMPILE" /D%POL_PATH% /WAIT %POL_PATH%\scripts\ecompile.exe -F
GOTO :ARGS_PARSE()

REM -- DATAWIPE() FUNCTION
:DATAWIPE()
RD %POL_PATH%\data\ /S /Q
GOTO :ARGS_PARSE()

REM -- KILLPOL() FUNCTION
:KILLPOL()
VER | FIND "2003" > NUL
IF %ERRORLEVEL% == 0 taskkill /F /IM pol.exe /T

VER | FIND "XP" > NUL
IF %ERRORLEVEL% == 0 taskkill /F /IM pol.exe /T

VER | FIND "2000" > NUL
IF %ERRORLEVEL% == 0 %PS_TOOLS_PATH%/PSKill.exe pol

GOTO :ARGS_PARSE()

REM -- QUIT() FUNCTION
:QUIT()
ECHO.
ECHO.

