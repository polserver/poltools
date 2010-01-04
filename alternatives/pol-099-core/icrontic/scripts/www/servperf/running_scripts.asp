<%
use uo;
use os;
%>

<html>
<head>
<title>Running Scripts</title>
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
  <p><img src="images/running.jpg"></p>
  <p>&nbsp;</p>
  <p> 
    <% 
  include header; 
  PrintHeader( "Running Scripts" );
%>
  </p>
</div>
<div align=center>
  <table border=1 cellspacing=0 cellpadding=5 class="account">
    <tr bgcolor=#99CCFF>
      <td><font color="#000000">Script Name</font></td>
      <td><font color="#000000">Cycles</font></td>
	<!--<td>Sleep Cycles</td>-->
      <td><font color="#000000">Cycles since Sleep</font></td>
      <td><font color="#000000">Prog Counter</font></td>
	  <td><font color="#000000">Call Depth</font></td>
	  <td><font color="#000000">Globals</font></td>
  </tr>

<%
  foreach script in (polcore().running_scripts)
%>
  <tr>
    <td><%=script.name%></td>
	<td><%=script.instr_cycles%></td>
	<!--<td><%=script.sleep_cycles%></td>-->
    <td><%=script.consec_cycles%></td>
	<td><%=script.PC%></td>
	<td><%=script.call_depth%></td>
	<td><%=script.num_globals%></td>
  </tr>
<%
    sleepms(1);
  endforeach
%>

</table>
</div>

</BODY>
</HTML>
