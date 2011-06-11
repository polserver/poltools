/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-19
*/
use uo;
use cfgfile;

include ":brainAI:npcUtil";

function MakeBleed( object, blood_color:=0 )

	// These checks are for players or individual NPCs.
	if( GetObjProperty( object, "NoBlood" ))
		return 0;
	elseif( !blood_color )
		blood_color := CInt( GetObjProperty( object, "BloodColor" ));
	endif
	
	if( object.npctemplate )
		// Living NPC
		var npc_info := NPC_GetNPCConfig( object.npctemplate );
		if( !npc_info.NoBlood )
			// NPC is set to not bleed.
			return 0;
		else
			if( !blood_color )
				// If no blood color is set - retrieve NPC's template blood color.
				blood_color := CInt( npc_info.BloodColor );
			endif
		endif
	elseif( object.IsA( POLCLASS_CORPSE ))
 
		// If it is a player corpse, then the CProp checks at the begining
		// of this function have taken care of what is needed. 
		// However, if it is an NPC corpse - the color needs to be retrieved from the npcdesc.cfg
  
		var template := object.NPCTemplate();
		if( template )
			var npc_info := NPC_GetNPCConfig( template );
			
			if( !npc_info.NoBlood )
				//Does this npc bleed?
				return 0;
			else
				if( !blood_color )
					//Does this npc have a special blood color?
					blood_color := CInt( npc_info.BloodColor );
				endif
			endif
		endif
	endif
	
	var blood := CreateItemAtLocation( object.x, object.y, object.z, "blood", 1, object.realm );
        if( blood.errortext )
                PrintTextAbove( object, "Error creating blood -->"+blood.errortext );
                return 0;
        endif
        
	blood.color := blood_color;
 
	return blood;
endfunction
