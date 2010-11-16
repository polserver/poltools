/* $Id: email.inc 640 2006-08-02 09:12:23Z austinheilman $
 *
 */

use uo;
use os;

/*
 * SendEmail(to_list, subject, message)
 *
 * Purpose
 * Serves as an interface for external scripts to send email messages.
 *
 * Parameters
 * to_list:	String containing addresses to send the email to.
 * subject:	String containing the subject of the email message.
 * message:	String containing the message to send.
 *
 * Return Values:
 * Returns a script reference or error
 *
 */
function SendEmail(to_list:="", subject:="Subject", message:="message")
	var script := Start_Script(":email:email", array{0, 0, to_list, subject, message});

	return script;
endfunction

