//$Id: zuluBooks.inc 386 2005-10-03 00:22:39Z muaddiblsd $

/*===============================================================
* Current Version
* ZULUBOOKS.INC - v1.0
* Updated 9/29/2005 4:00PM
*
* -- Revision v1.0 --
* Austin:
*  Created include file
===============================================================*/

use os;
use cfgfile;

/* SysBook_GetZuluBooksCfgFile()
 * 
 * Purpose
 * Loads zulubooks.cfg
 *
 * Parameters
 *
 * Return value
 * Config file
 *
 */
function SysBook_GetZuluCfgFile()
	var cfg_file := ReadConfigFile(":sysbook:config/zuluBooks");
	if ( cfg_file.errortext )
		SysLog("Error::SysBook_GetZuluBooksCfgFile() - Unable to open zuluBooks.cfg ->"+cfg_file.errortext);
	endif

	return cfg_file;
endfunction

/*
 * SysBook_GetZuluCfgElem(elem_name, cfg_file)
 *
 * Purpose
 * Retrieves a config elem for a book in zulubooks.cfg
 *
 * Parameters
 * elem_name:		Name of the book elem to retrieve.
 * cfg_file:		Optional parameter - reference to a config already read in.
 *
 * Return value
 * Returns a config file elem.
 *
 */
function SysBook_GetZuluCfgElem(elem_name, byref cfg_file:=0)
	if ( !cfg_file )
		cfg_file := SysBook_GetZuluCfgFile();
	endif
	
	var cfg_elem := cfg_file[elem_name];
	if ( cfg_elem.errortext )
		SysLog("Error::SysBook_GetZuluCfgElem() - unable to find cfg elem ["+elem_name+"] ->"+cfg_elem.errortext);
	endif
	
	return cfg_elem;
endfunction

/*
 * SysBook_ParseZuluCfgElem(elem_name, byref cfg_file)
 *
 * Purpose
 * Parses the insanity that exists within zulubooks.cfg and returns
 * a data structure that can be used with the method script of a book.
 *
 * Parameters
 * elem_name:		Name of the book elem to retrieve.
 * cfg_file:		Optional parameter - reference to a config already read in.
 *
 * Return value
 * Returns a config file elem.
 *
 */
function SysBook_ParseZuluCfgElem(elem_name, byref cfg_file)
	if ( !cfg_file )
		cfg_file := SysBook_GetZuluCfgFile();
	endif
	
	var cfg_elem :=  SysBook_GetZuluCfgElem(elem_name, cfg_file);
	if ( !cfg_elem )
		return 0;
	endif
	
	var book_info := struct;
	book_info.+Author := cfg_elem.Author;
	book_info.+Title := cfg_elem.Title;
	book_info.+NumPages := CInt(cfg_elem.Pages);
	book_info.+Contents := array;
	
	var num_pages := CInt(cfg_elem.Pages);
	var page_num := 1;
	for ( page_num; page_num <= num_pages; page_num:=page_num+1 )
		var line_num := 1;
		for ( line_num; line_num <= 8; line_num := line_num+1 ) // 8 lines per page
			var prop_string := "p"+page_num+"l"+line_num; //p#l#
			var line_text := GetConfigString(cfg_elem, prop_string);
						
			if ( !line_text )
				line_text := "";
			endif
					
			(book_info.contents).Append(line_text);
		
			sleepms(2);
		endfor
		sleepms(2);
	endfor
	
	return book_info;
endfunction
