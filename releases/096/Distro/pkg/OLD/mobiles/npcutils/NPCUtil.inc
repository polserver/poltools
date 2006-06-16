/////////////////////////////////////////////////////////////////////////////
//
//  NPCUTIL.INC: NPC.EM candidate functions
//
//  Functions included:
//      NpcWander()
//
/////////////////////////////////////////////////////////////////////////////


use npc;

//
// NpcWander()
//
//    Walk, generally forward.
//

function NpcWander()
    case RandomInt( 8 )
        0: 1: 2: 3: 4: 5:
            Move( Self().facing );
        6:
            Move( Self().facing - 1 );
        7:
            Move( Self().facing + 1 );
    endcase
endfunction