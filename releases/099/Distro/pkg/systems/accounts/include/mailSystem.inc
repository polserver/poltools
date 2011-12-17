/*
        Created by *Edwards

        2010-01-03
*/

include ":datafile:datafile";

var email_datafile := DFOpenDataFile( ":accounts:emails", DF_CREATE );

function CheckEmail( mobile )

         var data_elem := DFFindElement( email_datafile, mobile.acctname );
	 var email := data_elem.GetProp( "EMail" );
	 if( !email )
		  var acct := mobile.acct;
                  email := acct.EMail;
                  if( !email )
                          email := "None Specified";
                  endif
	 endif

         return email;
endfunction

function SaveMail( mobile, byref input, byref new_mail )

         new_mail := GFExtractData( input, new_mail );
         if( !Setmail( new_mail ))
                  return 0;
         endif

         var acct := mobile.acct;
         acct.SetProp( "EMail", new_mail );

         var data_elem := DFFindElement( email_datafile, mobile.acctname, DF_CREATE );

	 data_elem.SetProp( "LastEmailUpdate", polcore().systime );
	 data_elem.SetProp( "EMail", new_mail );

         return 1;
endfunction

function Setmail( new_mail )

         if( new_mail == "" || !new_mail )
                  return 0;
         endif

         var control_dot := 0,
             control_arobas := 0,
             length := len( new_mail );

         var i;
         for( i:=0; i<=length; i+=1 )
                  if( new_mail[i] == "@" && i > 3 )
        		control_arobas := 1;
                  elseif( new_mail[i] == "." )
        		if( new_mail[i+1] == "n" && new_mail[i+2] == "e" && new_mail[i+3] == "t" )
      				control_dot := 1;
      			elseif( new_mail[i+1] == "o" && new_mail[i+2] == "r" && new_mail[i+3] == "g" )
      				control_dot := 1;
      			elseif( new_mail[i+1] == "g" && new_mail[i+2] == "o" && new_mail[i+3] == "v" )
      				control_dot := 1;
      			elseif( new_mail[i+1] == "c" && new_mail[i+2] == "o" && new_mail[i+3] == "m" )
                                control_dot := 1;
                        elseif( new_mail[i+1] == "e" && new_mail[i+2] == "d" && new_mail[i+3] == "u" )
                                control_dot := 1;
      			elseif( new_mail[i+1] == "c" && new_mail[i+2] == "c" )
       				control_dot := 1;
      			elseif( new_mail[i+1] == "u" && new_mail[i+2] == "k" )
                                control_dot := 1;
      			elseif( new_mail[i+1] == "d" && new_mail[i+2] == "k" )
       				control_dot := 1;
      			elseif( new_mail[i+1] == "f" && new_mail[i+2] == "i" )
       				control_dot := 1;
      			elseif( new_mail[i+1] == "z" && new_mail[i+2] == "a" )
       				control_dot := 1;
      			elseif( new_mail[i+1] == "d" && new_mail[i+2] == "e" )
       				control_dot := 1;
      			elseif( new_mail[i+1] == "c" && new_mail[i+2] == "a" )
       				control_dot := 1;
      			elseif( new_mail[i+1] == "n" && new_mail[i+2] == "z" )
       				control_dot := 1;
      			elseif( new_mail[i+1] == "s" && new_mail[i+2] == "e" )
       				control_dot := 1;
      			elseif( new_mail[i+1] == "n" && new_mail[i+2] == "l" )
       				control_dot := 1;
      			endif
                  endif
         endfor

         if( !control_arobas || !control_dot )
                  return 0;
         endif

         return 1;
endfunction

function MaxAccountEmail( email_adress )

	var settings := ACCT_GetSettingsCfgElem( "Settings" );
	var maxAccounts := CInt( GetConfigInt( settings, "MaxAccounts" ));
        if( !maxAccounts )
		return 0;
	endif

        var email := 0;
        foreach acct_name in ( ListAccounts() )
                var account := FindAccount( acct_name );
                if( account.GetProp( "EMail" ) == email_adress )
                        email += 1;
                endif

		SleepMS(10);
	endforeach

        if( email > maxAccounts )
                return 1;
        endif

        return 0;
endfunction
