<% 
   use uo;
   use os; 
   set_priority(200);
%>
<html>

<head>
<title>NPC Grouping</title>
<meta name="GENERATOR" content="Microsoft FrontPage 3.0">
</head>
<style type="text/css">
<!--
.account {  font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold; color: #CCCCCC; text-decoration: none; background-color: #336699}
.pushbutton { color: #FFFFFF; text-decoration: none; background-color: #003366; font-family: Arial, Helvetica, sans-serif; font-size: 10px; font-weight: bold ; cursor: hand; letter-spacing: 1em}
-->
</style>
<style>
BODY {     scrollbar-3d-light-color:#003366;
           scrollbar-arrow-color:gray;
           scrollbar-base-color:999999;
           scrollbar-dark-shadow-color:white;
           scrollbar-face-color:black;
           scrollbar-highlight-color:black;
           scrollbar-shadow-color:black}
</style>
<body bgcolor="#000000" text="#FFFFFF" link="#990000" vlink="#990000" alink="#990000">
<div align="center">
  <p><img src="images/npc.jpg"></p>
  <p>&nbsp;</p>
  <p> 
    <% 
  include header; 
  PrintHeader( "NPC Grouping" );
%>
  </p>
</div>
<table border="1" width="100%" class="account">
  <tr>
    <td width="50%">(x,y) center</td>
    <td width="50%">NPC Count</td>
  </tr>
<% foreach zone in (top_ten_npc_zones()) %>
  <tr>
    <td><%=zone.xmid%>,<%=zone.ymid%></td>
	<td><%=zone.npc_count%></td>
  </tr>
<% endforeach %>
</table>
</body>
</html>

<%
use os;

function top_ten_npc_zones()
    var zones := {};
	var zone := {};
	zone.+xmid := 0;
	zone.+ymid := 0;
	zone.+npc_count := 0;
	zones[11] := zone;
	foreach z in zones
	    z := zone;
	endforeach;

	var xmid, ymid, lowest;
	lowest := 0;
	for( xmid := 31; xmid < 6144; xmid := xmid + 64 )
	    // print( CStr(xmid) );
	    for( ymid := 31; ymid < 4096; ymid := ymid + 64 )
			var npc_count := GetNpcCount( xmid, ymid, 32 );
			if (npc_count > lowest)
			    var i;
				for( i := 1; i <= 10; i := i + 1)
				    if (npc_count > zones[i].npc_count)
				        // print( "npc count for zone index " + i + " is " + npc_count );
						zone.xmid := xmid;
						zone.ymid := ymid;
						zone.npc_count := npc_count;
						zones.insert( i, zone );
						zones.erase( 11 );
						// print( zones );
						break;
					endif
				endfor
				lowest := zones[10].npc_count;
			endif
         	sleepms(1);
		endfor
	endfor
	zones.erase( 11 );
	return zones;
endfunction

function GetNpcCount( xmid, ymid, range )
    var count := 0;
	foreach obj in ListObjectsInBox( xmid-range, ymid-range, -128,
	                                 xmid+range, ymid+range, +127 )
	    if (obj.npctemplate)
		    count := count + 1;
		endif
	endforeach
	return count;
endfunction
%>
