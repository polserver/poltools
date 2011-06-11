/*
        Maintened by *Edwards

        2010-03-21
        
        Original Idea from ZuluHotel
*/
use uo;
use util;

include "include/itemUtil";
include ":attributes:attributes";
include ":classes:classes";

function RepairItem( mobile, craft_skill, item:=0 )

        if( !item )
                SendSysMessage( mobile, "Select the item you would like to repair." );
                item := Target( mobile );
                if( !item )
                        SendSysMessage( mobile, "Cancelled." );
                        return 0;
                endif
                
                if( !useItem_check( mobile, item, ITEM_INCONTAINER ))
                        return 0;
                endif
        endif
        
        if( !item.maxhp )
                SendSysMessage( mobile, "You cannot repair that!" );
                return 0;
        elseif( !craft_config[item.graphic] )
                SendSysMessage( mobile, "You cannot repair the item with your tool." );
                return 0;
        elseif( item.hp == item.maxhp ) //Check if the item is damaged
		SendSysMessage( mobile, "That item is in perfect condition already." );
		return 0;
	elseif( item.hp < 1 || item.hp > CInt( item.maxhp + item.maxhp_mod ))
		//The item should be broken already
		SendSysMessage( mobile, "The brittle material breaks when you handle it!" );
		DestroyItem( item );
		return 0;
	endif
 
	var damaged := CInt( item.maxhp - item.hp ),
	    arms_lore := AP_GetSkill( mobile, ARMS_LORE ),
	    difficulty := CInt( damaged + 20 );

	arms_lore *= ClasseBonus( mobile, CRAFTER );

	if( arms_lore >= damaged ) //Check if you have a chance to repair it
		if( SkillCheck( mobile, craft_skill, difficulty, "", ADV_DISABLE ) > 0 ) //Check if he repair it
			var chance := RandomInt(100);
			if( chance < 5 )
				//Bad luck, the item get damaged
				var dmg := CInt( damaged / 8 );
				if( dmg < 1 )
					dmg := 1;
				endif
			
				if( dmg >= item.hp )
					SendSysMessage( mobile, "You fail and destroy the item!" );
					DestroyItem( item );
					return 0;
				else
					SendSysMessage( mobile, "You fail in your repairing attempt and damage the item!" );
					item.hp := item.hp - dmg;
					return 0;
				endif
			elseif( chance == 99 )
				//Good luck, the item's quality improve
				SendSysMessage( mobile, "Your chance and skill improved the quality of the item!" );
				item.quality := CDbl( item.quality + 0.05 );
				item.hp := item.maxhp;
				return 0;
			else
				//You repair the item
				var amount := CInt(( RandomInt( CInt( arms_lore )) + 1 ) / 2 );
				if( amount >= damaged )
					item.hp := item.maxhp;
					SendSysMessage( mobile, "You repair the item completely." );
					return 1;
				else
					item.hp := item.hp + amount;
					SendSysMessage( mobile, "You successfully repair some of the damage." );
					return 1;
				endif
			endif
		else
			//You fail to repair it
			var dmg := CInt( damaged / 2 );
			if( dmg < 1 )
				dmg := 1;
			endif
			
			if( dmg >= item.hp )
				SendSysMessage( mobile, "You fail and destroy the item!" );
				DestroyItem( item );
                                return 0;
			else
				SendSysMessage( mobile, "You fail in your repairing attempt and damage the item!" );
				item.hp := item.hp - dmg;
                                return 0;
			endif
		endif
	else
		//You didn't even had a chance to repair it
		var dmg := CInt( damaged / 4 );
		if( dmg < 1 )
			dmg := 1;
		endif
		
		if( dmg >= item.hp )
			SendSysMessage( mobile, "You fail and destroy the item!" );
			DestroyItem( item );
			return 0;
		else
			SendSysMessage( mobile, "You fail in your repairing attempt and damage the item!" );
			item.hp := item.hp - dmg;
			return 0;
		endif
	endif
 
        return 1;
endfunction
