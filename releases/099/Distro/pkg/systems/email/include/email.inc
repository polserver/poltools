/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2010-05-10
*/
use uo;
use util;

include ":datafile:datafile";
include ":email:email_const";
include ":gumps:gumps";
include ":gumps:gumps_ex";
include ":gumps:gumpprompt";
include ":gumps:playerselectiongump";

var email_data_file := DFOpenDataFile( "Emails", DF_CREATE );
var block_list_data_file := DFOpenDataFile( "BlockLists", DF_CREATE );

var mail_elem;
var blocklist_elem;

function EmailIndex( who )

        mail_elem := DFFindElement( email_data_file, Hex( user_serial ), DF_CREATE );

        var gump := GFCreateGump();
        GFClosable( gump, 0 );

        GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 640, 480 );
        GFResizePic( gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 480, 450 );
        
        GFResizePic( gump, 500, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 125, 50 );
        GFResizePic( gump, 500, 70, GFGetCfgConst( "Defaults", "ForeGround" ), 125, 395 );
        
        GFAddAlphaRegion( gump, 0, 0, 640, 480 );

        GFTextLine( gump, 25, 20, 1720, "Read" );
        GFTextLine( gump, 80, 20, 1720, "Sender" );
        GFTextLine( gump, 250, 20, 1720, "Subject" );
        GFTextMid( gump, 25, 30, 500, 2100, "---------------------------------------------------------------" );

        var y_pos := 50;
        var counter := 1;
        var page_num := 1;

        GFPage( gump, page_num );
        foreach mail_number in ( mail_elem.propnames() )
                SleepMS(5);
                
                if( mail_number == "NewMsg" )
                        continue;
                endif
                
                var mail_info := mail_elem.GetProp( mail_number );
                        
                GFAddButton( gump, 30, y_pos+3, 2117, 2118, GF_CLOSE_BTN, mail_number );
                GFTextLine( gump, 80, y_pos, 2100, mail_info.from_name );
                        
                var subject := mail_info.subject;
                if( Len( subject ) > 23 )
                        subject := subject[1, 23]+"[...]";
                endif
                
                if( mail_info.unread )
                        GFTextCrop( gump, 250, y_pos, 210, 20, 1720, "[New]" );
                        GFTextCrop( gump, 290, y_pos, 210, 20, 2100, subject );
                else
                        GFTextCrop( gump, 250, y_pos, 210, 20, 2100, subject );
                endif

                y_pos := y_pos + 20;
                counter := counter + 1;
                if( counter >= 20 )
                        GFAddButton( gump, 465, 440, 2648, 2649, GF_PAGE_BTN, page_num+1 );
                        page_num := page_num+1;
                        GFPage( gump, page_num );
                        GFAddButton( gump, 465, 20, 2650, 2651, GF_PAGE_BTN, page_num-1 );

                        counter := 1;
                        y_pos := 50;
                endif
        endforeach

        GFPage( gump, 0 );

        GFTextMid( gump, 510, 20, 125, 1720, "Box Number" );
        GFTextMid( gump, 505, 40, 125, 2100, cstr( user_serial ));

        GFAddButton( gump, 510, 108, 2117, 2118, GF_CLOSE_BTN, NEW_LETTER );
        GFTextLine( gump, 530, 105, 1720, "New Email" );

        GFAddButton( gump, 510, 133, 2117, 2118, GF_CLOSE_BTN, DELETE_ALL );
        GFTextLine( gump, 530, 130, 1720, "Delete All" );

        GFAddButton( gump, 510, 158, 2117, 2118, GF_CLOSE_BTN, BLOCK_LIST );
        GFTextLine( gump, 530, 155, 1720, "Block List" );

        if( who.cmdlevel > 2 )
                GFAddButton( gump, 510, 183, 2117, 2118, GF_CLOSE_BTN, MASS_EMAIL );
                GFTextLine( gump, 530, 180, 1720, "Mass Email" );
        endif

        GFAddButton( gump, 515, 430, 0xFB6, 0xFB5, GF_CLOSE_BTN, CLOSE_EMAIL );
        GFTextLine( gump, 550, 430, 1720, "Exit" );

        var input := GFSendGump( who, gump );
        if( !input || input[CLOSE_EMAIL] )
                SendSysMessage( who, "Cancelled." );
                return 0;
        elseif( input[NEW_LETTER] )
                NewLetter( who );
        elseif( input[DELETE_ALL] )
                foreach mail_number in ( mail_elem.propnames() )
                        if( mail_number != "NewMsg" )
                                mail_elem.EraseProp( Hex( mail_number ));
                        endif
                        SleepMS(5);
                endforeach
        elseif( input[BLOCK_LIST] )
                BlockList( who );
        elseif( input[MASS_EMAIL] )
                NewLetter( who, "all" );
        else
                ReadMail( who, input[0] );
        endif

        return 1;
endfunction

function NewLetter( who, send_to:=" ", subject:=" ", message:=array{} )

	var gump := GFCreateGump();
	GFClosable( gump, 0 );

	GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 640, 480 );
	GFResizePic( gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 610, 80 );
 
	GFResizePic( gump, 15, 410, GFGetCfgConst( "Defaults", "ForeGround" ), 610, 55 );
 
        GFAddAlphaRegion( gump, 0, 0, 640, 480 );

	GFTextLine( gump, 25, 23, 1720, "Send To :" );
	GFGumpPic( gump, 100, 20, 1141 );
	var input_send := GFTextEntry( gump, 110, 22, 250, 20, 2100, send_to );

	GFTextLine( gump, 25, 48, 1720, "Subject :" );
	GFGumpPic( gump, 100, 45, 1141 );
	var input_subject := GFTextEntry( gump, 110, 47, 250, 20, 2100, subject );

	GFTextLine( gump, 25, 70, 2100, "Note: To send to multiple recipients, separate box numbers with a space." );

	GFTextLine( gump, 415, 23, 1720, "Addresses" );
	GFAddButton( gump, 380, 23, 4008, 4009, GF_CLOSE_BTN, ADDRESS_BOOK );

	var msg_array := array{},
	    y_pos := 100,
	    i;

	GFResizePic( gump, 15, 95, 2620, 610, 310 );
	for( i:=1; i<=MAX_MESSAGES; i+=1 )
                if( message[i] )
                        msg_array.Append( GFTextEntry( gump, 20, y_pos, 595, 20, 2100, message[i] ));
                else
                        msg_array.Append( GFTextEntry( gump, 20, y_pos, 595, 20, 2100, "" ));
                endif
		y_pos := y_pos+20;
                SleepMS(5);
	endfor

	GFAddButton( gump, 30, 425, 0xFB0, 0xFAF, GF_CLOSE_BTN, CLOSE_EMAIL );
	GFTextLine( gump, 65, 425, 1720, "Back" );

	GFAddButton( gump, 240, 425, 2117, 2118, GF_CLOSE_BTN, WORD_WRAP );
	GFTextLine( gump, 260, 425, 1720, "Word Wrap" );

 	GFAddButton( gump, 495, 425, 0xFBF, 0xFBE, GF_CLOSE_BTN, SEND_EMAIL );
	GFTextLine( gump, 530, 425, 1720, "Send Message" );

	var input := GFSendGump( who, gump );
        if( !input )
                return 0;
        elseif( input[CLOSE_EMAIL] )
                return 1;
        endif

	var temp_string := "";
	for( i:=1; i<=msg_array.Size(); i+=1 )
                var temp_msg := GFExtractData( input, msg_array[i] );
                if( temp_msg )
                        msg_array[i] := temp_msg;
		        temp_string := temp_string+" "+msg_array[i];
                endif
                SleepMS(5);
	endfor

	send_to := GFExtractData( input, input_send );
	subject := GFExtractData( input, input_subject );
	msg_array := GFWordWrap( temp_string, 660 );

        if( input[ADDRESS_BOOK] )
                var char_ref := SelectCharacter( who );
                if( !char_ref )
		        NewLetter( who, send_to, subject, message );
                else
		        NewLetter( who, send_to+" "+char_ref.serial, subject, message );
                endif
        elseif( input[WORD_WRAP] )
		NewLetter( who, send_to, subject, msg_array );
        elseif( input[SEND_EMAIL] )
		SendMail( who, send_to, subject, msg_array );
        endif

        return 1;
endfunction

function ReadMail( who, number )

	var mail_info := mail_elem.GetProp( Hex( number ));
        if( mail_info.unread )
                mail_info.unread := 0;
                mail_elem.SetProp( Hex( number ), mail_info );
        endif

	var gump := GFCreateGump();
	GFClosable( gump, 0 );

	GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 640, 480 );
	GFResizePic( gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 610, 80 );

	GFResizePic( gump, 15, 410, GFGetCfgConst( "Defaults", "ForeGround" ), 610, 55 );
 
        GFAddAlphaRegion( gump, 0, 0, 640, 480 );

        var box_info := "[Box: "+mail_info.from_box+"]";
        if( mail_info.from_box == "None" )
                box_info := "";
        endif

	GFTextLine( gump, 25, 20, 1720, "From: "+mail_info.from_name+""+box_info );
	GFTextLine( gump, 25, 40, 1720, "Subject: "+mail_info.subject );

        var time := StrFormatTime( "%m-%d-%Y %I:%M:%S %p", CInt( mail_info.time ));
        var server_time := StrFormatTime( "%m-%d-%Y %I:%M:%S %p", CInt( POLCore().systime ));

	GFTextLine( gump, 25, 60, 1720, "Mail Time: "+time+"        Server Time: "+server_time );

	var y_pos := 100,
	    i;

	GFResizePic( gump, 15, 95, 2620, 610, 310 );
	for( i:=1; i<=MAX_MESSAGES; i+=1 )
                if( mail_info.message[i] )
                        GFTextLine( gump, 20, y_pos, 2100, mail_info.message[i] );
                endif
		y_pos := y_pos+20;
                SleepMS(5);
	endfor

	GFAddButton( gump, 30, 425, 0xFB0, 0xFAF, GF_CLOSE_BTN, CLOSE_EMAIL );
	GFTextLine( gump, 65, 425, 1720, "Back" );

	GFAddButton( gump, 240, 425, 0xFB3, 0xFB2, GF_CLOSE_BTN, DELETE_MSG );
	GFTextLine( gump, 275, 425, 1720, "Delete" );

	GFAddButton( gump, 495, 425, 0xFBF, 0xFBE, GF_CLOSE_BTN, mail_info.from_box );
	GFTextLine( gump, 530, 425, 1720, "Reply" );

	var input := GFSendGump( who, gump );
        if( !input )
                return 0;
        elseif( input[mail_info.from_box] )
		NewLetter( who, mail_info.from_box, "Re: "+mail_info.subject );
        elseif( input[DELETE_MSG] )
		mail_elem.EraseProp( Hex( number ));
        endif

        return 1;
endfunction

function BlockList( who )

	blocklist_elem := DFFindElement( block_list_data_file, Hex( user_serial ), DF_CREATE );

	var gump := GFCreateGump();
	GFClosable( gump, 0 );

 	GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 640, 480 );
	GFResizePic( gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 610, 25 );

	GFResizePic( gump, 15, 45, GFGetCfgConst( "Defaults", "ForeGround" ), 610, 420 );
 
        GFAddAlphaRegion( gump, 0, 0, 640, 480 );

	GFTextMid( gump, 15, 15, 620, 1720, "Block List" );

	GFAddButton( gump, 30, 425, 0xFB0, 0xFAF, GF_CLOSE_BTN, CLOSE_EMAIL );
	GFTextLine( gump, 65, 425, 1720, "Back" );

	GFAddButton( gump, 340, 425, 0xFA8, 0xFA9, GF_CLOSE_BTN, ADDRESS_BOOK );
	GFTextLine( gump, 375, 425, 1720, "Update List" );

	GFAddButton( gump, 460, 425, 0xFA4, 0xFA3, GF_CLOSE_BTN, DELETE_ALL );
	GFTextLine( gump, 495, 425, 1720, "Erase List" );

	var entry_info := array{},
	    entry_list := array{},
	    button_list := array{};

	var y_pos := 65,
	    page_num := 1,
	    entry_num := 0,
	    counter := 1,
	    line_num;

	GFPage( gump, page_num );
	for( line_num:=1; line_num<=MAX_LINES; line_num+=1 )
		entry_num += 1;

		GFTextLine( gump, 25, y_pos, 1720, "Number" );
		GFTextLine( gump, 125, y_pos, 1720, "Name" );

		GFResizePic( gump, 25, y_pos+20, 2620, 100, 30 );
		GFResizePic( gump, 120, y_pos+20, 2620, 180, 30 );

		entry_info := blocklist_elem.GetProp( Hex( entry_num ));
		if( entry_info )
			entry_info[1] := GFTextEntry( gump, 35, y_pos+25, 80, 25, 2100, entry_info[1] );
			entry_info[2] := GFTextEntry( gump, 130, y_pos+25, 180, 25, 2100, entry_info[2] );
                else
                        entry_info := array{};
			entry_info[1] := GFTextEntry( gump, 35, y_pos+25, 80, 25, 2100, "" );
			entry_info[2] := GFTextEntry( gump, 130, y_pos+25, 180, 25, 2100, "" );
		endif

		entry_list.Append( entry_info );

		entry_num += 1;

		GFTextLine( gump, 340, y_pos, 1720, "Number" );
		GFTextLine( gump, 440, y_pos, 1720, "Name" );

		GFResizePic( gump, 340, y_pos+20, 2620, 100, 30 );
		GFResizePic( gump, 435, y_pos+20, 2620, 180, 30 );

		entry_info := blocklist_elem.GetProp( Hex( entry_num ) );
		if( entry_info )
			entry_info[1] := GFTextEntry( gump, 350, y_pos+25, 80, 25, 2100, entry_info[1] );
			entry_info[2] := GFTextEntry( gump, 445, y_pos+25, 180, 25, 2100, entry_info[2] );
                else
                        entry_info := array{};
			entry_info[1] := GFTextEntry( gump, 350, y_pos+25, 80, 25, 2100, "" );
			entry_info[2] := GFTextEntry( gump, 445, y_pos+25, 180, 25, 2100, "" );
		endif

		entry_list.Append( entry_info );

		y_pos += 50;
		counter += 1;

		if( counter > 7 && line_num < MAX_LINES )
			GFAddButton( gump, 605, 420, 2648, 2649, GF_PAGE_BTN, page_num+1 );
			page_num += 1;
			GFPage( gump, page_num );
			GFAddButton( gump, 605, 40, 2650, 2651, GF_PAGE_BTN, page_num-1 );

			counter := 1;
			y_pos := 65;
		endif

                SleepMS(5);
	endfor

	var input := GFSendGump( who, gump );
        if( !input )
                return 0;
        elseif( input[CLOSE_EMAIL] )
                return 1;
        elseif( input[ADDRESS_BOOK] )
		for( entry_num; entry_num>0; entry_num -= 1 )
                        var elem := entry_list[entry_num];
			var number := GFExtractData( input, elem[1] );
			var name := GFExtractData( input, elem[2] );

                        if( number && name )
			        blocklist_elem[entry_num].SetProp( Hex( entry_num ), {number, name} );
                        endif

                        SleepMS(5);
		endfor
                return BlockList( who );
        elseif( input[DELETE_ALL] )
		foreach propname in ( blocklist_elem.PropNames() )
			blocklist_elem.EraseProp( propname );
                	SleepMS(5);
		endforeach
                return BlockList( who );
        endif

        return 1;
endfunction

function SendMail( who, send_to, subject, msg_array )

	while( subject[1] == " " )
		subject[1] := "";
                SleepMS(5);
	endwhile

        if( send_to != "all" )
                send_to := SplitWords( send_to );

                var unique_box := array{};
                foreach box_number in send_to
                        SleepMS(5);
                        
		        var mail_box := DFFindElement( email_data_file, Hex( box_number ), DF_CREATE ),
                            track_serial := SystemFindObjectBySerial( CInt( box_number ), SYSFIND_SEARCH_OFFLINE_MOBILES );
                            
                        if( !track_serial.IsA( POLCLASS_MOBILE ) || track_serial.IsA( POLCLASS_NPC ))
                                GumpPrompt( who, "Box number "+box_number+" does not exist." );
                                return NewLetter( who );
                        elseif( CheckBlocked( user_serial, box_number ))
                                GumpPrompt( who, "Could not send message to box "+box_number+"." );
                                return NewLetter( who );
		        endif

                        if( box_number in unique_box )
                                continue;
                        else
                                unique_box.Append( box_number );
                        endif
	        endforeach
                send_to := unique_box;

                if( !send_to.Size() )
                        GumpPrompt( who, "Please enter a mail box number."  );
                        return NewLetter( who );
                elseif( !subject )
                        GumpPrompt( who, "Please enter a subject."  );
                        return NewLetter( who );
                endif
        endif

	var gump := GFCreateGump();
	GFClosable( gump, 0 );

 	GFResizePic( gump, 0, 0, GFGetCfgConst( "Defaults", "BackGround" ), 640, 480 );
	GFResizePic( gump, 15, 15, GFGetCfgConst( "Defaults", "ForeGround" ), 610, 25 );

	GFResizePic( gump, 15, 45, GFGetCfgConst( "Defaults", "ForeGround" ), 610, 420 );
 
        GFAddAlphaRegion( gump, 0, 0, 640, 480 );

	GFTextMid( gump, 15, 15, 620, 1720, "Mail Information" );

 	GFAddButton( gump, 495, 425, 0xFA7, 0xFA6, GF_CLOSE_BTN, SEND_EMAIL );
	GFTextLine( gump, 530, 425, 1720, "Continue" );

	if( !who )
		who := struct;
		who.+name := "N/A";
		who.+acctname := "N/A";
	endif

	var email := struct;
	email.+subject := subject;
	email.+message := msg_array;
	email.+from_name := who.name;
	email.+from_box := user_serial;
	email.+time := POLCore().systime;
	email.+unread := 1;

	if( send_to != "all" || !who.cmdlevel )
		var y_pos := 55;
		foreach box_number in send_to
			var mail_box := DFFindElement( email_data_file, Hex( box_number ));
			if( !mail_box )
				GFTextLine( gump, 20, y_pos, 2100, "Box number "+box_number+" does not exist." );
				y_pos += 20;
			elseif( CheckBlocked( user_serial, box_number ))
				GFTextLine( gump, 20, y_pos, 2100, "Could not send message to box "+box_number );
				y_pos += 20;
			else
				MessageToBox( mail_box, box_number, email );

				GFTextLine( gump, 35, y_pos, 2100, "Message to box "+box_number+" was sent." );
				y_pos += 20;
			endif
                	SleepMS(5);
		endforeach
        else
        	email.from_name := "Fantasia Shard";
	        email.from_box := "None";

                GFTextLine( gump, 35, 55, 2100, "This may take a while. Please wait..." );
 	        foreach acct_name in ( ListAccounts() )
                        var account := FindAccount( acct_name );
		        var i := 1;
                        while( i <= 5 )
                                var character := account.GetCharacter(i);
                                if( character )
                                        var mail_box := DFFindElement( email_data_file, Hex( character.serial ), DF_CREATE );
                                        MessageToBox( mail_box, character.serial, email );
                                endif
                                i += 1;
                                SleepMS(5);
                        endwhile
                        SleepMS(5);
	        endforeach
                GFTextLine( gump, 35, 75, 2100, "Message was sent to all players." );
        endif

	var input := GFSendGump( who, gump );
	if( !input )
		return 0;
	elseif( input[SEND_EMAIL] )
		return 1;
	endif

        return 1;
endfunction

function notify_player( serial )

        var mobile := SystemFindObjectBySerial( CInt( serial ));
        if( mobile )
                var script := start_script( ":email:emailMessage/newEmail", mobile );
                if( script.errortext )
                        SendSysMessage( mobile, "Error starting script <:email:emailMessage/newEmail> -->"+script.errortext );
                        return 0;
                endif
        endif

        return 1;
endfunction

function CheckBlocked( serial, box_number )

	var block_list := DFFindElement( block_list_data_file, Hex( box_number ));
	foreach entry in ( block_list.PropNames() )
		var prop_info := block_list.GetProp( Hex( entry ));
		if( CInt( serial ) == CInt( prop_info[1] ))
			return 1;
		endif
                SleepMS(5);
	endforeach

	return 0;
endfunction

function MessageToBox( mail_box, serial, email )

	var message := mail_box.GetProp( "NewMsg" );
	if( !message )
		message := 1;
	elseif( message > MAX_MESSAGES )
		message := 1;
	else
		message += 1;
	endif

	mail_box.SetProp( "NewMsg", message );
	mail_box.SetProp( Hex( message ), email );

	notify_player( serial );

        return 1;
endfunction
