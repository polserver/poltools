/*
 * $Id$
 *
 */

use uo;
use cfgfile;
use os;

/*
 * IDS_StartIdent(mobile, id_name, item)
 *
 *
 */
function IDS_StartIdent(mobile, id_name, item)

	var info := GetObjProperty( item, "ID" );

	if ( TypeOf(info) == "String" )
		var cfg := ReadConfigFile(":idscripts:config/idScripts");
		var elem := FindConfigElem(cfg, info);
		if ( elem )
			StartScript(elem.Script, item);
		endif
	endif

	return 1;
endfunction
