/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-21
*/
use uo;
use os;
use cfgfile;

include "include/sounds";
include ":timedscripts:timedScripts";

function Door_Toggle( door )

	if( door.IsOpen() )
		Door_Close( door );
	else
		Door_Open( door );
		TS_StartTimer( door, "AutoClose", 10 );
	endif
  
	return 1;
endfunction

function Door_Open( door )

	if( door.IsOpen() )
		return error{"errortext":="Door is already open."};
	endif

	ChangeDoor( door );

	SetObjProperty( door, "Open", 1 );

	case( Lower( ConfigInfo( door ).DoorType ))
		"metal" : PlaySoundEffect( door, SFX_METAL_DOOR_OPEN );
		"wooden" : PlaySoundEffect( door, SFX_WOODEN_DOOR_OPEN );
	endcase

	return 1;
endfunction

function Door_Close( door )

	if( !door.IsOpen() )
		return error{"errortext":="Door is already closed."};
	endif

	ChangeDoor( door );

	EraseObjProperty( door, "Open" );

	case( Lower( ConfigInfo( door ).DoorType ))
		"metal" : PlaySoundEffect( door, SFX_METAL_DOOR_CLOSE );
		"wooden" : PlaySoundEffect( door, SFX_WOODEN_DOOR_CLOSE );
	endcase

	return 1;
endfunction

function ChangeDoor( door )

	if( door.container )
		return 0;
	endif

	var door_info := ConfigInfo( door ),
            door_x := door.x+CInt( door_info.xmod ),
            door_y := door.y+CInt( door_info.ymod ),
            door_z := door.z,
            door_realm := door.realm;
            
        if( door.IsOpen() )
                detach();
                while( door )
                        var nearby := Len( Door_Occupied( door_x, door_y, door_z, door_realm ));
                        if( !nearby )
                                break;
                        endif

                        Sleep(1);
                endwhile
        endif
        
        door.movable := 1;
	door.graphic := door_info.ChangeTo;
        
	var result := MoveObjectToLocation( door, door_x, door_y, door_z, door_realm, MOVEOBJECT_FORCELOCATION );
	if( result.errortext )
		PrintTextAbove( door, "Error moving door -"+result.errortext );
	endif
 
        door.movable := 0;

	return 1;
endfunction

function Door_Occupied( x, y, z, door_realm )

	// Used to prevent a door slamming closed if a character is in the way.
	return ListMobilesNearLocationEX( x, y, z, 0, LISTEX_FLAG_HIDDEN+LISTEX_FLAG_NORMAL, door_realm );
endfunction

function ConfigInfo( door )

	var config := ReadConfigFile( ":*:itemdesc" );
 
	return config[Hex( door.graphic )];
endfunction
