<% use uo; %>

<html><head><title>Online Character Directory</title></head>
  <body>
             <div align=center>
<table border=1 cellspacing=0 cellpadding=5>
 <tr bgcolor=#99CCFF><td>Characters Currently On-Line</td></tr>
 <%
  foreach chr in EnumerateOnlineCharacters()
    if(chr.cmdlevel < 2)%>
      <tr><td><a href="charedit.ecl?Serial=<%=chr.serial%>"><%=chr.name%></a></td></tr>
 <%   endif 
  endforeach
%>
  </table></div>
  </BODY></HTML>