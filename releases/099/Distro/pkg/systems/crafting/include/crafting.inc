/*
        Created by *Edwards

        2009-11-06
        
        Last Modifications 2010-02-16
             * Added : Checks for LFucker items so they cannot be used to create new objects
*/
use uo;

include "include/facings";
include "include/itemUtil";
include "include/string";
include "include/uo_extend";
include ":classes:classes";
include ":crafting:craftGumps";
include ":crafting:craftSkill";
include ":crafting:toolWear";
include ":crafting:repairItem";

const CRAFT_DELAY := 1;

var itemConfig := ReadConfigFile( ":*:itemdesc" ),
    craft_skill,
    display_skill,
    craftL := 0,
    rcolor := 1,
    makermark := 1;
    
function Craft_CheckParts( who, elem )

	var parts_list := SplitWords( elem.parts );
	if( TypeOf( parts_list ) != "Array" || !parts_list )
		SendSysMessage( who, "This item does not require parts to be made! Please, contact a staff member." );
		return 0;
	endif

	var parts := dictionary;
	for i := 1 to ( parts_list.Size() / 2 )
		parts[CInt( parts_list[i] )] := CInt( parts_list[i+( parts_list.Size() / 2 )] );
                SleepMS(5);
	endfor

	var material_list := dictionary;

	foreach thing in ( EnumerateItemsInContainer( who.backpack ))
		// If thing.objtype is the same as the objtype in the part list array position....
		var thing_objtype := CInt( thing.objtype );
		if( thing_objtype in parts.Keys() )
                        if( GetObjProperty( thing, "LFucker" ))
                                //No items created can be used for crafting objects...
                                break;
                        elseif( CInt( material_list[thing_objtype] ) < CInt( parts[thing_objtype] ))
				material_list[thing_objtype] := CInt( material_list[thing_objtype] ) + thing.amount;
			endif
		endif
		SleepMS(5);
	endforeach

	foreach part in ( parts.Keys() )
		if( material_list[part] < parts[part] )
			SendSysMessage( who, "You do not have the required materials to make that." );
			return 0;
		endif
		SleepMS(5);
	endforeach

	return parts;
endfunction

function MaterialSelection( who, craft_skill, type )

	SendSysMessage( who, "Select the "+Lower( type )+" you would like to work with." );
	var material := Target( who );
	if( !material )
		return 0;
	elseif( !useItem_check( who, material, ITEM_INCONTAINER ))
                return 0;
	elseif( GetObjProperty( material, "LFucker" ))
		SendSysMessage( who, "[Anti-Cheating] You cannot use the material." );
                return 0;
	endif

	if( !canUseMaterial( who, craft_skill, itemConfig[material.objtype].Difficulty ))
		SendSysMessage( who, "You are not skilled enough to make anything out of that." );
		return 0;
	endif
 
        var material_objtype := 0;
	if( CInt( GetObjProperty( material, type )) > 0 )
		material_objtype := GetObjProperty( material, type );
	else
		material_objtype := material.objtype;
	endif

	var rtype := Lower( itemConfig[material_objtype].Type );
	if( rtype != type )
		SendSysMessage( who, "That's not the proper material." );
                return 0;
	endif

	var ret_val := array{};
	    ret_val.Append( material );
	    ret_val.Append( material_objtype );
	    ret_val.Append( itemConfig[material_objtype].Ptmod );
 
	return ret_val;
endfunction

function canUseMaterial( who, craft_skill, difficulty )

	if( AP_GetSkill( who, craft_skill ) >= CInt( difficulty ))
		return 1;
        endif

	return 0;
endfunction

function Craft_PlayEffects( who, elem, item:=0 )

	var craft_sound	:= elem.CraftSound,
	    craft_loops := CInt( elem.CraftLoops ),
	    craft_action := CInt( elem.CraftAnim );
     
	var craft_delay := CInt( elem.CraftDelay );
        if( !craft_delay  )
                craft_delay := 2;
        endif
 
	if( craft_loops )
                var i;
                for( i:=1; i<=craft_loops; i+=1 )
        		if( item )
                		TurnObjectToward( who, item.x, item.y );
        		endif
          
			if( craft_action )
				PerformAction( who, craft_action );
			endif
   
			PlaySoundEffect( who, craft_sound );
                        Sleep( craft_delay );
		endfor
	else
		PlaySoundEffect( who, craft_sound );
                Sleep( craft_delay );
	endif
 
	return 1;
endfunction

function Craft_ConsumeParts( who, parts_list )

	foreach material_objtype in ( parts_list.Keys() )
		var amount := parts_list[material_objtype];
		if( amount > 0 )
                        var consume := ConsumeSubstance( who.backpack, material_objtype, amount );
			if( consume.errortext )
				SendSysMessage( who, "You do not have the required materials to make that." );
				return 0;
			endif
		endif
  
		SleepMS(5);
	endforeach
 
	return 1;
endfunction

function Craft_ConsumeMaterial( who, elem, use_objtype )

	var amount := CInt( elem.Material );
	if( amount )
		var consume := ConsumeSubstance( who.backpack, use_objtype, amount );
		if( consume.errortext )
			SendSysMessage( who, "You do not have the required materials to make that." );
			return 0;
		endif
        else
                SendSysMessage( who, "This item does not require materials to be made! Please, contact a staff member." );
		return 0;
	endif

        return 1;
endfunction

function Craft_CreateItem( who, objtype, elem, useOn_obj:=0 )

	var the_item := CreateItemInContainer( who.backpack, objtype, 1 );
	if( !the_item )
		the_item := CreateItemAtLocation( who.x, who.y, who.z, objtype, 1, who.realm );
	endif
        the_item.movable := 1;
 
        if( useOn_obj )
                var elem_obj := itemConfig[useOn_obj],
                    iname := CStr( elem_obj.IName ),
                    icolor := CInt( elem_obj.Color );
                
                if( elem.Retain == 1 )
                        SetName( the_item, AddArticle( iname+" "+TruncateArticle( the_item.desc )));
                        if( the_item.IsDecoDeed() )
                                SetObjProperty( the_item, "IName", iname );
                        endif
                        
                        if( iname == "gems" )
                                icolor := CInt( elem_obj.IColor );
                        endif
                        
                        the_item.color := icolor;
	        endif
                
                if( the_item.IsA( POLCLASS_EQUIPMENT ))
                        SetObjProperty( the_item, "material", useOn_obj );
                        
               		var CProps := GetConfigStringArray( elem_obj, "Elem" );
                	foreach prop in CProps
                        	var splited := SplitWords( prop ),
                            	prop_name := splited[1],
                            	prop_value := CInt( splited[2] );

                        	SetObjProperty( the_item, prop_name, prop_value );

                        	SleepMS(5);
                	endforeach
                endif
	endif

        SetTrueType( who, the_item );

        if( IsStackable( the_item, the_item ))
                var find_stack := FindMatchingObjtypeInContainer( who.backpack, the_item );
                if( find_stack )
                        the_item.desc := TruncateArticle( the_item.desc );
                        find_stack.amount := find_stack.amount + 1;
                endif
        endif
        
	SendSysMessage( who, "You create the item and put it in your backpack." );

	return the_item;
endfunction

function SetTrueType( who, the_item )

        if( the_item.objtype == 0xE34 )
                return 0;
        endif

        var classe_name := GetClasseIdForSkillName( craft_skill ),
            true_type := "ByTrue"+UCFirst( GetClasseName( classe_name )),
            classe_level := CInt( GetObjProperty( who, classe_name ));

        SetObjProperty( the_item, true_type, classe_level );
endfunction
