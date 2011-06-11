/*
        Created by *Edwards

        2008-04-07
*/
use uo;

include ":attributes:attributes";

function Set_NewSkill_Caps( mobile )

         var raisecap_array := {};
         SetObjProperty( mobile, "RaiseCap", raisecap_array );
         
         var attributes := AP_ListAttributesByType( "Skill" );
         
         var get_caps := GetObjProperty( mobile, "RaiseCap" );
         
         var skill_id;
         foreach skill in attributes
                  skill_id := AP_AttributeNameToSkillId( skill );
		  get_caps[skill_id] := 0;
	 endforeach
  
         SetObjProperty( mobile, "RaiseCap", get_caps );
         
         return 1;
endfunction
