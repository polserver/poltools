/* $Id$
 *
 */

use http;

function DoHeader(title:="")
	WriteHTML("<HEAD>");
	WriteHTML("<TITLE>"+title+"</TITLE>");
	WriteHTML("<LINK REL='stylesheet' TYPE='text/css' HREF='../../stylesheet.css'>");
	WriteHTML("</HEAD>");

	WriteHTML("<BODY>");
	return 1;
endfunction

function DoFooter()
	WriteHTML("</BODY>");

	return 1;
endfunction

function TableHeader(name:="")
	WriteHtmlRaw("<TABLE WIDTH='100%'>"
		+"<TR>"
		+"<TD VALIGN='TOP' WIDTH='100%' CLASS='header'><STRONG><BIG>"+name+"</BIG></STRONG></TD>"
		+"<TD WIDTH='100' VALIGN='TOP' ALIGN='CENTER' NOWRAP CLASS='header2'><A HREF='index.ecl'><FONT SIZE=1>ServMgmt Home</FONT></A></TD>"
		+"</TR>"
		+"</TABLE>");

	return 1;
endfunction
