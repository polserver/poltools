function DoMoons(wstuff)
  var fphase := wstuff[1];
  var tphase := wstuff[2];
  var season := wstuff[8];
  fphase := fphase + 1;
  if (fphase == 8)
	fphase := 0;
	tphase := tphase +1;
  endif
  wstuff[1] := fphase;
  if (tphase == 8)
	tphase := 0;
	season := season + 1;
	if (season == 4)
		season := 0;
	endif
	wstuff[2] := tphase;
	wstuff[8] := season;
    SetGlobalProperty("Weather", wstuff);
  endif
endfunction

function ChangeMoonLightSet(wstuff)
  var tlight := FindLight(wstuff[2]);
  var flight := FindLight(wstuff[1]);
  var mlight := (tlight + flight);
  var moonlight := (mlight / 2);
  wstuff[5] := (18 - moonlight);
endfunction

function DuskTransition(wstuff)
  var dlight := wstuff[4];
  var mlight := wstuff[5];
  var inter_level_time := (CInt(transtime / (mlight - dlight)));
  var lightamt := dlight;
  while( lightamt <= mlight )
	wstuff := GetGlobalProperty("Weather");
	lightamt := lightamt + 1;
	SetRegionLightLevel("Background", lightamt);
	wstuff[6] := lightamt;
	SetGlobalProperty("Weather", wstuff);
    sleep(inter_level_time);
  endwhile
endfunction

function DawnTransition(wstuff)
  var dlight := wstuff[4];
  var mlight := wstuff[5];
  var inter_level_time := (CInt(transtime / (mlight - dlight)));
  var lightamt := mlight;
  while( lightamt >= dlight )
	wstuff := GetGlobalProperty("Weather");
	lightamt := lightamt - 1;
	SetRegionLightLevel("Background", lightamt);
	wstuff[6] := lightamt;
	SetGlobalProperty("Weather", wstuff);
    sleep(inter_level_time);
  endwhile
endfunction

function FindLight(phase)
  case(phase)
	0: return 0;
	1: return 1;
	2: return 2;
	3: return 3;
	4: return 4;
	5: return 3;
	6: return 2;
	7: return 1;
  endcase
endfunction