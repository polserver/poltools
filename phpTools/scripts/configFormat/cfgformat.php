<?

Main($argv);


function Main(&$argv)
{
	print "\n\n";
	print "POL Config organizer by Austin\n";
	print "----------\n";

	if ( preg_match("/\.cfg/i", $argv[1]) )
	{
		$elem_hash = BuildFileHash($argv[1]);
		$template = GetTemplate($argv[2]);
		CleanUp($elem_hash, $template);
	}
	else
	{
		print "Command: php cfgformat.php <file-to-clean> (template-file)\n\n";
	}
	return 1;
}

function BuildFileHash(&$file)
{
	Print(" * Building arrays for config file...\n");
	$elem_hash = array();
	$elem_key = "";

	$handle = @FOpen($file, "r");
	if ( !$handle )
	{
		Print("Cant open {$file} ({$php_errormsg}). Blame Stephen Donald.\n");
		exit;
	}

	while ( !FEof($handle) )
	{
		$line = FGets($handle);

		if ( preg_match("/^([a-zA-Z0-9]+)\s+([a-zA-Z0-9 ]+)/i", $line, $matches) )
		{
			$type = $matches[1];
			$name = $matches[2];

			// Config elem has begun!
			if ( preg_match("/0x([a-zA-Z0-9]+)/i", $name, $matches) )
				$line = "{$type}\t0x".StrToLower($matches[1]);

			$elem_key = $line;
			if ( !IsSet($elem_hash[$elem_key]) )
				$elem_hash[$elem_key] = array();
		}
		elseif ( preg_match("/^\s+([a-zA-Z0-9]+)\s+(.+)/i", $line, $matches) && $elem_key )
		{
			$property = $matches[1];
			$value = $matches[2];

			if ( !IsSet($elem_hash[$elem_key][$property]) )
				$elem_hash[$elem_key][$property] = array();

			Array_Push($elem_hash[$elem_key][$property], $value);
		}
		elseif ( preg_match("/^\}/", $line) )
		{
			$elem_key = 0;
		}
	}

	FClose($handle);

	return $elem_hash;
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
				Print("\t{$propname}\t{$value}\n");
			}
		}
		Print("}\n\n");
	}
}

function CleanUp(&$elem_hash, &$template)
{
	
}

function GetTemplate(&$file=0)
{
	Print(" * Loading template file...\n");
	if ( !$file )
	{
		$file = "template.cfg";
	}

	$template = @File($file);
	if ( $template == FALSE )
	{
		Print("Cant open template {$file} ({$php_errormsg}).\n");
		exit;
	}

	return $template;
}
