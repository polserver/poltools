/* $Id: properties.inc 892 2005-11-04 06:51:00Z muaddiblsd $
 *
 * Purpose
 * Provide property names and functions. Properties starting with "+" are to be protected.
 *
 */
use uo;

include ":logger:log";

const SECURE_MAGIC          := 0xBADC0DE;

enum MOB_PROPS
    PROP_STAFF_LEVEL        := "+StaffLevel",        // Text command access level
    PROP_CREATION_TIME      := "CreationTime",       // Character creation time (UNIX timestamp)
    MOB_CPROPS_END          := 0
endenum

enum ITEM_PROPS
    ITEM_CPROPS_END         := 0
endenum

enum WORLD_PROPS
    PROP_PID                := "#PID",              // Array with all resistent scripts
    PROP_TIMESTAMP          := "TimeStamp",         // Struct describing the time
    WORLD_PROPS_END         := 0
endenum


// These properties only exist for a short time
enum TEMP_PROPS
    TEMPPROP_JUST_CREATED   := "#JustCreated",      // Character was just created
    TEMPPROP_SCHED_PID      := "#SchedPID",         // PID of script scheduler
    TEMP_PROPS_END          := 0
endenum


/*
 * The following functions replace the default ObjProperty functions. Use these instead to avoid
 * changing secured properies. If your script is safe, pass SECURE_MAGIC as magic.
 *
 */
function SetProperty(what, property, value, magic := 0)
    if(property[1] == "+")
        if(magic == SECURE_MAGIC)
            Log(LOG_WARNING, "Overwrote secure property '" + property + "' with '" + CStr(value) + "' on '" +
                GetLogInfo(what) + "'");
        else
            Log(LOG_ERROR, "Failed to overwrite secure property '" + property + "' with '" + CStr(value) + "' on '" +
                GetLogInfo(what) + "'");
            return(error);
        endif
    endif
    return(SetObjProperty(what, property, value));
endfunction

function GetProperty(what, property, magic := 0)
    if(property[1] == "+" && magic != SECURE_MAGIC)
        Log(LOG_ERROR, "Refused to GetProperty on secure property '" + property + "' on '" + GetLogInfo(what) + "'");
        return(error);
    endif
    return(GetObjProperty(what, property));
endfunction

function EraseProperty(what, property, magic := 0)
    if(property[1] == "+" && magic != SECURE_MAGIC)
        Log(LOG_ERROR, "Refused to erase secure property '" + property + "' on '" + GetLogInfo(what) + "'");
        return(error);
    endif
    return(EraseObjProperty(what, property));
endfunction

function GetPropertyNames(what, magic := 0)
    var props, i;

    if(magic == SECURE_MAGIC)
        return(GetObjPropertyNames(what));
    endif

    props := GetObjPropertyNames(what);
    for(i := 1; i <= props.size(); i := i + 1)
        if(props[i][1] == "+")
            props.erase(i);
            i := i - 1;
        endif
    endfor
    return(props);
endfunction
