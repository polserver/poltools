<?php
/*  $Id$
 *
 *  Include file by Austin Heilman
 *  AustinHeilman@gmail
 *
 *  This is an include file for the PHP language.
 *  It is used to easily parse POL config files.
 *  I made it so that it works as closely to POL's cfgfile.em module as I could.
 *
 *  Version History
 *
 *  0.1
 *  1/09/2003 12:45AM - Austin
 *  Added     GetConfigElem(array, string)
 *
 *  0.2
 *  1/10/2003 10:09AM - Austin
 *  Added     GetConfigStringKeys()
 *  Added     ReadConfigFile()
 *  Changed   GetConfigElem() renamed to FindConfigElem() to match cfgfile.em
 *
 *  0.3
 *  1/10/2003 11:17AM - Austin
 *  Added     GetConfigString()
 *  Added     GetConfigStringArray()
 *
 *  0.4
 *  4/29/2004 1:36AM - MuadDib
 *  Added     GetConfigInt()
 *  Added     GetConfigReal()
 *  Added     GetConfigIntKeys()
 *  Added     GetConfigMaxIntKey()
 *
 * -------------------------
 *  require_once("polcfg.php");
 * -------------------------
 */

/* -------------------------
 *  CONSTANTS
 * -------------------------
 */
// For class labels in elem names.
Define("CLASS_LABELS_OFF", 0x0);
Define("CLASS_LABELS_ON", 0x1);


/*
 * ReadConfigFile($path)
 *
 * Purpose
 * Reads in a file.
 *
 * Parameters
 * $path:	Path to the config file.
 *
 * Return Values
 * Returns an array of strings on success.
 * Returns FALSE on failure.
 *
 */
function ReadConfigFile($path)
{
	$cfg_file = @File("$path");
	if ( !$cfg_file )
		return FALSE;

	return $cfg_file;
}

/*
 * GetConfigStringKeys($cfg_file)
 *
 * Purpose
 * Retrieves a list of the elem names in the config file.
 *
 * Parameters
 * $cfg_file:	A config file array read in with ReadConfigFile()
 * $class:	0 - Removes the class type from the elem name.
 * 		1 - Keeps the class type on the elem name.
 *		Example: Weapon 0xAB45 - 'Weapon' is the elem's class.
 *
 * Notes:
 * The simplest way to output the results is:
 * $info = GetConfigStringKeys($cfg_file);
 * foreach ( $info as $value )
 * {
 *	print "$value\n";
 * }
 *
 * Return Value
 * Returns an array of strings.
 *
 */
function GetConfigStringKeys(&$cfg_file, $class=CLASS_LABELS_OFF)
{
	$cfg_info = array();
	for ( $i=0; $i < Count($cfg_file); $i++ )
	{
		$cfg_line = RTrim($cfg_file[$i]);
		if ( !$cfg_line )
			// Blank line
			continue;
		elseif ( Preg_Match("/^(\/\/|#)/", $cfg_line) )
			//Comment line
			continue;
		elseif ( Preg_Match("/^([[:alnum:]]+\s+)([[:alnum:]]+)$/i", $cfg_line, $matches) )
		{
			$next_line = RTrim($cfg_file[$i+1]);
			if ( !Preg_Match("/\s*\{\s*/i", $next_line) )
			{
				// Not an elem line - maybe a property line with no spaces infront of it.
				continue;
			}
			// An elem key was found.
			// Remove the first word from the line and tuck it into an array.
			if ( !$class )
				$cfg_line = $matches[2];
			
			Array_Push($cfg_info, LTrim($cfg_line));
		}
	}
	/*
	foreach ( $cfg_file as $cfg_line )
	{
		$cfg_line = RTrim($cfg_line);
		if ( !$cfg_line )
			// Blank line
			continue;
		elseif ( Preg_Match("/^(\/\/|#)/", $cfg_line) )
			//Comment line
			continue;
		elseif ( Preg_Match("/^([[:alnum:]]+\s+)([[:alnum:]]+)$/i", $cfg_line, $matches) )
		{
			// An elem key was found.
			// Remove the first word from the line and tuck it into an array.
			if ( !$class )
				$cfg_line = $matches[2];

			Array_Push($cfg_info, LTrim($cfg_line));
		}
	}
	*/
	return $cfg_info;
}

/*
 * GetConfigIntKeys($cfg_file)
 *
 * Purpose
 * Returns a list of elem names that are integers.
 *
 * Paramters
 * $cfg_file:	A config file array read in with ReadConfigFile()
  * $class:	0 - Removes the class type from the elem name.
 * 		1 - Keeps the class type on the elem name.
 *		Example: Weapon 0xAB45 - 'Weapon' is the elem's class.
 *
 * Notes:
 * The simplest way to output the results is:
 * $info = GetConfigIntKeys($cfg_file);
 * foreach ( $info as $value )
 * {
 * 	print "$value\n";
 * }
 *
 * Return Values
 * Returns an array of integers.
 *
 */
function GetConfigIntKeys(&$cfg_file, $class=CLASS_LABELS_OFF)
{
	$cfg_info = array();
	foreach ( $cfg_file as $cfg_line )
	{
		$cfg_line = RTrim($cfg_line);
		if ( !$cfg_line )
			// Blank line
			continue;
		elseif ( Preg_Match("/^(\/\/|#)/", $cfg_line) )
			//Comment line
			continue;
		elseif ( Preg_Match("/^([[:alnum:]]+\s+)([[:digit:]]+)$/i", $cfg_line, $matches) )
		{
			// An elem key was found.
			// Remove the first word from the line and tuck it into an array.
			if ( !$class )
				$cfg_line = $matches[2];

			Array_Push($cfg_info, LTrim($cfg_line));
		}
	}
	return $cfg_info;
}

/*
 * GetConfigMaxIntKey($cfg_file)
 *
 * Purpose
 * Retrieves the elem with the highest integer value
 *
 * Parameters
 * $cfg_file:	A config file array read in with ReadConfigFile()
 *
 * Return Value:
 * Returns an integer
 *
 */
function GetConfigMaxIntKey(&$cfg_file)
{
	$high_num = 0;
	foreach ( $cfg_file as $cfg_line )
	{
		$cfg_line = RTrim($cfg_line);
		//Only check lines that are not blank.
		if ( !$cfg_line )
			// Blank line
			continue;
		elseif ( Preg_Match("/^(\/\/|#)/", $cfg_line) )
			//Comment lines
			continue;
		elseif ( Preg_Match("/^[[:alnum:]]+\s+{$elem_name}/i", $cfg_line) )
		{
			//An elem key was found.
			//Remove the first word from the line and check if it is an Integer
			//type element. If so, tuck it into an array.
			$cfg_line = Preg_Replace("/^[[:alnum:]]+\s+/", "", $cfg_line);
               		if ( (Is_Numeric($cfg_line) ? IntVal($cfg_line) == $cfg_line : false) )
               		{
                		if ( $cfg_line > $high_num )
                			$high_num = IntVal($cfg_line);
             		}
		}
	}

	return $high_num;
}

/*
 * FindConfigElem($cfg_file, $elem_name)
 *
 * Purpose
 * Retrieves a config elem from the config file.
 *
 * Parameters
 * $cfg_file:	A config file array read in with ReadConfigFile()
 * $elem_name:	Name of the elem to retrieve.
 *
 * Notes:
 * $info = GetConfigElem($cfg_file, "MyInfo");
 * foreach ( $info as $key => $value_array )
 * {
 * 	foreach ( $value_array as $value )
 *	{
 *		print "$value\n";
 *	}
 * }
 * However, it is best further parsed by
 * GetConfigString() and GetConfigStringArray()
 *
 * Return Value
 * Returns an array of strings on success.
 * Returns false if the elem was not found.
 *
 */
function FindConfigElem(&$cfg_file, $elem_name)
{
	$cfg_info = array();
	$inside = 0;
	for ( $i=0; $i < Count($cfg_file); $i++ )
	{
		$cfg_line = $cfg_file[$i];
		$cfg_line = RTrim($cfg_line);
		$cfg_line = LTrim($cfg_line);
		if ( !$cfg_line )
			// Blank line
			continue;
		elseif ( Preg_Match("/^(\/\/|#)/", $cfg_line) )
			//Comment line
			continue;
		elseif ( Preg_Match("/^(([[:alnum:]]+)\s+({$elem_name})|$elem_name)$/i", $cfg_line, $matches) )
		{
			$next_line = RTrim($cfg_file[$i+1]);
			if ( !Preg_Match("/\s*\{\s*/i", $next_line) )
			{
				// Not an elem line - maybe a property line with no spaces infront of it.
				continue;
			}
			//Print("Inside (0){$matches[0]} (1){$matches[1]} (2){$matches[2]} (3){$matches[3]}\n");
			//It is inside the elem that it has been told to read.
			$inside = 1;
		}
		elseif ( $inside )
		{
			if ( Preg_Match("/^{/i", $cfg_line) )
				//Ignore the { line
				continue;
			elseif ( Preg_Match("/^}/i", $cfg_line) )
			{
				//It reached the } line, which means it is done reading the elem.
				//Stop going through the rest of the file at this point.
				$inside = 0;
				break;
			}
			else
			{
				//It is still inside the elem's brackets.
				//Split the lines up into key value pairs.
				//Tuck the values into the array[key]

				$info = Preg_Split("/\s+/", $cfg_line, 2, PREG_SPLIT_NO_EMPTY);
				$key = $info[0];
				$value = $info[1];

				if ( !Is_Array($cfg_info[$key]) )
				{
					// If cfg_info[key] is not already an array,
					// make it one so it can hold multiple values.
					$cfg_info[$key] = array();
				}
				Array_Push($cfg_info[$key], $value);
			}
		}
	}

	if ( Count($cfg_info) < 1 )
		return FALSE;
	else
		return $cfg_info;
}

/*
 * GetConfigString($cfg_elem, $key)
 *
 * Purpose
 * Retrieves a property from an elem as a string.
 *
 * Parameters
 * $cfg_elem:	A config file elem read in with FindConfigElem()
 * $key:	Name of the property to retrieve.
 *
 * Return Value
 * Returns a string on success.
 * Returns FALSE on failure.
 *
 */
function GetConfigString(&$cfg_elem, $key)
{
	if ( IsSet($cfg_elem[$key][0]) )
		return StrVal($cfg_elem[$key][0]);
	else
		return FALSE;
}

/*
 * GetConfigInt($cfg_elem, $key)
 *
 * Purpose
 * Retrieves a property from an elem as an integer.
 *
 * Parameters
 * $cfg_elem:	A config file elem read in with FindConfigElem()
 * $key:	Name of the property to retrieve.
 *
 * Return Value
 * Returns an integer on success.
 * Returns FALSE on failure.
 *
 */
function GetConfigInt(&$cfg_elem, $key)
{
	if ( IsSet($cfg_elem[$key][0]) )
		return IntVal($cfg_elem[$key][0]);
	else
		return FALSE;
}

/*
 * GetConfigInt($cfg_elem, $key)
 *
 * Purpose
 * Retrieves a property from an elem as a float.
 *
 * Parameters
 * $cfg_elem:	A config file elem read in with FindConfigElem()
 * $key:	Name of the property to retrieve.
 *
 * Return Value
 * Returns an float on success.
 * Returns FALSE on failure.
 *
 */
function GetConfigReal(&$cfg_elem, $key)
{
	if ( IsSet($cfg_elem[$key][0]) )
		return FloatVal($cfg_elem[$key][0]);
	else
		return FALSE;
}

/*
 * GetConfigStringArray($cfg_elem, $key)
 *
 * Purpose
 * Retrieves multiple property values from a config elem.
 *
 * Parameters
 * $cfg_elem:	A config file elem read in with FindConfigElem()
 * $key:	Name of the property to retrieve.
 *
 * Return Value
 * Returns an array of strings on success.
 * Returns FALSE on failure.
 *
 */
function GetConfigStringArray(&$cfg_elem, $key)
{
	if ( IsSet($cfg_elem[$key]) )
		return $cfg_elem[$key];
	else
		return FALSE;
}

/*
 * GetElemPropertyNames($cfg_elem)
 *
 * Purpose
 * Retrieves all of the property names in a config elem.
 *
 * Parameters
 * $cfg_elem:	A config file elem read in with FindConfigElem()
 *
 * Return Value
 * Returns an array of strings
 *
 */
function GetElemPropertyNames(&$cfg_elem)
{
	$prop_names = array();
	foreach ( $info as $key => $value_array )
	{
		Array_Push($prop_names, $key);
	}
}
?>