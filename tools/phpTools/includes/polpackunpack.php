<?
//////////////////////////////////////////////////
// Include file by MuadDib
// MuadDib@lostsoulsshard.org
// http://www.lostsoulsshard.org
//
// This is an include file for the PHP language.
// It is used to easily pack and unpack pol strings
// and arrays for work with php and aux svc packages.
// Packing packs to POL standard, and unpack breaks
// it back down to a way PHP can read it.
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
//////////////////////////////////////////////////
// require_once("includes/polpackunpack.php");
//////////////////////////////////////////////////


// Unpacks a standard string.
// IE: sHello World! is Hello World!
function unpackpolstr($var)
{
  return substr_replace("$var", "", 0, 1);
}

// Unpacks a single array string from POL to PHP.
// IE: "s12:Hello World!" becomes "Hello World!"
function unpackpolarraystr($var)
{
  $pos = strpos($var, ":");
  return substr_replace("$var", "", 0, $pos);
}

// Unpacks a POL Integer for PHP.
// IE: "i1" becomes an Integer of 1
function unpackpolint($var)
{
  $i = preg_replace("/[^0-9.]/", "", substr_replace("$var", "", 0, 1));
  $ix = preg_replace("/\.[0-9]*$/", "", $i);
  $iy = (int) $ix;
  return $iy;
}

// Unpacks a string in pol format that uses
// a delimiter(keyword). The delimiter is used to seperate
// entries and create an array of the elements.
// IE: "entry1;entry2;entry3", with ; as the keyword
// (spaces work too of course), becomes an array of
// {entry1, entry2, entry3}. Or in PHP terms,
// { 1 -> entry1, 2 -> entry2, 3 -> entry3 }
function unpackpolstrkeyword($var, $keyword)
{
  $strfix = explode($keyword, substr_replace("$var", "", 0, 1));
  return ($strfix);
}

// If you do not want to use the unpackpolarraystr and
// unpackpolint seperately all the time or if the information
// could be int or str, can use this function to auto convert
// convert it for you as needed.
function unpackpolvar($var)
{
  if($var{0} == "i")
  {
    return unpackpolint($var);
  }
  else
  {
    return unpackpolarraystr($var);
  }
}

// Unacks a POL array into a PHP format.
// IE: a2:S6:entry1S6:entry2 becomes
// {1 -> entry1, 2 -> entry2 } or also
// known as { entry1, entry2 } (how seen in POL).
// ONLY WORKS on a single state array. This
// Does NOT handle array/dicts/structs inside of
// of an array.
function unpackpolarray($polsarray)
{
  $polarray = substr_replace($polsarray, "", 0, 1);
  $pos = strpos($polarray, ":");

/* Commented out, left in for records of future functions */
/*  $arraycnt = "";
  for($i = 0; $i < $pos; $i++) {
    $arraycnt .= $polarray{$i};
  }
  $j = preg_replace("/[^0-9.]/", "", $arraycnt);
  $j = preg_replace("/\.[0-9]*$/", "", $j);
  $j = (int) $j;
*/

  $polarray = substr_replace("$polarray","", 0, ($pos+1));
  $phparray = array();
  $chkit = 0;
  while ($chkit == 0) {
    if($polarray{0} === "i") {
      /* Remove the "i" from it. */
      $polarray = substr_replace($polarray, "", 0, 1);
      $intstr = "";
      $intstrcnt = 0;
      for($i = 0; $i < strlen($polarray); $i++) {
        if(($polarray{$i} === "i") or ($polarray{$i} === "S")) {
          break;
        }
        $intstr .= $polarray{$i};
        $intstrcnt++;
      }
      $j = preg_replace("/[^0-9.]/", "", $intstrcnt);
      $j = preg_replace("/\.[0-9]*$/", "", $j);
      $j = (int) $j;
      $tmppol = preg_replace("/[^0-9.]/", "", $intstr);
      $tmppol = preg_replace("/\.[0-9]*$/", "", $tmppol);
      $tmppol = (int) $tmppol;
      $phparray[] = $tmppol;
      $polarray = substr_replace("$polarray", "", 0, $j);
      if(!strlen($polarray)) {
        $chkit = 1;
      }
    } else {
      $polarray = substr_replace($polarray, "", 0, 1);
      $pos = strpos($polarray, ":");
      $strcnt = "";
      for($i = 0; $i < $pos; $i++) {
        $strcnt .= $polarray{$i};
      }
      $j = preg_replace("/[^0-9.]/", "", $strcnt);
      $j = preg_replace("/\.[0-9]*$/", "", $j);
      $j = (int) $j;
      $polarray = substr_replace("$polarray", "", 0, ($pos+1));
      $tmppol = substr("$polarray",0, $j);
      $phparray[] = $tmppol;
      $polarray = substr_replace("$polarray", "", 0, $j);
      if(!strlen($polarray)) {
        $chkit = 1;
      }
    }
  }

  return $phparray;
}

// Packs a string to POL format for strings inside
// an array.
// IE: "a1:S3:123"  <-- creates the S3:123 part from "123"
function packpolarraystr($str)
{
  return "S".strlen($str).":$str";
}

// Packs an Integer passed to the function into
// POL Packed format.
// IE: 1 becomes i1
function packpolint($int)
{
  return "i$int";
}

// If you do not want to use the packpolarraystr and
// packpolint seperately all the time or if the information
// could be int or str, can use this function to auto convert
// convert it for you as needed.
function packpolvar($var)
{
  if(is_int($var))
  {
    return packpolint($var);
  }
  else
  {
    return packpolarraystr($var);
  }
}

// Packs a PHP array into a POL format.
// IE: {1 -> entry1, 2 -> entry2 } or also
// known as { entry1, entry2 }, would
// become a2:S6:entry1S6:entry2
function packpolarray($array)
{
  $polarray = "a".count($array).":";
  while ($elem = each($array))
  {
    $polarray .= packpolvar($elem[1]);
  }

  return $polarray;
}


?>









