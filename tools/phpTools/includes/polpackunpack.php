<?
//////////////////////////////////////////////////
// Title: POL Pack/Unpack Include file for PHP
// Author: MuadDib (Scott E. Royalty)
// contact: muaddib@polserver.com
// Website: http://www.polserver.com
//
// This is an include file for the PHP language.
// It is used to easily pack and unpack pol "Pack()" created data
// for work with php and aux svc packages/config files. Packing 
// packs to POL standard, and unpack breaks it back down to a way 
// PHP can read it. Do NOT Attempt to unpack Multi-Dimensional
// Arrays, Structs, and Dictionaries. This is NOT supported.
//
// Notes:
// 11/26/2008
//			Complete Rewrite began. Why? This was written when
//			I began PHP. It's BLOATED AND SLOW. RegEx is
//			a waste of Processor. Especially for this. Let's move
//			into this century shall we? This rewrite includes renaming
//			and removal of functions. This was done to simplify the code,
//			make it a bit more readable, and because I felt like it.
//			If you used a version prior to .6, DO NOT USE THIS UNTIL
//			YOU UPGRADE YOUR PHP SCRIPTS FOR THIS NEW ONE! :)
//			I debated making this all Object and Class based. But...... nah. :)
//
// Function Useage:
// -------------------
// unPackPolVar(POL_PACKED_STRING)
//	POL_PACKED_STRING - Packed POL string recieved from the Aux Svc or Cfg File.
// 	Definition: Call this Function in order to unpack any data recieved from POL or read in 
//	from a Cfg file that stored the information as a Packed string (Such as Cprops in the 
//	cfg, DataFiles etc.) Does not handle Array/Dict/Struct inside of Array/Dict/Struct
//
// packPolVar(PHP_VARIABLE)
//	PHP_VARIABLE - Variable in PHP Format (double, integer, string, indexed array, etc).
//	Definition: Call this Function in order to Pack a PHP Variable into the correct format
//	either for writing back to a POL CFG File, POL DataFile, or sending via the Aux Svc
//	connection to POL.
//
// explodePOL(POL_PACKED_STRING, KEYWORD)
//	POLPACKED_STRING - A single String recieved from POL or POL Cfg File/DataFile.
//	KEYWORD - Keyword to be used as the entry delimiter. Identical with PHP'd explode()
//	                   except there is no default delimiter for this function. :P
//	Definition: Call this function to remove the initial "s" from the POL String, and then
//	explode the string based on the Keyword into an Indexed Array in PHP. For more detailed
//	information about explode please refer to the base PHP Documentation for explode().
//
// To-Do:
// 1.) Add Dictionary Handling (Single State)
// 2.) Add Struct Packing
//
// Version History
//
// 0.1
// 3/14/2004
// Added     unpackpolstr(pol packed string)
// Added     unpackpolarraystr(pol string packed inside array)
// Added     unpackpolstrkeyword(pol packed string, delimiter)
// Added     packpolarraystr(php string)
// Added     packpolint(php integer)
// Added     packpolvar(php integer or string)
// Added     packpolarray(php array)
//
// 0.2
// 3/15/2004
// Added     unpackpolarray($polarray)
// Added     unpackpolint(pol packed integer)
//
// 0.3
// 3/16/2004
// Updated   Several functions updated to be more compatible
//           for removal of characters.
//
// 0.4
// 3/24/2004
// Updated   Optimized some of the code.
//
// 0.5
// 3/26/2004
// Updated   Optimized unpackpolarray and also added
//           ability for it to distinguish between a given
//           string and integer element and handle them
//           accordingly
//
// 0.6a
// 11/26/2008
// Removed -	unPackPolStr()
//			unPackPolInt()
//			unPackPolVar()
//			unPackPOLArrayStr()
//			unPackPolStrKeyword()
// Added -		unPackPolVar() version 2.0.
//			Switch Handling for S<length>:<String>, s<String>, i<integer> unpacking.
// Updated -	unPackPolArray()
//			Upgraded existing code for single state handling. Much more efficient.
//			Did I mention I hate RegEx?
//
// 0.6b
// 11/27/2008
// Updated -	unPackPolArray()
//			Can now handle Doubles to native PHP (double).
// Updated -	unPackPolVar()
//			Can now handle Doubles to native PHP (double) and send arrays on to unPackPolArray().
// Rename - 		unPackPolStrKeyword renamed to explodePOLVar().
// 			Explodes a POL Packed String using a passed Keyword. Similar to PHP 'Explode'.
//
// 0.6c
// 11/28/2008
// Removed -	packpolint()
//			packpolarraystr()
//			packpolstr()
// Updated -	packPolVar()
//			Using a currently horrendous if/elseif check of the passed PHP Object to determine
//			the Data Type and handle accordingly. Currently handles Int, Double, Float, Real, String
// Updated - 	packPolArray()
//			Rewrite for new methodology. Also handles Multi-Dimensional Arrays.
// Note -		NOT SUPPORTING MULTI-DIMENSIONAL ARRAY/STRUCT/DICT!
//
//////////////////////////////////////////////////
// Useage:
// require_once("includes/polpackunpack.php");
//////////////////////////////////////////////////

/****************************************************
* Unpacking Code Below					      *
****************************************************/

// unPackPolVar(String)
// Unpacks a literal string into useable PHP Format based on the Packed POL Data.
// String: Packed version of the POL Variable/Object.
function unPackPolVar($var) {
	global $PolArray, $PolStruct;

	$check = $var{0};
	switch($check) {
		case "S":	$var = substr_replace("$var", "", 0, 1); // Remove the S first off.
					$pos = strpos($var, ":") + 1; // make it 1-based to keep with standards used in eScript for POL.
					if ($pos < 2) {
						return array( "Error" => "'S<length>:String' passed with no Length given" );
					}
					return substr_replace("$var", "", 0, ($pos-1));
		case "s":	return substr_replace("$var", "", 0, 1);
		case "i":	return (int)substr_replace("$var", "", 0, 1);
		case "r":	return (double)substr_replace("$var", "", 0, 1);
		case "a":	return unPackPolArray($var);
		case "t":	return unPackPolStruct($var);
		case "d":	return array( "Error" => "Dictionary Not Yet Implemented" );
		default: 	return array( "Error" => "Unknown Format Passed To unPackPolVar()" );
	}
	return array( "Error" => "unPackPolVar() Switch Case Failed" );
}

// explodePOL(String, Keyword)
// Unpacks a string from pol format that uses
// a delimiter(keyword). The delimiter is used to seperate
// entries and create an array of the elements.
// IE: "entry1;entry2;entry3", with ; as the keyword
// (spaces work too of course), becomes an array of
// {entry1, entry2, entry3}. Or in PHP terms,
// { 1 -> entry1, 2 -> entry2, 3 -> entry3 }
// String - POL Packed String
// Keyword - Delimeter to use with PHP Explode
function explodePol($var, $keyword)
{
  return explode($keyword, substr_replace("$var", "", 0, 1));
}

// unPackPolArray(String)
// Unacks a POL array into a PHP format.
// IE: a2:S6:entry1S6:entry2 becomes
// {1 -> entry1, 2 -> entry2 } or also
// known as { entry1, entry2 } (how seen in POL).
// String - Packed POL Array
function unPackPolArray($PolArray)
{
	$PolArray = substr_replace($PolArray, "", 0, 1); // Remove the initial "a" that depicts an array.
	$Pos = strpos($PolArray, ":"); // Helps us get how many times we need to step through the array! Great for Multi-Dimensional Arrays!
	$ArrayLength = (int)substr_replace("$PolArray","", $Pos, strlen($PolArray)); // Get the integers before the :
	$PolArray = substr_replace("$PolArray","", 0, $Pos+1); // Remove the :
	$PHPArray = array();
	for($i = 1; $i < $ArrayLength+1; $i++) {
		//First, let's isolate the current element of the POL Array, and remove it from $POLArray.
		$ElemType = substr_replace("$PolArray","", 1, strlen($PolArray)); // We just need the first character right now.
		$Element;
		$ElementError = 0;
		$ElementLength;
		switch($ElemType) {
			case "S":	// String Element
						$PolArray = substr_replace("$PolArray", "", 0, 1); // Remove the S first off.
						$SPos = strpos($PolArray, ":");
						if ($SPos < 1) {
							return array( "Error" => "'S<length>:String' passed with no Length given" );
						}
						$ElementLength = (int) substr_replace("$PolArray", "", $SPos, strlen($PolArray));
						$PolArray = substr_replace("$PolArray", "", 0, $SPos+1); // Remove the # and :
						$Element = substr_replace("$PolArray", "", $ElementLength, strlen($PolArray));
						// Remove Element from $PolArray
						$PolArray = substr_replace("$PolArray", "", 0, $ElementLength);
						break;
			case "i":	$PolArray = substr_replace("$PolArray", "", 0, 1); // Remove the i first off.
						// Now we must manually find the end of this Integer. PHP Sucks, so we cannot simply check each step to see if it is
						// an Int class object. PHP Will treat an Int as a String Character if it was initialized as a String, which this is.
						$finished = 0;
						for($j = 1; $j < strlen($PolArray)+1; $j++) {
							switch($PolArray{$j}) { // We use a switch to make it a quick and easy Check. Blah.
								case "S": // All types (cept plain "s" strings, since not possible) are checked, unlike old version.
								case "i":
								case "a":
								case "r":
								case "t":
								case "d": 	// We reached the first letter of the next element! YAY!
											$Element = (int) substr_replace("$PolArray", "", $j, strlen($PolArray));
											// Remove Element from $PolArray
											$PolArray = substr_replace("$PolArray", "", 0, $j);
											$finished = 1;
											break; // Break out of the case. We are done!
								default:	// See if we are at the end of the String. If so, we are done and nothing more to look for.
											if($j === strlen($PolArray)){
												$Element = (int) substr_replace("$PolArray", "", $j, strlen($PolStruct));
												$PolArray = substr_replace("$PolArray", "", 0, $j);
												break; // Break out of the case. We are done!
											}
											continue;
							}
							if($finished) {
								break;
							}
						}
						break;
			case "r":	$PolArray = substr_replace("$PolArray", "", 0, 1); // Remove the r first off.
						// Now we must manually find the end of this Integer. PHP Sucks, so we cannot simply check each step to see if it is
						// an Int class object. PHP Will treat an Int as a String Character if it was initialized as a String, which this is.
						$finished = 0;
						for($j = 1; $j < strlen($PolArray)+1; $j++) {
							switch($PolArray{$j}) { // We use a switch to make it a quick and easy Check. Blah.
								case "S": // All types (cept plain "s" strings, since not possible) are checked, unlike old version.
								case "i":
								case "a":
								case "r":
								case "t":
								case "d": 	// We reached the first letter of the next element! YAY!
											$Element = (double) substr_replace("$PolArray", "", $j, strlen($PolArray));
											// Remove Element from $PolArray
											$PolArray = substr_replace("$PolArray", "", 0, $j);
											$finished = 1;
											break; // Break out of the case. We are done!
								default:	// See if we are at the end of the String. If so, we are done and nothing more to look for.
											if($j === strlen($PolArray)){
												$Element = (double) substr_replace("$PolArray", "", $j, strlen($PolStruct));
												$PolArray = substr_replace("$PolArray", "", 0, $j);
												break; // Break out of the case. We are done!
											}
											continue;
							}
							if($finished) {
								break;
							}
						}
						break;
						break;
			case "a":	$Element = array( "Error" => "Arrays With Arrays Not Supported" ); $ElementError = 1;
						break;
			case "t":	$Element = array( "Error" => "Arrays With Structs Not Supported" ); $ElementError = 1;
						break;
			case "d":	$Element = array( "Error" => "Arrays With Dictionaries Not Supported" ); $ElementError = 1;
						break;
			default: 	$Element = array( "Error" => "Unknown Format Passed To unPackPolArray()" ); $ElementError = 1;
						break;
		}
		if( $Element && !$ElementError) {
			$PHPArray[] = $Element;
		} else {
			return array( "Error" => "Unhandled Element Within POL Packed Array. Error: " . $Element["Error"] . "" );
		}
	}
	return $PHPArray;
}

// unPackPolStruct(String)
// Unacks a POL Struct into a PHP format.
// IE: t2:S8:KeyName1S5:ValueS8:KeyName2S5:Value
// becomes {KeyName1 -> Value, KeyName2 -> Value } or also
// known as Keyed Arrays (Associative).
// String - Packed POL Struct
function unPackPolStruct($PolStruct)
{
	$PolStruct = substr_replace($PolStruct, "", 0, 1); // Remove the initial "t" that depicts an array.
	$Pos = strpos($PolStruct, ":"); // Helps us get how many times we need to step through the struct!
	$StructLength = (int)substr_replace("$PolStruct","", $Pos, strlen($PolStruct)); // Get the integers before the :
	$PolStruct = substr_replace("$PolStruct","", 0, $Pos+1); // Remove the :
	$PHPArray = array();
	for($i = 1; $i < $StructLength+1; $i++) {
		//First, let's isolate the current element of the POL Struct, and remove it from $PolStruct.
		// Remember, there are TWO parts to each entry. The Key Name, and then the Value for the Key
		$PolStruct = substr_replace("$PolStruct", "", 0, 1); // Remove the S first off from the Key Name entry
		$KPos = strpos($PolStruct, ":");
		if ($KPos < 1) {
			return array( "Error" => "'S<length>:KeyName' passed with no Length given" );
		}
		$KeyLength = (int) substr_replace("$PolStruct", "", $KPos, strlen($PolStruct));
		$PolStruct = substr_replace("$PolStruct", "", 0, $KPos+1); // Remove the # and :
		$KeyName = substr_replace("$PolStruct", "", $KeyLength, strlen($PolStruct));
		// Remove Element from $PolStruct
		$PolStruct = substr_replace("$PolStruct", "", 0, $KeyLength);
		// KeyName is now finished being handled. We can now check the next element to get the Value for the KeyName entry!
		$ElemType = substr_replace("$PolStruct","", 1, strlen($PolStruct)); // We just need the first character right now.
		$Element;
		$ElementError = 0;
		$ElementLength;
		switch($ElemType) {
			case "S":	// String Element
						$PolStruct = substr_replace("$PolStruct", "", 0, 1); // Remove the S first off.
						$SPos = strpos($PolStruct, ":");
						if ($SPos < 1) {
							return array( "Error" => "'S<length>:String' passed with no Length given" );
						}
						$ElementLength = (int) substr_replace("$PolStruct", "", $SPos, strlen($PolStruct));
						$PolStruct = substr_replace("$PolStruct", "", 0, $SPos+1); // Remove the # and :
						$Element = substr_replace("$PolStruct", "", $ElementLength, strlen($PolStruct));
						// Remove Element from $PolStruct
						$PolStruct = substr_replace("$PolStruct", "", 0, $ElementLength);
						break;
			case "i":	$PolStruct = substr_replace("$PolStruct", "", 0, 1); // Remove the i first off.
						// Now we must manually find the end of this Integer. PHP Sucks, so we cannot simply check each step to see if it is
						// an Int class object. PHP Will treat an Int as a String Character if it was initialized as a String, which this is.
						$finished = 0;
						for($j = 1; $j < strlen($PolStruct)+1; $j++) {
							switch($PolStruct{$j}) { // We use a switch to make it a quick and easy Check. Blah.
								case "S": // All types (cept plain "s" strings, since not possible) are checked, unlike old version.
								case "i":
								case "a":
								case "r":
								case "t":
								case "d": 	// We reached the first letter of the next element! YAY!
											$Element = (int) substr_replace("$PolStruct", "", $j, strlen($PolStruct));
											// Remove Element from $PolStruct
											$PolStruct = substr_replace("$PolStruct", "", 0, $j);
											$finished = 1;
											break; // Break out of the case. We are done!
								default:	// See if we are at the end of the String. If so, we are done and nothing more to look for.
											if($j === strlen($PolStruct)){
												$Element = (int) substr_replace("$PolStruct", "", $j, strlen($PolStruct));
												$PolStruct = substr_replace("$PolStruct", "", 0, $j);
												break; // Break out of the case. We are done!
											}
											continue;
							}
							if($finished) {
								break;
							}
						}
						break;
			case "r":	$PolStruct = substr_replace("$PolStruct", "", 0, 1); // Remove the r first off.
						// Now we must manually find the end of this Integer. PHP Sucks, so we cannot simply check each step to see if it is
						// an Int class object. PHP Will treat an Int as a String Character if it was initialized as a String, which this is.
						$finished = 0;
						for($j = 1; $j < strlen($PolStruct)+1; $j++) {
							switch($PolStruct{$j}) { // We use a switch to make it a quick and easy Check. Blah.
								case "S": // All types (cept plain "s" strings, since not possible) are checked, unlike old version.
								case "i":
								case "a":
								case "r":
								case "t":
								case "d": 	// We reached the first letter of the next element! YAY!
											$Element = (double) substr_replace("$PolStruct", "", $j, strlen($PolStruct));
											// Remove Element from $PolArray
											$PolStruct = substr_replace("$PolStruct", "", 0, $j);
											$finished = 1;
											break; // Break out of the case. We are done!
								default:	// See if we are at the end of the String. If so, we are done and nothing more to look for.
											if($j === strlen($PolStruct)){
												$Element = (double) substr_replace("$PolStruct", "", $j, strlen($PolStruct));
												$PolStruct = substr_replace("$PolStruct", "", 0, $j);
												break; // Break out of the case. We are done!
											}
											continue;
							}
							if($finished) {
								break;
							}
						}
						break;
			case "a":	$Element = array( "Error" => "Structs With Arrays Not Supported" ); $ElementError = 1;
						break;
			case "t":	$Element = array( "Error" => "Structs With Structs Not Supported" ); $ElementError = 1;
						break;
			case "d":	$Element = array( "Error" => "Structs With Dictionaries Not Supported" ); $ElementError = 1;
						break;
			default: 	$Element = array( "Error" => "Unknown Format Passed To unPackPolStruct()" ); $ElementError = 1;
						break;
		}
		if( $Element && !$ElementError) {
			$PHPArray[$KeyName] = $Element;
		} else {
			return array( "Error" => "Unhandled Element Within POL Packed Struct. Error: " . $Element["Error"] . "");
		}
	}
	return $PHPArray;
}

/****************************************************
* Packing Code Below					      *
****************************************************/

// packPolVar(Object)
// This will check the type of Object being passed and create
// a POL Packed String to return accordingly.
// Object - PHP Object to be Packed.
// In_Array - Check to see if the Object is to be inside an Array.
function packPolVar($PHPObject, $In_Array=FALSE)
{
	if(is_null($PHPObject))
	{
		return array( "Error" => "PHPObject passed to packPolVar() was NULL" );
	}
	
	// We use the is_* Functions for this purpose because of the inaccuracy of the gettype() function.
	// Not only are the is_ functions more efficient from a processing point of view, but by using them at all
	// times to validate the data type of a variable, you get around the real possibility that a PHP Object
	// will have its type changed again somewhere else within the script. All much better OVERALL than a switch case.
	if(is_int($PHPObject))
	{
		return 'i'.(string)$PHPObject.'';
	}
	else if(is_float($PHPObject) || is_double($PHPObject) || is_real($PHPObject))
	{
		(double)$PHPObject; // We force Cast to eliminate any issue on POL's end for handling the Numbers. 
		return 'r'.(string)$PHPObject.'';
	}
	else if(is_string($PHPObject))
	{
		if($In_Array === FALSE) 
		{
			return 's'.$PHPObject.'';
		} else {
			return 'S' . (string)strlen($PHPObject) . ':' . $PHPObject . '';
		}
	}
	else if(is_array($PHPObject))
	{
		// Check if it's empty. If it is, BOOOOO. Don't send EMPTY arrays. useless and buggy. Send a String instead.
		if(count($PHPObject) < 1)
		{
			return array( "Error" => "Empty Array sent to packPolVar(). BAD DOG!" );
		}
		// First check for Keys existing in the array. If they do, this needs to become a POL Struct!
		// Never mix keyed and indexed arrays. Makes for messy code and results. We check the first result of
		// array_keys to see if it is a string. If it is, there was Keys, and thus we need to make a POL Struct. At this stage, it
		//  COULD be a Struct or Dictionary. But leaving it as a Struct only for the moment. This is mainly because I am lazy
		// and do NOT want to check all results of they array_keys to see if this is a MIXED array of int and string keys.
		$KeyCheck = array_keys($PHPObject);
		if(is_string($KeyCheck[0])) // This is an Keyed array! YAY A STRUCT!
		{
			return array ( "Error" => "Packing of STRUCT is not yet implemented" );
		}
		else if(is_int($KeyCheck[0])) // This is an Indexed array! YAY A PLAIN JANE ARRAY!
		{
			// Time to begin packing. Will even handle multi-dimensional by passing them BACK to packPolVar()  for handling each entry.
			return packPolArray($PHPObject);
//			return array ( "Error" => "Packing of Arrays is not yet implemented" );
		}
	}
}

// packPolArray($PHPArray)
// Packs a PHP array into a POL format.
// IE: {1 -> entry1, 2 -> entry2 } or also
// known as { entry1, entry2 }, would
// become a2:S6:entry1S6:entry2
// PHPArray - PHP Based Array Class Type.
function packPolArray($PHPArray)
{
	// count() does not recursively count into other arrays. Treats them as a single entry. YAY!
	$ArrayLimit = count($PHPArray);
	$POLString = "a" . $ArrayLimit . ":";
    for($i = 0; $i < $ArrayLimit; $i++)
	{
		$POLString = $POLString . packPolVar($PHPArray[$i], TRUE);
	}
	return $POLString;
}

?>