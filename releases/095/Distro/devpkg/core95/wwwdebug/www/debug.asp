<html>
<%
use os;
use uo;

var debuggerex;
var debug_ctx;
function DebugCommand( cmd )
    var request := struct;
	request.+pid := getpid();
	request.+command := cmd;

    debuggerex.sendevent( request );

	return wait_for_event( 200 );
endfunction

    var pid := QueryParam( "pid" );
    var action := QueryParam("Action");

	WriteHtml( "PID is " + pid );
	WriteHtml( "Action is " + action );
// find the debugger script for this PID:
	var pidkey := "#Debug"+pid;
	var debuggerpid := GetGlobalProperty( pidkey );
	if (debuggerpid)
		// debugger script already running
		WriteHtml( "Debugger PID is " + debuggerpid );
	    debuggerex := getprocess( debuggerpid );
	else
	    // there isn't a debugger yet, make one
		WriteHtml( "Starting Debugger script for PID" + pid );
		debuggerex := start_script( "debugsvc", pid );
	endif

	case action
		"InsTrace": DebugCommand( "instrace" );
		"StepInto": DebugCommand( "stepinto" );
	endcase

%>

<pre>
<%
    // Show execution point
	var rsp := DebugCommand( "ins" );
	foreach ins in (rsp.results)
	    WriteHtml( ins );
	endforeach
%>
</pre>
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
	var firstline := linenum-20;
	var lastline := linenum+20;
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
<%
    foreach varname in (DebugCommand("localvars").results)
	    var varidx := _varname_iter-1;
		var value := DebugCommand("localvar "+varidx).results[1];
%>
<!-- todo: translate < and > -->
 <%=varname%>: <%=value%>
<%
	endforeach
%>
	<%=DebugCommand("localvars").results%>
</pre>
<form action=debug.ecl method=GET>
   <input type=hidden name=pid value=<%=pid%>>
   <input type=submit name=Action value="InsTrace">
   <input type=submit name=Action value="StepInto">
   <input type=submit name=Action value="Refresh">
</form>
</html>
