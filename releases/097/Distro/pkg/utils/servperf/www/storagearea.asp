<%
use uo;
use polsys;
use storage;
%>
<html>

<head>
<title>POL Server Performance Center</title>
</head>

<body>

<%
    var areaname := QueryParam( "Area" );
	var areas := StorageAreas();
	var area := areas[ areaname ];

%>
<p>POL Server Performance Center</p>
<p>Storage Area: <%=area%></p>
<p>Root Items: <%=area.count%></p>
<p>Total Items: <%=area.totalcount%></p>

<table>
<tr>
  <td>Item Name</td>
  <td>Total Items</td>
</tr>
<%
foreach item in area
%>
<tr>
  <td><%=item.name%></td>
  <td><%=item.item_count%></td>
</tr>
<%
endforeach
%>
</table>

</body>
</html>
