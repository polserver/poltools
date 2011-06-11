/*
        Maintened by *Edwards
        
        2009-08-17
*/

var immunities := { "AirProtection",
                    "EarthProtection",
                    "FireProtection",
                    "WaterProtection",
                    "NecroProtection",
                    "PermPoisonProtection",
                    "PermMagicProtection",
                    "PhysicalProtection",
                    "PermSpellReflection",
                    "HealingIncrease"
		  };
    
var wardings := { "PoisonProtection",
                  "MagicProtection",
                  "SpellReflection",
                  "FreeAction"
                };
    
var regenerations := { "HitsRegen",
                       "StamRegen",
                       "ManaRegen"
                     };

function BanishCurse( item )

	EraseObjProperty( item, "Cursed" );
	NameByEnchant( item );

        return 1;
endfunction

