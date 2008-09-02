/* $Id: begging.inc 950 2007-01-17 16:44:49Z sroyalty $
 *
 */

var Pleade := { "Spare some change?",
		"Alms for the poor?",
		"Donate to my charity?",
		"Wanna buy a rock?",
		"C'mon...   please?",
		"Spare a few gold?"
	      };

var Failed := { "I think not",
		"Go away!",
		"Off with thee, peasant!",
		"Thou dost not look trustworthy... No gold for thee today!",
		"I feel sorry for thee... Thou dost not look trustworthy... No gold for thee today!",
		"Ye wont be getting any gold from me today."
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
		"Does it look like I have pockets in this armor?",
		"Not while I'm on duty."
	    };

var NegKarma := { "I will not give money to such as thee!",
		  "Take off, eh?",
		  "I do not give money to naughty evildoers!",
		  "Thou dost not look trustworthy... no gold for thee today!"
		};


function begpurse(me)
  var amt := Cint(GetObjProperty(me, "begpurse"));
  if(!amt)
    amt := 50;
  endif
  var cnfg := ReadConfigFile("::npcdesc");
  var wealth := cnfg[me.npctemplate].wealth;
  var cap := 50;
  case(wealth)
    "upper":  cap := 150;
    "middle": cap := 100;
    "poor":   cap := 50;
    "broke":  cap := 0;
  endcase
  if(amt < cap)
    amt := amt + 1;
    SetObjProperty(me, "begpurse", amt);
  endif
endfunction


function PrintBeggingMessage( speaker, begMessage )

	if(begMessage.size())
		PrintTextAbove(speaker, begMessage[RandomInt(begMessage.size())+1]);
	endif
	return 1;

endfunction
