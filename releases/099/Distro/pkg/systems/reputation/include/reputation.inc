/*
        Maintened by *Edwards
        For FantasiaShard.com
        
        Edwards@FantasiaShard.com
        
        2008-04-19
        
        Last Modifcations 2010-02-22
             * Cleaned codes for better performaces
             * Changed: karma/fame PvM will based on npc dexterity!
*/
use uo;
use math;
use util;

include ":attributes:attributes";
include ":brainAI:npcUtil";

const FAME := "Fame";
const KARMA := "Karma";
const FACTOR := 0.1;

const FAME_LOST_ON_DEATH := -100;

function GetKarmaFameName( mobile )

	var KF_Matrix := {
	//Karma\Fame  0              1250            2500         5000             10000
	/* -10000 */ {"Outcast",     "Wretched",     "Nefarious", "Dread",         "Dread Lord/Lady"        },
	/*  -9999 */ {"Despicable",  "Dastardly",    "Wicked",    "Evil",          "Evil Lord/Lady"         },
	/*  -4999 */ {"Scoundrel",   "Malicious",    "Vile",      "Villainous",    "Dark Lord/Lady"         },
	/*  -2499 */ {"Unsavory",    "Dishonorable", "Ignoble",   "Sinister",      "Sinister Lord/Lady"     },
	/*  -1249 */ {"Rude",        "Disreputable", "Notorious", "Infamous",      "Dishonored Lord/Lady"   },
	/*   -624 */ {"",            "Notable",      "Prominent", "Renowned",      "Lord/Lady"              },
	/*   +625 */ {"Fair",        "Upstanding",   "Reputable", "Distinguished", "Distinguished Lord/Lady"},
	/*  +1250 */ {"Kind",        "Respectable",  "Proper",    "Eminent",       "Eminent Lord/Lady"      },
	/*  +2500 */ {"Good",        "Honorable",    "Admirable", "Noble",         "Noble Lord/Lady"        },
	/*  +5000 */ {"Honest",      "Commendable",  "Famed",     "Illustrious",   "Illustrious Lord/Lady"  },
	/* +10000 */ {"Trustworthy", "Estimable",    "Great",     "Glorious",      "Glorious Lord/Lady"     }
        };

	var Karma_Value := Get_Reputation( KARMA, mobile );
	var Fame_Value  := Get_Reputation( FAME, mobile );

	var KF_Title := KF_Matrix[ GetReputationID( KARMA, Karma_Value ) ][ GetReputationID( FAME, Fame_Value )];

	if( Karma_Value < -3000 )
		mobile.SetCriminal(0);
		mobile.SetMurderer(1);

	elseif( Karma_Value < -750 )
 		mobile.SetMurderer(0);
		mobile.SetCriminal(1);
	else
		mobile.SetMurderer(0);
	endif

	if( KF_Title["Lord/Lady"] )
		if( mobile.gender == 0 )
			KF_Title["Lord/Lady"] := "Lord";
		else
			KF_Title["Lord/Lady"] := "Lady";
		endif
	endif

	return KF_Title;
endfunction

function GetReputationID( reputation, value )

        var KF_array,
            index;

        if( reputation == FAME );
	        KF_array := { 0, 1250, 2500, 5000, 10000 };
        elseif( reputation == KARMA )
 	        KF_array := { -30000, -9999, -4999, -2499, -1249, -625, +625, +1250, +2500, +5000, +10000 };
	endif

        index := len(KF_array);
	while (( index > 1 ) && ( value < KF_array[index] ))
		index -= 1;
                SleepMS(5);
        endwhile

	return index;
endfunction

function Award_Reputation( mobile, reputation, value )

	var reputation_value := Get_Reputation( reputation, mobile );
	if( !reputation_value )
		reputation_value := 0;
	endif

        var speech := "";
	if( value > 0 ) // Gain
		if( value < 50 )
			speech := "tiny";
		elseif( value < 100 )
			speech := "small";
		elseif( value < 300 )
			speech := "good";
		elseif( value < 600 )
			speech := "very good";
		elseif( value < 1000 )
			speech := "great";
		else
			speech := "large";
		endif

                SendSysMessage( mobile, "You have gained a "+speech+" amount of "+reputation+"!" );
                
	elseif( value < 0 ) // Lose
		if( value > -50 )
			speech := "tiny";
		elseif( value > -100 )
			speech := "small";
		elseif( value > -300 )
			speech := "good";
		elseif( value > -600 )
			speech := "very good";
		elseif( value > -1000 )
			speech := "great";
		else
                        speech := "large";
		endif

                SendSysMessage( mobile, "You have lost a "+speech+" amount of "+reputation+"!" );
	endif

	Set_Reputation( mobile, reputation, reputation_value+value );
 
        return 1;
endfunction

function Set_Reputation( mobile, reputation, value )

	if( mobile.isA( POLCLASS_NPC ) || mobile.cmdlevel || !value )
               return 0;
        endif

	if( value > 10500 )
		value := 10500;
	elseif( value < -10500 )
		value := -10500;
	endif
 
        if( reputation == KARMA )
                var Old_Karma := Get_Reputation( KARMA, mobile );
                
                if( value < -3000 )
			if( Old_Karma >= -3000 )
				SendSysMessage( mobile, "For all yours sins, you're now considered as a murderer!" );
			endif
		elseif( value < -750 )
			if( Old_Karma >= -750 )
				SendSysMessage( mobile, "From now, you're considered as a criminal!" );
			elseif( Old_Karma < -3000 )
				SendSysMessage( mobile, "You slowly purge yours sins, you're now considered as a criminal!" );
			endif
		else
			if( Old_Karma < -750 )
				SendSysMessage( mobile, "You are no longer a criminal!" );
			endif
		endif
        endif

	SetObjProperty( mobile, reputation, value );
        SetNewNotoName( mobile );
 
        return 1;
endfunction

function SetNewNotoName( mobile )

	var prefix := GetKarmaFameName( mobile );

	if( len( prefix ) > 0 )
		mobile.title_prefix := "The "+prefix+" ";
	else
		mobile.title_prefix := "";
	endif

        return 1;
endfunction

function Get_Reputation( reputation, object )

	var value := GetObjProperty( object, reputation );
        if( !value )
               value := 0;
        endif
        
        var npc_template := GetObjProperty( object, "npctemplate" );
        if( npc_template )
                var npc_cfg := NPC_GetNPCConfig( npc_template ),
                    strength_mod := CInt( npc_cfg.Strength / 25 );
                    
                value := RandomDiceRoll( "1d50+"+strength_mod );
                    
                if( npc_cfg.alignment == "evil" || npc_cfg.alignment == "neutral" )
                        strength_mod := - strength_mod;
                endif
        endif

	return value;
endfunction

function AffectKarmaAndFameForKill( defender, attacker )

        if( attacker.IsA( POLCLASS_NPC ))
                return 0;
        elseif( defender.serial == attacker.serial )
                return 0;
        elseif( GetObjProperty( defender, "Master" ) == attacker.serial )
                return 0;
        endif

	// Award the fame if you killed something more famous than you, if so award fame
        var attacker_fame := Get_Reputation( FAME, attacker ),
            defender_fame := Get_Reputation( FAME, defender ),
	    award_fame := attacker_fame - defender_fame;
        
	if( award_fame > 0 )
		award_fame *= FACTOR;
		if( award_fame != 0 )
  			Award_Reputation( attacker, FAME, award_fame );
		endif
	endif

	var attacker_karma := Get_Reputation( KARMA, attacker ),
	    defender_karma := Get_Reputation( KARMA, defender ),
            award_karma;

        //Made me sick for 4 consecutive hours.
        if( attacker_karma >= 0 )
                 if( defender_karma > 0 )
                         award_karma := ( attacker_karma - defender_karma ) * FACTOR;
                         if( award_karma > 0 )
                                 award_karma := 0;
                         endif
                 else
                         award_karma := ( Abs( defender_karma ) - attacker_karma ) * FACTOR;
                         if( award_karma < 0 )
                                 award_karma := 0;
                         endif
                 endif
        else
                 if( defender_karma >= 0 )
                         award_karma := ( -attacker_karma - defender_karma ) * FACTOR;
                         if( award_karma > 0 )
                                 award_karma := 0;
                         endif
                 elseif( attacker_karma > defender_karma )
                         award_karma := ( attacker_karma - defender_karma ) * FACTOR;
                         if( award_karma < 0 )
                                 award_karma := 0;
                         endif
                 endif
        endif

        if( award_karma != 0 )
                 Award_Reputation( attacker, KARMA, award_karma );
        endif
        
        return 1;
endfunction
