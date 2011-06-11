/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2008-10-12
*/

include ":brainAI:npcCommands";

var Pleade := { "Spare some change?",
		"Alms for the poor?",
		"Donate to my charity?",
		"Wanna buy a rock?",
		"C'mon...   please?",
                "Give me something plz...",
                "I need something to eat!",
                "I've got four children, please help me!",
                "Could thee spare a dime?",
		"Some thieves stole me everything, I'm broken now..."
	      };

var Failed := { "I think not",
		"Go away!",
		"Off with thee, peasant!",
		"Thou dost not look trustworthy... No gold for thee now!",
		"I feel sorry for thee... Thou dost not look trustworthy... No gold for thee now!",
		"Ye wont be getting any gold from me now!"
	      };

var Broke := {	"Ain't got nothin' t' give ya!",
		"No' got nothin'. Sorry.",
		"Not much better off meself'.",
		"Maybe ye can spare a coin for me?"
	     };

var Empty := {  "Sorry, gave it all away already.",
		"Ain't got nothin' t' give ya!",
		"Nope. Gots nothing. Sorry.",
		"Thy cohorts cleaned me out already!"
	     };

var None := {	"I'm sorry, I can give thee nothing.",
		"Does it look like I have pockets?",
                "Hey!! I seem a bank?",
                "Go away, you poor beggar!",
                "Get lost!",
                "Don't bug me now!",
                "I've got nothing to give you, I'm sorry...",
                "I'm sorry but I gotta go now...",
                "You won't have ANYTHING from me!"
	    };

var NegKarma := { "I will not give money to such as thee!",
		  "Take off, eh?",
		  "I do not give money to naughty evildoers!",
		  "Thou dost not look trustworthy... no gold for thee today!"
		};
  
var Accept := {	"Take these, poor beggar...",
		"Let me help you...",
		"Use these wisely!",
                "Alright, but stop disturbing me!",
                "I wish I could give more..."
	    };

var Flee := {	"I have to go!",
		"I must leave!!",
		"no....ehm...bye!",
                "Bye!!",
                "I gotta go!",
                "I must flee!!"
	    };

function PrintBeggingMessage( speaker, beggee, begMessage )

        TurnObjectToward( speaker, beggee.x, beggee.y );
        
	if( begMessage.Size() )
                if( speaker.IsA( POLCLASS_NPC ))
			AI_Speak( speaker, begMessage.RandomEntry() );
                else
			PrintTextAbove( speaker, begMessage.RandomEntry() );
                endif
        else
		PrintTextAbove( speaker, begMessage );
	endif
 
	return 1;
endfunction
