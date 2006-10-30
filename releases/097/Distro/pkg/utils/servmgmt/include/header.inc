/* $Id$
 *
 */

use http;

function DoHeader(title:="")
	WriteHTML("<HEAD>");
	WriteHTML("<TITLE>"+title+"</TITLE>");
	WriteHTML("<LINK REL='stylesheet' TYPE='text/css' HREF='/stylesheets/main.css'>");
	WriteHTML("</HEAD>");

	WriteHTML("<BODY>");
	return 1;
endfunction

function DoFooter()
	WriteHTML("<DIV ID='footer'>");
	WriteHTML("<P>Copyright &copy; 2006 - POL Development Team.</P>");
	WriteHTML("</DIV>");
	WriteHTML("</DIV>");
	WriteHTML("</BODY>");
	WriteHTML("</HTML>");

	return 1;
endfunction

function TableHeader(name:="")
	WriteHtmlRaw("<TABLE WIDTH='100%' CELLSPACING='1' CELLPADDING='2'>"
		+"<TR>"
		+"<TD VALIGN='TOP' WIDTH='100%' CLASS='header'><STRONG>"+name+"</STRONG></TD>"
		+"<TD WIDTH='100' VALIGN='TOP' ALIGN='CENTER' NOWRAP CLASS='header2'><A HREF='index.ecl'><FONT SIZE=1>ServMgmt Home</FONT></A></TD>"
		+"</TR>"
		+"</TABLE>");

	return 1;
endfunction
