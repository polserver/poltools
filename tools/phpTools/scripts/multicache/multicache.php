<?php
/************************************************************************/
/* PHP-POL                                                              */
/* ===========================                                          */
/*                                                                      */
/* Copyright (c) 2005 by MuadDib                                        */
/* http://www.lostsoulsshard.org                                        */
/*                                                                      */
/* This program is free software. You can redistribute it and/or modify */
/* it under the terms of the GNU General Public License as published by */
/* the Free Software Foundation; either version 2 of the License.       */
/************************************************************************/
/************************************************************************/
/* The purpose of this PHP Script is simple. Set the $polroot setting   */
/* to the location of your Multicache.dat file you wish to convert.     */
/* This file is what the newest UO Clients use to store Multi info as   */
/* you walk past it. This script will convert it to a cfg file that can */
/* be read back into pol, to build each house. This cfg is formated to  */
/* work with my version of AddSet and CreateSet v2.0 and Higher (to be  */
/* released in May or June of 2005).                                    */
/************************************************************************/

$polroot = null;
process_parameters($polroot);

// Check for pol.log from pol root and process it if it exists.
Echo "Checking for $polroot/Multicache.dat......\r\n\r\n";
if (file_exists("$polroot/Multicache.dat")) {
    $plf = file("$polroot/Multicache.dat") or die("Could Not Access $polroot/Multicache.dat");
    ProcessCacheLog($plf);
}
fgets(STDIN);

function ProcessCacheLog($lftp) {

  global $polroot;

  Echo "File found, processing Multicache.dat.\r\n";
  Echo "Please be patient......\r\n\r\n";
  $reccheck = array();
  if(file_exists("$polroot/multicache.cfg")) {
    $lrf = file("$polroot/multicache.cfg");
  } else {
    $lrf = array();
  }

  $fdh = fopen("$polroot/multicache.cfg", "wb") or die("Could not create file!");

  $chkstr = 0;
  $cntr = 1;
  foreach ($lftp as $elem) {
    if(strstr($elem, "ver")) {
      continue;
    }
    $elem = trim($elem);
    $strfix = explode("\t", $elem);
    if(intval($strfix[0]) > 100000) {
      if($chkstr === 0) {
        $lrf[] = "Multi ".$cntr."\r\n";
        $lrf[] = "{\r\n";
        $chkstr = 1;
        $cntr++;
      } else {
        $lrf[] = "}\r\n";
        $lrf[] = "\r\n";
        $lrf[] = "Multi ".$cntr."\r\n";
        $lrf[] = "{\r\n";
        $cntr++;
      }
    } else if(intval($strfix[0]) < 100000) {
      $tmpit = $strfix[1];
      $strfix[1] = $strfix[2];
      $strfix[2] = $strfix[3];
      $strfix[3] = $strfix[4];
      $strfix[4] = $tmpit;
      $elem = implode(" ", $strfix);
      $lrf[] = "\tItem\t$elem\r\n";
      $chkit;
    }
  }
  if($chkstr <> 0) {
    $lrf[] = "}\r\n";
  }


  foreach ($lrf as $elem) {
    fwrite($fdh, "$elem");
  }

  fclose($fdh);
  Echo "Completed.";
}

	function usage()
	{
		echo "\n";
		echo "Usage: multicache directory\n";
		echo "\n";
        echo "Directory is the location of the multicache.dat you wish to process.\r\n";
        echo "Example \"C:\\POL095\"\n";
        echo "\n";
		sleep(2);
		exit;
	}

	function process_parameters(&$polroot)
	{
		global $argc, $argv;

		$polroot = null;

		if ($argc != 2) // first is executable file
			usage();

		$polroot = $argv[1];
	}


?>