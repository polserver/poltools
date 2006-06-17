<% use uo; %>

<html><head><title>Char Edit</title></head>
  <body>
 
<%
    var serial := Cint(QueryParam( "Serial" ));
    var chr := SystemFindObjectBySerial( serial );
  %>
             <div align=center>
<%=serial%><br>
<%=chr.name%><br>
<%=chr.serial%><br>
x=<%=chr.x%><br>
y=<%=chr.y%><br>
z=<%=chr.z%><br>

<form method=get action="chareditaction.ecl">
 <input type=hidden name=Serial value=<%=serial%> ID="Hidden1">
 <input type=text name=X>
 <input type=text name=Y>
 <input type=submit name=Action value="Move">
</form>
</div>
  </BODY></HTML>