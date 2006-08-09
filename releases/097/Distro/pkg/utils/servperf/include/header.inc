/* $Id$
 *
 */

use http;

function DoHeader(title:="")
	WriteHTML("<HEAD>");
	WriteHTML("<TITLE>"+title+"</TITLE>");
	//WriteStyleInfo();
	WriteHTML("<LINK REL='stylesheet' TYPE='text/css' HREF='../../stylesheet.css'>");
	WriteHTML("</HEAD>");

	WriteHTML("<BODY>");
	return 1;
endfunction

function DoFooter()
	WriteHTML("</BODY>");

	return 1;
endfunction

function WriteStyleInfo()
	WriteHTML("<STYLE TYPE='TEXT/CSS'>");
	WriteHTMLRaw("body, td, th, table, input{");
	WriteHTMLRaw("font-family: Verdana, Tahoma, Trebuchet MS, Arial, MS Sans Serif, sans-serif;");
	WriteHTMLRaw("font-size: 13px; color: #000000; background: #C0C0C0");
	WriteHTML("}");

	WriteHTML("table{border-collapse: collapse}");
	WriteHTML("td{border-style:solid; border-width:1; border-color:#000000; padding:2}");
	WriteHTML("td.header{width: 400; text-align:center; font-weight:bold; color:#000000; background-color:808080; column-span:2}");
	WriteHTML("td.normal{color: #000000; background-color: #C0C0C0; padding-left:3}");
	WriteHTML("td.delete{width:50; text-align:center; color: #FFFFFF; background-color: DARKRED}");
	WriteHTML("td.edit{width:30; text-align:center; color: #FFFFFF; background-color: DARKGREEN}");
	WriteHTML("td.addform{text-align:center}");

	WriteHTML("input.delete{color: #FFFFFF; background-color: DARKRED; border-style:dotted; border-width:1; padding:0}");
	WriteHTML("input.edit{color: #FFFFFF; background-color: DARKGREEN; border-style:dotted; border-width:1; padding:0}");
	WriteHTML("</STYLE>");

	return 1;
endfunction

function TableHeader(name:="")
	WriteHtmlRaw("<TABLE WIDTH='100%'>"
		+"<TR>"
		+"<TD VALIGN='TOP' WIDTH='100%' CLASS='header'><STRONG><BIG>"+name+"</BIG></STRONG></TD>"
		+"<TD WIDTH='100' VALIGN='TOP' ALIGN='RIGHT' NOWRAP CLASS='header2'><A HREF='index.ecl'><FONT SIZE=1>ServPerf<BR>"
		+"Home</FONT></A></TD>"
		+"</TR>"
		+"</TABLE>");

	return 1;
endfunction
