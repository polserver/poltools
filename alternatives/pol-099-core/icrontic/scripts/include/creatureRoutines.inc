//////////////////////////////////////////////////////////
//
//   creatureRoutines.inc
//
//   12/29/98
//
//////////////////////////////////////////////////////////
var buddy1      := 0;
var buddy2      := 0;
var leader      := 0;
var dx          := 0;
var dy          := 0;
var myvalue     := 0; // this one's meant to be overwritten

var fertility   := CInt(GetObjProperty( (npc::Self()), "fertility" ));

if (fertility > 99)
   fertility := 99;
elseif (fertility < 1)
   fertility := 1;
endif

fertility := (100 - fertility) * 50;


//////////////////////////////////////////////////////////
//
//   distFrom()
//
//   A simple distance formula without using roots.
//
//////////////////////////////////////////////////////////
include "include/attributes";
function cr_myDistFrom( obj2 )
    var obj1 := npc::Self();

    while (obj1.container)
        obj1 := obj1.container;
    endwhile
    while (obj2.container)
        obj2 := obj2.container;
    endwhile

    var xd := obj1.x - obj2.x;
    var yd := obj1.y - obj2.y;
    if (xd < 0)
        xd := -xd;
    endif
    if (yd < 0)
        yd := -yd;
    endif
    if (xd > yd)
        return xd;
    else
        return yd;
    endif
endfunction

/////////////////////////////////////////////////////////
//
//  wander()
//
//  This function moves me based on a vector pair (dx,dy),
//  which is occasionally modified.  Thus my movement,
//  though purposeless, doesn't appear to be to abrupt.
//
/////////////////////////////////////////////////////////
function cr_wander()
   if (RandomInt(100) > 90)
      dx := RandomInt(3) - 1;
   elseif ( RandomInt(100) > 90)
      dy := RandomInt(3) - 1;
   endif

   if ( dx > 0 )
      Move( "E" );
   elseif ( dx < 0 )
      Move( "W" );
   endif

   if ( dy > 0 )
      Move( "S" );
   elseif ( dy < 0 )
      Move( "N" );
   endif     
endfunction

/////////////////////////////////////////////////////
//
//  flee()
//
//  Run from badGuy.
//  flee() returns 0 until badGuy is 15+ units distant,
//  after which 1 is returned.
//
/////////////////////////////////////////////////////
function cr_flee( badguy )
   var d    := cr_myDistFrom( badguy );
   var gone := 0;

   if ( d < 8 )     RunAwayFrom( badguy );
   elseif (d < 16 ) cr_wander();
   else             gone := 1;
   endif

   return gone;
endfunction

/////////////////////////////////////////////////////
//
//  justLikeMe()
//
//  Returns 1 if the object type is the same as
//  my object type.  Otherwise, it returns 0.
//
/////////////////////////////////////////////////////
function cr_justLikeMe( dude )
   if ( dude.objtype == (npc::Self()).objtype )
      return 1;
   endif
   return 0;
endfunction

////////////////////////////////////////////////////
//
//  setBuddy() tries to give a creature a buddy.
//  This creature can have up to two buddies it
//  tags along with.  Together, a group of creatures
//  will exhibit a form of flocking behavior, as
//  each one seeks to be near the others, but not
//  TOO near.  Being too close renders the group
//  more vulnerable to area-effect attacks.
//
////////////////////////////////////////////////////
function cr_setBuddy( dude, msg )
   if (buddy1 && buddy2 )
      return;
   elseif (dude.objtype == (npc::Self()).objtype )
      if (buddy1 == 0)
         buddy1 := dude;
         say( msg );
      elseif (buddy2 == 0 && dude.serial != buddy1.serial )  
         buddy2 := dude;
         say( msg );
      endif
   endif
endfunction

////////////////////////////////////////////////////
//
//  checkBuddies()
//
//  Check the status of buddies and leader.
//  Re-set any dead ones.
//
////////////////////////////////////////////////////
function cr_checkBuddies()
   if (buddy1.dead)
      buddy1 := 0;  // if i could, you know i would ...
   endif

   if (buddy2.dead)
      buddy2 := 0;  // if i could, i would ...
   endif

   if (leader.dead)
      leader := 0;  // let it go ... (words by U2)
   endif
endfunction

////////////////////////////////////////////////////
//
//  value()
//
//  Part of the leader function,
//  this function establishes an arbitrary value
//  for a unit.
//
////////////////////////////////////////////////////
function cr_value( dude )
   return dude.maxStrength
        + dude.maxDexterity
        + dude.maxstamina
        + dude.pack_order;
endfunction

function cr_setMyValue( factor )
   myvalue := cr_value( npc::Self() ) * factor;
endfunction

////////////////////////////////////////////////////
//
//  setLeader()
//
//  This sets my 'leader' variable, if it is not
//  already set, and if the dude in question is
//  more valuable than me.
//
////////////////////////////////////////////////////
function cr_setLeader( dude, msg )
   if ( leader == 0
     && myvalue < cr_value( dude ) )
      leader := dude;
      SetObjProperty( (npc::Self()), "leader", dude.serial );
      say( msg );
      return 1;
   endif
   return 0;
endfunction

/////////////////////////////////////////////////////////
//
//  follow()
//
//  This function makes sure I am within the proper
//  distance from dude.
//
//  It returns a 1 on success, or 0 on failure.
//
/////////////////////////////////////////////////////////
function cr_follow( dude, maxdistance, mindistance )
   if (dude == 0)
      return 0;
   endif

   var d := cr_myDistFrom( dude );
   if ( d > CInt( maxdistance ) )
      WalkToward( dude );
   elseif( d < CInt( mindistance ) )
      RunAwayFrom( dude );
   else
      return 0;
   endif
   return 1;
endfunction

/////////////////////////////////////////////////////
//
//  Target dude for termination.
//
/////////////////////////////////////////////////////
function cr_target( dude, msg )
   say( msg );
   SetOpponent( dude );
   SetWarMode(1);
   return dude;
endfunction

///////////////////////////////////////////////////
//
//  spawn()
//
//  If we're happy enough, we'll spawn off a new
//  critter.
//
///////////////////////////////////////////////////
function cr_spawn( potential )
   var me   := npc::Self();

   if (potential < 1)
      potential := 1;
   endif

   var fert_pot := fertility * potential;
   if (fert_pot < 99)
           fert_pot := 99;
   endif

   if ( template && RandomInt( fert_pot ) == 0)
      newdude := CreateNpcFromTemplate( template, me.x, me.y, me.z, 0 );
      SetObjProperty( newdude, "spawnedBy", me.serial );
      return 1;
   endif
   return 0;
endfunction