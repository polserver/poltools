    // SetAnchor: set a wander-anchor
    //            centerx, centery: the anchor-point
    //            distance_start: distance from center where boundary kicks in
    //            percent_subtract: percent chance per tile past the boundary
    //                              that moving away will fail
SetAnchor( centerx, centery, distance_start, percent_subtract );

	// Self(): return a mobileref to self
Self();

    //
    // Wander: walk, usually forward, sometimes turning
    //
Wander();

	
Move( direction ); // N, S, E, W, NW, NE, SW, SE
					// a boundingbox is okay, too

	// Move toward, or away from, something.
	// When a pathfinding algorith exists, these functions will
	// use if necessary.
	// Puts the script to sleep for a period of time proportional
	// to the NPC's agility.
	// Note also, walking in range of your opponent may cause you
	// to attack.
WalkToward( object );
WalkAwayFrom( object );
RunToward( object );
RunAwayFrom( object );
TurnToward( object );
TurnAwayFrom( object );

WalkTowardLocation( x, y );
WalkAwayFromLocation( x, y );
RunTowardLocation( x, y );
RunAwayFromLocation( x, y );
TurnTowardLocation( x, y );
TurnAwayFromLocation( x, y );

	
	// Set your opponent.  If you are in range (or are carrying
	// a projectile weapon and have LOS), you will attack.  If
	// a player character is connected, the NPC will highlight.
	// Implicitly sets war mode.
SetOpponent( character );

	
	// SetWarMode: Usually used to leave warmode, but can be used
	// to enter warmode independently of setting an opponent.
	// Setting war mode to 0 clears your opponent.
SetWarMode( warmode );

say( text );
// npc_yell( text );
// npc_whisper( text );

	// deprecated ( use Self().x, Self().y, Self().z )
position();

	// Get a custom property.  Equivalent to GetObjProperty( Self(), propertyname )
GetProperty( propertyname );

	// Set a custom property.  Equivalent to SetObjProperty( Self(), propertyname, propertyvalue )
SetProperty( propertyname, propertyvalue );


	// Make a bounding box from an area string.  An area string is
	// a number of 'x1 y1 x2 y2' entries.
	// "1 1 10 10 10 3 20 6" would create a walk area something like this:
	//
	//   XXX 
	//   XXXXXX
	//   XXX

MakeBoundingBox( areastring );


	// IsLegalMove: Given your position, and a tentative move, would it
	// fall within a bounding box?
IsLegalMove( move, boundingbox );
