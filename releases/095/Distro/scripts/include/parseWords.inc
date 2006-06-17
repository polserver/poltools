function parsewords( parsestr )
    return SplitWords( parsestr );

        /*  
                // Samizdat, only pulls out more than two words if they exist.
          
            local first, second;
            local space;
            space := Find( parsestr, " ", 1 );
            if (space)
                first := parsestr[1, space-1];
                local start := space+1;
                local remainder := len(parsestr);
                second := parsestr[start, remainder]; // compiler bug here !
            else
                first := parsestr;
                second := "";
            endif

            local result array;
            result[1] := first;
            result[2] := second;
    
            return result;
        */
endfunction