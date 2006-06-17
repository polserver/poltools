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
  var num := unload_scripts();
%>
  Unloaded <%=num%> Scripts.

  <a href="index.ecl">Back to home</a>
</body>
</html>
