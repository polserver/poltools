/*
        Created by *Edwards

        2009-11-25
*/


function IsMinable( landtile, targ )

	if(( landtile >= 0xe2  && landtile <= 0xe7 ) || ( landtile >= 0xf3 && landtile <= 0xf7 ) ||
	   ( landtile >= 0x104 && landtile <= 0x107 ) || ( landtile >= 0x110 && landtile <= 0x113 ) ||
	   ( landtile >= 0x122 && landtile <= 0x125 ) || ( landtile >= 0x1d3 && landtile <= 0x1da ) ||
	   ( landtile >= 0x21f && landtile <= 0x230 ) || ( landtile >= 0x232 && landtile <= 0x23b ) ||
	   ( landtile >= 0x6cd && landtile <= 0x6d1 ) || ( landtile >= 0x6da && landtile <= 0x6dd ) ||
	   ( landtile >= 0x6eb && landtile <= 0x6fe ) || ( landtile >= 0x70d && landtile <= 0x714 ) ||
	   ( landtile >= 0x71d && landtile <= 0x720 ) || ( landtile >= 0x72b && landtile <= 0x732 ) ||
	   ( landtile >= 0x73b && landtile <= 0x73e ) || ( landtile >= 0x749 && landtile <= 0x750 ) ||
	   ( landtile >= 0x759 && landtile <= 0x75c ) || ( landtile >= 0xc4  && landtile <= 0xc7 ))
		return 1;
	elseif( targ >= 0x053b && targ <= 0x0553 && targ != 0x0550 )
		return 1;
	endif

	return 0;
endfunction

function IsSand( landtile )

	if(( landtile >= 0x0016  && landtile <= 0x004b ) || ( landtile >= 0x011e  && landtile <= 0x012d ) ||
	   ( landtile >= 0x0192  && landtile <= 0x01ab ) || ( landtile >= 0x01b9  && landtile <= 0x01d1 ) ||
	   ( landtile >= 0x0282  && landtile <= 0x0291 ) || ( landtile >= 0x03b7  && landtile <= 0x03ca ) ||
	   ( landtile >= 0x064b  && landtile <= 0x0672 ))
		return 1;
	endif

	return 0;
endfunction

function IsSwamp( landtile )

	if(( landtile >= 0x240 && landtile <= 0x250 ) || ( landtile >= 0x3d65 && landtile <= 0x3ef0 ))
		return 1;
	endif

	return 0;
endfunction

function IsTree( landtile )

	if( landtile == 0x0c9e || landtile == 0x0ca8 || landtile == 0x0caa || landtile == 0x0cab )
		return 1;
	elseif( landtile >= 0x0c99 && landtile <= 0x0cea )
		return 1;
	elseif( landtile >= 0x0cf3 && landtile <= 0x0d03 )
		return 1;
	elseif( landtile >= 0x0d41 && landtile <= 0x0dab )
		return 1;
	elseif( landtile >= 0x0d37 && landtile <= 0x0d38 )
		return 1;
	endif

	return 0;
endfunction
