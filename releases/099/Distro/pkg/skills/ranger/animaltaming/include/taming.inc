/*
        Created by *Edwards

        2009-12-22
*/
use uo;

include ":attributes:attributes";
include ":classes:classes";
include ":timedscripts:timedScripts";

function tameCreature( npc, mobile )

        var delay := CInt( AP_GetSkill( mobile, ANIMAL_LORE ) + 1 ) * 50;
        delay *= ClasseBonus( mobile, RANGER );

        if( delay < 1500 )
                delay := 1500;
        endif
        
        TS_StartTimer( npc, "Tame", CInt( delay ), 0, mobile );
        
        return 1;
endfunction

function GetMaxPets( mobile )

         var petmax := CInt( AP_GetSkill( mobile, ANIMAL_LORE ) / 15 );
         petmax *= ClasseBonus( mobile, RANGER );
         
         return CInt( petmax + 1 );
endfunction

function CheckPet( mobile )

         var max_pets := GetMaxPets( mobile );
         var num_pets := CountPet( mobile );
         
         if( mobile.cmdlevel )
                  //That doesn't matter if its a staff
                  return 1;
         elseif( num_pets > max_pets && num_pets > 2 )
                  return 0;
         endif

         return 1;
endfunction

function CountPet( mobile )

         var num_pets := 0;

         var mobs := ListMobilesNearLocation( mobile.x, mobile.y, mobile.z, 20, mobile.realm );
         foreach creature in mobs
                  if( creature.master && mobile == creature.master )
                           num_pets += 1;
                  endif
                  SleepMS(5);
         endforeach
         
         return num_pets;
endfunction
