/*
        Created by *Edwards

        Last Modifications 03-31-2011

        CONST settings:
                BIT_MASK  = 0x00000003 DO NOT CHANGE!

         Global Variables:
                STATCAP = Set this to your shard's Stat Cap. If based on age, race, etc, just add a function
                          at the beginning of the exported function to check and set the statcap variable.
                          Otherwise use the new UOClient.cfg statcap entry for a static amount.

                VAR_STATCAP = If your statcap is not static, set this to 1. This will require you to populate
                          the GetStatCap() function to build the statcap amount based on your customs.

                MODE = The mode of the Status Packet. Populated by the Settings Configuration file in the pkg.
                       This is used to know how to edit the packet for AOS extended stuff.

                STATLOCK = If Statelock is enabled. sends the Stat Lock feature to the client. Requires
                           another package to handle recieving the statlock packets back from client.
*/
use uo;

include "include/client";
include ":animaltaming:taming";
include ":combat:critical";
include ":damage:damage";
include ":equipment:protections";
include ":weapons:weaponInfo";

CONST BIT_MASK := 0x00000003;

//    BYTE[2]  statcap
//    BYTE     pets current
//    BYTE     pets max
//    BYTE[2]  fireresist
//    BYTE[2]  coldresist
//    BYTE[2]  poisonresist
//    BYTE[2]  energyresist
//    BYTE[2]  luck
//    BYTE[2]  damage max
//    BYTE[2]  damage min
//    BYTE[4]  titching points

function Send_StatCap( byref packet )

         packet.SetInt8( 42, 4 );
         packet.SetInt16( 66, 390 );
                
         return 1;
endfunction

function Send_CurrentPetInfos( byref packet, character )

         // Sets their current pet count in packet
         packet.SetInt8( 68, CountPet( character ));
         
         // Sets their current pet max count based on taming in packet
         packet.SetInt8( 69, GetMaxPets( character ));

         return 1;
endfunction

function Send_ResistInfos( byref packet, character )

         // Set the resistances
         // If negative, it's 65536 plus the resistance. -10 would be 65536 + -10
         
         var resistances := GetObjProtections( character );

         var fire := CInt( resistances["FireProtection"] );
         if( fire > 0 )
                  packet.SetInt16( 70, fire );
         else
                  packet.SetInt16( 70, 0x10000 + fire );
         endif

         var cold := CInt( resistances["WaterProtection"] );
         if( cold > 0 )
                  packet.SetInt16( 72, cold);
         else
                  packet.SetInt16( 72, 0x10000 + cold );
         endif

         var poison := CInt( resistances["PermPoisonProtection"] );
         if( poison > 0 )
                  packet.SetInt16( 74, poison );
         else
                  packet.SetInt16( 74, 0x10000 + poison );
         endif

         var energy := CInt( resistances["AirProtection"] );
         if( energy > 0 )
                  packet.SetInt16( 76, energy );
         else
                  packet.SetInt16( 76, 0x10000 + energy );
         endif
  
         return 1;
endfunction

function Send_Luck( byref packet, character )

         // NOTE**** Just like the statcap, if it is over 255, have the if check statcap uses!!
         // Set the luck in the packet
         
         var luck := getLuck( character );
         if( luck > 0 )
                  packet.SetInt16( 78, luck );
         else
                  packet.SetInt16( 78, 0x10000 + luck );
         endif

         return 1;
endfunction

function Send_DamageInfos( byref packet, character )

         var weap_dmg := CS_GetDamage( character.weapon );

         // Set the min damage in the packet
         packet.SetInt8( 81, weap_dmg.min_dmg );

         // Set the max damage in the packet
         packet.SetInt8( 83, weap_dmg.max_dmg );

         return 1;
endfunction

function Send_TitchingPoints( byref packet )

         /*Set the tithing points in the packet
         var tithing := 0;
         if ( tithing > 0 )
                  packet.SetInt16(86, tithing);
         else
                  packet.SetInt16(86, 0x10000 + tithing);
         endif*/
  
         return 1;
endfunction
