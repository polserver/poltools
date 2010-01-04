////////////////////////////////////////////////////////////////////
//     Used to help block actions/spells etc for regions
//     Written by MuadDib of IcronticShard and LostSouls
//
//     Example to use this function for spells or runebooks
//
//        if(Checkplace(caster))
//         DestroyItem(cast_on);
//         MoveCharacterToLocation( caster, oldx, oldy, oldz );
//         SendSysMessage(caster, "You have no rights to stay here...(Your rune is vanished!)");
//         return;
//         endif
//
////////////////////////////////////////////////////////////////////

use uo;
use os;

Function Checkplace(me)

 If ( me.x>=5376 and me.y>=1176 and me.x<=5414 and me.y<=1224)
  Return "Region Maze Tourney Entrance";
 Elseif ( me.x>=5720 and me.y>=858 and me.x<=5770 and me.y<=900)
  Return "Region kingofthering";
 Elseif ( me.x>=5379 and me.y>=1279 and me.x<=5629 and me.y<=1510)
  Return "Region Maze Tourny Complex";
 Elseif ( me.x>=5377 and me.y>=1081 and me.x<=5406 and me.y<=1101)
  Return "Region Tourny Complex";
 Elseif ( me.x>=5271 and me.y>=1159 and me.x<=5312 and me.y<=1192)
  Return "Region Jail";
 Elseif ( me.x>=5859 and me.y>=1147 and me.x<=5908 and me.y<=1236)
  Return "Region HeadHunter";
 Else
  Return 0;
 Endif
Endfunction
