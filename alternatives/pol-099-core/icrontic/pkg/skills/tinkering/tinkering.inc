function IsExceptional(thetool)
// returns 1 if the char is using an exceptional tool
//    SendSysmessage(me,"ToolName: " + thetool.name);
    
    if (find(lower(thetool.name), "exceptional",1))
		return 1;
      else
		return 0;
    endif

endfunction

function ReportError(me, errtext, errfunction)

    //Reports an error to the player and logs to the syslog
    SendSysMessage(me, "An Error occurred in the Tinkering/" + errfunction + ":" + errtext + " : Current Character: " + me.name, 3, 53);
    SysLog("An Error occurred in the Tinkering/" + errfunction + ":" + errtext + " : Current Character: " + me.name);

endfunction
