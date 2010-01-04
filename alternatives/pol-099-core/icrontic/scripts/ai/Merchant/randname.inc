//randname.inc
//
//assigns a random name from the list according to obtype and gender

use uo;
use os;
use cfgfile;
use util;
use basic;

include "randname_util";

function RandomName( me )

var newname := " ";

	case( me.graphic )
	    OBJ_BIRD:		newname := AssignName( me , BIRD_IDX );
	    OBJ_DAEMON:		newname := AssignName( me , DAEMON_IDX );
	    OBJ_DAEMON_SWORD:	newname := AssignName( me , DAEMON_IDX );
	    OBJ_DRAGON_RED:	newname := AssignName( me , DRAGON_IDX );
	    OBJ_DRAGON_GREY: 	newname := AssignName( me , DRAGON_IDX );
	    OBJ_MALE: 		newname := AssignName( me , MALE_IDX );
	    OBJ_FEMALE:		newname := AssignName( me , FEMALE_IDX );
	    OBJ_LIZARD:		newname := AssignName( me , LIZARD_IDX );
	    OBJ_LIZARD_MACE:	newname := AssignName( me , LIZARD_IDX );
	    OBJ_LIZARD_SPEAR:	newname := AssignName( me , LIZARD_IDX );
	    OBJ_ORC:		newname := AssignName( me , ORC_IDX );
	    OBJ_ORC_CLUB:	newname := AssignName( me , ORC_IDX );
	    OBJ_ORC_CAPT:	newname := AssignName( me , ORC_IDX );
	    OBJ_OGRE:		newname := AssignName( me , OGRE_IDX );
	    OBJ_RATMAN:		newname := AssignName( me , RATMAN_IDX );
	    OBJ_RATMAN_SWORD:	newname := AssignName( me , RATMAN_IDX );
	    OBJ_RATMAN_AXE:	newname := AssignName( me , RATMAN_IDX );
	   // OBJ_ANGEL:		newname := AssignName2( me , ANGEL_IDX);
	    default: 		exit;
	endcase
	
	return(newname);
endfunction


function AssignName( me , index )

    var cfg := ReadConfigFile( "names" );
    var elem := FindConfigElem( cfg , index );
    var number := GetConfigInt( elem , "Count" );

    var dice_str := "1d" + cstr(number);

    var choice := RandomDiceRoll( dice_str );
    var entry := "Name" + choice;

    var str := GetConfigString( elem , entry );
    var title := PreserveTitle( me );
   
    str := str + title;

    return( str );

endfunction


function PreserveTitle( me )

  var myname := me.name;
  var start := find( myname , ">" , 1 );

if( !start )
  return;
endif

  var title := myname[ start+1 , (len( myname ) - 8) ];

if( !title )
  return("");
endif

  return( title );

endfunction




