/* $Id: goText.inc 1011 2005-11-16 07:20:21Z muaddiblsd $
 *
 */


function Go_Parse_Text(who, text)
	if (!text)
		return 0;
        endif

	var params := SplitWords(text);

	case (lower(params[1]))
		"refresh": return Go_RefreshMyGumps(who);
		"xyz":     return Go_GoXYZ(who, params);
		default:   return Go_FindLocationByText(who, params);
	endcase

	return 0;
endfunction


function Go_GoXYZ(who, params)
	var num_params := len(params);

	case (num_params)
		1:
		2: SendSysMessage(who, "Error. Insufficient data supplied. \".go xyz x y [z] [string realm name]\"");
		3: params[4] := GetMapInfo( CInt(params[1]), CInt(params[2]), who.realm ).z;
		params[5] := who.realm;
		4: params[5] := who.realm;
		default:
	endcase

	if (Realms().Exists(lower(params[5])))
		var move_check := 0;
		if (who.realm == lower(params[5]))
			move_check := MoveCharacterToLocation(who, CInt(params[2]), CInt(params[3]), CInt(params[4]));
		else
			move_check := MoveObjectToRealm(who, lower(params[5]), CInt(params[2]), CInt(params[3]), CInt(params[4]));
                endif
		if( move_check == error )
			GumpPrompt(who, move_check);
			return 1;
		endif
	else
		GumpPrompt(who, "Invalid Realm name in xyz!");
		return 1;
	endif


	return 1;
endfunction


function Go_FindLocationByText(who, params)
	case (lower(params[params.size()]))
		"private": Go_ByPrivateLocation(who, params);
		"public":  Go_ByPublicLocation(who, params);
		default:   Go_ByDefaultLocation(who, params);
	endcase

	return 1;
endfunction

function Go_ByPrivateLocation(who, params)

        if(go_prop.Exists( lower(params[1]) ) )
                var temp := go_prop[lower(params[1])];
                SendSysMessage(who, "Going to private location [" + params[1] + "]");
                var move;
                if( who.realm == temp.Realm )
                        move := MoveCharacterToLocation(who, CInt(temp.X), CInt(temp.Y), CInt(temp.Z), MOVECHAR_FORCELOCATION);
                else
                        move := MoveObjectToRealm( who, temp.Realm, temp.X, temp.Y, temp.Z, MOVECHAR_FORCELOCATION );
                endif
                if ( TypeOf(move) != "Integer" )
                        GumpPrompt(who, "Error moving: X[" + temp.X +"] Y[" + temp.Y + "] Z[" + temp.Z + "]");
                        return 0;
                endif
        else
                GumpPrompt(who, "Private Location [" + params[1] + "] does not exist.");
                return 0;
        endif

        return 0;

endfunction


function Go_ByPublicLocation(who, params)

        if(go_prop_base.Exists( lower(params[1]) ) )
                var temp := go_prop_base[lower(params[1])];
                SendSysMessage(who, "Going to public location [" + params[1] + "]");
                var move;
                if( who.realm == temp.Realm )
                        move := MoveCharacterToLocation(who, CInt(temp.X), CInt(temp.Y), CInt(temp.Z), MOVECHAR_FORCELOCATION);
                else
                        move := MoveObjectToRealm( who, temp.Realm, temp.X, temp.Y, temp.Z, MOVECHAR_FORCELOCATION );
                endif
                if ( TypeOf(move) != "Integer" )
                        GumpPrompt(who, "Error moving: X[" + temp.X +"] Y[" + temp.Y + "] Z[" + temp.Z + "]");
                        return 0;
                endif
        else
                GumpPrompt(who, "Public Location [" + params[1] + "] does not exist.");
                return 0;
        endif

        return 0;

endfunction


function Go_ByDefaultLocation(who, params)
        if (go_prop.Exists( lower(params[1]) ) )
		var temp := go_prop[lower(params[1])];
		SendSysMessage(who, "Going to private location [" + params[1] + "]");
		var move;
                if (who.realm == temp.Realm)
                        move := MoveCharacterToLocation(who, CInt(temp.X), CInt(temp.Y), CInt(temp.Z), MOVECHAR_FORCELOCATION);
                else
                        move := MoveObjectToRealm( who, temp.Realm, temp.X, temp.Y, temp.Z, MOVECHAR_FORCELOCATION );
                endif
                if ( TypeOf(move) != "Integer" )
                        GumpPrompt(who, "Error moving: X[" + temp.X +"] Y[" + temp.Y + "] Z[" + temp.Z + "]");
                        return 0;
                endif
        elseif(go_prop_base.Exists( lower(params[1]) ) )
                var temp := go_prop_base[lower(params[1])];
                SendSysMessage(who, "Going to public location[" + params[1] + "]");
                var move;
                if( who.realm == temp.Realm )
                        move := MoveCharacterToLocation(who, CInt(temp.X), CInt(temp.Y), CInt(temp.Z), MOVECHAR_FORCELOCATION);
                else
                        move := MoveObjectToRealm( who, temp.Realm, temp.X, temp.Y, temp.Z, MOVECHAR_FORCELOCATION );
                endif
                if ( TypeOf(move) != "Integer" )
                        GumpPrompt(who, "Error moving: X[" + temp.X +"] Y[" + temp.Y + "] Z[" + temp.Z + "]");
                        return 0;
                endif
        else
                GumpPrompt(who, "Location [" + params[1] + "] does not exist.");
                return 0;
        endif
endfunction
