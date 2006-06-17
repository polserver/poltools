<%
use uo;
use os;
%>

<html>
<head>
<title>Executing Script</title>
</head>

<body>
<% 
  include header; 
  PrintHeader( "Executing Script" );

  var pid := CInt(QueryParam( "pid" ));
  var script := getprocess( pid );
  script.LoadSymbols();
%>

<div align=center>
<table border=1 cellspacing=0 cellpadding=5>
  <tr bgcolor=#99CCFF>
	<td colspan=2>Script Status</td>
  </tr>
  <tr>  <td>Script Name:</td>		<td><%=script.name%></td>			</tr>
  <tr>	<td>PID:</td>				<td><%=script.pid%></td>			</tr>
  <tr>	<td>Cycles:</td>			<td><%=script.instr_cycles%></td>	</tr>
  <tr>	<td>State</td>				<td><%=script.state%></td>			</tr>
  <tr>	<td>Cycles since Sleep</td>	<td><%=script.consec_cycles%></td>	</tr>
  <tr>	<td>Prog Counter</td>		<td><%=script.PC%></td>				</tr>
  <tr>	<td>Call Depth</td>			<td><%=script.call_depth%></td>		</tr>
  <tr>	<td>Variable Size:</td>		<td><%=script.var_size%></td>		</tr>
  <tr>  <td>Actions:</td>           <td><form action="debug2.ecl">
                                        <input type=hidden name=pid value=<%=script.pid%>>
			                            <input type=submit value="Debug">
			                            </form>  </td>                  </tr>
</table>

<table border=1 cellspacing=0 cellpadding=5>
  <tr bgcolor=#99CCFF>
    <td colspan=4>Global Variables</td>
  </tr>
  <tr bgcolor=#99CCFF>
    <td>Name</td> <td>Type</td> <td>Size</td> <td>Value</td>
  </tr>

<% 
  var globals := script.globals;
  foreach key in (globals.keys())
%>
  <tr>
    <td><%=key%></td>
	<td><%=TypeOf(globals[key])%></td>
	<td><%=SizeOf(globals[key])%></td>
    <td><%=globals[key]%></td>
  </tr>
<% 
  endforeach 
%> 	  

</table>
</div>

</BODY>
</HTML>
