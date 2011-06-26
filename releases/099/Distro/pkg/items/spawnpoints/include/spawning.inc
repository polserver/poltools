/*
        Creatred by *Edwards

        2010-03-29
*/
use uo;
use os;
use util;

function SP_SelectCoordsInArea( x1, y1, x2, y2 )

	var coord := struct;
	coord.+x := RandomInt( x2-x1 )+x1;
	coord.+y := RandomInt( y2-y1 )+y1;

	return coord;
endfunction

function SpawnNpc( spawnpoint, sp_settings )

        var created;
        var i;
        
        //Just make sure it doesn't loop forever. 10 iterations should be enough.
        for( i:=0; i<=10; i+=1 )
                Sleep(1);

		var coords := SP_GetCoords( spawnpoint, sp_settings ),
            	    destination := SP_SelectCoordsInArea( coords.x1, coords.y1, coords.x2, coords.y2 ),
            	    z_pos := CInt( GetWorldHeight( destination.x, destination.y, spawnpoint.realm ));

		if( z_pos.multi )
			continue;
		endif

                created := CreateNpcFromTemplate( sp_settings[2], destination.x, destination.y, z_pos, 0, spawnpoint.realm );
                if( created )
                        break;
                endif
        endfor

        SetObjProperty( created, "spawnpoint_serial", spawnpoint.serial );

	return created.serial;
endfunction

function SpawnGroup( spawnpoint, sp_settings )

	var coords := SP_GetCoords( spawnpoint, sp_settings ),
            destination := SP_SelectCoordsInArea( coords.x1, coords.y1, coords.x2, coords.y2 ),
            z_pos := CInt( GetWorldHeight( destination.x, destination.y, spawnpoint.realm ));

	if( z_pos.multi )
		return 0;
	endif

        var group_entries := SS_GetEntriesInGroup( sp_settings[2] ).Keys(),
	    num_entries := group_entries.Size(),
            spawn_object := group_entries[RandomInt( num_entries )+1];

	var created := CreateNpcFromTemplate( spawn_object, destination.x, destination.y, z_pos, 0, spawnpoint.realm );
        if( created.errortext )
                return 0;
        endif

        SetObjProperty( created, "spawnpoint_serial", spawnpoint.serial );

	return created.serial;
endfunction

function SpawnItem( spawnpoint, sp_settings )

	var obj_type := sp_settings[2];
	if( !obj_type )
		obj_type := GetObjtypeByName( obj_type );
		if( !obj_type )
			return 0;
		endif
	endif

	var coords := SP_GetCoords( spawnpoint, sp_settings ),
            destination := SP_SelectCoordsInArea( coords.x1, coords.y1, coords.x2, coords.y2 ),
            z_pos := CInt( GetWorldHeight( destination.x, destination.y, spawnpoint.realm ));

	if( z_pos.multi )
		return 0;
	endif

	var item := CreateItemAtLocation( destination.x, destination.y, z_pos, obj_type, 1, spawnpoint.realm );
        if( item.errortext )
                return 0;
        endif

        SetObjProperty( item, "spawnpoint_serial", spawnpoint.serial );

	return item.serial;
endfunction

function SpawnContainer( spawnpoint, sp_settings )

	var obj_type := sp_settings[2];
	if( !obj_type )
		obj_type := GetObjtypeByName( obj_type );
		if( !obj_type )
			return 0;
		endif
	endif

	var coords := SP_GetCoords( spawnpoint, sp_settings ),
            destination := SP_SelectCoordsInArea( coords.x1, coords.y1, coords.x2, coords.y2 ),
            z_pos := CInt( GetWorldHeight( destination.x, destination.y, spawnpoint.realm ));

	if( z_pos.multi )
		return 0;
	endif

        var chest := CreateItemAtLocation( destination.x, destination.y, z_pos, obj_type, 1, spawnpoint.realm );
        if( chest.errortext )
                return 0;
        endif

        if( !chest.IsA( POLCLASS_CONTAINER ))
                DestroyItem( chest );
                return 0;
        endif

        var lootIndex := RandomInt(3) + 1;
	Loot_Generate( chest, lootIndex, 1 );

        SetObjProperty( chest, "spawnpoint_serial", spawnpoint.serial );

	return chest.serial;
endfunction
