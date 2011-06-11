/*
        Maintened by *Edwards
        For FantasiaShard.com
        
        Edwards@FantasiaShard.com
        
        2009-10-22
*/
use cfgfile;

function CS_GetWeaponInfo( weapon, prop_name )

	var config := ReadConfigFile( ":*:itemdesc" );
	var info := config[weapon.objtype];
	
	return GetConfigString( info, prop_name );
endfunction

function CS_GetAverageDamage( weapon )

	var dmg := CS_GetWeaponInfo( weapon, "Damage" );

	dmg["d"] := " ";
	dmg["-"] := " ";
	dmg["+"] := " ";
	dmg := SplitWords( dmg );
 
	var X := CInt( dmg[1] );
	var Y := CInt( dmg[2] );
	var Z := CInt( dmg[3] );
 
	Z += CInt(( weapon ).dmg_mod );
 
	var average := ( X * Y ) + Z;
 
	average += CDbl((( average + weapon.dmg_mod ) * weapon.hp ) / ( weapon.maxhp + weapon.maxhp_mod ));

	return CInt( average / 3 );
endfunction

function CS_GetDamage( weapon )

	var dmg := CS_GetWeaponInfo( weapon, "Damage" );
 
	dmg["d"] := " ";
	dmg["-"] := " ";
	dmg["+"] := " ";
	dmg := SplitWords( dmg );

	var X := CInt( dmg[1] );
	var Y := CInt( dmg[2] );
	var Z := CInt( dmg[3] );

	Z += CInt(( weapon ).dmg_mod );
 
        var index := struct;
            index.min_dmg := x;
            index.max_dmg := y+z;

	return index;
endfunction
