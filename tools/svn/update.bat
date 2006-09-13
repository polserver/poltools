REM - $Id$
REM - THIS REQUIRES SOME CONFIGURATION TO USE.

@ECHO OFF

SET SVN_BIN_PATH=D:\Tortoise\SVN\bin
SET SVN_UPDATE_PATH=E:\UOL\Distros

@ECHO ON

%SVN_BIN_PATH%\TortoiseProc.exe /command:update /path:"%SVN_UPDATE_PATH%" /notempfile /closeonend:1
