CONST FIELD_SHAPE_VERTICAL := 1;
CONST FIELD_SHAPE_HORIZONTAL := 2;

include ":timedscripts:timedScripts";

function CreateField(targ, shape, objtype, size, duration, cprops:="", mobile:="", check_los:=1, delay:=2)
	var start_cord, stop_cord, object;
	var field := array{};
	
	Print("Shape: "+shape+" objtype:"+objtype);
	
	case ( shape )
		FIELD_SHAPE_VERTICAL:
			if ( size % 2 )
				start_cord := targ.x - ((size - 1) / 2);
				stop_cord := targ.x + CInt(size / 2);
			else
				start_cord := targ.x - (size / 2);
				stop_cord := targ.x + (size / 2) + 1;
			endif
			
			// Create objects and SetDuration & SetOwner
			for i := start_cord to stop_cord
				object := CreateItemAtLocation(i, targ.y, targ.z, objtype, 1, targ.realm);
				if ( object )
					// If there is no SetDuration method, set it manually
					if ( object.SetDuration(duration) == error && mobile )
						Print("No duration, setting manually.");
						TS_StartTimer(object, "destroyObject", duration, 0, mobile);
					endif

					// If there is no SetOwner method, set it manually
					if ( object.SetOwner(mobile.serial) == error && mobile )
						SetObjProperty(field, "Owner", mobile.serial);
					endif

					field.Append(object);
				endif

				SleepMS(delay);
			endfor
			break;
		FIELD_SHAPE_HORIZONTAL:
			if ( size % 2 )
				start_cord := targ.y - ((size - 1) / 2);
				stop_cord := targ.y + CInt(size / 2);
			else
				start_cord := targ.y - (size / 2);
				stop_cord := targ.y + (size / 2) + 1;
			endif
			
			// Create objects and SetDuration() & SetOwner()
			for i := start_cord to stop_cord
				object := CreateItemAtLocation(targ.x, i, targ.z, objtype, 1, targ.realm);
				if ( object )
					// If there is no SetDuration method, set it manually
					if ( object.SetDuration(duration) == error && mobile )
						Print("No duration, setting manually.");
						TS_StartTimer(object, "destroyObject", duration, 0, mobile);
					endif

					// If there is no SetOwner method, set it manually
					if ( object.SetOwner(mobile.serial) == error && mobile )
						SetObjProperty(field, "Owner", mobile.serial);
					endif

					field.Append(object);
				endif

				SleepMS(delay);
			endfor
			break;
	endcase

	if ( field )
		// Make sure we have LOS to the field
		if ( check_los )
			// object type doesn't matter, it'll be deleted soon
			check_los := CreateItemAtLocation(targ.x, targ.y, targ.z, 0x002, 1, targ.realm);
			check_los.invisible := 1;

			foreach object in field
				if ( !CheckLineOfSight(check_los, object) )
					DestroyItem(object);
					continue;
				endif
			endforeach

			DestroyItem(check_los);
		endif
			
		// Set cprops, can be array of structs or just a struct
		foreach object in field
			if ( cprops )
				if ( Lower(TypeOf(cprops)) == "array" )
					foreach prop_struct in cprops
						foreach prop_name in ( cprops.Keys() )
							SetObjProperty(object, prop_name, cprops[prop_name]);
						endforeach
					endforeach
				elseif ( Lower(TypeOf(cprops)) == "struct" )
					foreach prop_name in ( cprops.Keys() )
						SetObjProperty(object, prop_name, cprops[prop_name]);
					endforeach
				endif
			endif
		endforeach
	endif

	return 1;
endfunction