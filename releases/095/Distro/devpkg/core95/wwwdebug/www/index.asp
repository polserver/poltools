<%
use uo;
use os;
%>

<html>
<head>
<title>Debug Package</title>
</head>

<body>
<% 
  include header; 
  PrintHeader( "All Scripts" );
%>
<form method=GET action="list_scripts.ecl">
  <input type=text name=ScriptName>
  <input type=submit name=Action value="ListScripts">
</form>
  
<form method=GET action="unload.ecl">
  <input type=submit name=Action value="Unload Scripts">
</form>

<form method=GET action="kill_process.ecl">
  <input type=text name=Pid>
  <input type=submit name=Action value="Kill">
</form>

</BODY>
</HTML>
