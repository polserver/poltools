/*
        Maintened by *Edwards

        2009-08-16
*/
use uo;
use os;

function useCharge( mobile, item )

	var charges := CInt( GetObjProperty( item, "Charges" ));
	if( !charges )
		DestroyItem( item );
		return 0;
	else
	        charges -= 1;
	        SetObjProperty( item, "Charges", charges );
	endif
 
        SendSysMessage( mobile, "You used 1 charge from "+item.desc+". There is "+CInt( charges )+" left." );
         
        PlaySoundEffect( mobile, 0x1F3 );
        PlayMovingEffectXYZ( mobile.x, mobile.y, mobile.z+75, mobile.x, mobile.y, mobile.z, 0x373A, 7, 14, 0, mobile.realm );

        return 1;
endfunction

function watchDelay( mobile )

        if( GetObjProperty( mobile, "#Enchant" ) > ReadGameClock() )
                SendSysMessage( mobile, "You must wait before using it again." );
                return 0;
        else
                SetObjProperty( mobile, "#Enchant", ReadGameClock() + 30 );
        endif
        
        return 1;
endfunction
