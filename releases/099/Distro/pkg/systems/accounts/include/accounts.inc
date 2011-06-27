/*
        Maintened by *Edwards
        
        2009-06-04
        
        Last Modifications 2010-01-24
             * Changed order of param for CreateNewAccount()
             * Added signup_ip param to CreateNewAccount()

        Last Modifications 2010-03-01
             * Removed MaxAccountIPs()
             * Removed CheckForMaxClientsOnline()
*/
use uo;
use os;

include "include/security";
include ":accounts:settings";

// Max numbers of characters an account can have.
CONST ACCT_MAX_CHARACTERS	:= 5;

function CreateNewAccount( username, password, signup_ip, email:="", cmdlevel:=0, expansion:="" )

	var account := CreateAccount( username, password, 1 );
	if( account.errortext )
		return account;
	endif

	if( cmdlevel )
		account.SetDefaultCmdLevel( CInt( cmdlevel ));
	endif

	if( expansion )
		account.Set_UO_Expansion( expansion );
	endif

	if( email )
		account.SetProp( "EMail", email );
	endif

	account.SetProp( "SignupIP", signup_ip );
	ACCT_SetLastLogin( account );

	return account;
endfunction

function ACCT_SetLastLogin( account, time:=0 )

	if( TypeOf( account ) != "AccountRef" )
		account := FindAccount( account );
		if( account.errortext )
			return account;
		endif
	endif

	if( !time )
		time := POLCore().systime;
	endif
        
	return account.SetProp( "LastLogin", time );
endfunction

function ACCT_GetLastLogin( account )

	if( TypeOf( account ) != "AccountRef" )
		account := FindAccount( account );
		if( account.errortext )
			return account;
		endif
	endif

	return CInt( account.GetProp( "LastLogin" ));
endfunction

function ACCT_GetCharacters( account )

	if( TypeOf( account ) != "AccountRef" )
		account := FindAccount( account );
		if( account.errortext )
			return account;
		endif
	endif

	var mobile_list := array;
	for i:=1 to ACCT_MAX_CHARACTERS
		var mobile := account.GetCharacter(i);
		if( mobile )
			mobile_list.Append( mobile );
		endif
		SleepMS(2);
	endfor

	return mobile_list;
endfunction

function ACCT_GetNumCharacters( account )

	if( TypeOf( account ) != "AccountRef" )
		account := FindAccount( account );
		if( account.errortext )
			return account;
		endif
	endif

	var count := 0;
	for i:=1 to ACCT_MAX_CHARACTERS
		if( account.GetCharacter( i ))
			count := count+1;
		endif
		SleepMS(2);
	endfor

	return count;
endfunction

function ACCT_DeleteAllCharacters( account )

	if( TypeOf( account ) != "AccountRef" )
		account := FindAccount( account );
		if( account.errortext )
			return account;
		endif
	endif

	var count := 0;
	for i:=1 to ACCT_MAX_CHARACTERS
		account.DeleteCharacter( i );
		SleepMS(2);
	endfor

	return count;
endfunction

function ACCT_DeleteAccount( account )

	if( TypeOf( account ) != "AccountRef" )
		account := FindAccount( account );
		if( account.errortext )
			return account;
		endif
	endif

	foreach package in ( POLCore().packages )
		Run_Script_To_Completion( ":"+package+":acctDelete/acctDelete", account );
		SleepMS(2);
	endforeach

	ACCT_DeleteAllCharacters( account );

	return account.Delete();
endfunction

function CheckForMaxClientsOnline( who )

	var settings := ACCT_GetSettingsCfgElem( "Settings" );
	var IpsOnline := GetConfigInt( settings, "MaxIpsOnline" );
        if( !IpsOnline )
		return 1;
	endif

        var ini_ip := who.ip;
        var ips := 0;
        foreach player in EnumerateOnlineCharacters()
                if( player.ip == ini_ip )
                        ips += 1;
                endif
		SleepMS(10);
	endforeach

        if( ips > IpsOnline )
                return 0;
        endif

        return 1;
endfunction

function VerifyStaffOnline( who )

        foreach player in EnumerateOnlineCharacters()
                SleepMS(5);

                if( who == player )
                        continue;
                elseif( player.ip != who.ip )
                        continue;
                endif
        endforeach

        return 1;
endfunction
