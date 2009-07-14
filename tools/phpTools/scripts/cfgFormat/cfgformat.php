<?php

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
	Print(" * Purging CleanedConfig.cfg for new write...\n");
	
	Print(" * Finding elem names...");
	$elem_names = GetConfigStringKeys($cfg_file, CLASS_LABELS_ON);
	Print("(".Count($elem_names).")\n");
	
	$handle = FOpen("CleanedConfig.cfg", "w");
	FClose($handle);
	
	foreach ( $elem_names as $elem_name )
	{
		$new_elem = CleanUpElem($elem_name, $cfg_file, $template);
		WriteElemToFile($elem_name, $new_elem);
	}
}

function CleanUpElem(&$elem_name, &$cfg_file, &$template)
{
	// Fixes HEX strings to look like 0xABCDEF12345 rather than 0Xabc or 0xaf
	if ( Preg_Match("/(0x)([a-fA-F0-9]+)/i", $elem_name, $matches) )
		$elem_name = Preg_Replace("/(0x)([a-fA-F0-9]+)/i", "0x".StrToUpper($matches[2]), $elem_name);
	Print("    Cleaning up elem '{$elem_name}'\n");
	
	$cfg_elem = FindConfigElem($cfg_file, $elem_name);
	$cfg_elem = array_change_key_case($cfg_elem, CASE_LOWER);
	
	$new_elem = array();
	$last_label = 0;
	foreach ( $template as $indice => $line )
	{
		if ( Preg_Match('/\[Label=([[:alnum:][:space:][:punct:]]+)\]/', $line, $matches) )
		{
			if ( $last_label )
				UnSet($new_elem[$last_label]);
			
			$matches[1] = Preg_Replace("/%s/i", " ", $matches[1]);	
			$line = "//{$matches[1]}";
			if ( $indice > 1 )
				$line = "\n\t{$line}";
			$last_label = $line;
			$new_elem[$line] = array("");
			continue;
		}
		else
		{
			$property = StrToLower($line);
			if ( $cfg_elem[$property] )
			{
				$new_elem[$line] = $cfg_elem[$property];
				UnSet($cfg_elem[$property]);
				$last_label = 0;
			}
		}
	}
	if ( Count($cfg_elem) > 0 )
	{
		$new_elem["\n\t//Custom Values"] = array("");
		// Lines not in the template go at the end as custom values
		foreach ( $cfg_elem as $key => $value )
			$new_elem[$key] = $value;
	}
	if ( $last_label )
		UnSet($new_elem[$last_label]);
	
	return $new_elem;
}

function WriteElemToFile(&$elem_name, &$elem_lines)
{
	$handle = FOpen("CleanedConfig.cfg", "a");
	FWrite($handle, "{$elem_name}\n");
	FWrite($handle, "{\n");
	foreach ( $elem_lines as $property => $values )
	{
		foreach ( $values as $value )
			FWrite($handle, "\t{$property}\t$value\n");
	}
	FWrite($handle, "}\n\n");
	FClose($handle);
	
	return 1;
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
