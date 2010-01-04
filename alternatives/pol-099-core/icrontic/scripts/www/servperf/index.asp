<% use uo; %>
<html>

<head>
<title>Shard Status Index</title>
</head>

<%
if (QueryParam( "PriDiv" ))
    polcore().set_priority_divide( CInt(QueryParam("PriDiv")) );
endif
%>
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
<body bgcolor="#000000" text="#FFFFFF" link="#990000" vlink="#990000" alink="#990000">
<table width="100%" border="0" cellpadding="0" cellspacing="0" height="100%">
  <tr> 
    <td width="351"></td>
  </tr>
  <tr> 
    <td valign="top" height="7"> 
      <div align="right"> 
        <p align="center"><img src="images/index.jpg"></p>
        </div>
    </td>
  </tr>
  <tr> 
    <td height="657" valign="top"> 
      <p align="right"><a href="../../acctadd.html"><font size="2" face="Arial, Helvetica, sans-serif"><b><font color="#CCCCCC">Click 
        Here to Create An Account</font></b></font></a></p>
      <p><font size="4"><b><font size="2" face="Arial, Helvetica, sans-serif" color="#CCCCCC">Server 
        Vitals:</font></b></font></p>
      <table border="1" width="100%" class="account">
        <tr> 
          <td width="50%" class="account">System Load:</td>
          <td width="50%" class="account"><%=polcore().sysload%>&nbsp;(<%=polcore().sysload_severity%>)</td>
        </tr>
        <tr> 
          <td width="50%" class="account">Mobiles:</td>
          <td width="50%" class="account"><%=polcore().mobilecount%></td>
        </tr>
        <tr> 
          <td width="50%" class="account">Toplevel Items:</td>
          <td width="50%" class="account"><%=polcore().itemcount%></td>
        </tr>
        <tr> 
          <td width="50%" class="account">Online Players:</td>
          <td width="50%" class="account"><%=EnumerateOnlineCharacters().size()%></td>
        </tr>
        <tr> 
          <td width="50%" class="account">Events per Minute:</td>
          <td width="50%" class="account"><%=polcore().events_per_min%></td>
        </tr>
        <tr> 
          <td width="50%" class="account">Script Instructions per Minute:</td>
          <td width="50%" class="account"><%=polcore().instr_per_min%></td>
        </tr>
        <tr> 
          <td width="50%" class="account">Bytes Sent:</td>
          <td width="50%" class="account"><%=polcore().bytes_sent%></td>
        </tr>
        <tr> 
          <td width="50%" class="account">Bytes Received:</td>
          <td width="50%" class="account"><%=polcore().bytes_received%></td>
        </tr>
        <tr> 
          <td width="50%" class="account">Priority Divider:</td>
          <td width="50%"> 
            <form>
              <span class="account"> 
              <input type=text name=PriDiv value="<%=polcore().priority_divide%>">
              </span> 
            </form>
          </td>
        </tr>
      </table>
      <ul>
        <li><a href="npcgroup.ecl"><font size="2" face="Arial, Helvetica, sans-serif"><b>NPC 
          Grouping</b></font></a> 
        <li><b><font size="2" face="Arial, Helvetica, sans-serif"><a href="script_profiles.ecl">Script 
          Profiles</a> </font></b>
        <li><b><font size="2" face="Arial, Helvetica, sans-serif"><a href="running_scripts.ecl">Running 
          Scripts</a> </font></b>
        <li><b><font size="2" face="Arial, Helvetica, sans-serif"><a href="long_running_scripts.ecl">Long 
          Running Scripts</a> </font></b>
        <li><b><font size="2" face="Arial, Helvetica, sans-serif"><a href="bandwidth.ecl">Bandwidth 
          Usage</a> </font></b>
      </ul>
    </td>
  </tr>
</table>
<p align="right">&nbsp;</p>
<p align="right">&nbsp;</p>

</body>
</html>
