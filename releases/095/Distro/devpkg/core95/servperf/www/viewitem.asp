<%
use uo;
use polsys;
%>
<html>

<head>
<title>POL Server Performance Center</title>
</head>

<body>

<%
    var serial := CInt(QueryParam( "Serial" ));
    var item :=  SystemFindObjectBySerial( serial );

%>
<p>Item Details</p>
<table border="1" cellpadding="5" cellspacing="0">
	<tr bgcolor="#99CCFF">
		<td>Property</td>
		<td>Value</td>
	</tr>
	<tr><td>serial</td><td><%=Hex(item.serial)%></td></tr>
	<tr><td>objtype</td><td><%=Hex(item.objtype)%></td></tr>
	<tr><td>name</td><td><%=item.name%></td></tr>
	<tr><td>desc</td><td><%=item.desc%></td></tr>
	<tr><td>buyprice</td><td><%=item.buyprice%></td></tr>
	<tr><td>sellprice</td><td><%=item.sellprice%></td></tr>
</table>

<%
	if (item.isa( POLCLASS_CONTAINER ))
%>
		<p>Contained Items:</p>
		<table>
			<tr>
				<td>Serial</td>
				<td>Sub Item</td>
				<td>Objtype</td>
				<td>Total Items</td>
			</tr>
<%
		foreach subitem in EnumerateItemsInContainer(item)
		    if (subitem.container != item)
		        continue;
		    endif
%>
			<tr>
			<td><a href="viewitem.ecl?Serial=<%=subitem.serial%>"><%=Hex(subitem.serial)%></a></td>
			<td><%=subitem.desc%></td>
			<td><%=subitem.objtype%></td>
			<td><%=subitem.item_count%></td>
			</tr>
<%
		endforeach
%>
		</table>
<%
	endif
%>

</body>
</html>
