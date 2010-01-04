//fist find the year;
const YearLen := 3600*24*365;
const LeadYearLen := 3600*24*366;
const DayLen := 3600*24;

function GetTimeString()

var abstime := polcore().systime;

var year := 1970;
var month := 1;
var day :=1;
var hour :=0;
var min :=0;

while(abstime >= YearLen)
	if(mod_fast(year,4) =0) //LeadYear
		abstime := abstime-LeadYearLen;
	else
		abstime := abstime-YearLen;
	endif
	year := year+1;
endwhile

var monthlenlist := {   31,	   28,	 31,	   30,    31,     30,     31,    31,      30,
			    31,        30,       31, };
var monthname:={"Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep", "Oct","Nov","Dec",};

if(mod_fast(year,4) =0)
	monthlenlist[2] := 29;
endif

var monthlen := monthlenlist[month]*DayLen;

//find the month
while(abstime  >= monthlen)
	abstime := abstime-monthlen;
	month := month+1;
	monthlen := monthlenlist[month]*DayLen;
endwhile

//the day
while(abstime >= DayLen)
	abstime := abstime-DayLen;
	day := day+1;
endwhile

//find the hour
while(abstime >= 3600)
	abstime := abstime-3600;
	hour := hour+1;
endwhile

//minute
while(abstime >= 60)
	abstime := abstime-60;
	min := min+1;
endwhile

var macro :=0;

If ((hour>=6) and (hour<=13))
macro := (13 - hour);
if (macro==0)
macro :=666;
endif
endif

If (macro==666)
  return "Macroing is allowed for another " + (60 - min) + " mins";
endif

if (macro==0)
  return "Macroing is not allowed at this time, you will be jailed when caught";
else
  return "Macroing is allowed for another " + macro + " hrs " + (60 - min) + " mins";
endif

endfunction

function GetDate()

var abstime := polcore().systime;

var year := 1970;
var month := 1;
var day :=1;
var hour :=0;
var min :=0;

while(abstime >= YearLen)
	if(mod_fast(year,4) =0) //LeadYear
		abstime := abstime-LeadYearLen;
	else
		abstime := abstime-YearLen;
	endif
	year := year+1;
endwhile

var monthlenlist := {   31,	   28,	 31,	   30,    31,     30,     31,    31,      30,
			    31,        30,       31, };
var monthname:={"Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep", "Oct","Nov","Dec",};

if(mod_fast(year,4) =0)
	monthlenlist[2] := 29;
endif

var monthlen := monthlenlist[month]*DayLen;

//find the month
while(abstime  >= monthlen)
	abstime := abstime-monthlen;
	month := month+1;
	monthlen := monthlenlist[month]*DayLen;
endwhile

//the day
while(abstime >= DayLen)
	abstime := abstime-DayLen;
	day := day+1;
endwhile

return (monthname[month] + " " + day + ", " + year );

endfunction

function mod_fast(n,d)
var ndbl := cdbl(n)/cdbl(d);
var nint := cdbl(n/d);
if(ndbl > nint)
	var rest := (ndbl-nint)*cdbl(d);
	if(rest > cdbl(cint(rest)) )
		rest := cint(rest)+1;
	else
		rest := cint(rest);
	endif
	return rest; 
else
	return 0;
endif

endfunction
