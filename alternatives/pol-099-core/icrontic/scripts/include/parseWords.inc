function parsewords( parsestr )
    return SplitWords( parsestr );

        /*  
                // Samizdat, only pulls out more than two words if they exist.
          
            var first, second;
            var space;
            space := Find( parsestr, " ", 1 );
            if (space)
                first := parsestr[1, space-1];
                var start := space+1;
                var remainder := len(parsestr);
                second := parsestr[start, remainder]; // compiler bug here !
            else
                first := parsestr;
                second := "";
            endif

            var result array;
            result[1] := first;
            result[2] := second;
    
            return result;
        */
endfunction