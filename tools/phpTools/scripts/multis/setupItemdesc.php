<?php
/* $Id$
 *
 */

Require_Once("../../includes/polcfg.php");
Main($argv);

function Main(&$argv)
{
	print "\n\n";
	print "Multis.cfg to Itemdesc.cfg by Austin\n";
	print "----------\n";

	if ( Preg_Match("/\.cfg/i", $argv[1]) )
	{
		BuildItemDesc($argv[1]);
	}
	else
	{
		Print("Command: php setupItemdesc.php <multis.cfg path>\n\n");
	}
	return 1;
}

function BuildItemDesc($file)
{
	Print(" * Loading multis config file...\n");
	$cfg_file = ReadConfigFile($file);
	if ( $cfg_file == FALSE )
	{
		Print("Cant open {$file} ({$php_errormsg}). Blame Stephen Donald.\n");
		exit;
	}

	Print(" * Purging itemdesc.cfg for new write...\n");
	$handle = FOpen("itemdesc.cfg", "w");
	FClose($handle);
	$handle = FOpen("itemdesc.cfg", "a");

	Print(" * Finding elem names...");
	$elem_names = GetConfigStringKeys($cfg_file, CLASS_LABELS_ON);
	Print("(".Count($elem_names).")\n");


	foreach ( $elem_names as $elem_name )
	{
		if ( !Preg_Match("/^House\s+/i", $elem_name) )
		{
			continue;
		}

		$multi_id = Preg_Replace("/^House\s+/i", "", $elem_name);

		$objtype = HexDec($multi_id);
		$objtype += 16384; // 0x4000
		$objtype = "0x".DecHex($objtype);

		FWrite($handle, "House {$objtype}\n");
		FWrite($handle, "{\n");
		FWrite($handle, "	// Main Stuff\n");
		FWrite($handle, "	Name		House{$objtype}\n");
		FWrite($handle, "	Graphic		{$objtype}\n");
		FWrite($handle, "\n");
		FWrite($handle, "	// Multi Info\n");
		FWrite($handle, "	MultiID		{$multi_id}\n");
		FWrite($handle, "}\n\n");
	}
	FClose($handle);

	return 1;
}
