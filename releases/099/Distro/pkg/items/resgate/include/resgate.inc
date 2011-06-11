/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2010-07-07
*/
use uo;
use cfgfile;

include "include/client";
include ":attributes:attributes";
include ":death:resurrection";

function ResurrectGate( mobile )

	if( GetObjProperty( mobile, "#JustRess" ) >= ReadGameClock() )
		SendSysMessage( mobile, "You must wait a moment before being resurrected again." );
		return 1;
	endif

	if( !ResurrectionGump( mobile ))
		SendSysMessage( mobile, "Cancelled." );
		return 0;
	endif

	SendSysMessage( mobile, "Live again by the Virtues!" );
	PlaySoundEffect( mobile, 0x215 );
	AP_SetVital( mobile, STAMINA, 5 );

	var effect := RandomInt( 100 );
	if( effect <= 5 )
		PlaySoundEffect( mobile, 0x203 );
		PlayObjectCenteredEffect( mobile, 0x375a, 7, 14 );

		AP_RefreshVitals( mobile );
	endif

	var robe := GetEquipmentByLayer( mobile, LAYER_ROBE );
	if( !GetObjProperty( robe, "Cursed" ))
		DestroyItem( robe );
	endif

	return 1;
endfunction
