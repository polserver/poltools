<%
use uo;
use os;
%>

<html>
<head>
<title>Script Profiles</title>
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
  <p><img src="images/prof.jpg"></p>
  <p> 
    <% 
  include header; 
  PrintHeader( "Script Profiles" );
%>
    <% 
  
  if (QueryParam( "Action" ) = "Clear Counters") 
      polcore().clear_script_profile_counters();
	    // even better would be to redirect, but we can't.
%>
  </p>
</div>
<p><center>
  </center></p>
<%
  endif 
%>
<div align=center>
  <table border=1 cellspacing=0 cellpadding=5 class="account">
    <tr bgcolor=#99CCFF>
      <td><font color="#000000">Script Name</font></td>
      <td><font color="#000000">Cycles</font></td>
      <td><font color="#000000">Invocations</font></td>
      <td><font color="#000000">Instr/Invoc</font></td>
	  <td><font color="#000000">%</font></td>
  </tr>

<%
  foreach script in (polcore().script_profiles)
%>
  <tr>
    <td><%=script.name%></td>
	<td><%=script.instr%></td>
    <td><%=script.invocations%></td>
	<td><%=script.instr_per_invoc%></td>
	<td><%=script.instr_percent%></td>
  </tr>
<%
    sleepms(1);
  endforeach
%>

</table>
</div>

<center><form><input type=submit name=Action Value="Clear Counters"></form>
  <strong><a href="script_profiles.ecl">Reload this page without clearing</a></strong>
</center>
</BODY>
</HTML>
