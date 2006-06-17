use uo;
use os;

function CreateMoongates()
  var moongates := {
	{4467,	1283,	5, "Moonglow"},		// moonglow
    {1336,	1997,	5, "Britain"},		// britain
    {1499,	3771,	5, "Jhelom"},		// jehlom
	{771,	753,	5, "Yew"},		    // yew
    {2701,	693,	5, "Minoc"},		// minoc
    {1828,	2948, -20, "Trinsic"},	    // trinsic
    {643,	2067,	5, "Skara"},		// skara
    {3564,	2140,  34, "Magincia"} 	    // magincia
  };
  var moongate;
  syslog( "Creating moongates." );
  foreach gate in moongates
    foreach thing in ListItemsNearLocation(gate[1], gate[2], gate[3], 0);
      if(thing.objtype != 0x887b)
        DestroyItem(thing);
      endif
    endforeach
	moongate := CreateItemAtLocation( gate[1], gate[2], gate[3], "systemmoongate", 1 );
    SetObjProperty(moongate, "City", gate[4]);
    moongate.facing := 29;
    moongate.decayAt := 0;
    moongate.movable := 0;
  endforeach
endfunction
