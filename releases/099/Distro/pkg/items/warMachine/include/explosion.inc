/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2008-06-01 //Happy Birthay Momy!

        Thanks to ZuluHotel and his developers for this original Idea!
*/
use uo;
use os;

include ":damage:damage";

function FireWorks( mobile, catapult, location, type )

         var locationx,
             locationy;

         if( location.x > catapult.x+10 )
               locationx := catapult.x+10;
         else
               locationx := location.x;
         endif

         if( location.y > catapult.y+10 )
               locationy := catapult.y+10;
         else
               locationy := location.y;
         endif
         
         var effect;
         if( type == "Catapult" )
               effect := 0x1364;
         elseif( type == "Cannon" )
               effect := 0xe73;
         else
               return 0;
         endif

         var dmg := RandomDiceRoll( "10d10+40" );
         var near := ListMobilesNearLocationEx( locationx, locationy, location.z, 4, LISTEX_FLAG_NORMAL, catapult.realm );

         PlayMovingEffectXYZ( catapult.x, catapult.y, catapult.z, locationx, locationy, location.z, effect, 5, 5, 0, catapult.realm );
         PlaySoundEffect( location, 0x0220 );
             
         PlayObjectCenteredEffect( catapult, 0x3728, 14, 7 );

         foreach player in near
               PlayObjectCenteredEffect( player, 0x36b0, 7, 7 );
               ApplyDamageEX( player, dmg, DMG_PHYSICAL+DMG_FIRE, mobile );
               SleepMS(5);
         endforeach

         return 1;
endfunction
