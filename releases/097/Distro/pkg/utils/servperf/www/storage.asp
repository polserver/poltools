<%
use uo;
use polsys;
%>
<html>

<head>
<title>POL Server Performance Center</title>
</head>

<body>

<p>POL Server Performance Center</p>
<p>Storage Areas:</p>

<table>
<tr>
  <td>Storage Area</td>
  <td>Root Items</td>
  <td>Total Items</td>
</tr>
<%
foreach sa in StorageAreas()
%>
<tr>
  <td><a href="storagearea.ecl?Area=<%=sa%>"><%=sa%></a></td>
  <td><%=sa.count%></a></td>
  <td><%=sa.totalcount%></td>
</tr>
<%
endforeach
%>
</table>

</body>
</html>
