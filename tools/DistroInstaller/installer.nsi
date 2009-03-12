!include MUI2.nsh

;--------------------------------
;The name of the installer
Name "POL 097 Distro"

;The file to create
OutFile "distro097.exe"

;Default installation dir
InstallDir "$PROGRAMFILES\POL Distro"

BrandingText "POL Distro Installer - POL Team"

;--------------------

VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "POL 097 Distro"
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "POL"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "097"

VIProductVersion "0.0.0.1"

;--------------------

!define MUI_ABORTWARNING


; Variables ---------
Var StartMenuFolder

;--------------------

!define MUI_WELCOMEPAGE_TITLE "POL Distro Installer"
!define MUI_WELCOMEPAGE_TEXT "This is the installer for POL Distro. \
This program will install everything you need to get POL running."
!insertmacro MUI_PAGE_WELCOME

!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY

!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\POL"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"

!insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder

!insertmacro MUI_PAGE_INSTFILES

!define MUI_FINISHPAGE_SHOWREADME $INSTDIR\readme.txt
!insertmacro MUI_PAGE_FINISH

; TODO: Copy mul files from game dir to $INSTDIR\MULs
;Var MULFOLDER
;!define MUI_DIRECTORYPAGE_VARIABLE $MulFolder
;!define MUI_DIRECTORYPAGE_TEXT_TOP "Select the folder where the .mul files are located"
;!insertmacro MUI_PAGE_DIRECTORY


!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"


;--------------------
SectionGroup "POL Distro" poldistro
	Section "Distro Files" distro
		SetOutPath $INSTDIR
		File /r 097\Distro\*
	SectionEnd

	Section "POL Binary" polbin
		File /r 097\release\*
	SectionEnd
	
	Section ""
		WriteUninstaller $INSTDIR\Uninstall.exe
		
	!insertmacro MUI_STARTMENU_WRITE_BEGIN Application
		CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
		CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
		CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Start POL.lnk" "$INSTDIR\StartHere.bat"
		CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Readme.lnk" "$INSTDIR\readme.txt"
	!insertmacro MUI_STARTMENU_WRITE_END
	SectionEnd
SectionGroupEnd

;----------------
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
	
	DeleteRegKey /ifempty HKCU "Software\POL"	
SectionEnd