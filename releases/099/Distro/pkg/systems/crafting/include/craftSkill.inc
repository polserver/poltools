/*
        Created by *Edwards
        
        2009-11-06
        
        Last Modifications 2009-11-19
             * Added material difficulty
*/
use uo;
use util;

include "include/string";
include ":attributes:attributes";
include ":classes:classes";
include ":reputation:reputation";

const CRAFT_NO_ADV := 0x1;
const CRAFT_ADV_ALL := 0x2;

const ARMOR_QUALITY_DIVIDER := 3.5;
const WEAPON_QUALITY_DIVIDER := 5;

function Craft_SkillCheck( who, elem, advance:=CRAFT_NO_ADV, material_difficulty:=0 )

        var who_skill := AP_GetSkill( who, craft_skill );
        if( !who_skill )
                 who_skill := 0;
        endif

	var difficulty := CDbl( elem.Difficulty );
        if( material_difficulty > difficulty )
                difficulty := material_difficulty;
        endif
        
        var pass_success;
	if( advance & CRAFT_ADV_ALL )
		pass_success := SkillCheck( who, craft_skill, difficulty );
        else
        	pass_success := CalcSuccessPercent( who, craft_skill, difficulty, who_skill );
	endif
        
	return pass_success;
endfunction

function Craft_ExpCheck( who, elem, the_item:=0 )

        var who_skill := AP_GetSkill( who, craft_skill );
        if( !who_skill )
                 who_skill := 0;
        endif

	var difficulty := CDbl( elem.Difficulty ),
	    classe := GetClasseIdForSkillName( craft_skill ),
            pass_success := CInt(( who_skill - difficulty + 20 ) / 10 );
     
        if( GetObjProperty( who, classe ))
                pass_success *= ClasseBonus( who, classe );
        endif
        
        if( pass_success < 1 )
                pass_success := 1;
        endif
        
	if( the_item )
                var useOn_objtype := GetObjProperty( the_item, "material" ),
                    useOn_elem := itemConfig[useOn_objtype],
                    base_quality := CDbl( useOn_elem.Quality ),
                    quality := GetQuality( who, classe, elem, base_quality );
                    
                if( useOn_objtype && base_quality )
			if( the_item.IsA( POLCLASS_WEAPON ))
				the_item.maxhp_mod := CInt( CDbl( the_item.maxhp / WEAPON_QUALITY_DIVIDER ) * quality );
				the_item.hp := the_item.maxhp + the_item.maxhp_mod;
				//Not applying dmg_mod here because we want player hunting for good weapons...
			elseif( the_item.IsA( POLCLASS_ARMOR ))
				the_item.maxhp_mod := CInt( CDbl( the_item.maxhp / ARMOR_QUALITY_DIVIDER ) * quality );
				the_item.hp := the_item.maxhp + the_item.maxhp_mod;
			endif
                endif
                
		if( RandomInt( 100 ) >= CInt( pass_success ))
			return 0;
		endif
  
                if( useOn_objtype && base_quality )
                	if( the_item.IsA( POLCLASS_ARMOR ))
				the_item.ar_mod := CInt( CDbl( the_item.ar / ARMOR_QUALITY_DIVIDER ) * quality );
			endif
                endif

                var name := the_item.name;
                if( !name )
                	name := the_item.desc;
                endif
                
		if( elem.Exceptional )
			SetName( the_item, "an exceptional "+TruncateArticle( name ));
		endif
  
		if( elem.Mark == 1 && makermark == 1 )
			SetObjProperty( the_item, "ExceptionalType", menu_config[craft_skill].ExceptionalName );
			SetObjProperty( the_item, "Exceptional", who.serial );
		endif

		SendSysMessage( who, "This masterpiece will make you famous around the world!" );
		Award_Reputation( who, FAME, CInt( difficulty ));
	endif

	return pass_success;
endfunction

function GetQuality( who, classe, elem, base_quality )

	var who_aid := CInt( AP_GetSkill( who, elem.AidSkill ) / 10 ) + 5;
	if( who_aid < 1 )
		who_aid := 1;
	endif

	var quality := CDbl( base_quality * (( ClasseBonus( who, classe ) * who_aid ) / 20 ));
	if( quality < base_quality )
		quality := base_quality;
	endif
 
	return quality;
endfunction
