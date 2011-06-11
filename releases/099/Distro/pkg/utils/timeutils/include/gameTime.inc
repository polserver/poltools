/* $Id: gameTime.inc 373 2006-06-17 18:27:33Z austinheilman $
 *
 *
 */
 
use uo;
use os;

include ":timeutils:time";

/*
 * GetGameTime(day_length)
 *
 * Purpose
 * Takes the value from ReadGameClock() and converts it into a time.
 *
 * Parameters
 * day_length: Number of real life seconds a game day should last.
 *
 * Return Value
 * Returns a string
 *
 */
function TIME_GetGameTime(day_length)
	var cur_time := ReadGameClock();
endfunction

function TIME_GetReadableString()

	var MonthDays := array{ 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

	var MonthNames := array {
		"January",
		"February",
		"March",
		"April",
		"May",
		"Juin",
		"July",
		"August",
		"September",
		"October",
		"November",
		"December"
	};

	//Seconds since 1/1/1970
	var systime := polcore().systime;
 
	//Lets skip ahead to y2k
	systime -= 946684800;
 
	//convert to days
	var days := CInt( systime / SECONDS_IN_DAY ) + 1;

	var current_year_counter := 0;
	var current_month_counter := 1;
	var days_in_month, done := 0;
 
	repeat
		if( current_month_counter == 2 )
			if( current_year_counter % 4 == 0 )
				days_in_month := 29;
			else
				days_in_month := 28;
			endif
		else
			days_in_month := MonthDays[current_month_counter];
		endif

		if( days_in_month <= days )
			days -= days_in_month;
			current_month_counter += 1;
			if( current_month_counter > 12 )
				current_month_counter := 1;
				current_year_counter += 1;
			endif
		else
			done := 1;
		endif
  
                SleepMS(2);
	until( done );

	if( days < 10 )
		days := "0"+days;
	endif
 
	var timestring := MonthNames[current_month_counter]+" "+days+", "+( 2000 + current_year_counter );

	//now figure out hours and minutes
	systime := polcore().systime;
	systime := CInt( systime % SECONDS_IN_DAY );
	var hour := CInt( systime / 3600 );

	systime := polcore().systime;
	systime := CInt( systime % 3600 );
	var minute := CInt( systime / 60 );

	if( hour == 0 )
		hour := 24;
	elseif( hour < 10 )
		hour := "0"+hour;
	endif
 
	if( minute < 10 )
		minute := "0"+minute;
	endif

	timestring := timestring+" "+hour+":"+minute+"GMT";
 
	return timestring;
endfunction

function TIME_GetCurrentMonth()

	var year := TIME_Systime_GetYear();
	var month := Lower( TIME_Systime_GetMonth() );

	return( year+"_"+month );
endfunction

function TIME_GetCurrentPlaytimeMonth()

	var year := TIME_Systime_GetYear();
	var month := Lower( TIME_Systime_GetMonth() );

	return( year+"_"+month+"_play_time" );
endfunction

function TIME_Systime_GetYear()

	//Seconds since 1/1/1970
	var systime := polcore().systime;
 
	//Lets skip ahead to y2k
	systime -= 946684800;
 
	var current_year_counter := 0;
	var done := 0;
	repeat
		if( current_year_counter % 4 == 0 )
			systime -= 31622400;
		else
			systime -= 31536000;
		endif
  
		if( systime >= 0 )
			current_year_counter += 1;
		else
			done:= 1;
		endif
  
                SleepMS(2);
	until( done );
 
	return CInt( 2000 + current_year_counter );
endfunction

function TIME_Systime_GetMonth()

	var MonthDays := array{ 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

	var MonthNames := array {
		"January",
		"February",
		"March",
		"April",
		"May",
		"June",
		"July",
		"August",
		"September",
		"October",
		"November",
		"December"
	};

	//Seconds since 1/1/1970
	var systime := polcore().systime;
 
	//Lets skip ahead to y2k
	systime -= 946684800;

	//convert to days
	var days := CInt( systime / SECONDS_IN_DAY );

	var current_year_counter := 0;
	var current_month_counter := 1;
	var done := 0;
	var days_in_month := 0;
 
	repeat
		if( current_month_counter == 2 )
			if( current_year_counter % 4 == 0 )
				days_in_month := 29;
			else
				days_in_month := 28;
			endif
		else
			days_in_month := MonthDays[current_month_counter];
		endif

		if( days_in_month <= days )
			days -= days_in_month;
			current_month_counter += 1;
			if( current_month_counter > 12 )
				current_month_counter := 1;
				current_year_counter += 1;
			endif
		else
			done := 1;
		endif

                SleepMS(2);
	until( done );

	return MonthNames[current_month_counter];
endfunction

function TIME_GetTimeDesc( seconds, showseconds := 0 )

	seconds := CInt( seconds );
	var minutes := CInt( seconds / 60 );
	var hours := CInt( minutes / 60 );
	var days := CInt( hours / 24 );

	if( seconds < 60 )
		if( showseconds )
			return( seconds+" seconds" );
		else
			return "< 1 minute";
		endif
	endif

	hours -= days * 24;
	minutes -= ( days * 24 + hours )*60;

	var returnstring := "";
	if( days )
		if( days > 1 )
			returnstring := returnstring+days+" days, ";
		else
			returnstring := returnstring+"1 day, ";
		endif
	endif

	if( hours || days )
		if( hours > 1 )
			returnstring := returnstring+hours+" hours, ";
		else
			returnstring := returnstring+"1 hour, ";
		endif
	endif

	if( minutes || hours || days )
		if( minutes > 1 )
			returnstring := returnstring+minutes+" minutes";
		else
			returnstring := returnstring+"1 minute";
		endif
	endif

	if( showseconds )
		seconds := CInt( seconds % 60 );
		if( seconds > 1 )
			returnstring := returnstring+", "+seconds+" seconds ";
		elseif( seconds == 1 )
			returnstring := returnstring+", "+"1 second ";
		else
			returnstring := returnstring+", "+"0 seconds ";
		endif
	endif

	return returnstring;
endfunction
