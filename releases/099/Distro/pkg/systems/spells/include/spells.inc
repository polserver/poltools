/*
        Maintened by *Edwards

        2008-10-24
        
        Last Modifications 2009-08-19
             * Added CheckLOS with targs()

        Last Modifications 2010-01-07
             * Fixed peacemaking check before casting
*/
use uo;
use os;
use util;
use cfgfile;

include "include/client";
include "include/packets";
include "include/sounds";
include ":attributes:attributes";
include ":security:report";
include ":spells:settings";
include ":spells:spells_ex";
include ":spells:spells_math";
include ":timedscripts:targetUtil";

function SPELL_GetSpellsCfgFile()

	var cfg := ReadConfigFile( ":spells:spells" );

	if( cfg.errortext )
		ReportText( "Error::SPELL_GetSpellsCfgFile() - Unable to open [:magery:spells.cfg] ->"+cfg.errortext, REPORT_SYSLOG );
	endif

	return cfg;
endfunction

function SPELL_GetSpellsCfgElem( elem_name, byref cfg_file := 0 )

	if( !cfg_file )
		cfg_file := SPELL_GetSpellsCfgFile();
	endif

	var elem := cfg_file[elem_name];
	if( elem.errortext )
		ReportText( "Error::SPELL_GetSpellsCfgElem() - Unable to find elem ["+elem_name+"] ->"+elem.errortext, REPORT_SYSLOG );
	endif

	return elem;
endfunction

function SPELL_SpellDebug( mobile, text )

	if( mobile.cmdlevel )
		SendSysMessage( mobile, ""+text );
		return 1;
	endif
	return 0;
endfunction

function SPELL_Target( mobile, prompt:="Select a target.", option )

	var orig_x := mobile.x,
	    orig_y := mobile.y,
	    settings_elem := SPELL_GetSettingsCfgElem( "Settings" ),
	    allow_precast := CInt( settings_elem.AllowPrecasting );

	SendSysMessage( mobile, prompt );
	
	var targ := TargetEX( mobile, 10, TGTOPT_CHECK_LOS + option );
        if( !targ )
                return 0;
	elseif( Distance( mobile, targ ) > settings_elem.MaxRange )
		SendSysMessage( mobile, "That is too far away." );
		return 0;
	elseif( !allow_precast )
		if( mobile.x != orig_x || mobile.y != orig_y )
			SendSysMessage( mobile, "Your concentration is disturbed." );
			return 0;
		endif
	endif

	return targ;
endfunction

function SPELL_TargetCoordinates( mobile, prompt:="Select a target." )

	var orig_x := mobile.x,
	    orig_y := mobile.y,
	    settings_elem := SPELL_GetSettingsCfgElem( "Settings" ),
	    allow_precast := CInt( settings_elem.AllowPrecasting );
	
	SendSysMessage( mobile, prompt );
 
	var targ := TargetCoordinatesEX( mobile, 10 );
        if( !targ )
                return 0;
	elseif( CoordinateDistance( mobile.x, mobile.y, targ.x, targ.y ) > settings_elem.MaxRange )
		SendSysMessage( mobile, "That is too far away." );
		return 0;
        elseif( !CheckLosAt( mobile, targ.x, targ.y, targ.z ))
                SendSysMessage( mobile, "You cannot see that." );
                return 0;
	elseif( !allow_precast )
		if( mobile.x != orig_x || mobile.y != orig_y )
			SendSysMessage( mobile, "Your concentration is disturbed." );
			return 0;
		endif
	endif
	
	return targ;
endfunction

function SPELL_GetValidTargets( targ, area_size:=1, harmful:=0, excludes_array:={}, party_ref:=0 )

	var targs := ListMobilesNearLocation( targ.x, targ.y, targ.z, area_size, targ.realm );
	for i:=1 to targs.Size()
                if( excludes_array.Size() > 0 && targs[i].serial in excludes_array )
                	targs.Erase(i);
                endif
                
		if( harmful )
			if( party_ref )
                        	if( party_ref == targs[i].party )
		        		targs.Erase(i);
                                endif
			endif
                else
			if( targs[i].criminal || targs[i].murderer )
		        	targs.Erase(i);
			endif
		endif
                SleepMS(5);
	endfor

	return targs;
endfunction

function SPELL_MobileCheck( mobile, targ )

        if( !targ || targ.dead || targ.hidden || targ.concealed )
                SendSysMessage( mobile, "Cancelled." );
                return 0;
	elseif( !targ.IsA( POLCLASS_MOBILE ))
		SendSysMessage( mobile, "This spell won't work on that." );
		return 0;
	endif
	
	return 1;
endfunction

function SPELL_PlaySpellSFX( spellid, targ )

	var cfg_elem := SPELL_GetSpellsCfgElem( spellid );
	var sfx := GetConfigInt( cfg_elem, "SFX" );
	
	return PlaySoundEffect( targ, sfx );
endfunction

function SPELL_PlaySpellGFX( spellid, targ, source, speed:=3, loop:=14, explode:=0 )

	var cfg_elem := SPELL_GetSpellsCfgElem( spellid ),
	    gfx := GetConfigInt( cfg_elem, "GFX" ),
	    gfx_type := GetConfigInt( cfg_elem, "GFXType" ),
            gfx_color := GetConfigInt( cfg_elem, "GFXColor" );

	case( gfx_type )
		1: PlayObjectCenteredEffectEx( targ, gfx, speed, loop, gfx_color, 0, 0, 0 );
                   break;
		2:
                   PlayMovingEffectXYZHued( source, targ, gfx, speed, loop, explode, gfx_color );
                   
                   var coord_list := GetCoordsInLine( source.x, source.y, targ.x, targ.y );
	           foreach coord in coord_list
                        //Avoid compiler warnigns
                        coord := coord;
                   	SleepMS(105);
                   endforeach
                   break;
		3: PlayStationaryEffect( targ.x, targ.y, targ.z, gfx, speed, loop, 0, targ.realm );
                   break;
	endcase

	return 1;
endfunction

function SPELL_GetCircle( spellid )

	var cfg_elem := SPELL_GetSpellsCfgElem( spellid );
	var circle := GetConfigInt( cfg_elem, "Circle" );

	return CInt( circle );
endfunction

function FailSpell( mobile, message:="The spell fizzles." )

	EraseObjProperty( mobile, "#Casting" );

        if( !mobile.IsA( POLCLASS_NPC ))
                SendSysMessage( mobile, message );
	        PlaySoundEffect( mobile, SFX_SPELL_FAIL );
                PlayObjectCenteredEffect( mobile, GFX_FIZZLE, 5, 50 );
        endif

	return 1;
endfunction

function canDamage( mobile, targ, noto )

        if( noto == "helpful" )
                //We accept any kind of helpful cast everywhere :)
		return 1;
        elseif( GetObjProperty( mobile, "#Peacemaking" ) > ReadGameClock() )
		return 0;
        elseif( mobile.cmdlevel >= 5 )
                return 1;
        else
                if( targ )
                        if( targ.cmdlevel > mobile.cmdlevel && mobile.cmdlevel < 4 )
			        FailSpell( mobile );
                                return 0;
                        elseif( A_IsIn( targ, AREAS_NO_DAMAGE ))
			        FailSpell( mobile, "The spells cannot be cast there." );
			        return 0;
                        endif
                else
                        if( noto == "harmful" )
                                if( A_IsIn( mobile, AREAS_NO_DAMAGE ))
			                FailSpell( mobile, "The spells cannot be cast here." );
			                return 0;
                                endif
                        endif
                endif
        endif
 
        return 1;
endfunction

function SendBoostMessage( mobile, targ, spell, points, delay )
        
        if( mobile.serial != targ.serial )
                SendSysMessage( mobile, "You cast "+spell+" on "+targ.name+" for "+points+" points. The effect will last for "+CInt( delay / 60 )+" minutes." );
        endif

	return 1;
endfunction
