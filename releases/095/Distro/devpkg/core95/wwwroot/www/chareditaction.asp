<% use uo; %>

<html><head><title>Char Edit</title></head>
  <body>
 
<%
    var action := QueryParam( "Action" );
    var serial := Cint(QueryParam( "Serial" ));
    var chr := SystemFindObjectBySerial( serial );
  %>
             <div align=center>
Action: <%=action%><br>
<%=serial%><br>
<%=chr.name%><br>
<%=chr.serial%><br>
x=<%=chr.x%><br>
y=<%=chr.y%><br>
z=<%=chr.z%><br>

<%  var result;

    case( action )
        "Move": result := MoveCharacterToLocation( chr,
                                         CInt(QueryParam("X")),
                                         CInt(QueryParam("Y")),
                                         0 );
                                         
        
    endcase
    
%>
Result: <%=result%><br>
<a href="charedit.ecl?Serial=<%=serial%>">Back to Char Edit Main</a>
</div>
  </BODY></HTML>