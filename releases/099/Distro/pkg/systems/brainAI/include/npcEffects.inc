/*
        Created by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-12
        
        Last Modifications 2009-10-14
             * Change in NPC_GetIdleGraphic() now if no IdleGraphic in npcdesc will return the most
               popular value with the creatures graphics
             * Change in NPC_GetDamagedGraphic() now if no DamagedGraphic in npcdesc will return the most
               popular value with the creatures graphics
             * Change in NPC_GetHitGraphic() now if no AttackHitGraphic in npcdesc will return the most
               popular value with the creatures graphics
             * Added NPC_GetCastSpellGraphic()

        Last Modifications 2010-02-22
             * Cleaned codes for better performances
        
        NPC_GetBarkSound( source )  BarkSound
        NPC_GetCastSpellGraphic( source )  AttackCastGraphic
        NPC_GetDamagedGraphic( source )  DamagedGraphic
        NPC_GetDamagedSound( source )  DamagedSound
        NPC_GetDeathSound( source )  DeathSound
        NPC_GetIdleGraphic( source )  IdleGraphic
        NPC_GetIdleSound( source )  IdleSound
        NPC_GetHitGraphic( source )  AttackHitGraphic
        NPC_GetHitSound( source )  AttackHitSound
        NPC_GetMissSound( source )  AttackMissSound
*/
use uo;
use os;
use util;
use cfgfile;

include ":brainAI:npcUtil";

function NPC_GetBarkSound( source )

	var source_effects := NPC_GetNPCConfig( source.npctemplate );
	if( !source_effects )
		return 0;
	endif

	var bark_sound_list := GetConfigStringArray( source_effects, "BarkSound" );

	if( TypeOf( bark_sound_list ) != "Array" )
		return 0;
	endif

	return bark_sound_list.RandomEntry();
endfunction

function NPC_GetCastSpellGraphic( source )

	var source_effects := NPC_GetNPCConfig( source.npctemplate );
	if( !source_effects )
		return 0;
	endif

	var castspell_graphic_list := GetConfigStringArray( source_effects, "AttackCastGraphic" );
	if( TypeOf( castspell_graphic_list ) != "Array" )
		return NPC_GetIdleGraphic( source ); //use idle atleast the creatures will do something
	endif

	return castspell_graphic_list.RandomEntry();
endfunction

function NPC_GetDamagedGraphic( source )

	var source_effects := NPC_GetNPCConfig( source.npctemplate );
	if( !source_effects )
		return 0;
	endif

	var damaged_graphic_list := GetConfigStringArray( source_effects, "DamagedGraphic" );
	if( TypeOf( damaged_graphic_list ) != "Array" )
		damaged_graphic_list := array{10}; //Hit most popular movment
	endif

	return damaged_graphic_list.RandomEntry();
endfunction

function NPC_GetDamagedSound( source )

	var source_effects := NPC_GetNPCConfig( source.npctemplate );
	if( !source_effects )
		return 0;
	endif

	var damaged_sound_list := GetConfigStringArray( source_effects, "DamagedSound" );
	if( TypeOf( damaged_sound_list ) != "Array" )
		return 0;
	endif

	return damaged_sound_list.RandomEntry();
endfunction

function NPC_GetDeathSound( source )

	var source_effects := NPC_GetNPCConfig( source.npctemplate );
	if( !source_effects )
		return 0;
	endif

	var death_sound_list := GetConfigStringArray( source_effects, "DeathSound" );
	if( TypeOf( death_sound_list ) != "Array" )
		return 0;
	endif

	return death_sound_list.RandomEntry();
endfunction

function NPC_GetHitGraphic( source )

	var source_effects := NPC_GetNPCConfig( source.npctemplate );
	if( !source_effects )
		return 0;
	endif

	var hit_graphic_list := GetConfigStringArray( source_effects, "AttackHitGraphic" );
	if( TypeOf( hit_graphic_list ) != "Array" )
		hit_graphic_list := array{4, 5, 6}; //Hit most popular movment
	endif

	return hit_graphic_list.RandomEntry();
endfunction

function NPC_GetHitSound( source )

	var source_effects := NPC_GetNPCConfig( source.npctemplate );
	if( !source_effects )
		return 0;
	endif

	var hit_sound_list := GetConfigStringArray( source_effects, "AttackHitSound" );

	if( TypeOf( hit_sound_list ) != "Array" )
		return 0;
	endif

	return hit_sound_list.RandomEntry();
endfunction

function NPC_GetIdleGraphic( source )

	var source_effects := NPC_GetNPCConfig( source.npctemplate );
	if( !source_effects )
		return 0;
	endif

	var idle_graphic_list := GetConfigStringArray( source_effects, "IdleGraphic" );
	if( TypeOf( idle_graphic_list ) != "Array" )
		idle_graphic_list := array{17}; //Idle most popular movment
	endif

	return idle_graphic_list.RandomEntry();
endfunction

function NPC_GetIdleSound( source )

	var source_effects := NPC_GetNPCConfig( source.npctemplate );
	if( !source_effects )
		return 0;
	endif

	var idle_sound_list := GetConfigStringArray( source_effects, "IdleSound" );
	if( TypeOf( idle_sound_list ) != "Array" )
		return 0;
	endif

	return idle_sound_list.RandomEntry();
endfunction

function NPC_GetMissSound( source )

	var source_effects := NPC_GetNPCConfig( source.npctemplate );
	if( !source_effects )
		return 0;
	endif

	var miss_sound_list := GetConfigStringArray( source_effects, "AttackMissSound" );
	if( TypeOf( miss_sound_list ) != "Array" )
		return 0;
	endif

	return miss_sound_list.RandomEntry();
endfunction
