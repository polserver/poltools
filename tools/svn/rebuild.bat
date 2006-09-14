@ECHO OFF

VER | FIND "2003" > NUL
IF %ERRORLEVEL% == 0 GOTO VER_XP-2003

VER | FIND "XP" > NUL
IF %ERRORLEVEL% == 0 GOTO VER_XP-2003

VER | FIND "2000" > NUL
IF %ERRORLEVEL% == 0 GOTO VER_2000

ECHO Machine undetermined.
GOTO exit

:VER_XP-2003
:Run Windows XP-specIFic commands here.
ECHO Windows XP or 2003
REM - taskkill /F /IM pol* /T
GOTO exit

:VER_2000
:Run Windows 2000-specIFic commands here.
ECHO Windows 2000
REM - pskill pol
GOTO exit

:exit
PAUSE