/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        Last Modifications 2010-08-21
*/
use uo;
use os;

include "include/client";
include "include/yesNo";

function HarvestBotanic( who, targ )

       	if( !useItem_check( who, targ, DEFAULT_ITEM, 1 ))
		return 0;
	endif

        var cfg := ReadConfigFile( ":botanic:seed" );
        if( cfg.errortext )
                SendSysMessage( who, "Error unable to read cfgfile <:botanic:seed> -->"+cfg.errortext );
                return 0;
        elseif( !cfg[targ.objtype] )
                SendSysMessage( who, "You can not harvest anything from that." );
                return 0;
        endif

    	var amount := GetObjProperty( targ, "#amt" );
        if( !amount )
                amount := 0;
        endif

    	var resource_type := CInt( cfg[targ.objtype].product );
    	if( !resource_type )
		SendSysMessage( who, "It's too young to produce anything." );
		return 0;
	endif

    	var max_amount := CInt( cfg[targ.objtype].maxamount );
        if( max_amount.errortext )
                SendSysMessage( who, "Error unable to read max_amount -->"+max_amount.errortext );
                return 0;
        endif

        var last_harvest := GetObjProperty( targ, "#Last_Harvest" );
        if( !last_harvest )
                amount := 10;
      	        SetObjProperty( targ, "#amt", 10 );
        endif

      	var fruit := cfg[targ.objtype].productname;
      	if( fruit.errortext )
      	      	SendSysMessage( who, "Error unable to create product -->"+fruit.errortext );
      	      	return 0;
      	endif
       
        if( last_harvest > ReadGameClock() && amount <= 0 )
                SendSysMessage( who, cfg[targ.objtype].productname+"s are still growing." );
                if( fruit != "grape" )
                        if( YesNo( who, "Destroy tree?" ))
		                DestroyItem( targ );
                        endif
                endif
                return 0;
        endif

        var sfx := { 0x249,0x50 };
      	PlaySoundEffect( who, sfx.RandomEntry() );

        TurnObjectToward( who, targ.x, targ.y );
      	PerformAction( who, ANIM_SALUTE );

      	Sleep(2);

        if( SkillCheck( who, LUMBERJACKING, -1 ) > 0 )

                var num := RandomInt(4) + 1;
                
        	var create := CreateItemInBackpack( who, CInt( cfg[targ.objtype].product ), num );
                if( create.errortext )
                        SendSysMessage( who, "Error unable to create product -->"+create.errortext );
                        return 0;
                endif

        	SetObjProperty( targ, "#amt", amount - num );
                SetObjProperty( targ, "#Last_Harvest", ReadGameClock() + 1000 );

                if( num > 1 )
		        SendSysMessage( who, "You put "+num+" "+fruit+"s in your backpack." );
                else
		        SendSysMessage( who, "You put "+num+" "+fruit+" in your backpack." );
                endif
        else
		SendSysMessage( who, "You failed to harvest "+fruit+"s." );
      	endif

        return 1;
endfunction

function HarvestFlower( who, item )

        if( !useItem_check( who, item, ITEM_INCONTAINER ))
		return 0;
        endif

        var cfg := ReadConfigFile( ":botanic:seed" );
        if( cfg.errortext )
                SendSysMessage( who, "Error unable to read cfgfile <:botanic:seed> -->"+cfg.errortext );
                return 0;
        elseif( !cfg[item.objtype] )
                SendSysMessage( who, "You can not harvest anything from that." );
                return 0;
        endif

      	var fruit := cfg[item.objtype].productname;
      	if( fruit.errortext )
      	      	SendSysMessage( who, "Error unable to create product -->"+fruit.errortext );
      	      	return 0;
      	endif

        TurnObjectToward( who, item.x, item.y );
      	PerformAction( who, ANIM_SALUTE );

      	Sleep(2);

      	var create := CreateItemInBackpack( who, CInt( cfg[item.objtype].product ), 1 );
      	if( create.errortext )
      	      	SendSysMessage( who, "Error unable to create product -->"+create.errortext );
      	      	return 0;
      	endif

        DestroyItem( item );
        SendSysMessage( who, "You put 1 "+fruit+" in your backpack." );
        
        return 1;
endfunction
