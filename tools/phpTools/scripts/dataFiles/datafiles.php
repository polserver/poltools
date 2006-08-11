<?

Require_Once("../../includes/polcfg.php");

$class_types = "Item|StorageArea|Account|General|Guild|NPC|Character|Multi|DataFile|System|GlobalProperties|RegionalResourcePool|GlobalResourcePool";

Main($argv);

function Main(&$argv)
{
	print "\n\n";
	print "DataFile Integrity Checker by Austin\n";
	print "----------\n";

	if ( Preg_Match("/[.txt|.cfg]$/i", $argv[1]) )
	{
		VerifyElems($argv[1], $template);
		Print("Verification complete.\n");
	}
	else
	{
		Print("Command: php cfgformat.php <path to data file>\n\n");
	}
	return 1;
}

function VerifyElems(&$path, &$template)
{
	global $class_types;

	Print(" * Loading file...\n");
	$data_file = @File("$path");
	if ( !$data_file )
	{
		Print("Cant open {$file} ({$php_errormsg}). Blame Stephen Donald.\n");
		exit;
	}

	$in_elem = 0;

	Print("Running checking elems for proper formatting...\n");
	foreach ( $data_file as $line_num => $data_line )
	{
		$data_line = RTrim($data_line);
		if ( !$data_line )
			// Blank line
			continue;
		elseif ( Preg_Match("/^(\/\/|#)/", $data_line) )
			//Comment line
			continue;
		elseif ( Preg_Match("/^\{$/", $data_line) )
			$in_elem = true;
		elseif ( Preg_Match("/^\}$/", $data_line) )
			$in_elem = false;
		elseif ( !$in_elem )
		{
			if ( !Preg_Match("/^[{$class_types}]/i", $data_line) )
			{
				Print("Found data outside an elem on line {$line_num}\n");
			}
		}
	}
}
