/* $Id$
 * 
 *
 */

use uo;
use os;

include ":accounts:settings";

/* 
 * CreateNewAccount(username, password, cmdlevel, expansion, email)
 *
 * Purpose
 * Used to create a new account and set appropriate settings on it.
 *
 * Parameters:
 * username:	Username of the account to make.
 * password:	Password to set on the account. Note it is case-sensitive!
 * cmdlevel:	Default command level for the account.
 * expansion:	UO Expansion the account should use. DEfault is 'T2A'
 * email:	Email address to set on the account.
 *
 * Return Value:
 * Returns error on failure.
 * Returns an account object on success.
 *
 */
function CreateNewAccount(username, password, cmdlevel:=0, expansion:="", email:="")
	var account := CreateAccount(username, password, 1);
	if ( account.errortext )
		return account;
	endif
	
	if ( cmdlevel )
		account.SetDefaultCmdLevel(CInt(cmdlevel));
	endif
	
	if ( expansion )
		account.Set_UO_Expansion(expansion);
	endif
	
	if ( email )
		account.SetProp("EMail", email);
	endif
	
	return account;
endfunction
