/*
        Maintened by *Edwards

        2009-02-15
        
        Last Modifications 2009-12-10
             * Fixed crash with wrong parameter..
             * Changed skill value: if merchant has skill > 30 he will train to 33 for 330gp

        Last Modifications 2010-03-18
             * Changed: price is based on skill_left to 33.
*/
use uo;
use util;

include ":attributes:attributes";

function MerchantTrain( npc, source, text )

	text := SplitWords( text );

	// The third word is the skill we are looking for
	var skill := text[3],
            npc_skill := AP_GetSkill( npc, skill ),
            source_skill := AP_GetSkill( source, skill );

	if( !skill )
		var trainable := array{};

	        foreach attrib_name in AP_ListAttributesByType( "skill" )
			var npc_skill := AP_GetSkill( npc, attrib_name );
                        if( npc_skill >= 30 && AP_GetSkill( source, attrib_name ) < 30 )
                        	trainable.Append( attrib_name );
                        endif
		        SleepMS(5);
	        endforeach

		if( trainable.Size() > 0 )
                        AI_Speak( npc, "I can train thee in the following skills:" );
                        Sleep(1);
                        foreach attrib_name in trainable
                        	AI_Speak( npc, attrib_name );
                                SleepMS(5);
                        endforeach
		else
                        AI_Speak( npc, "I can not train thee." );
                        return 0;
		endif
	else
                if( npc_skill >= 30 && source_skill < 30 )
                        var price := CInt(( 33 - AP_GetTrueSkill( source, skill )) * 10 );
                        AI_Speak( npc, "I will train thee 33.0 for "+price+" golds." );
                        SetObjProperty( source, "#VendorTrain", skill );
                else
                        AI_Speak( npc, "I cannot teach you "+skill+"." );
                        return 0;
                endif
	endif

	return 1;
endfunction

function TrainSkill( npc, byref source, byref item )

	var train_skill := GetObjProperty( source, "#VendorTrain" );
	if( !train_skill )
		AI_Speak( npc, "I don't know what this is for, but thanks!" );
		MoveItemToContainer( item, source.backpack );
		return 0;
	endif
         
	var npc_skill := AP_GetSkill( npc, train_skill ),
            source_skill := AP_GetTrueSkill( source, train_skill );

	if( npc_skill >= 33 && source_skill < 33 )
                var train_value := CInt( item.amount / 10 );
                if( !train_value )
			AI_Speak( npc, "You do not have enough money!" );
		        return 0;
                elseif( train_value >= 33 )
                	train_value := 33;
                endif
                
                SubtractAmount( item, CInt( train_value * 10 ));
                
                AI_Speak( npc, "Let me show you how it's done!" );
                Sleep(1);

                var new_skill := CInt( source_skill + train_value );
                if( new_skill > 33 )
                	new_skill := 33;
                endif
                
		AP_SetTrueSkill( source, train_skill, new_skill );

                EraseObjProperty( source, "#VendorTrain" );
		MoveItemToContainer( item, source.backpack );
	else
                AI_Speak( npc, "I cannot teach you "+train_skill+"." );
                MoveItemToContainer( source.backpack, item );
                return 0;
	endif

	return 1;
endfunction
