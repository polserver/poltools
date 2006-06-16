@ECHO OFF

REM - $Id: LoopPOL.bat 182 2006-06-02 04:51:36Z austinheilman $

REM -- If a special path is needed to pol.exe set it here
SET POL_PATH=pol.exe
REM ----------

GOTO :START()

REM -- START() FUNCTION
:START()
CLS
ECHO loopPOL.bat (v 1.0) by Austin
ECHO =============================
ECHO   Starting %POL_PATH%
ECHO =============================
ECHO.

%POL_PATH%

ECHO.
ECHO.
ECHO =============================
ECHO   POL has finished running
eCHO =============================
ECHO.

GOTO :START()
