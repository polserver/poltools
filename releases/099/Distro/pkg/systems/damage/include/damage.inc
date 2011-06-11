/*
        Maintened by *Edwards

        Last Modifications 2008-10-14

        Last Modifications 2009-10-02
             * Added textcmd notifyDamage for players

        Last Modifications 2009-10-25
             * Cleaned codes for better performances
             * Fixed reportMurder with SetLastDamageInfo() previously situated in the wrong "if" statement
             * Added damage_ex.inc and will eventually move all elemental resist functions here
             * Added new function param flags for DAMAGE_USE_REPSYS or DAMAGE_NO_REPSYS newly supported by core
*/
use uo;
use os;
use util;
use math;
use vitals;

include "include/security";
include "include/packets";
include ":areas:managment";
include ":attributes:attributes";
include ":blood:blood";
include ":damage:damage_ex";
include ":security:damageReport";

enum DAMAGE_TYPES
     DMG_FIRE        := "Fire",
     DMG_AIR	     := "Air",
     DMG_EARTH       := "Earth",
     DMG_WATER       := "Water",
     DMG_NECRO       := "Necro",
     DMG_HOLY	     := "Holy",
     DMG_POISON      := "Poison",
     DMG_ACID	     := "Acid",
     DMG_PHYSICAL    := "Physical",
     DMG_RANGED      := "Ranged",
     DMG_MAGIC       := "Magic",
     DMG_ASTRAL      := "Astral",
     DMG_SONG        := "Song",
     DMG_FORCED      := "Forced"
endenum

function ApplyDamageEX( mobile, amount, type:=DMG_FORCED, source:=0, flag:=DAMAGE_USE_REPSYS )

	if( !amount )
		return 0;
	elseif( GetObjProperty( mobile, "Invul" ))
                return 0;
        elseif( amount < 0 )
		HealDamage( mobile, Abs( amount ));
                return 0;
        elseif( source )
                if( mobile.cmdlevel > source.cmdlevel && source.cmdlevel < 4 )
                        return 0;
                elseif( type != DMG_POISON )
                        if( A_IsIn( mobile, AREAS_NO_DAMAGE ))
                                if( source.IsA( POLCLASS_NPC ))
                                        SetObjProperty( source, "GuardKill", 1 );
                                        source.Kill();
                                endif
                                return 0;
                        endif
                endif

                SetScriptController( source );
                SetLastDamageInfo( mobile, amount, type, source );

                if( amount >= AP_GetVital( mobile, HITS ))
                        if( mobile.acctname && source.acctname )
                                SendVictimMessage( mobile, source );
                                NotifyStaffOnline( mobile, "is killed by "+source.name );
                                DMG_ReportText( mobile, "Killed by: "+source.serial+" Account: "+source.acctname+" by type: "+type+" x:"+mobile.x+" y:"+mobile.y+" z:"+mobile.z+" realm:"+mobile.realm, DMG_REPORT_SYSLOG );
                                DMG_ReportText( source, "Kills: "+mobile.serial+" Account: "+mobile.acctname+" by type: "+type+" x:"+mobile.x+" y:"+mobile.y+" z:"+mobile.z+" realm:"+mobile.realm, DMG_REPORT_SYSLOG );
                        endif
                endif
        elseif( type != DMG_POISON )
                if( A_IsIn( mobile, AREAS_NO_DAMAGE ))
                        return 0;
                endif
        endif

        if( GetObjProperty( mobile, "NotifyDmg" ))
                DisplayDamage( mobile, amount );
        endif

        ApplyRawDamage( mobile, CInt( amount ), flag );
        mobile.paralyzed := 0;
        
        if( !mobile.IsA( POLCLASS_NPC ) && amount > 30 )
                MakeBleed( mobile );
        endif

        return 1;
endfunction

function SendVictimMessage( mobile, source )

        var text_noto := GetObjProperty( source, "KillNoto" );
        if( source.IsA( POLCLASS_NPC ) || !text_noto )
                SendSysMessage( mobile, source.name+" has killed you.", 3, 33 );
        else
                SendSysMessage( mobile, source.name+": "+text_noto, 3, 33 );
        endif

        return 1;
endfunction
