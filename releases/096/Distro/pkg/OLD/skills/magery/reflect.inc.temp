use uo;
use os;

include "include/eventID";
include "include/sound";
include "include/tileEffects";


function spell_reflect(cast_on)
	var mr := CInt(GetObjProperty(cast_on, "mr"));
	if (mr >= 1)
		PlaySoundEffect(cast_on, SFX_SPELL_MAGIC_REFLECTION);
		PlayObjectCenteredEffect(cast_on, FX_CURSE_EFFECT, 10,10);
		SendSysMessage(cast_on, "Your magic reflect spell saved you!");
		if (mr == 1)
			EraseObjProperty(cast_on, "mr");
		elseif (mr == 2)
			var holder, pid;
			var eqp := GetObjProperty(cast_on, "EquipTimers");
			foreach thing in ListEquippedItems(cast_on)
				foreach entry in eqp
					if ((thing.serial == entry[1]) and (entry[2] == "ref"))
						holder := GetObjProperty(thing, "pid");
						break;
					endif
				endforeach
			endforeach
			pid := getprocess(holder);
			if (pid)
				var k := struct;
				k.+type := EVID_DISCHARGE;
				pid.sendevent(k);
			else
				EraseObjProperty(cast_on, "mr");
			endif
		endif
		return 1;
	endif

	return 0;
endfunction
