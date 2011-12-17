/*
        Modified by *Edwards

        2008-04-07

        Last Modifications 2009-02-10
             * Fixed the code it's working!

        Thanks to ZuluHotel for this idea
*/
use uo;
use cfgfile;

include "include/string";
include ":areas:areas";
include ":areas:areasConstants";

function A_IsIn( object, option, obj_prop := 0 )

        if( object.multi )
                //We do not want restrictions for someone in a multi ( boat/house )
                return 0;
        endif

        var get_main_prop := GetGlobalProperty( "Areas_"+option );
        if( obj_prop )
                get_main_prop := GetObjProperty( object, "Areas_"+option );
	endif

        if( !get_main_prop )
                return 0;
        endif

        var areas := ListAreasByType();
        foreach area_name in areas

                var area := GetAreaSettings( area_name ),
                    MinX := CInt( area.Min_X ),
                    MaxX := CInt( area.Max_X ),
                    MinY := CInt( area.Min_Y ),
                    MaxY := CInt( area.Max_Y ),
                    Realm := CStr( area.Realm );

                if( CInt( get_main_prop[area_name] ))
                        if( A_IsInBox( object, MinX, MaxX, MinY, MaxY, Realm ))
				return 1;
                        endif
                endif

                SleepMS(5);
        endforeach

        return 0;
endfunction

function A_IsInArea( object, location, only := 0 )

        var areas := ListAreasByType();

        var loc_only := 0,
            other_loc := 0;

        foreach area_name in areas

                var area := GetAreaSettings( area_name ),
                    MinX := CInt( area.Min_X ),
                    MaxX := CInt( area.Max_X ),
                    MinY := CInt( area.Min_Y ),
                    MaxY := CInt( area.Max_Y ),
                    Realm := CStr( area.Realm );

                if( A_IsInBox( object, MinX, MaxX, MinY, MaxY, Realm ))
                        if( Lower( area_name ) == Lower( location ))
                                loc_only := 1;
                        else
                                other_loc += 1;
                        endif
                endif

                SleepMS(5);
        endforeach

        if( loc_only )
                if( only && other_loc )
                        return 0;
                endif
                return 1;
        endif

        return 0;
endfunction

function A_IsWhere( object )

        var areas := ListAreasByType(),
            result := "Britannia-Fantasia";

        foreach area_name in areas
                if( area_name == "Britannia-Fantasia" )
                        //We do not want to know we are always in Britannia territory...
                        continue;
                endif

                var area := GetAreaSettings( area_name ),
                    MinX := CInt( area.Min_X ),
                    MaxX := CInt( area.Max_X ),
                    MinY := CInt( area.Min_Y ),
                    MaxY := CInt( area.Max_Y ),
                    Realm := CStr( area.Realm );

                if( A_IsInBox( object, MinX, MaxX, MinY, MaxY, Realm ))
                        result := area_name;
                endif

                SleepMS(5);
        endforeach

        if( result["Britannia-"] )
                result := RemoveFromString( result, "Britannia-" );
        endif

        return result;
endfunction

