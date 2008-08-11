@ECHO OFF

REM - $Id: StartHere.bat 841 2006-09-21 06:27:56Z austinheilman $

REM -- If a special path is needed to the batch files set it here
SET BATCH_PATH=batchFiles\
REM ----------

GOTO :MENU()

REM -- MENU FUNCTION
:MENU()
CLS
ECHO StartHere.bat (v 1.0) by Austin
ECHO ===============================
ECHO Command        Purpose
ECHO  [ a ] - RealmGen menu        (Realm building tools)
ECHO  [ b ] - Ecompiler menu       (Ecompile tools)
ECHO  [ c ] - Cleanup menu         (File removal tools)
ECHO.
ECHO  [ d ] - Start POL.exe        (Returns to menu on exit)
ECHO  [ e ] - Keep POL.exe running (Restarts when it exits. Use CTRL+C to stop)
ECHO.
ECHO  [ x ] - Quit

SET /p CMD=Command:

IF /i "%CMD%" == "a" GOTO :REALM_GEN()
IF /i "%CMD%" == "b" GOTO :ECOMPILE()
IF /i "%CMD%" == "c" GOTO :CLEANUP()
IF /i "%CMD%" == "d" GOTO :POL()
IF /i "%CMD%" == "e" GOTO :POL_LOOP()
IF /i "%CMD%" == "x" GOTO :QUIT()

ECHO.
ECHO Invalid command.
GOTO :RETURN_TO_MENU()

REM -- RETURN_TO_MENU() FUNCTION
:RETURN_TO_MENU()
GOTO :MENU()

REM -- REALM_GEN() FUNCTION
:REALM_GEN()
CALL %BATCH_PATH%RealmGen.bat
GOTO RETURN_TO_MENU()

REM -- ECOMPILE() FUNCTION
:ECOMPILE()
CALL %BATCH_PATH%Ecompile.bat
GOTO :RETURN_TO_MENU()

REM -- CLEANUP() FUNCTION
:CLEANUP()
CALL %BATCH_PATH%CleanUp.bat
GOTO :RETURN_TO_MENU()

REM -- POL() FUNCTION
:POL()
CALL %BATCH_PATH%POL.bat
GOTO :RETURN_TO_MENU()

REM -- ECOMPILE() FUNCTION
:POL_LOOP()
CALL %BATCH_PATH%LoopPOL.bat
GOTO :RETURN_TO_MENU()

REM -- QUIT FUNCTION
:QUIT()