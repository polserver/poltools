<%
use os;
use uo;
%>

<HTML><title>Shard Bandwidth</title>
<style type="text/css">
<!--
.account {  font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold; color: #CCCCCC; text-decoration: none; background-color: #336699}
.pushbutton { color: #FFFFFF; text-decoration: none; background-color: #003366; font-family: Arial, Helvetica, sans-serif; font-size: 10px; font-weight: bold ; cursor: hand; letter-spacing: 1em}
-->
</style>
<style>
BODY {     scrollbar-3d-light-color:#003366;
           scrollbar-arrow-color:gray;
           scrollbar-base-color:999999;
           scrollbar-dark-shadow-color:white;
           scrollbar-face-color:black;
           scrollbar-highlight-color:black;
           scrollbar-shadow-color:black}
</style>
<BODY bgcolor="#000000" text="#FFFFFF" link="#990000" vlink="#990000" alink="#990000">
<div align="center"><img src="images/band.jpg" width="468" height="60"> 
  <% 
  include header; 
  PrintHeader( "Bandwidth Usage" );
%>
</div>
<div align="center">
  <p>&nbsp;</p>
  <p>&nbsp;</p>
</div>
<div align="center">
  <table border=1 cellspacing=0 cellpadding=5 bordercolor="#669999" bgcolor="#003366" class="account">
    <tr> 
      <td bgcolor="#99CCFF"><font color="#000000">Bytes Sent</font></td>
      <td bgcolor="#99CCFF"><font color="#000000">BPS Out</font></td>
      <td bgcolor="#99CCFF"><font color="#000000">Received Data</font></td>
      <td bgcolor="#99CCFF"><font color="#000000">BPS In</font></td>
    </tr>
    <% foreach elem in (GetGlobalProperty( "#:perfmon:bandwidth" )) %>
    <tr> 
      <td><%=elem[2]%></td>
      <td><%=elem[2]/6%></td>
      <td><%=elem[1]%></td>
      <td><%=elem[1]/6%></td>
    </tr>
    <% endforeach %>
  </table>
</div>
</body>
</html>
