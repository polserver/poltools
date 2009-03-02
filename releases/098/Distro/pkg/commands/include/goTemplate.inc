/* $Id$
 *
 */


function Go_CheckInit(who)

	if ( !go_prop_base )
		SendSysMessage(who, "Creating initial go list dictionary...");
		var oldlocs := ReadConfigFile(":commands:golocs");
		if ( oldlocs.errortext )
			return 0;
		endif
		go_prop_base := dictionary;
		foreach key in ( GetConfigStringKeys(oldlocs) )
			// Open files and set veriables
			var dest := oldlocs[key];
			Go_SetLocation("base_Distro", who, dest.name, dest.realm, CInt(dest.x), CInt(dest.y), CInt(dest.z));
			SleepMS(2);
		endforeach

		go_prop_base := go_elem.GetProp("base_Distro");
		go_keys_base := go_prop_base.Keys();
		Go_RefreshGump("LocationIndex", "base_Distro");
	endif

	return 0;

endfunction

function Go_RefreshMyGumps(who)

	var elem := DFFindElement(go_data_gumps, "LocationIndex");
	elem.EraseProp("base_Distro");
	elem.EraseProp(who.acctname);

	elem := DFFindElement(go_data_gumps, "CopyIndex");
	elem.EraseProp("base_Distro");
	elem.EraseProp(who.acctname);

	return 1;

endfunction