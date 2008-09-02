/* $Id: time.inc 931 2006-12-28 12:55:37Z austinheilman $
 *
 * Purpose
 * Provide various functions and CONSTants for time formats.
 *
 * Created by Folko
 *
 */
use uo;

include ":timeutils:settings";

enum TIME_UNITS
	// Base unit: Millisecond
	MIL_SECOND	:= 1000,
	MIL_MINUTE	:= MIL_SECOND * 60,
	MIL_HOUR	:= MIL_MINUTE * 60,
	MIL_DAY		:= MIL_HOUR * 24,
	MIL_WEEK	:= MIL_DAY * 7,
	MIL_MONTH	:= MIL_DAY * 30,
	MIL_YEAR	:= MIL_MONTH * 12,

	// Base unit: Second
	SEC_MINUTE  	:= 60,
	SEC_HOUR	:= SEC_MINUTE * 60,
	SEC_DAY	 	:= SEC_HOUR * 24,
	SEC_WEEK	:= SEC_DAY * 7,
	SEC_MONTH	:= SEC_DAY * 30,
	SEC_YEAR	:= SEC_DAY * 365,

	// SleepMs value to prevent RunAway
	NO_RUNAWAY  := 10
endenum

enum TIME_HELPERS
	SECONDS_IN_MINUTE	:= 60,		// (60)
	SECONDS_IN_HOUR		:= 3600,	// (SECONDS_IN_MINUTE * 60)
	SECONDS_IN_DAY		:= 86400,	// (SECONDS_IN_HOUR * 24)
	SECONDS_IN_WEEK		:= 604800,	// (SECONDS_IN_DAY * 7)
	SECONDS_IN_YEAR		:= 31536000,	// (SECONDS_IN_DAY * 365)
	MINUTES_IN_HOUR		:= 60,		// (60)
	MINUTES_IN_DAY		:= 1440,	// (MINUTES_IN_HOUR * 24)
	MINUTES_IN_WEEK		:= 10080,	// (MINUTES_IN_DAY * 7 )
	HOURS_IN_DAY		:= 24,		// (24)
	HOURS_IN_WEEK		:= 168		// (HOURS_IN_DAY * 7)
endenum
