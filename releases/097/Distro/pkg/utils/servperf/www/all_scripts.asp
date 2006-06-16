<%
use uo;
use os;
%>

<html>
<head>
<title>All Scripts</title>
</head>

<body>
<% 
  include header; 
  PrintHeader( "All Scripts" );
%>

<div align=center>
<table border=1 cellspacing=0 cellpadding=5>
  <tr bgcolor=#99CCFF>
	<td>PID</td>
    <td>Script Name</td>
    <td>Cycles</td>
	<!--<td>Sleep Cycles</td>-->
    <td>Cycles since Sleep</td>
    <td>Prog Counter</td>
	<td>Call Depth</td>
	<td>Globals</td>
	<td>Var Size</td>
	<td>State</td>
  </tr>

<%
  foreach script in (polcore().all_scripts)
%>
  <tr>
    <td><a href="scriptex.ecl?pid=<%=script.pid%>"><%=script.pid%></a></td>
    <td><%=script.name%></td>
	<td><%=script.instr_cycles%></td>
	<!--<td><%=script.sleep_cycles%></td>-->
    <td><%=script.consec_cycles%></td>
	<td><%=script.PC%></td>
	<td><%=script.call_depth%></td>
	<td><%=script.num_globals%></td>
	<td><%=script.var_size%></td>
	<td><%=script.state%></td>
  </tr>
<%
    sleepms(1);
  endforeach
%>

</table>
</div>

</BODY>
</HTML>
