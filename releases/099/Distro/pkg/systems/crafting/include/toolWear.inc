/*
        Maintened by *Edwards

        2009-11-22
*/
use uo;
use os;
use util;

include ":attributes:attributes";

function CheckToolWear( who, tool, craft_skill )

        if( GetObjProperty( tool, "IsGMItem" ))
                return 0;
        endif
                
	var swings := CInt( GetObjProperty( tool, "swings" ));
	if( !swings )
                swings := 100;
        endif

	if( swings <= 1 )
		DestroyItem( tool );
		SendSysMessage( who, "Your tool breaks!" );
	else
		var skill := AP_GetSkill( who, craft_skill );
		if( skill < 70 )
			skill := 70;
		endif
  
  
		if(( RandomInt( 100 ) + 1 ) > skill )
			SendSysMessage( who, "You damage your tool!" );
			swings -= 1;
		endif
	endif
 
	SetObjProperty( tool, "swings", swings );
 
        return 1;
endfunction
