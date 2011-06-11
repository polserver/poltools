/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-25
*/
use uo;
use polsys;

function SetLastDamageInfo( mobile, amount, type, source := "None" )

	var serial := source.serial,
	    time := ReadGameClock();

        var acctname := source.npctemplate;
	if( source.acctname )
		acctname := source.acctname;
	endif

        var name;
	if( source.name )
		name := source.name;
	elseif( source.IsA( POLCLASS_ITEM ))
		name := source.desc;
	endif

        var mobiletemplate;
        if( mobile.npctemplate )
                mobiletemplate := mobile.npctemplate;
        endif

        var last_damage := {serial, time, amount, type, acctname, name, mobiletemplate};
	SetObjProperty( mobile, "LastDamage", last_damage );

	return last_damage;
endfunction

function GetLastDamageInfo( mobile )

	return GetObjProperty( mobile, "LastDamage" );
endfunction
