<?
//////////////////////////////////////////////////
// Include file by Austin Heilman
// Austin@tsse.net
// http://www.tsse.net
//
// This is an include file for the PHP language.
// It is used to easily parse POL config files.
// I made it so that it works as closely to POL's
// cfgfile.em module as I could.
//
// Version History
//
// 0.1
// 1/09/2003 12:45AM
// Added     GetConfigElem(array, string)
//
// 0.2
// 1/10/2003 10:09AM
// Added     GetConfigStringKeys()
// Added     ReadConfigFile()
// Changed   GetConfigElem() renamed to FindConfigElem() to match cfgfile.em
//
// 0.3
// 1/10/2003 11:17AM
// Added     GetConfigString()
// Added     GetConfigStringArray()
//
// 0.4
// 4/29/2004 1:36AM
// Added     GetConfigInt()        - MuadDib
// Added     GetConfigReal()       - MuadDib
// Added     GetConfigIntKeys()    - MuadDib
// Added     GetConfigMaxIntKey()  - MuadDib
//////////////////////////////////////////////////
// require_once("polcfg_inc.php");
//////////////////////////////////////////////////

// Opens a config file.
// Returns -1 if it is unable to find the file.
// Returns an array of the file if it can.
function ReadConfigFile($path)
{
	$cfg_file = @file("$path");
	if ( !cfg_file )
		return -1;

	return $cfg_file;
}

// Pass the config file's array.
// Returns an array of strings.
//
// The simplest way to output the results is:
// $info = GetConfigStringKeys($cfg_file);
// foreach ( $info as $value )
// {
// 	print "$value <br>\n";
// }
function GetConfigStringKeys(&$cfg_file)
{
	$cfg_info = array();
	foreach ( $cfg_file as $cfg_line )
	{
		$cfg_line = rtrim($cfg_line);
		//Only check lines that are not blank.
		if ( $cfg_line )
		{
			if ( preg_match("/^(\/\/|#)/", $cfg_line) )
			{
				//Comment lines
				continue;
			}
			elseif ( preg_match("/^[[:alnum:]]+\s+{$elem_name}/i", $cfg_line) )
			{
				//An elem key was found.
				//Remove the first word from the line and tuck it into an array.
				$cfg_line = preg_replace("/^[[:alnum:]]+\s+/", "", $cfg_line);
				array_push($cfg_info, $cfg_line);
			}
		}
	}
	return $cfg_info;
}

// Pass the config file's array.
// Returns an array of the elems that are integers
//
// The simplest way to output the results is:
// $info = GetConfigIntKeys($cfg_file);
// foreach ( $info as $value )
// {
// 	print "$value <br>\n";
// }
function GetConfigIntKeys(&$cfg_file)
{
	$cfg_info = array();
	foreach ( $cfg_file as $cfg_line )
	{
		$cfg_line = rtrim($cfg_line);
		//Only check lines that are not blank.
		if ( $cfg_line )
		{
			if ( preg_match("/^(\/\/|#)/", $cfg_line) )
			{
				//Comment lines
				continue;
			}
			elseif ( preg_match("/^[[:alnum:]]+\s+{$elem_name}/i", $cfg_line) )
			{
				//An elem key was found.
				//Remove the first word from the line and check if it is an Integer
				//type element. If so, tuck it into an array.
				$cfg_line = preg_replace("/^[[:alnum:]]+\s+/", "", $cfg_line);
                if((is_numeric($cfg_line) ? intval($cfg_line) == $cfg_line : false)) {
                	array_push($cfg_info, $cfg_line);
                }
			}
		}
	}
	return $cfg_info;
}

// Pass the config file's array.
// Returns the elem that is the highest integer
//
// The simplest way to output the result is:
// $info = GetConfigMaxIntKey($cfg_file);
// foreach ( $info as $value )
// {
// 	print "$value <br>\n";
// }
function GetConfigMaxIntKey(&$cfg_file)
{
	$cfg_info = array();
	foreach ( $cfg_file as $cfg_line )
	{
		$cfg_line = rtrim($cfg_line);
		//Only check lines that are not blank.
		if ( $cfg_line )
		{
			if ( preg_match("/^(\/\/|#)/", $cfg_line) )
			{
				//Comment lines
				continue;
			}
			elseif ( preg_match("/^[[:alnum:]]+\s+{$elem_name}/i", $cfg_line) )
			{
				//An elem key was found.
				//Remove the first word from the line and check if it is an Integer
				//type element. If so, tuck it into an array.
				$cfg_line = preg_replace("/^[[:alnum:]]+\s+/", "", $cfg_line);
                if((is_numeric($cfg_line) ? intval($cfg_line) == $cfg_line : false)) {
                	array_push($cfg_info, $cfg_line);
                }
			}
		}
	}
    //Sort the array of integers, small to largest then reset the array to save it
    sort($cfg_info, SORT_NUMERIC);
    reset($cfg_info);

    $retval = $cfg_info[((count($cfg_info))-1)];
	return $retval;
}


// Pass the file's array, and the elem you want to
// get the information for.
//
// Returns an associative array of arrays.
// The main array's keys are the config line keys.
// The array values inside the keys are the values.
//
// The simplest way to output the results is:
// $info = GetConfigElem($cfg_file, "MyInfo");
// foreach ( $info as $key => $value_array )
// {
// 	foreach ( $value_array as $value )
//	{
//		print "$value <br>\n";
//	}
// }
// However, it is best further parsed by
// GetConfigString() and GetConfigStringArray()
function FindConfigElem(&$cfg_file, $elem_name)
{
	$cfg_info = array();
	foreach ( $cfg_file as $cfg_line )
	{
		$cfg_line = rtrim($cfg_line);
		//Only check lines that are not blank.
		if ( $cfg_line )
		{
			if ( preg_match("/^(\/\/|#)/", $cfg_line) )
			{
				//Comment lines
				continue;
			}
			elseif ( preg_match("/^[[:alnum:]]+\s+{$elem_name}/i", $cfg_line) )
			{
				//It is inside the elem that it has been told to read.
				$inside = 1;
			}
			elseif ( $inside )
			{
				if ( preg_match("/^{/i", $cfg_line) )
				{
					//Ignore the { line
					continue;
				}
				elseif ( preg_match("/^}/i", $cfg_line) )
				{
					//It reached the } line, which means it is
					//done reading the elem. Stop going through the rest
					//of the file at this point.
					$inside = 0;
					break;
				}
				else
				{
					//It is still inside the elem's brackets.
					//Split the lines up into key value pairs.
					//Tuck the values into the array[key]

					$info = preg_split("/\s+/", $cfg_line, 2, PREG_SPLIT_NO_EMPTY);
					$key = $info[0];
					$value = $info[1];

					if ( !is_array($cfg_info[$key]) )
					{
						//If cfg_info[key] is not already an array
						//make it one so we know we can fit more values
						//into it.
						$cfg_info[$key] = array();
					}
					array_push($cfg_info[$key], $value);
				}
			}
		}
	}
	return $cfg_info;
}

// Pass the array received by FindConfigElem()
// Returns a string.
//
// The simplest way to output the results is:
// $info = GetConfigStringKey($cfg_file);
// print "$info <br>\n";
function GetConfigString(&$cfg_elem, $key)
{
	return $cfg_elem[$key][0];
}

// Returns an integer.
//
// The simplest way to output the results is:
// $ints = GetConfigInt(&$cfg_elem, $key);
// print "$ints <br>\n";
function GetConfigInt(&$cfg_elem, $key)
{
    $i = intval($cfg_elem[$key][0]);
    return $i;
}

// Returns an real integer.
//
// The simplest way to output the results is:
// $ints = GetConfigReal(&$cfg_elem, $key);
// print "$ints <br>\n";
function GetConfigReal(&$cfg_elem, $key)
{
    $i = (float)$cfg_elem[$key][0];
    return $i;
}

// Pass the array received by FindConfigElem()
// and a string of the key you want to get.
// Returns an array of strings.
//
// The simplest way to output the results is:
// $info = GetConfigStringArray($cfg_file);
// foreach ( $info as $value )
// {
// 	print "$value <br>\n";
// }
function GetConfigStringArray(&$cfg_elem, $key)
{
	return $cfg_elem[$key];
}

?>