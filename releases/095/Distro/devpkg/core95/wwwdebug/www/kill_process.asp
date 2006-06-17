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
  var pid := CInt(QueryParam( "Pid" ));
  
  
%>
  Kill Pid <%=pid%>: <%=getprocess( pid ).kill()%>

  <a href="index.ecl">Back to home</a>
</body>
</html>
