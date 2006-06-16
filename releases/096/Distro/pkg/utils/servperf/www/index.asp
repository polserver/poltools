<%
use uo;
use polsys;
%>
<html>

<head>
<title>POL Server Performance Center</title>
</head>

<%
if (QueryParam( "PriDiv" ))
    polcore().set_priority_divide( CInt(QueryParam("PriDiv")) );
endif
%>

<body>

<p>POL Server Performance Center</p>
<p>Server Vitals:</p>

<table border="1" width="100%">
  <tr>
    <td width="50%">System Load:</td>
    <td width="50%"><%=polcore().sysload%>&nbsp;(<%=polcore().sysload_severity%>)</td>
  </tr>
  <tr>
    <td width="50%">Mobiles:</td>
    <td width="50%"><%=polcore().mobilecount%></td>
  </tr>
  <tr>
    <td width="50%">Toplevel Items:</td>
    <td width="50%"><%=polcore().itemcount%></td>
  </tr>
  <tr>
    <td width="50%">Online Players:</td>
    <td width="50%"><%=EnumerateOnlineCharacters().size()%></td>
  </tr>
  <tr>
    <td>Storage Areas:</td>
	<td><a href="storage.ecl"><%=StorageAreas().count%></a></td>
  </tr>
  <tr>
    <td width="50%">Events per Minute:</td>
    <td width="50%"><%=polcore().events_per_min%></td>
  </tr>
  <tr>
    <td width="50%">Error creations per Minute:</td>
    <td width="50%"><%=polcore().error_creations_per_min%></td>
  </tr>
  <tr>
    <td width="50%">Skill Checks per Minute:</td>
    <td width="50%"><%=polcore().skill_checks_per_min%></td>
  </tr>
  <tr>
    <td width="50%">Combat Operations per Minute:</td>
    <td width="50%"><%=polcore().combat_operations_per_min%></td>
  </tr>
  <tr>
    <td width="50%">Task scheduler deadlines met:</td>
<% 
   var tasks_ontime := polcore().tasks_ontime_per_min;
   var tasks_late := polcore().tasks_late_per_min;
   var tasks_late_ticks := polcore().tasks_late_ticks_per_min;
%>
    <td width="50%"><%=tasks_ontime%>/<%=tasks_ontime+tasks_late%> (<%=tasks_late_ticks%> ticks)</td>
  </tr>
  <tr>
    <td width="50%">Script scheduler deadlines met:</td>
<%
   var scripts_ontime := polcore().scripts_ontime_per_min;
   var scripts_late := polcore().scripts_late_per_min;
%>
    <td width="50%"><%=scripts_ontime%>/<%=scripts_ontime+scripts_late%></td>
  </tr>
  <tr>
    <td width="50%">Script Instructions per Minute:</td>
    <td width="50%"><%=polcore().instr_per_min%></td>
  </tr>
  <tr>
    <td width="50%">Bytes Sent:</td>
    <td width="50%"><%=polcore().bytes_sent%></td>
  </tr>
  <tr>
    <td width="50%">Bytes Received:</td>
    <td width="50%"><%=polcore().bytes_received%></td>
  </tr>
  <tr>
    <td width="50%">Priority Divider:</td>
    <td width="50%"><form><input type=text name=PriDiv value="<%=polcore().priority_divide%>"></form></td>
  </tr>
</table>

<ul>
<li><a href="npcgroup.ecl">NPC Grouping</a>
<li><a href="script_profiles.ecl">Script Profiles</a>
<li><a href="running_scripts.ecl">Running Scripts</a>
<li><a href="long_running_scripts.ecl">Long Running Scripts</a>
<li><a href="all_scripts.ecl">All Scripts</a>
<li><a href="bandwidth.ecl">Bandwidth Usage</a>
</ul>
</body>
</html>
