<html>
<%
use os;
use uo;

var debuggerex;
var debug_ctx;

function DebugCommand( cmd )
	var r := debug_ctx.process( cmd );
	var response := struct;
	response.+results := r;
	response.+prompt := debug_ctx.prompt;
	return response;
endfunction

    var pid := QueryParam( "pid" );
    var action := QueryParam("Action");

	var pidkey := "#Debug"+pid;
	var debuggerpid := GetGlobalProperty( pidkey );
	if (debuggerpid)
		// debugger script already running
		WriteHtml( "Debugger PID is " + debuggerpid );
	    debuggerex := getprocess( debuggerpid );
	else
	    // there isn't a debugger yet, make one
		WriteHtml( "Starting Debugger script for PID" + pid );
		debuggerex := start_script( "debugsvc2", pid );
	endif

	debuggerex.sendevent( getprocess( getpid() ) );
	debug_ctx := wait_for_event( 200 );
	WriteHtml( "DebugCtx is " + debug_ctx );
	if (!debug_ctx)
		// exit
	endif

	WriteHtml( "PID is " + pid );
	WriteHtml( "Action is " + action );
// find the debugger script for this PID:

	case action
		"InsTrace": DebugCommand( "instrace" );
		"StepInto": DebugCommand( "stepinto" );
		"StepOver": DebugCommand( "stepover" );
		"StopDebugging": DebugCommand( "detach" );
		"Break": DebugCommand( "break" );
	endcase

%>

Status is <%=DebugCommand("state").results[1]%>
<hr>
<pre>
<%
    var fileline := DebugCommand( "fileline" ).results[1];
	fileline := SplitWords( fileline );
	var filenum := CInt(fileline[1]);
	var linenum := CInt(fileline[2]);
	var filename := DebugCommand( "files" ).results[filenum+1];
	WriteHtml( filenum );
	WriteHtml( linenum );
	WriteHtml( filename );
	var firstline := linenum-10;
	var lastline := linenum+5;
	if (firstline < 1) 
	    firstline := 1; 
	endif
	var dbgcmd := "filecont "+filenum+" "+firstline+" "+lastline;
	var curline := firstline;
	foreach line in (DebugCommand(dbgcmd).results)
	    var disp := curline + ": " + line;
		if (curline == linenum)
		    disp := "<font color=blue>&gt&gt"+disp+"</font>";
		else
		    disp := "  " + disp;
		endif
		WriteHtml( disp );
		curline := curline + 1;
	endforeach
%>
<table border=1 cellspacing=0 cellpadding=5>
  <tr bgcolor="#99CCFF">
    <td>Local Variable</td>
	<td>Value</td>
  </tr>

<%
    foreach varname in (DebugCommand("localvars").results)
	    var varidx := _varname_iter-1;
		var value := DebugCommand("localvar "+varidx).results[1];
%>
  <tr>
    <td><%=varname%></td>
	<td><%=value%></td>
  </tr>
<%
	endforeach
%>
</table>
<hr>
</pre>
<form action=debug2.ecl method=GET>
   <input type=hidden name=pid value=<%=pid%>>
   <input type=submit name=Action value="InsTrace">
   <input type=submit name=Action value="StepInto">
   <input type=submit name=Action value="StepOver">
   <input type=submit name=Action value="Refresh">
   <input type=submit name=Action value="StopDebugging">
   <input type=submit name=Action value="Break">
</form>

<pre>
<%
    // Show execution point
	var rsp := DebugCommand( "ins" );
	foreach ins in (rsp.results)
	    WriteHtml( ins );
	endforeach
%>
</pre>

</html>
