<?

Require_Once("../../includes/polcfg.php");
Main($argv);

function Main(&$argv)
{
	print "\n\n";
	print "POL Config organizer by Austin\n";
	print "----------\n";

	if ( Preg_Match("/\.cfg/i", $argv[1]) )
	{
		$template = GetTemplate($argv[2]);
		CleanUpElems($argv[1], $template);
	}
	else
	{
		Print("Command: php cfgformat.php <file-to-clean> (template-file)\n\n");
	}
	return 1;
}

function CleanUpElems(&$file, &$template)
{
	Print(" * Loading config file...\n");
	$cfg_file = ReadConfigFile($file);
	if ( $cfg_file == FALSE )
	{
		Print("Cant open {$file} ({$php_errormsg}). Blame Stephen Donald.\n");
		exit;
	}
	Print(" * Finding elem names...");
	$elem_names = GetConfigStringKeys($cfg_file, CLASS_LABELS_ON);
	Print("(".Count($elem_names).")\n");
	
	foreach ( $elem_names as $elem_name )
	{
		CleanUpElem($elem_name, $cfg_file, $template);
	}
}

function CleanUpElem(&$elem_name, &$cfg_file, &$template)
{
	$cfg_elem = FindConfigElem($cfg_file, $elem_name);

	// Fixes HEX strings to look like 0xABCDEF12345 rather than 0Xabc or 0xaf
	if ( Preg_Match("/(0x)([a-fA-F0-9]+)/i", $elem_name, $matches) )
		$elem_name = Preg_Replace("/(0x)([a-fA-F0-9]+)/i", "0x".StrToUpper($matches[2]), $elem_name);
	
	foreach ( $template as $line )
	{
		if ( $cfg_elem[$line] )
			Print("{$line}\n");
	}
}

function DisplayElemHash(&$elem_hash)
{
	foreach ( $elem_hash as $elem_key => $elem_info )
	{
		Print("{$elem_key}\n");
		Print("{\n");
		foreach ( $elem_info as $propname => $propvals )
		{
			foreach ( $propvals as $value )
			{
				Print("Property - \t{$propname}\t{$value}\n");
			}
		}
		Print("}\n\n");
	}
}

function GetTemplate(&$file=0)
{
	Print(" * Loading template file...\n");
	
	if ( !$file )
		$file = "template.cfg";
	$cfg_file = ReadConfigFile($file);
	if ( $cfg_file == FALSE )
	{
		Print("Cant open template {$file} ({$php_errormsg}).\n");
		exit;
	}
	
	$template = FindConfigElem($cfg_file, "Template");
	if ( $template == FALSE )
	{
		Print("Cant find elem 'Template'.\n");
		exit;
	}
	else
	{
		$order = array();
		foreach ( $template as $key => $value )
			Array_Push($order, $key);
		
		return $order;
	}
}
