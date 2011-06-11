/*
        Creatred by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2010-03-29
*/
use uo;
use os;
use cfgfile;

function SPM_GetSettingsCfgFile()

	var cfg := ReadConfigFile( ":spawnpoints:settings" );
	if( cfg.errortext )
		SysLog( "Error::SPM_GetSettingsCfgFile() - Unable to open [:spawnpoints:settings] ->"+cfg.errortext );
                return 0;
	endif

	return cfg;
endfunction

function SPM_GetSettingsCfgElem( elem_name, byref cfg_file:=0 )

	if( !cfg_file )
		cfg_file := SPM_GetSettingsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		SysLog( "Error::SPM_GetSettingsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext );
                return 0;
	endif

	return elem;
endfunction

function SPM_GetDataFile()

        var datafile := DFOpenDataFile( ":spawnpoints:spawnpoints", DF_CREATE );
        if( datafile.errortext )
                SysLog( "Error reading datafile <:spawnpoints:spawnpoints> -->"+datafile.errortext );
                return 0;
        endif
        
        return datafile;
endfunction

function GetSpawnpointSettings( spawnpoint )

	var sp_settings := GetObjProperty( spawnpoint, "Settings" );
	if( !sp_settings )
		sp_settings := array{};

		sp_settings[1] := "NPC"; // Type
		sp_settings[2] := "nil"; // Template
		sp_settings[3] := 1; // Max appear
		sp_settings[4] := 8; // Appear Range
		sp_settings[5] := 0; // Wander Range
		sp_settings[6] := 120; // Frequency
		sp_settings[7] := 0; // Disabled
		sp_settings[8] := "no notes"; // Notes
		SetObjProperty( spawnpoint, "Settings", sp_settings );
	endif

        return sp_settings;
endfunction
