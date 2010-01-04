/////////////////////////////////////////////////////////////////
//
// GUMPS.INC - v2.1
//
// Gump Functions - to make life easier! :)
//
// Author: Myrathi <myrathi@templar.dhs.org>
//
//==Revision v2.1==
// Updated: 24-Apr-2000, Myrathi
//  - Added BKGND_???????? 'gumpid' constants for GFResizePic()
//  - Added some missing characters to Shini's align-functions
//
//==Revision v2.0==
// Updated: 19-Apr-2000, Myrathi
//  - Couple of minor bugfixes.
//  - Updated the HTML help files.
//  - Code more robust. Moved to full revision v2.0! :)
//
//==Revision v1.3==
// Updated: 04-Feb-2000, Myrathi
//	- Added a sleepms() in places to stop runaway scripts! Yay!
//	- Added GFExtractData(): extracts string from TextEntry key
//
//==Revision v1.2==
// Updated: 30-Jan-2000, Myrathi
//	- Added storage/retrieval functions to allow multiple gumps
//	  in different functions in the same script! (headache)
//	- GFSendGumpV() displays a gump from a "stored gump" var.:)
//	- Added Shinigami's text-positioning functions...
//	  ( XGFTextWidth/Left/Mid/Right() )
//
//==Revision v1.1==
// Updated: 27-Jan-2000, Myrathi
//	- Changed the GFref variable to a structure (nicer)
//	- GFInitGump now accepts optional 'x', 'y' origin params.
//	- GFSetRef now returns the old {x,y} origin
//	- GFSetRefRel added - new origin is *relative* to old one
//	- GFTextCrop added. Supports "croppedtext" control
//	- GF[Get|Set]NextIndex and GFIncIndex functions added
//	- GF[Get|Set]Data and GFGet/SetLayout added to stop mixups
//	  between zero-based and one-based indexing. (Use 0-idxs!!)
//	- Appropriate functions now return a structure containing
//	  indexes to the changes in the layout array as well as
//	  data-index and/or keyid values (see headers or help file)
//	- Added constants for the index functions
//	- Added two internal helper-functions (XGFxxxxxxxx)
//
//==Revision v1.0==
// Updated: 25-Jan-2000, Myrathi
//	- GFInitGump now auto-inserts "nodispose".
//	- GFSetRadioGroup added for advanced radio-button support
//  - Minor bugs skwished. No other bugs found.
//  - Updated to full revision v1.0 :)
//
//==Revision v0.1a==
// Created: 24-Jan-2000, Myrathi
//  - Basic gump creation. A couple of minor bugs :/
//
// Notes:
//	+ All functions take hex values and convert to integer
//	  form, where necessary (gumps crash with hex values! Ugh!)
//
//	+ "Page 0" is the main background! It shows at ALL times!
//	  Anything you want on separate pages should be on page
//	  1, 2, 3 and so on showing 0+1, 0+2, 0+3, 0+4, etc, etc.
//
// To do:
//	+ Add the option of supplying the KeyIDs (is this wanted?)
//	+ Update the const-list of commonly used ID's (GFX + colours)
//  + Update "GFTextMid()" to cater for mid-x coord only (no 'w')
//
// Bugs:
//	- None known... now :)
//
// Thanks:
//	+ To Ignatius for his GUMPDOCS file which cleared up
//	  a couple of issues I was unsure of. :)
//	+ To Shinigami for "croppedtext" and his align-functions :]
//	+ To Horus for shutting up and letting me work! :P"
//
/////////////////////////////////////////////////////////////////

use uo;
use os;
use basic;

// Constants (for GF[Get|Set]NextIndex)
/////////////////////////////////////////////////////////////////
const GFIDX_TEXT        := 0x01;	// Static Text (TextLine)
const GFIDX_TEXTENTRY   := 0x02;	// Return Text (TextEntry)
const GFIDX_BUTTON      := 0x03;	// Action Buttons (ButtonID)
const GFIDX_CHECKBOX    := 0x04;	// Check-boxes
const GFIDX_RADIO       := 0x05;	// Radio Buttons
/////////////////////////////////////////////////////////////////

// Constants for index starting values (SHOULD be enough! Heh!)
/////////////////////////////////////////////////////////////////
const GFINIT_TEXT       := 0x000;	// Static Text (GFdata[])
const GFINIT_TEXTENTRY  := 0x201;	// Return Text (GFdata[])
const GFINIT_BUTTON     := 0x401;	// Action Buttons (KeyIDs)
const GFINIT_CHECKBOX   := 0x601;	// Check-boxes (KeyIDs)
const GFINIT_RADIO      := 0x801;	// Radio Buttons (KeyIDs)
/////////////////////////////////////////////////////////////////

// 'gumpid' Constants for the GFResizePic() function
/////////////////////////////////////////////////////////////////
const BKGND_SILVERTRIM  := 0x0053;  // Fancy, dark-grey backing
const BKGND_SCROLL      := 0x09d8;  // Unrolling scroll style
const BKGND_GOLDTRIM    := 0x0a3c;  // Gold-press, black backing
const BKGND_PAPER       := 0x0bb8;  // Very thin edging
const BKGND_PAPER_EX    := 0x0dac;  //  \_with fancy inner border
const BKGND_BLACKSLAB   := 0x0e10;  // Raised edge (blocky)
const BKGND_STONESLAB   := 0x13be;  // Plain, small corners
const BKGND_STONE       := 0x13ec;  // Marble stone, rounded edge
/////////////////////////////////////////////////////////////////

// Function List
/////////////////////////////////////////////////////////////////
//
// GFSetRef( x := 0, y := 0 )
// GFSetRefRel( dx, dy );
// GFInitGump( sx := 0, sy := 0 )
// GFPage( page )
// GFNoClose()
// GFNoDispose()
// GFNoMove()
// GFUser1( line )
// GFUser2( l_line, d_pos, d_line )
// GFGumpPic( x, y, gumpid )
// GFResizePic( x, y, gumpid, w, h )
// GFTilePic( x, y, tileid )
// GFTextLine( x, y, colour, text );
// GFTextMid(x, y, w, colour, text );
// GFTextRight( rx, y, colour, text );
// GFTextCrop( x, y, w, h, colour, text );
// GFTextEntry( x, y, w, h, colour, text := "" )
// GFButtonPage( x, y, release, press, page )
// GFButtonID( x, y, release, press, close )
// GFCheckBox( x, y, off, on, state )
// GFRadioButton( x, y, off, on, state )
// GFSetRadioGroup( group )
//
// GFExtractData( keyid, keyarray )
//
// GFGetNextIndex( type )
// GFSetNextIndex( type, value )
// GFIncIndex( type )
//
// GFGetData( index )
// GFSetData( index, text )
// GFGetLayout( index )
// GFSetLayout( index, text )
//
// GFStoreGump()
// GFRestoreGump( gumpvar )
//
// GFSendGump( who )
// GFSendGumpV( who, gumpvar )
//
/////////////////////////////////////////////////////////////////

// Internal Functions
/////////////////////////////////////////////////////////////////
//
// XGFReset()
// XGFRetVal( lytidx := -1, dtidx := -1, keyid := -1 )
//
// XGFTextWidth(gumptext)
// XGFTextLeft(gumptext, x, width)
// XGFTextMid(gumptext, x, width)
// XGFTextRight(gumptext, rightx)
//
/////////////////////////////////////////////////////////////////

// Global variables
/////////////////////////////////////////////////////////////////
var gfref := { };		// X- and Y-offsets for gump operations
    gfref.+x := 0;
    gfref.+y := 0;

var gflayout := { };	// Main layout array for entire gump
var gfdata := { };	// Storage array for static-text and
				//  text-entry initialisation strings
				// Indexes/KeyIDs for...
var gfdtidx;		// ...text in GFdata (index)
var gftxtid;		// ...TeXT-entry returns (keyid)
var gfbutid;		// ...'action' BUTtons (keyid)
var gfcbxid;		// ...CheckBoXes (keyid)
var gfradid;		// ...RADio buttons (keyid)
/////////////////////////////////////////////////////////////////

// Return-value structure layout
/////////////////////////////////////////////////////////////////
//  rvsl.lytidx;	// ...index into layout array
//  rvsl.dtidx;	// ...index into data array
//  rvsl.keyid;	// ...KeyID allocated / other data (eg. page#)
/////////////////////////////////////////////////////////////////

// Global Initialisation (Just in case! :-P )
/////////////////////////////////////////////////////////////////
//
XGFReset();

// Function Definitions
/////////////////////////////////////////////////////////////////

// Sets the top-left co-ordinate origin (absolute)
//
// Returns: A structure {x,y} containing the old origin
//
function GFSetRef( x := 0, y := 0 )
	var oldref := gfref;
	gfref.x := CInt(x);
	gfref.y := CInt(y);
	return oldref;
endfunction

// Sets the top-left co-ordinate origin (relative to current)
//
// Returns: A structure {x,y} containing the old origin
//
function GFSetRefRel( dx, dy )
	var oldref := gfref;
	gfref.x := gfref.x + CInt(dx);
	gfref.y := gfref.y + CInt(dy);
	return oldref;
endfunction

// Initialises this include file's global variables
//
function GFInitGump( sx := 0, sy := 0 )
	XGFReset();
	GFSetRef( sx, sy );
	GFPage( 0 );
	GFNoDispose();
endfunction

// Gump page #x - start of definition
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.keyid  = Page number
//
function GFPage( page )
	page := CInt(page);
	gflayout.append( "page " + CStr(page) );
	return XGFRetVal( gflayout.size(), -1, page );
endfunction

// Won't let the gump be closed with a right-click
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//
function GFNoClose()
	gflayout.append( "noclose" );
	return XGFRetVal( gflayout.size() );
endfunction

// Won't let the gump be closed by clicking ESCAPE
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//
function GFNoDispose()
	gflayout.append( "nodispose" );
	return XGFRetVal( gflayout.size() );
endfunction

// Disallows dragging of the gump.
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//
function GFNoMove()
	gflayout.append( "nomove" );
	return XGFRetVal( gflayout.size() );
endfunction

// Adds a generic line to the GFlayout array
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//
function GFUser1( line )
	gflayout.append( line );
	return XGFRetVal( gflayout.size() );
endfunction

// Adds a generic line to the GFlayout array and
// a line to the GFdata array at position 'd_pos'
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.dtidx  = Data-array index changed
//
function GFUser2( l_line, d_pos, d_line )
	gflayour.append( line );
	gfdata[CInt(d_pos)] := d_line;
	return XGFRetVal( gflayout.size(), d_pos );
endfunction

// Single "art gump"
// (Uses "Gump" art from InsideUO)
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//
function GFGumpPic( x, y, gumpid )
	var line := "gumppic " + CStr(gfref.x + CInt(x)) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(gumpid));
	gflayout.append( line );
	return XGFRetVal( gflayout.size() );
endfunction

// Resized "gump" made up of art from 'gumpid' to 'gumpid+8'
// (Uses "Gump" art from InsideUO)
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//
function GFResizePic( x, y, gumpid, w, h )
	var line := "resizepic " + CStr(gfref.x + CInt(x)) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(gumpid)) + " " + CStr(CInt(w)) + " " + CStr(CInt(h));
	gflayout.append( line );
	return XGFRetVal( gflayout.size() );
endfunction

// Single "tile" graphic
// (Uses "Static Tiles" from InsideUO [under Artwork])
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//
function GFTilePic( x, y, tileid )
	var line := "tilepic " + CStr(gfref.x + CInt(x)) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(tileid));
	gflayout.append( line );
	return XGFRetVal( gflayout.size() );
endfunction

// Display a line of text on the gump
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.dtidx  = Data-array index changed (0-based index)
//
function GFTextLine( x, y, colour, text );
	var line := "text " + CStr(gfref.x + CInt(x)) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(colour)) + " " + CStr(gfdtidx);
	gflayout.append( line );
	gfdtidx := gfdtidx + 1;		// Update index into GFdata[]
	gfdata[gfdtidx] := text;	// arrays start at #1... id#0 goes into [1]!!
	return XGFRetVal( gflayout.size(), gfdtidx - 1 );
endfunction

// Display a centred line of text on the gump
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.dtidx  = Data-array index changed (0-based index)
//
function GFTextMid(x, y, w, colour, text )
	x := XGFTextMid( text, CInt(x), w );
	var line := "text " + CStr(gfref.x + x) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(colour)) + " " + CStr(gfdtidx);
	gflayout.append( line );
	gfdtidx := gfdtidx + 1;		// Update index into GFdata[]
	gfdata[gfdtidx] := text;	// arrays start at #1... id#0 goes into [1]!!
	return XGFRetVal( gflayout.size(), gfdtidx - 1 );
endfunction

// Display a right-aligned line of text on the gump
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.dtidx  = Data-array index changed (0-based index)
//
function GFTextRight( rx, y, colour, text )
	rx := XGFTextRight( text, CInt(rx) );
	var line := "text " + CStr(gfref.x + rx) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(colour)) + " " + CStr(gfdtidx);
	gflayout.append( line );
	gfdtidx := gfdtidx + 1;		// Update index into GFdata[]
	gfdata[gfdtidx] := text;	// arrays start at #1... id#0 goes into [1]!!
	return XGFRetVal( gflayout.size(), gfdtidx - 1 );
endfunction

// Display a line of text on the gump but restrict it to a bounding box
// (shows '...' if it exceeds the rightmost edge)
//
// Syntax : x   y   w   h  colour  dtidx
// Example: 125 300 150 50 32      0
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.dtidx  = Data-array index changed (0-based index)
//
function GFTextCrop( x, y, w, h, colour, text )
	var line := "croppedtext " + CStr(gfref.x + CInt(x)) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(w)) + " " + CStr(CInt(h)) + " " + CStr(CInt(colour));
	line := line + " " + CStr(gfdtidx);
	gflayout.append( line );
	gfdtidx := gfdtidx + 1;		// Update index into GFdata[]
	gfdata[gfdtidx] := text;	// arrays start at #1... id#0 goes into [1]!!
	return XGFRetVal( gflayout.size(), gfdtidx - 1 );
endfunction

// Display a text-entry control on the gump
//
// Syntax : x   y   w   h  colour  txtid  dtidx
// Example: 125 300 150 50 32      1050   0
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.dtidx  = Data-array index changed (0-based index)
//	.keyid  = KeyID allocated to the text when the Gump returns
//	          (This method is DAFT! But hey, I didn't write it! :-P" )
//              (Returns eg:  "Key=2024, Value=2024: This is the text" )
//
function GFTextEntry( x, y, w, h, colour, text := "" )
	var line := "textentry " + CStr(gfref.x + CInt(x)) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(w)) + " " + CStr(CInt(h)) + " " + CStr(CInt(colour));
	line := line + " " + CStr(gftxtid) + " " + CStr(gfdtidx);
	gflayout.append( line );
	gfdtidx := gfdtidx + 1;		// Update index into GFdata[]
	gfdata[gfdtidx] := text;	// arrays start at #1... id#0 goes into [1]!!
	gftxtid := gftxtid + 1;		// Update KeyID (txtid)
	return XGFRetVal( gflayout.size(), gfdtidx - 1, gftxtid - 1 );
endfunction

// Standard "button" syntax:
// "button x y release press close page keyid"

// Display a button that changes to another page
//
// Syntax: x, y, release, press, 0, page
// (close=0 - don't want it closing when it's changing page, eh?)
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.keyid  = Page number that the button changes to
//
function GFButtonPage( x, y, release, press, page )
	// "button x y release press 0 page"
	var line := "button " + CStr(gfref.x + CInt(x)) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(release)) + " " + CStr(CInt(press));
	line := line + " 0 " + CStr(CInt(page));
	gflayout.append( line );
	return XGFRetVal( gflayout.size(), -1, page );
endfunction

// Display an "action button" (returns a key value and can close gump)
//
// Syntax: x, y, release, press, close, 0, keyid
// (page=0 - it returns a value instead of changing page)
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.keyid  = KeyID allocated to the button - returned from Gump if pressed
//	          (butid)
//
function GFButtonID( x, y, release, press, close )
	// "button x y release press close 0 key"
	var line := "button " + CStr(gfref.x + CInt(x)) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(release)) + " " + CStr(CInt(press)) + " ";
	if ( close == 0 )
		line := line + "0 0 ";
	else
		line := line + "1 0 ";
	endif
	line := line + CStr(gfbutid);
	gfbutid := gfbutid + 1;		// Update KeyID (butid)
	gflayout.append( line );
	return XGFRetVal( gflayout.size(), -1, gfbutid - 1 );
endfunction

// Displays a check-box (off/on)
//
// Syntax: x, y, off, on, initial state (0/1), cbxid
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.keyid  = KeyID allocated to the checkbox (cbxid)
//
function GFCheckBox( x, y, off, on, state )
	var line := "checkbox " + CStr(gfref.x + CInt(x)) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(off)) + " " + CStr(CInt(on)) + " ";
	if ( state == 0 )
		line := line + "0 ";
	else
		line := line + "1 ";
	endif
	line := line + CStr(gfcbxid);
	gfcbxid := gfcbxid + 1;		// Update KeyID (cbxid)
	gflayout.append( line );
	return XGFRetVal( gflayout.size(), -1, gfcbxid - 1 );
endfunction

// Displays a radio button (off/on)
//
// Syntax: x, y, off, on, initial state (0/1), radid
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.keyid  = KeyID allocated to the radio-button (radid)
//
function GFRadioButton( x, y, off, on, state )
	var line := "radio " + CStr(gfref.x + CInt(x)) + " " + CStr(gfref.y + CInt(y));
	line := line + " " + CStr(CInt(off)) + " " + CStr(CInt(on)) + " ";
	if ( state == 0 )
		line := line + "0 ";
	else
		line := line + "1 ";
	endif
	line := line + CStr(gfradid);
	gfradid := gfradid + 1;		// Update KeyID (radid)
	gflayout.append( line );
	return XGFRetVal( gflayout.size(), -1, gfradid - 1 );
endfunction

// Groups radio button (I think)
//
// Returns: A structure...
//	.lytidx = Layout-array index changed
//	.keyid  = The group number (integer form)
//
function GFSetRadioGroup( group )
	group := CInt(group);
	gflayout.append( "group " + CStr(group) );
	return XGFRetVal( gflayout.size(), -1, group );
endfunction

// Extracts string from a TextEntry key
//
// Returns: The text string
//
function GFExtractData( keyid, keyarray )
	var keystr := CStr(keyid)+": ";
	var datstr := keyarray[CInt(keyid)];
	var retval := datstr[len(keystr)+1,len(datstr)];
	sleepms(2);
	return retval;
endfunction

// Returns: Value of the next index to be used
//
function GFGetNextIndex( type )
	sleepms(2);
	case ( type )
		GFIDX_TEXT:
			return gfdtidx;
		GFIDX_TEXTENTRY:
			return gftxtid;
		GFIDX_BUTTON:
			return gfbutid;
		GFIDX_CHECKBOX:
			return gfcbxid;
		GFIDX_RADIO:
			return gfradid;
		default:
			print( "GFGetNextIndex(): Bad type" );
	endcase
	return;
endfunction

// Sets the next index to be used
//
// Returns: The old index value
//
function GFSetNextIndex( type, value )
	var oldidx;
	value := CInt(value);
	sleepms(2);
	case ( type )
		GFIDX_TEXT:
			oldidx := gfdtidx;
			gfdtidx := value;
		GFIDX_TEXTENTRY:
			oldidx := gftxtid;
			gftxtid := value;
		GFIDX_BUTTON:
			oldidx := gfbutid;
			gfbutid := value;
		GFIDX_CHECKBOX:
			oldidx := gfcbxid;
			gfcbxid := value;
		GFIDX_RADIO:
			oldidx := gfradid;
			gfradid := value;
		default:
			print( "GFSetNextIndex(): Bad type" );
			return;
	endcase
	return oldidx;
endfunction

// Increments the current <type> index by one
//
// Returns: The new index value
//
function GFIncIndex( type )
	var oldidx := GFSetNextIndex( type, GFGetNextIndex( type ) + 1 );
	return (oldidx + 1); // Because it's just been incremented! :P"
endfunction

// Gets text from GFdata[]
//
// Returns: the text
//
function GFGetData( index )
	// Internally, the gump uses 0-based indexing...
	// Take that into account when altering the 1-based array
	return gfdata[index+1];
endfunction

// Set the text in GFdata[]
//
// **WARNING** NO ERROR CHECKING!
//
// Returns: Index changed
//
function GFSetData( index, text )
	// Internally, the gump uses 0-based indexing...
	// Take that into account when altering the 1-based array
	gfdata[index+1] := text;
	return index;
endfunction

// Get a text item from the layout array
//
// Returns: the text
//
function GFGetLayout( index )
	return gflayout[index];
endfunction

// Sets a specified text item in the layout array
//
// **WARNING** NO ERROR CHECKING!
//
// Returns: Index changed
//
function GFSetLayout( index, text )
	gflayout[index] := text;
	return index;
endfunction

// Stores a structure containing the gump and extra details
//
// Returns: A structure:
//	.layout =	A copy of GFlayout[]
//	.data   =	A copy of GFdata[]
//	.indexes
//		.dt	= Copy of GFdtidx
//		.txt	= Copy of GFtxtid
//		.but	= Copy of GFbutid
//		.cbx	= Copy of GFcbxid
//		.rad	= Copy of GFradid
//
function GFStoreGump()
	var idx := { };
	idx.+dt := gfdtidx;
	idx.+txt := gftxtid;
	idx.+but := gfbutid;
	idx.+cbx := gfcbxid;
	idx.+rad := gfradid;

	var ret := { };
	ret.+layout := gflayout;
	ret.+data := gfdata;
	ret.+indexes := idx;

	return ret;
endfunction

// Restores a gump layout from a structure saved by GFStoreGump()
//
// Returns:
//	0 = Failure
//	1 = Success
//
function GFRestoreGump( gumpvar )
	if ( !(gumpvar.layout) || !(gumpvar.data) || !(gumpvar.indexes) )
		print("GFRG(): missing one of .layout/.data/.indexes");
		return 0;
	endif
	var idx := gumpvar.indexes;
	if ( !(idx.dt) || !(idx.txt) || !(idx.but) || !(idx.cbx) || !(idx.rad) )
		print("GFRG(): missing index value");
		return 0;
	endif

	gflayout := gumpvar.layout;
	gfdata := gumpvar.data;
	gfdtidx := idx.dt;
	gftxtid := idx.txt;
	gfbutid := idx.but;
	gfcbxid := idx.cbx;
	gfradid := idx.rad;
	return 1;
endfunction

// Call this to display/use the defined gump
//
// Returns: an array of keys and values
//          (ie. what the SendDialogGump() returns)
//
// NOTE: This WILL crash the client if you've created duff arrays!!
//
function GFSendGump( who )
	return SendDialogGump( who, gflayout, gfdata );
endfunction

// Call this to display a gump using "gumpvar"
//
// Returns: an array of keys and values
//		(ie. what the SendDialogGump() returns)
//
// NOTE: This WILL crash the client if you've corrupted the array!
//
function GFSendGumpV( who, gumpvar )
	if ( !(gumpvar.layout) || !(gumpvar.data) )
		return;
	endif
	return SendDialogGump( who, gumpvar.layout, gumpvar.data );
endfunction

/////////////////////////////////////////////////////////////////
//
// INTERNAL FUNCTIONS
//
/////////////////////////////////////////////////////////////////

// Called (internally) to reset all the variables.
//
function XGFReset( )
	gflayout := { };
	gfdata := { };
	gfdtidx := GFINIT_TEXT;
	gftxtid := GFINIT_TEXTENTRY;
	gfbutid := GFINIT_BUTTON;
	gfcbxid := GFINIT_CHECKBOX;
	gfradid := GFINIT_RADIO;
endfunction

// Called (internally) to setup the return structure
// of a function
//
//  rvsl.lytidx;	// ...index into layout array
//  rvsl.dtidx;	// ...index into data array
//  rvsl.keyid;	// ...KeyID allocated / other data (eg. page#)
//
function XGFRetVal( lytidx := -1, dtidx := -1, keyid := -1 )
	var rvsl := { };
	if ( lytidx > -1 )
		rvsl.+lytidx := lytidx;
	endif
	if ( dtidx > -1 )
		rvsl.+dtidx := dtidx;
	endif
	if ( keyid > -1 )
		rvsl.+keyid := keyid;
	endif
	sleepms(2);	// Will hopefully stop runaway script problem :/

	return rvsl;
endfunction

/////////////////////////////////////////////////////////////////
//  Author:    Shinigami
//  eMail:     Shinigami@gmx.net
//  Modified:  Myrathi
/////////////////////////////////////////////////////////////////

// Constants
/////////////////////////////////////////////////////////////////
var agftlength:={ };
       agftlength.+charsf:=   // Space as Filler is not allowed
         "||||||||||||||"+ //  2
         "`',.;:::::::::"+ //  3
         "!!!!!!!!!!!!!!"+ //  4
         "il1[]]]]]]]]]]"+ //  5
         "bcejot”(){}==="+ //  6
         "adfpqrszI„~~~~"+ //  7
         "ghnuvy?>----"+ //  8
         "xJ578$?+++++++"+ //  9
         "kCE2369^/*****"+ // 10
         "mwABDFOPŽ™40%%"+ // 11
         "GLNQSZ@#######"+ // 12
         "HRT&__________"+ // 13
         "KšUYYYYYYYYYYY"+ // 14
         "XXXXXXXXXXXXXX"+ // 15
         "VVVVVVVVVVVVVV"+ // 16
         "MMMMMMMMMMMMMM"+ // 18
         "WWWWWWWWWWWWWW"; // 21
       agftlength.+width:={
          2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
          12, 13, 14, 15, 16, 18, 21
       };
/////////////////////////////////////////////////////////////////

// Functions
/////////////////////////////////////////////////////////////////

// Returns the Width of GumpText
//
function XGFTextWidth(gumptext)
  var points:=0;
  var j;

  for (j:=len(gumptext); j>0; j:=j-1)
    var i:=find(agftlength.charsf, gumptext[j], 1)-1;
    if (i>=0)
      points:=points+agftlength.width[CInt(i/14)+1];
    else
      print("XGFTextWidth: Unknown Character = '"+gumptext[j]+"'");
    endif
  endfor

  return (points);
endfunction

// Returns the X-Value (Left-Alignment)
//
function XGFTextLeft(gumptext, x, width)
  return (x);
endfunction

// Returns the X-Value (Mid-Alignment)
//
function XGFTextMid(gumptext, x, width)
  return (x+CInt((width-XGFTextWidth(gumptext))/2));
endfunction

// Returns the X-Value (Right-Alignment)
//
function XGFTextRight(gumptext, rightx)
  return (rightx-XGFTextWidth(gumptext));
endfunction