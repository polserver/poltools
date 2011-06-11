/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2009-10-31
*/
use uo;
use os;
use cfgfile;
use polsys;

function GetItemDescCfgFile( package:="" )

	var file_name := ":"+package+":itemdesc";
	var item_cfg := ReadConfigFile( file_name );

	if( item_cfg.errortext )
		return error{"errortext":="Unable to open ["+file_name+"] ->"+item_cfg.errortext};
	endif

	return item_cfg;
endfunction

function GetItemDescInfo( obj_type )

	var item_cfg := GetItemDescCfgFile( "*" );
	if( !item_cfg )
		return item_cfg; // Contains an error;
	endif

	return item_cfg[obj_type];
endfunction

function GetObjTypeGraphic( obj_type )

	obj_type := CInt( obj_type );

	var cfg_info := GetItemDescInfo( obj_type );
	if(( !cfg_info ) && ( obj_type >= 0x4000 ))
		return cfg_info; // Contains an error
	endif

	var graphic := CInt( cfg_info.Graphic );
	if( graphic )
		return graphic;
	else
		return obj_type;
	endif
endfunction

function GetObjTypeDesc( source, plural:=0 )

	var obj_type := 0;
	if( source.IsA( POLCLASS_ITEM ))
		obj_type := source.objtype;
	elseif( TypeOf( source ) == "Integer" )
		obj_type := CInt( source );
	endif

	if( obj_type )
		var cfg_info := GetItemDescInfo( obj_type );
		if( cfg_info.errortext )
			return error{"errortext":="Object type not defined in any itemdesc.cfg"};
		elseif( cfg_info.Desc )
			source := cfg_info.Desc;
		elseif( cfg_info.Name )
			source := cfg_info.Name;
		else
			return error{"errortext":="Object type does not have a Name or Desc entry"};
		endif
	endif

	var name := source;

	if( !name["%"] ) // Plain desc line - easy enough to deal with.
		if( plural == 0 )
			return name;
		elseif( name[Len( name )] in array{"s"} )
			return name;
		elseif( name[Len( name )] in array{"h","j","x","z"} )
			return name+"es";
		else
			return name+"s";
		endif
	endif

	/* This section will deal with:
	 	Bread Stick%s
		Bread Loa%ves/f%
		Lump%s% of Coal
		Loa%ves/f% of Bread
	*/
	var first := Find( name, "%", 1 );
	var slash := Find( name, "/", first );
	var second := Find( name, "%", first + 1 );

	if( !second )
		// Force-append a % if a '/' exists!
		name += "%";
		second := Len( name );
	endif
 
	if( !slash )
		slash := second;
	endif

	// wwwww%xx[/yy]%[zzzzz]
	// 12345678     9 01234
	// 12345678 901 2 34567

	var s_plur := name[first+1, slash-( first+1 )];
	var s_sing := "";
	if( slash != first )
		s_sing := name[slash+1, second-( slash+1 )];
	endif

	var s_first := name[1, first-1];
	var s_middle := s_plur;
	var s_last := "";
	if( Len( name ) > second )
		s_last := name[second+1, Len( name )-second];
	endif

	if( plural == 0 )
		s_middle := s_sing;
	endif

	if( !s_first )
		s_first := "";
	endif
	if( !s_middle )
		s_middle := "";
	endif
	if( !s_last )
		s_last := "";
	endif

	return s_first + s_middle + s_last;
endfunction
