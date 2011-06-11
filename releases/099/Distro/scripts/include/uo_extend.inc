/*
        Maintened by *Edwards
        
        2010-11-19
*/
use uo;
use os;
use util;
use polsys;
use cfgfile;
use math;

//
// For: EnumerateOnlineCharactersABC()
//
CONST SORT_ASCENDING	:= 0x0; // Default
CONST SORT_DESCENDING	:= 0x1;

// For: GetIsAType()
CONST ISA_GET_MAIN	:= 0x0; // Default
CONST ISA_GET_ALL	:= 0x1;

// For: ListMobilesInRange()
CONST RNG_SQUARE := 0x00; // Range is "square" (ie, simply +/- to x & y)
CONST RNG_CIRCLE := 0x01; // Range is a real "circle" [DEFAULT]

function EnumerateOnlineCharactersABC(dir:=SORT_ASCENDING, cmdlevel:=-1)
	var dict := dictionary;
	foreach player in ( EnumerateOnlineCharacters() )
		if ( cmdlevel != -1 )
			if ( player.cmdlevel > cmdlevel )
				continue;
			endif
		endif

		var key := Upper(player.name);
		if ( !dict.Exists(key) )
			dict[key] := array{};
		endif
		dict[key].Append(player);
		SleepMS(2);
	endforeach

	var z_list := array{};
	foreach list in ( dict )
		foreach player in ( list )
			if ( dir == SORT_ASCENDING )
				z_list.Append(player);
			else
				z_list.Insert(1, player);
			endif
			SleepMS(2);
		endforeach
		SleepMS(2);
	endforeach

	return z_list;
endfunction

function EnumeratePhysicalItems(mobile)
	var equip_list := array{};

	// Hair, beard, backpack, mount
	var skip_layers := array{11, 16, 21, 25};
	var i;

	for ( i:=0; i<=25; i+=1 )
		if ( i in skip_layers )
			continue;
		else
			var item := GetEquipmentByLayer(mobile, i);
			if ( item )
				equip_list.Append(item);
			endif
		endif
		SleepMS(2);
	endfor

	return equip_list;
endfunction

function GetIsAType(object, flags:=ISA_GET_MAIN)
	var type_list := array;
	var i := 16; // Max number of IsA Id #s
	for ( i; i > 0; i-=1 )
		if ( !object.IsA(i) )
			continue;
		elseif ( flags & ISA_GET_ALL )
			type_list.Append(i);
		else
			return i;
		endif
		SleepMS(2);
	endfor

	if ( type_list.Size() > 0 )
		return type_list;
	else
		return error{"errortext":="Unable to determine IsA type of '"+TypeOf(object)+"'"};
	endif
endfunction

function GetIsAName(isa_id)
	isa_id := CInt(isa_id);
	var type := "POLCLASS_";
	case ( isa_id )
		1: return type+"UOBJECT"; break;
		2: return type+"ITEM"; break;
		3: return type+"MOBILE"; break;
		4: return type+"NPC"; break;
		5: return type+"LOCKABLE"; break;
		6: return type+"CONTAINER"; break;
		7: return type+"CORPSE"; break;
		8: return type+"DOOR"; break;
		9: return type+"SPELLBOOK"; break;
		10: return type+"MAP"; break;
		11: return type+"MULTI"; break;
		12: return type+"BOAT"; break;
		13: return type+"HOUSE"; break;
		14: return type+"EQUIPMENT"; break;
		15: return type+"ARMOR"; break;
		16: return type+"WEAPON"; break;
		default: return error{"errortext":="'"+isa_id+"' is not a valid IsA number."};
	endcase
endfunction

function ListMobilesInRange( x, y, z, range, realm := _DEFAULT_REALM, flags := LISTEX_FLAG_NORMAL, range_type := RNG_SQUARE )

	if( range < 0 )
		return 0;
	endif

	// If passed normal flags and range is a square, then use the internal function
	if( !( flags & LISTEX_FLAG_CONCEALED ) && range_type == RNG_SQUARE )
		return ListMobilesNearLocationEx( x, y, z, range, flags, realm );
	endif

	if( !( flags & LISTEX_FLAG_NORMAL ))
		flags := flags | LISTEX_FLAG_NORMAL;
	endif

	// Validate the min/max values
	var min_range := { x-range, y-range };
	var max_range := { x+range, y+range };
 
	if( min_range[1] < 0 )
                min_range[1] := 0;
        endif

	if( min_range[2] < 0 )
                min_range[2] := 0;
        endif
        
        var map_maxX := 6143,
            map_maxY := 4095;
            
        if( realm == "britannia" )
                if( max_range[1] > map_maxX )
                       max_range[1] := map_maxX;
                endif
        
	        if( max_range[2] > map_maxY )
                       max_range[2] := map_maxY;
                endif
        endif

	set_script_option( SCRIPTOPT_NO_RUNAWAY, 1 );

	var mobiles := ListObjectsInBox( min_range[1], min_range[2], z-15, max_range[1], max_range[2], z+15, realm );
	var dTrig := dictionary;
	if( range_type == RNG_CIRCLE )
		// Set up the trig dictionary: key (x) to value (+y)
		var iHyp := Pow( range, 2 );
		for xpos := ( x-range ) to ( x+range )
			dTrig[xpos] := CInt( Ceil( Sqrt( iHyp - Pow( Abs( x-xpos ), 2 )))); //DeltaY
		endfor
	endif

	var fNrm := flags & LISTEX_FLAG_NORMAL;
	var fHid := flags & LISTEX_FLAG_HIDDEN;
	var fGho := flags & LISTEX_FLAG_GHOST;
	var fCon := flags & LISTEX_FLAG_CONCEALED;

	var valid_mob := array;
	foreach mob in mobiles
		if( mob.isa( POLCLASS_MOBILE ))
			if(( mob.hidden && fHid ) || ( mob.dead && fGho ) || ( mob.concealed == 0 && fCon ) ||
			   ( !mob.hidden && !mob.dead && !mob.concealed && fNrm ))
      
				if( range_type == RNG_CIRCLE )
					if(( mob.y >= ( y - dTrig[mob.x] )) && ( mob.y <= ( y + dTrig[mob.x] )))
						valid_mob.append( mob );
					endif
				else
					valid_mob.append( mob );
				endif
			endif
		endif
                SleepMS(5);
	endforeach

	set_script_option( SCRIPTOPT_NO_RUNAWAY, 0 );

	return valid_mob;
endfunction

function FindMatchingObjtypeInContainer( container, item )

	var result := 0;
 
        foreach object in EnumerateItemsInContainer( container );
                SleepMS(5);
                
                if( object.objtype != item.objtype )
                        continue;
                elseif( object.color != item.color )
                        continue;
                elseif( object.graphic != item.graphic )
                        continue;
                elseif( object.amount <= 1 )
                        continue;
                endif
                
                result := object;
                break;
        endforeach

	return result;
endfunction

function DupeItem( item, newitem )

	newitem.graphic := item.graphic;
	newitem.movable	:= item.decayAt;

	if( item.name )
                newitem.name := item.name;
        endif
	if( item.maxhp_mod )
                newitem.maxhp_mod := item.maxhp_mod;
        endif
	if( item.ar_mod )
                newitem.ar_mod := item.ar_mod;
        endif
	if( item.dmg_mod )
                newitem.dmg_mod := item.dmg_mod;
        endif
	if( item.hitscript )
                newitem.hitscript := item.hitscript;
        endif
	if( item.quality )
                newitem.quality := item.quality;
        endif
	if( item.color )
                newitem.color := item.color;
        endif
	if( item.newbie )
                newitem.newbie := item.newbie;
        endif
	if( item.facing )
                newitem.facing := item.facing;
        endif
	if( item.hp )
                newitem.hp := item.hp;
        endif
	if( item.buyprice )
                newitem.buyprice := item.buyprice;
        endif
	if( item.decayAt )
                newitem.decayAt	:= item.decayAt;
        endif

	foreach prop in GetObjPropertyNames( item )
		SetObjProperty( newitem, prop, GetObjProperty( item, prop ));
                SleepMS(5);
	endforeach

        return 1;
endfunction
