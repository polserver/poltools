/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2008-10-24
*/
use uo;
use os;
use cfgfile;

include ":spells:spells";

function SPELL_SpellNameToId( spell_name )

	var spellex_cfg := ReadConfigFile( ":spells:spells_ex" );
	var cfg_elem := spellex_cfg["NameToId"];
	
	return GetConfigInt( cfg_elem, spell_name );
endfunction

function SPELL_SpellIDToScrollName( scroll_id )

        var spells_cfg := SPELL_GetSpellsCfgFile(),
            spell_elem := spells_cfg[scroll_id];

        return CStr( spell_elem.Name );
endfunction

function SPELL_SpellIDToScrollGraphic( scroll_id )

        var spells_cfg := SPELL_GetSpellsCfgFile(),
            spell_elem := spells_cfg[scroll_id];

        return CInt( spell_elem.TileGraphic );
endfunction

function SPELL_SpellIDToScrollObjType( scroll_id )

        var spells_cfg := SPELL_GetSpellsCfgFile(),
            spell_elem := spells_cfg[scroll_id];

        return CInt( spell_elem.ScrollObjType );
endfunction
