use os;
use cfgfile;

function checkBans (who)

  var ban := who.ip;
  var acct := FindAccount(who.acctname);
  var bancfg := ReadConfigFile("bannedips");
  var searchbanned := FindConfigElem( bancfg, Cstr(ban) );
  if( searchbanned )
     DisconnectClient(who);
	 return 0;
  endif
  return;
  
endfunction

function verifyStaff(who)

  if(who.cmdlevel >= 1)
  var staffipcfg := ReadConfigFile("staffips");
  var staffacct := who.acctname;
  var searchstaff := FindConfigElem( staffipcfg, Cstr(who.acctname) );
  var searchstaffip := searchstaff.ipmatch;
  var ignore := searchstaff.ignore;
    if(!ignore)
      if(!searchstaff)
          SendSysMessage( who, "This is not a registered staff account" );
          syslog("Ip: " + who.ip + " attempted illegal use of cmdlevel char on account " + who.acctname);
          sleep(2);
          DisconnectClient(who);
          return 0;
      elseif(searchstaff)
          if(searchstaffip != who.ip)
            SendSysMessage( who, "You are not authorized to use this account!" );
            syslog("Ip: " + who.ip + " attempted illegal use of account " + who.acctname);
            sleep(3);
            DisconnectClient(who);
            return 0;
          endif
      endif
    endif
  SendSysMessage(who, "Staff Account Verified!", 3, 40);
  if(ignore)
    Syslog("Account " + who.acctname + " used Ignore IP on " + who.ip);
  endif
  endif
  
  return;
  
endfunction

function multiUO(who)
  var allowedcfg := ReadConfigFile("allowedips");
  var allowanycfg := ReadConfigFile("allowany");
  var allowany := who.acctname;
  var allow := who.ip;
  var searchallowed := FindConfigElem( allowedcfg, Cstr(who.ip) );
  var searchallowany := FindConfigElem( allowanycfg, Cstr(who.acctname) );
  var anyelem := searchallowany.AllowMatch;
// Section for multiple ip connections allowed
  foreach onlineperson in EnumerateOnlineCharacters()
  if((onlineperson.cmdlevel <= 1) or (who.cmdlevel <= 1))
    if((who.ip == onlineperson.ip) && (who.serial != onlineperson.serial))
      if(!searchallowed)
        if(!searchallowany)
          SendSysMessage( who, "You are not allowed to connect more than once." );
          sleep(5);
          DisconnectClient(who);
          return 0;
        endif
      endif
    endif
  endif
  endforeach
// End multpiple check section
  return;
endfunction

function ipban(who)

	var victim := who;

	if (!victim.ip)
		syslog("Invalid ip for attempted illegal use of staff acct " + FindAccount(who.acctname));
		return;
	endif

    var bancfg := ReadConfigFile("bannedips");
    var elem:={ };
    elem.append({"IPMatch",  CStr(victim.ip)});
    var banned := AppendConfigFileElem("bannedips", "BannedIP", CStr(victim.ip), elem);


    if(banned)
    syslog("Banned IP via staff security: " + CStr(victim.ip) );
    UnloadConfigFile( "bannedips" );
    else
    syslog("Ip not banned: staff security : ip: " + who.ip);
    endif
endfunction
