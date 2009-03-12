!include MUI2.nsh
!include Sections.nsh

!define POLVersion 097
!define CopyMulOption 1

;!define DebugVersion

;--------------------------------
;The name of the installer
Name "POL ${POLVersion} Distro"

;The file to create
OutFile "distro${POLVersion}.exe"

;Default installation dir
InstallDir "$PROGRAMFILES\POL Distro ${POLVersion}"

BrandingText "POL Distro Installer - POL Team"

;--------------------

VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "POL ${POLVersion} Distro"
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "POL"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" ""

VIProductVersion "0.0.0.1"

;--------------------

!define MUI_ABORTWARNING


; Variables ---------
Var StartMenuFolder
Var MULFOLDER

;------------ PAGES -----------------------------------------------

!define MUI_WELCOMEPAGE_TITLE "POL Distro Installer"
!define MUI_WELCOMEPAGE_TEXT "This is the installer for POL Distro. \
This program will install everything you need to get POL running."
!insertmacro MUI_PAGE_WELCOME

!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY

;---- MUL Copy Page
!if ${CopyMulOption}
	!define MUI_PAGE_CUSTOMFUNCTION_PRE CopyMulPRE
	!define MUI_PAGE_CUSTOMFUNCTION_SHOW CopyMulSHOW
	!define MUI_PAGE_HEADER_TEXT "Choose The .mul Files Folder"
	!define MUI_PAGE_HEADER_SUBTEXT "Select from where you want to copy the .mul files"
	!define MUI_DIRECTORYPAGE_VARIABLE $MulFolder
	!define MUI_DIRECTORYPAGE_TEXT_TOP "Define the folder where the following files are located:$\n\
statics0.mul$\n\
statidx0.mul$\n\
map0.mul$\n\
..."
	!define MUI_DIRECTORYPAGE_TEXT_DESTINATION "Folder where the MUL files are located"
	!define MUI_DIRECTORYPAGE_VERIFYONLEAVE
	!insertmacro MUI_PAGE_DIRECTORY
!endif
;----------------

!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\POL ${POLVersion}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"

!insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder

!insertmacro MUI_PAGE_INSTFILES

!define MUI_FINISHPAGE_SHOWREADME $INSTDIR\readme.txt
!define MUI_FINISHPAGE_RUN $INSTDIR\StartHere.bat
!define MUI_FINISHPAGE_RUN_TEXT "Run POL Console (StartHere.bat)" ; Later change it to POL Launch
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"


;------------- SECTIONS -----------
SectionGroup "POL Distro" poldistro
	Section "Distro Files" distro
		SetOutPath $INSTDIR
	!ifndef DebugVersion
		File /r ${POLVersion}\Distro\*
	!endif
	SectionEnd

	Section "POL Binary" polbin
		SetOutPath $INSTDIR
	!ifndef DebugVersion
		File /r ${POLVersion}\release\*
	!endif
	SectionEnd
	
	Section ""
		WriteUninstaller $INSTDIR\Uninstall.exe
		
	!insertmacro MUI_STARTMENU_WRITE_BEGIN Application
		CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
		CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
		CreateShortCut "$SMPROGRAMS\$StartMenuFolder\POL Console.lnk" "$INSTDIR\StartHere.bat"
		CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Readme.lnk" "$INSTDIR\readme.txt"
	!insertmacro MUI_STARTMENU_WRITE_END
	SectionEnd
SectionGroupEnd

!if ${CopyMulOption}
	Section "Copy MUL files" copymul
		Call CopyMulFiles
	SectionEnd
!endif

;---------------------------------------
; Descriptions

	LangString DESC_distro ${LANG_ENGLISH} "Distro files (default scripts for POL)"
	LangString DESC_polbin ${LANG_ENGLISH} "POL binary and modules"
	LangString DESC_poldistro ${LANG_ENGLISH} "All that is needed to start POL using Distro"
	
	!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
		!insertmacro MUI_DESCRIPTION_TEXT ${poldistro} $(DESC_poldistro)
		!insertmacro MUI_DESCRIPTION_TEXT ${distro} $(DESC_distro)
		!insertmacro MUI_DESCRIPTION_TEXT ${polbin} $(DESC_polbin)
	!insertmacro MUI_FUNCTION_DESCRIPTION_END
;---------

; TODO: This should be more intelligent
Section "Uninstall"
	Delete $INSTDIR\Uninstall.exe
	RMDir /r $INSTDIR
	
	!insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
	
	Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
	Delete "$SMPROGRAMS\$StartMenuFolder\Start POL.lnk"
	Delete "$SMPROGRAMS\$StartMenuFolder\Readme.lnk"
	RMDir "$SMPROGRAMS\$StartMenuFolder"
	
	DeleteRegKey /ifempty HKCU "Software\POL ${POLVersion}"
SectionEnd

Function CopyMulFiles
	CreateDirectory $INSTDIR\MUL

	CopyFiles /SILENT "$MULFOLDER\statics*.mul" "$INSTDIR\MUL"
	CopyFiles /SILENT "$MULFOLDER\statidx*.mul" "$INSTDIR\MUL"
	CopyFiles /SILENT "$MULFOLDER\stadif*.mul" "$INSTDIR\MUL"
	
	CopyFiles /SILENT "$MULFOLDER\multi.idx" "$INSTDIR\MUL"
	CopyFiles /SILENT "$MULFOLDER\multi.mul" "$INSTDIR\MUL"
	
	CopyFiles /SILENT "$MULFOLDER\tiledata.mul" "$INSTDIR\MUL"
	
	CopyFiles /SILENT "$MULFOLDER\map*.mul" "$INSTDIR\MUL"

	CopyFiles /SILENT "$MULFOLDER\verdata.mul" "$INSTDIR\MUL"
FunctionEnd

Function CopyMulSHOW
	;StrCpy $MULFOLDER 
	SendMessage $mui.DirectoryPage.Directory ${WM_SETTEXT} 0 "STR:$MULFOLDER"
FunctionEnd

Function CopyMulPRE
	SectionGetFlags ${copymul} $R0
	IntOp $R0 $R0 & ${SF_SELECTED}
	IntCmp $R0 ${SF_SELECTED} show
	
	Abort
	
	show:
FunctionEnd