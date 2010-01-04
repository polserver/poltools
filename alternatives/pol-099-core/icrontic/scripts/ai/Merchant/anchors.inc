

function drop_guard_anchor()

 var anchoritem := CreateItemAtLocation( me.x, me.y, me.z, 0x1F13, 1 );
 SetName( anchoritem, "Guard Anchor Gem" );
 anchoritem.movable := 0;
 anchoritem.invisible := 1;
 SetObjProperty( anchoritem, "guard", me.serial );
 
 SetObjProperty( me, "anchoritem", anchoritem.serial );

endfunction

function drop_anchor()

// assumes that global me = npc::self

var cfg := readconfigfile("npcdesc");

var dstart := cfg[me.npctemplate].dstart;
var psub := cfg[me.npctemplate].psub;

if (!psub)
        psub := 20;
endif

if (dstart)
       SetAnchor(me.x, me.y, dstart, psub);
endif

endfunction