/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-02-09
*/

include ":attributes:attributes";

function AP_SkillIdToAttributeName( skill_number )

	var skills_cfg := ReadConfigFile( ":attributes:uoskills" );
	if( skills_cfg.errortext )
		SysLog( "Error::AP_IDToAttributeName() - Unable to open :attributes:uoskills.cfg ->"+skills_cfg.errortext );
	endif

	var cfg_elem := skills_cfg[skill_number];
	if( cfg_elem.errortext )
		SysLog( "Error::AP_IDToAttributeName() - unable to find cfg elem ["+skill_number+"] ->"+cfg_elem.errortext );
	endif

        return cfg_elem.Attribute;
endfunction

function AP_AttributeNameToSkillId( attribute_name )

	var cfg_elem := AP_GetAttributeCfgElem( attribute_name );

	return cfg_elem.SkillId;
endfunction
