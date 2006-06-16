
function array2string(thearray)
        var retval;

        foreach word in thearray
                retval := retval + " " + thearray;
        endforeach

        retval := retval[2,1000000];

        return retval;
endfunction


function string2array(thestring)

        var retval := array;
        retval := SplitWords(thestring);

endfunction


       