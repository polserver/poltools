/*
        Created by *Edwards

        2010-07-06
*/
use uo;

include ":datafile:datafile";

var capskills_datafile := DFOpenDataFile( ":attributes:capskills", DF_CREATE );

function AP_GetSkillsCap()

	var cfg_elem := AP_GetSettingsCfgElem( "Skills" );
	var cap_val := GetConfigReal( cfg_elem, "SkillCap" );

	return CInt( cap_val );
endfunction

function SetNewSkillCaps( who )

         var cap_array := {},
             attributes := AP_ListAttributesByType( "Skill" );

         foreach skill in attributes
                  var skill_id := AP_AttributeNameToSkillId( skill );
		  cap_array[skill_id+1] := AP_GetSkillsCap();
	 endforeach
  
         var data_elem := DFFindElement( capskills_datafile, who.serial, DF_CREATE );
         data_elem.SetProp( "Caps", cap_array );

         return cap_array;
endfunction

function AP_GetDataSkillsCap( who )

        var data_elem := DFFindElement( capskills_datafile, who.serial, DF_CREATE );
	var caps := data_elem.GetProp( "Caps" );
        if( !caps )
                caps := SetNewSkillCaps( who );
        endif

	return caps;
endfunction

function AP_GetSkillCap( who, skill_name )

	var caps := AP_GetDataSkillsCap( who ),
            skill_id := AP_AttributeNameToSkillId( skill_name );

	return CInt( caps[skill_id+1] );
endfunction

function AP_SetSkillCap( who, skill_name, points )

	var caps := AP_GetDataSkillsCap( who ),
            skill_id := AP_AttributeNameToSkillId( skill_name );

        caps[skill_id+1] := CInt( points );
        var skill_cap := caps[skill_id+1];
        if( skill_cap > 150 )
                caps[skill_id+1] := 150;
        elseif( skill_cap <= 0 )
                caps[skill_id+1] := 0;
        endif

        var data_elem := DFFindElement( capskills_datafile, who.serial, DF_CREATE );
        data_elem.SetProp( "Caps", caps );
        
	return 1;
endfunction

function AP_GetStatCap( skill_name )

	var cfg_elem := AP_GetSettingsCfgElem( "Stats" );

	// Check if there is a cap for that particular skill.
	var cap_val := GetConfigReal( cfg_elem, skill_name );

	return cap_val;
endfunction
