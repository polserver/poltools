<% use polsys; %>
<html>

<head>
<title>POL Server Performance Center</title>
</head>

<body>

<p>POL Internal Web Server</p>

<p>The purpose of the internal web server is to make it possible to manage the server from
remote, and to use scripts to do so.&nbsp; </p>

<p>The internal web server understands &quot;.htm&quot;, &quot;.html&quot;, and
&quot;.ecl&quot; files.&nbsp; Only the &quot;GET&quot; method for forms works.&nbsp; </p>

<p>An example:<a href="online.ecl">Online Characters</a></p>

<%
	foreach pkg in Packages()
		if (pkg.supports_http)
			WriteHtml( "<a href=\"/pkg/" + pkg.name + "\">" + pkg.name + "</a>" );
	    else
			WriteHtml( pkg.name );
		endif
	endforeach
%>

</body>
</html>
