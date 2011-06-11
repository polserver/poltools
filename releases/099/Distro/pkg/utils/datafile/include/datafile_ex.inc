use uo;
use os;
use datafile;

include ":datafile:datafile";

function ConfigToDataFile(cfg_file, data_file)

	if ( Lower(TypeOf(cfg_file)) == "string" )
		cfg_file := ReadConfigFile(cfg_file);
	endif
 
	if ( Lower(TypeOf(data_file)) == "string" )
		data_file := DFOpenDataFile(data_file, DF_CREATE);
	endif

	if ( cfg_file.errortext )
		return cfg_file;
	elseif ( data_file.errortext )
		return data_file;
	else
		DFPurgeFile(data_file);
	endif

	foreach cfg_elem_name in ( GetConfigStringKeys(cfg_file) )
		var df_elem_ref := DFFindElement(data_file, cfg_elem_name, DF_CREATE);
		var cfg_elem_ref := FindConfigElem(cfg_file, cfg_elem_name);

		foreach prop_name in ( ListConfigElemProps(cfg_elem_ref) )
			var prop_vals := GetConfigStringArray(cfg_elem_ref, prop_name);

			if ( prop_vals.Size() > 1 )
				// Store all values as an array
				df_elem_ref.SetProp(prop_name, prop_vals);
			else
				// Single value, store as a string
				df_elem_ref.SetProp(prop_name, prop_vals[1]);
			endif

			SleepMS(2);
		endforeach

		SleepMS(2);
	endforeach

	return 1;
endfunction

function DataFileToConfig(data_file, cfg_path)
	if ( Lower(TypeOf(data_file)) == "string" )
		data_file := DFOpenDataFile(data_file, DF_CREATE);
	endif
	if ( Lower(TypeOf(cfg_path)) != "string" )
		return error{"errortext":="Argument 'cfg_path' must be a string."};
	endif
	
	if ( data_file.errortext )
		return data_file;
	endif

	foreach df_elem_name in ( DFGetElemNames(data_file) )
		var df_elem_ref := DFFindElement(data_file, df_elem_name);
		var elem_info := array{};
		var prop_line := struct{"name", "value"};
		
		foreach prop_name in ( DFGetElemProps(df_elem_ref) )
			prop_line.name := prop_name;
			var value := DFGetProp(df_elem_ref, prop_name);
			
			// Basic types can be stored just fine
			// More advanced (arrays, structs, dicts, etc.) must be packed.
			case ( Lower(TypeOf(value)) )
				"double":
				"integer":
				"string":
					break;
				default:
					value := Pack(value);
			endcase				
			
			prop_line.value := value;
			
			SleepMS(2);
		endforeach
		
		AppendConfigFileElem(cfg_path, "Elem", df_elem_name, elem_info);
		SleepMS(2);
	endforeach
	
	return 1;
endfunction
