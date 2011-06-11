use uo;
use os;
use cfgfile;
use polsys;

function IU_GetTileInfo(graphic)
	var config := ReadConfigFile("::tiles");
	if ( !config )
		return config; // Contains error
	endif

	return config[Hex(CInt(graphic))];
endfunction
