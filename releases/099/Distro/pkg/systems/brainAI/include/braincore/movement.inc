/*
        Maintened by *Edwards

        2009-10-10
        
        Last Modifications 2009-10-10
             * Added PathFinding for AI

        Last Modifications 2009-10-11
             * Added OpenDoors ability in PathFinding :)

        Last Modifications 2009-10-12
             * Added Auto-AntiWalling ability in PathFinding :)
             
        Last Modifications 2010-03-28
             * Removed MoveLoopCheck() from MovementXY()
*/

include ":attributes:attributes";
include ":brainAI:commonFunctions";

function MovementXY( ev )

	var old_pri := Set_Priority(50);

        var path := CheckPath( ev ),
            x := path[1],
            y := path[2];
            
	case( ev.speed )
		NEMOVE_WALK:
			case( ev.direction )
				NEMOVE_AWAY:
                                        WalkAwayFromLocation( ev.x, ev.y );
					break;
				NEMOVE_TOWARD:
                                        path := CheckPath( ev );
                                        if( path )
                                                WalkTowardLocation( x, y );
                                        else
                                                WalkTowardLocation( ev.x, ev.y );
                                        endif
					break;
			endcase
                        break;
		NEMOVE_RUN:
			case( ev.direction )
				NEMOVE_AWAY:
                                        RunAwayFromLocation( ev.x, ev.y );
					break;
				NEMOVE_TOWARD:
                                        path := CheckPath( ev );
                                        if( path )
                                                RunTowardLocation( x, y );
                                        else
                                                RunTowardLocation( ev.x, ev.y );
                                        
                                        endif
					break;
			endcase
                        break;
	endcase

	Set_Priority(old_pri);

	return 1;
endfunction

function CheckPath( byref ev )

        var pathfind_range := CInt( g_settings["AreaSize"] );
        if( pathfind_range < 5 )
                pathfind_range := 5;
        endif
        
        var flags := FP_IGNORE_MOBILES;
        if( g_settings["OpenDoors"] )
                flags += FP_IGNORE_DOORS;
        endif

        return GetNewPath( npc, ev.x, ev.y, ev.z, pathfind_range, flags );
endfunction

function MoveLoopCheck( byref ev )

	if( ev.loop < 1 )
		return 0;
	elseif( npc.frozen )
		return 0;
	elseif( npc.paralyzed )
		return 0;
	endif
 
        //Might get problems with that check if you ask the NPC to move to a specific location
	if( ev.direction == NEMOVE_TOWARD )
		if( CoordinateDistance( npc.x, npc.y, ev.x, ev.y ) <= 2 )
			return 0;
		endif
	endif

	ev.loop -= 1;

	return 1;
endfunction
