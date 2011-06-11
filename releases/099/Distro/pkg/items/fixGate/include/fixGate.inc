/*
        Created by *Edwards
        
        2010-11-02
*/
use uo;
use os;

include ":attributes:attributes";
include ":timedScripts:timedScripts";

function FixCharacterMods( mobile )

        var list_items := 0;
        foreach item in ListEquippedItems( mobile )
                SleepMS(5);
                if( item == mobile.backpack )
                        continue;
                elseif( item.IsHair() )
                        continue;
                elseif( item.IsBeard() )
                        continue;
                elseif( item.IsTalisman() )
                        continue;
                endif

                list_items := 1;
        endforeach

        if( list_items )
                //Do not take risk and abord process now.
                return 0;
        endif

        WipeMods( mobile );

	foreach attrib_name in AP_ListAttributesByType()
		AP_SetSkillMod( mobile, attrib_name, 0 );
		SleepMS(10);
	endforeach
        
        return 1;
endfunction

function WipeMods( mobile )

        var timers := { "Clumsy", "FeebleMind", "Weaken", "Agility", "Cunning", "Protection",
                        "Strength", "Bless", "Curse", "Polymorph", "EarthBless", "Liche", "Wraith",
                        "ReactiveArmor" };

        foreach timer in timers
                TS_LowerDuration( mobile, timer, -1 );
                SleepMS(5);
        endforeach

        mobile.ar := 0;
        mobile.ar_mod := 0;

        return 1;
endfunction
