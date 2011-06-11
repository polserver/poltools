/*
        Created by *Edwards
        
        2011-03-23
*/
use uo;
use os;

function GetLockPickLevel( item )

        var lock_level := GetObjProperty( item, "LockLevel" );
        if( !lock_level )
                lock_level := 0;
        endif
        
        return CInt( lock_level );
endfunction

function GetLockPickDifficulty( item )

        var lock_level := GetLockPickLevel( item );

        if( item.IsA( POLCLASS_DOOR ))
                lock_level := 6;
        endif

        return CInt( lock_level * 25 ); //Assume that value is not <= 0
endfunction
