/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///				Karma/Fame System							      ///
///				By Developer Link							      ///
///				see readme.txt for installation instructions									      ///
///													      ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function AwardKarmaFame(corpse, npctemplate)



	var hitlist := GetObjProperty (corpse, "hitlist");
	if (!hitlist)
		return;
	endif

	var npccfg := ReadConfigFile ("::npcdesc");
	var karma := cint(npccfg[npctemplate].karma);
	var fame := cint(npccfg[npctemplate].fame);
	var hostile := cint(npccfg[npctemplate].hostile);
	
	if(!karma)
	karma := 0;
	endif

	if(!fame)
	fame := 0;
	endif

// levels 
// 1 karma/fame per monster ends at 1249
// 2 "                            " 2499
// 3 "                            " 4999
// 4 "                            " 9999
// 5+ only option to reach glorious lord



	

	if (npccfg[npctemplate].alignment = "good") 
		karma:= -200;
                fame := -200;
	endif

	foreach person in hitlist
		var you := SystemFindObjectBySerial (person);
		if (you)
			if (karma or fame)
			    var yourkarma := GetObjProperty(you,"karma");
			    var yourfame := GetObjProperty(you,"fame");
	if (!yourkarma) yourkarma := 0; endif
        if (!yourfame) yourfame := 0; endif

if(karma = 1 and yourkarma > 1249)
karma := 0;
endif

if(karma = 2 and yourkarma > 2499)
karma := 0;
endif

if(karma = 3 and yourkarma > 4999)
karma := 0;
endif

if(karma = 4 and yourkarma > 9999)
karma := 0;
endif


if(fame = 1 and yourfame > 1249)
karma := 0;
endif

if(fame = 2 and yourfame > 2499)
karma := 0;
endif

if(fame = 3 and yourfame > 4999)
karma := 0;
endif

			     AddKF(you, karma, fame);
			endif
		endif
	endforeach

	EraseObjProperty (corpse, "hitlist");

endfunction





function AddKF(who, karma, fame)

	
	var yourkarma := GetObjProperty(who,"karma");
	if (!yourkarma) yourkarma := 0; endif
	if (yourkarma >= 10000) 

if(karma > 0)
karma := 0;
endif
	endif

if (yourkarma <= -10000) 

if(karma < 0)
karma := 0;
endif
	endif

	var yourfame := GetObjProperty(who,"fame");
	if (!yourfame) yourfame := 0; endif

if (yourfame >= 10000) 
if(fame > 0)
fame := 0;
endif
	endif


if (yourfame <= -10000) 
if(fame < 0)
fame := 0;
endif
	endif

var test;
var test1;
	var karma1;
	case (karma)
		0: test := 1;
		1: karma1 := " a tiny amount of ";
		2: karma1 := " a very small amount of ";
		3: karma1 := " a small amount of ";
		4: karma1 := " some ";
		5: karma1 := " a good amount of ";
		10: karma1 := " a lot of ";
		default: karma1 := " a great amount of ";
	endcase
	
	
	var fame1;
	case (fame)
		0: test1 := 1;
		1: fame1 := " a tiny amount of ";
		2: fame1 := " a very small amount of ";
		3: fame1 := " a small amount of ";
		4: fame1 := " some ";
		5: fame1 := " a good amount of ";
		10: fame1 := " a lot of ";
		default: fame1 := " a great amount of ";
	endcase

if(yourfame + fame < 0)
yourfame := 0;
fame := 0;
endif


	yourkarma := yourkarma + karma;
	yourfame := yourfame + fame;

if(!test)
if(karma > 1) 
	
	SendSysMessage(who,"You have gained" + karma1 + "karma");	
else
	
	SendSysMessage(who,"You have lost" + karma1 + "karma");
	
endif
endif


if(!test1)
if( fame > 1)
			
			SendSysMessage(who,"You have gained" + fame1 + "fame");
	else
			
			SendSysMessage(who,"You have lost" + fame1 + "fame");
endif
endif

	SetObjProperty(who,"fame",yourfame);
	SetObjProperty(who,"karma",yourkarma);
	
	
	SetMyTitle(who, yourkarma, yourfame);

endfunction



function SetMyTitle(who, yourkarma, yourfame )





var nothing_ := who.title_prefix;


if( yourkarma <= -1 and yourkarma >= -624)

     who.setcriminal(0);
     SendSysMessage(who,"You are no longer criminal");

elseif( yourkarma <= -625 and yourkarma >= -1249)

    SetObjProperty(who, "permared", 0);
   who.setmurderer(0); 
   who.setcriminal(1);
 SendSysMessage(who,"You are now a criminal");

elseif( yourkarma <= -1250 and yourkarma >= -2499)


    SetObjProperty(who, "permared", 1);
    who.setcriminal(0);
    who.setmurderer(1);
    SendSysMessage(who, "You are now a murderer");
    

elseif( yourkarma <= -2499 and yourkarma >= -4999)


  SetObjProperty(who, "permared", 1);
  who.setmurderer(1);


elseif( yourkarma <= -5000 and yourkarma >= -9999)


  SetObjProperty(who, "permared", 1);
  who.setmurderer(1);



elseif( yourkarma <= -10000)


  SetObjProperty(who, "permared", 1);
  who.setmurderer(1);


else


endif

if ( yourfame >= 0 and yourfame <= 1249 )
			if(yourkarma <= -1 and yourkarma >= -624)

				who.title_prefix := " ";
		
			elseif(yourkarma <= -625 and yourkarma >= -1249)
				who.title_prefix := "The Rude ";
			
			elseif(yourkarma <= -1250 and yourkarma >= -2499)

				who.title_prefix := "The Unsavory ";
				
			elseif(yourkarma <= -2500 and yourkarma >= -4999)
	
				who.title_prefix := "The Scoundrel ";
			
			elseif(yourkarma <= -5000 and yourkarma >= -9999)

				who.title_prefix := "The Despicable ";
			
			elseif(yourkarma >= -10000)

				who.title_prefix := "The Outcast ";
			
		        endif
	elseif(yourfame >= 1250 and yourfame <= 2499)
	
			if(yourkarma <= -625 and yourkarma >= -1249)

				who.title_prefix := "The Disreputable ";
			   
			elseif(yourkarma <= -1250 and yourkarma >= -2499)

				who.title_prefix := "The Dishonorable ";
				
			elseif(yourkarma <= -2500 and yourkarma >= -4999)

				who.title_prefix := "The Malicious ";
				
			elseif(yourkarma <= -5000 and yourkarma >= -9999)

				who.title_prefix := "The Dastardly ";
				
			elseif(yourkarma <= -10000)

				who.title_prefix := "The Wretched ";
				
			endif
	
	
elseif(yourfame >= 2500 and yourfame <= 4999)
	
			
			if(yourkarma <= -625 and yourkarma >= -1249)

				who.title_prefix := "The Notorious ";
				
			elseif(yourkarma <= -1250 and yourkarma >= -2499)

				who.title_prefix := "The Ignoble ";
		
			elseif(yourkarma <= -2500 and yourkarma >= -4999)
	
				who.title_prefix := "The Vile ";
	
			elseif(yourkarma <= -5000 and yourkarma >= -9999)

				who.title_prefix := "The Wicked ";

			elseif(yourkarma <= -10000)

				who.title_prefix := "The Nefarious ";

			endif	
	
	
	
	
elseif(yourfame >= 5000 and yourfame <= 9999)
	
			 if(yourkarma <= -625 and yourkarma >= -1249)

				who.title_prefix := "The Infamous ";
				
			elseif(yourkarma <= -1250 and yourkarma >= -2499)

				who.title_prefix := "The Sinister ";
				
			elseif(yourkarma <= -2500 and yourkarma >= -4999)

				who.title_prefix := "The Villainous ";

			elseif(yourkarma <= -5000 and yourkarma >= -9999)

				who.title_prefix := "The Evil ";
	
			elseif(yourkarma <= -10000)

				who.title_prefix := "The Dread ";

			endif		



elseif(yourfame >= 10000)
	
			if(yourkarma <= -625 and yourkarma >= -1249)

				if(who.gender)
					who.title_prefix := "The Dishonored Lady ";
				else 
					who.title_prefix := "The Dishonored Lord ";
				endif
			
			elseif(yourkarma <= -1250 and yourkarma >= -2499)

				if(who.gender)
					who.title_prefix := "The Sinister Lady ";
				else 
					who.title_prefix := "The Sinister Lord ";
				endif
			
			elseif(yourkarma <= -2500 and yourkarma >= -4999)

				if(who.gender)
					who.title_prefix := "The Dark Lady ";
				else 
					who.title_prefix := "The Dark Lord ";
				endif
			
			elseif(yourkarma <= -5000 and yourkarma >= -9999)

				if(who.gender)
					who.title_prefix := "The Evil Lady ";
				else 
				who.title_prefix := "The Evil Lord ";
				endif
			
			elseif(yourkarma <= -10000)

				if(who.gender)
					who.title_prefix := "The Dread Lady ";
				else 
					who.title_prefix := "The Dread Lord ";
				endif
			
			endif

endif


	if ( yourfame >= 0 and yourfame <= 1249 )
			if(yourkarma >= 0 and yourkarma <= 624)

				who.title_prefix := " ";
		
			elseif(yourkarma >= 625 and yourkarma <= 1249)

				who.title_prefix := "The Fair ";
			
			elseif(yourkarma >= 1250 and yourkarma <= 2499)

				who.title_prefix := "The Kind ";
				
			elseif(yourkarma >= 2500 and yourkarma <= 4999)
	
				who.title_prefix := "The Good ";
			
			elseif(yourkarma >= 5000 and yourkarma <= 9999)

				who.title_prefix := "The Honest ";
			
			elseif(yourkarma >= 10000)

				who.title_prefix := "The Trustworthy ";
			
		        endif
	elseif(yourfame >= 1250 and yourfame <= 2499)
	
			if(yourkarma >= 0 and yourkarma <= 624)

				who.title_prefix := "The Notable ";
	
			elseif(yourkarma >= 625 and yourkarma <= 1249)

				who.title_prefix := "The Upstanding ";
			
			elseif(yourkarma >= 1250 and yourkarma <= 2499)

				who.title_prefix := "The Respectable ";
			
			elseif(yourkarma >= 2500 and yourkarma <= 4999)

				who.title_prefix := "The Honorable ";
			
			elseif(yourkarma >= 5000 and yourkarma <= 9999)

				who.title_prefix := "The Commendable ";
			
			elseif(yourkarma >= 10000)

				who.title_prefix := "The Estimable ";
			
			endif
	
	
elseif(yourfame >= 2500 and yourfame <= 4999)
	
			if(yourkarma >= 0 and yourkarma <= 624)

				who.title_prefix := "The Prominent ";
	
			elseif(yourkarma >= 625 and yourkarma <= 1249)

				who.title_prefix := "The Reputable ";
			
			elseif(yourkarma >= 1250 and yourkarma <= 2499)

				who.title_prefix := "The Proper ";
			
			elseif(yourkarma >= 2500 and yourkarma <= 4999)
	
				who.title_prefix := "The Admirable ";
			
			elseif(yourkarma >= 5000 and yourkarma <= 9999)

				who.title_prefix := "The Famed ";
			
			elseif(yourkarma >= 10000)

				who.title_prefix := "The Great ";
			
			endif	
	
	
	
	
elseif(yourfame >= 5000 and yourfame <= 9999)
	
			if(yourkarma >= 0 and yourkarma <= 624)

				who.title_prefix := "The Renowned ";
	
			elseif(yourkarma >= 625 and yourkarma <= 1249)

				who.title_prefix := "The Distinguished ";
			
			elseif(yourkarma >= 1250 and yourkarma <= 2499)

				who.title_prefix := "The Eminent ";
			
			elseif(yourkarma >= 2500 and yourkarma <= 4999)

				who.title_prefix := "The Noble ";
			
			elseif(yourkarma >= 5000 and yourkarma <= 9999)

				who.title_prefix := "The Illustrious ";
			
			elseif(yourkarma >= 10000)

				who.title_prefix := "The Glorious ";
			
			endif		


elseif(yourfame >= 10000)
	
			if(yourkarma >= 0 and yourkarma <= 624)
				if(who.gender)
					who.title_prefix := "The Lady ";
				else 
					who.title_prefix := "The Lord ";
				endif
		
			elseif(yourkarma >= 625 and yourkarma <= 1249)

				if(who.gender)
					who.title_prefix := "The Distinguished Lady ";
				else 
					who.title_prefix := "The Distinguished Lord ";
				endif
			
			elseif(yourkarma >= 1250 and yourkarma <= 2499)

				if(who.gender)
					who.title_prefix := "The Eminent Lady ";
				else 
					who.title_prefix := "The Eminent Lord ";
				endif
			
			elseif(yourkarma >= 2500 and yourkarma <= 4999)

				if(who.gender)
					who.title_prefix := "The Noble Lady ";
				else 
					who.title_prefix := "The Noble Lord ";
				endif
			
			elseif(yourkarma >= 5000 and yourkarma <= 9999)

				if(who.gender)
					who.title_prefix := "The Illustrious Lady ";
				else 
				who.title_prefix := "The Illustrious Lord ";
				endif
			
			elseif(yourkarma >= 10000)

				if(who.gender)
					who.title_prefix := "The Glorious Lady ";
				else 
					who.title_prefix := "The Glorious Lord ";
				endif
			
			endif

endif

if(who.title_prefix = nothing_ or who.title_prefix = " ")
return;
else
SendSysMessage(who, "Your title is now " + who.title_prefix);
endif

endfunction
