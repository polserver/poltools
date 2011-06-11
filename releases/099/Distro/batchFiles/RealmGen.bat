@ECHO OFF

REM - $Id: RealmGen.bat 916 2006-12-17 17:29:41Z austinheilman $

REM -- If a special path is needed to uoconvert.exe set it here
SET UOCNVRT_PATH=uoconvert.exe
REM ----------
SET BUILD_ALL=0

GOTO :MENU()

REM -- MENU FUNCTION
:MENU()
CLS
ECHO RealmGen.bat (v 1.0) by Austin
ECHO ==============================
ECHO Command        Purpose
ECHO  [ a ] - Build multis.cfg
ECHO  [ b ] - Build tiles.cfg
ECHO  [ c ] - Build landtiles.cfg
ECHO  [ d ] - Build all config files
ECHO.
ECHO  [ e ] - Build 'Britannia' realm     (mapid=0)
ECHO  [ f ] - Build 'Britannia Alt' realm (mapid=1)
ECHO  [ g ] - Build 'Ilshenar' realm      (mapid=2)
ECHO  [ h ] - Build 'Malas' realm         (mapid=3)
ECHO  [ i ] - Build 'Tokuno' realm        (mapid=4)
ECHO  [ j ] - Build all realms - Takes a very long time!
ECHO.
ECHO  [ o ] - Copy needed client files to pol\MUL\
ECHO.
ECHO  [ x ] - Back

SET /p CMD=Command:

IF /i "%CMD%" == "a" GOTO :MULTIS.CFG()
IF /i "%cMD%" == "b" GOTO :TILES.CFG()
IF /i "%cMD%" == "c" GOTO :LANDTILES.CFG()
IF /i "%cMD%" == "d" GOTO :ALLCONFIGS()
IF /i "%CMD%" == "e" GOTO :REALM_BRITANNIA()
IF /i "%CMD%" == "f" GOTO :REALM_BRITANNIA_ALT()
IF /i "%CMD%" == "g" GOTO :REALM_ILSHENAR()
IF /i "%CMD%" == "h" GOTO :REALM_MALAS()
IF /i "%CMD%" == "i" GOTO :REALM_TOKUNO()
IF /i "%CMD%" == "j" GOTO :BUILD_ALL_REALMS()
IF /i "%CMD%" == "o" GOTO :COPY_CLIENT_FILES()

IF "%CMD%" == "x" GOTO :QUIT()

ECHO.
ECHO Invalid command.
GOTO :RETURN_TO_MENU()

REM -- RETURN_TO_MENU() FUNCTION
:RETURN_TO_MENU()
PAUSE
GOTO :MENU()

REM -- MULTIS.CFG FUNCTION
:MULTIS.CFG()
%UOCNVRT_PATH% multis
MOVE multis.cfg config
IF %BUILD_ALL% == 1 GOTO :TILES.CFG()
GOTO :RETURN_TO_MENU()

REM -- TILES.CFG FUNCTION
:TILES.CFG()
%UOCNVRT_PATH% tiles
MOVE tiles.cfg config
IF %BUILD_ALL% == 1 GOTO :LANDTILES.CFG()
GOTO :RETURN_TO_MENU()

REM -- LANDTILES.CFG FUNCTION
:LANDTILES.CFG()
%UOCNVRT_PATH% landtiles
MOVE landtiles.cfg config
IF %BUILD_ALL% == 1 SET BUILD_ALL=0
GOTO :RETURN_TO_MENU()

REM -- ALLCONFIGS FUNCTION
:ALLCONFIGS()
SET BUILD_ALL=1
GOTO :MULTIS.CFG()

REM -- REALM_BRITANNIA()
:REALM_BRITANNIA()
ECHO Command        Purpose
ECHO  [ a ] - T2A - UOSE Britannia Map
ECHO  [ b ] - Mondain's Legacy extended Britannia Map
SET /p CMD=Command:
IF /i NOT "%CMD%" == "b" %UOCNVRT_PATH% map realm=britannia mapid=0 usedif=1 width=6144 height=4096
IF /i "%CMD%" == "b" %UOCNVRT_PATH% map realm=britannia mapid=0 usedif=1 width=7168 height=4096
%UOCNVRT_PATH% statics realm=britannia
%UOCNVRT_PATH% maptile realm=britannia
IF %BUILD_ALL% == 1 GOTO :REALM_BRITANNIA_ALT()
GOTO :RETURN_TO_MENU()

REM -- REALM_BRITANNIA_ALT() FUNCTION
:REALM_BRITANNIA_ALT()
%UOCNVRT_PATH% map     realm=britannia_alt mapid=1 usedif=1 width=6144 height=4096
%UOCNVRT_PATH% statics realm=britannia_alt
%UOCNVRT_PATH% maptile realm=britannia_alt
IF %BUILD_ALL% == 1  GOTO :REALM_ILSHENAR()
GOTO :RETURN_TO_MENU()

REM -- REALM_ILSHENAR FUNCTION
:REALM_ILSHENAR()
%UOCNVRT_PATH% map     realm=ilshenar mapid=2 usedif=1 width=2304 height=1600
%UOCNVRT_PATH% statics realm=ilshenar
%UOCNVRT_PATH% maptile realm=ilshenar
IF %BUILD_ALL% == 1 GOTO :REALM_MALAS()
GOTO :RETURN_TO_MENU()

REM -- REALM_MALAS FUNCTION
:REALM_MALAS()
%UOCNVRT_PATH% map     realm=malas mapid=3 usedif=1 width=2560 height=2048
%UOCNVRT_PATH% statics realm=malas
%UOCNVRT_PATH% maptile realm=malas
IF %BUILD_ALL% == 1 GOTO :REALM_TOKUNO()
GOTO :RETURN_TO_MENU()

REM -- REALM_TOKUNO FUNCTION
:REALM_TOKUNO()
%UOCNVRT_PATH% map     realm=tokuno mapid=4 usedif=1 width=1448 height=1448
%UOCNVRT_PATH% statics realm=tokuno
%UOCNVRT_PATH% maptile realm=tokuno
IF %BUILD_ALL% == 1 SET BUILD_ALL=0
GOTO :RETURN_TO_MENU()

REM -- BUILD_ALL_REALMS() FUNCTION
:BUILD_ALL_REALMS()
SET BUILD_ALL=1
GOTO :REALM_BRITANNIA()


REM -- COPY_CLIENT_FILES() FUNCTION
:COPY_CLIENT_FILES()
SET /p UOPATH=Full path to UO directory:
IF "%UOPATH%" == "" GOTO :RETURN_TO_MENU()
IF NOT EXIST MUL\ MKDIR MUL
@ECHO ON
COPY "%UOPATH%\multi.*" "MUL\"
COPY "%UOPATH%\map*" "MUL\"
COPY "%UOPATH%\staidx*" "MUL\"
COPY "%UOPATH%\statics*" "MUL\"
COPY "%UOPATH%\tiledata.mul" "MUL\"
@ECHO OFF
GOTO :RETURN_TO_MENU()

REM -- QUIT FUNCTION
:QUIT()