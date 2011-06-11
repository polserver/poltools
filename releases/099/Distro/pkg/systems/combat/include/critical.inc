/*
        Made by *Edwards
        
        Last Modifications 2010-02-26
             * Cleaned codes for better performances

        Great thanks to ZuluHotel, UnrealLegend and PolDistroTeam.
        
*/
use uo;
use os;
use cfgfile;

function CriticalHit( attacker, defender, rawDamage )

        // Only call CriticalHit if attacker is player
        if( attacker == defender )
                return rawDamage;
        elseif( attacker.IsA( POLCLASS_NPC ))
                return rawDamage;
        elseif( defender.multi || attacker.multi )
                return rawDamage;
        endif

        var base_chances := getLuck( attacker );
        if( RandomDiceRoll( "1d1000" ) <= base_chances )

                rawDamage += RandomDiceRoll( CInt( rawDamage / 10 )+"d3+10" );
                
                if( defender.IsA( POLCLASS_NPC ))
                        return CDbl( rawDamage );
                endif
                
                var active_quote := GetObjProperty( attacker, "DisableQuote" );
                if( !active_quote )
		        var cfg_file := ReadConfigFile( ":combat:quotes" );
		        if( cfg_file.errortext )
			        SendSysMessage( attacker, "Error couldn't read config <:combat:quotes> -->"+cfg_file.errortext );
                	        return 0;
		        endif

        	        var cfg_elem := FindConfigElem( cfg_file, "Quotes" );
		        if( cfg_elem.errortext )
			        SendSysMessage( attacker, "Error couldn't find elem Quotes in <:combat:quotes> -->"+cfg_elem.errortext );
                	        return 0;
		        endif

                        var elems := GetConfigStringArray( cfg_elem, "Quote" ),
                            randomMessage := elems.RandomEntry();

                        PrintTextAbove( attacker, randomMessage, 3, 33 );
                endif
        endif

        return CDbl( rawDamage );
endfunction

function getLuck( attacker )

        var base_chances := 5;
        var classe_name := Get_Mobile_Classe_Name( attacker );
        if( classe_name )
                base_chances *= GetObjProperty( attacker, classe_name );
        endif
        
        return CInt( base_chances );
endfunction
