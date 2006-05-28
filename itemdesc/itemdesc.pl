#!perl

# MAIN
print "\n\n";
print "POL Config organizer by Austin\n";
print "----------\n";

if ( $ARGV[0] =~ /\.cfg/i)
{
	my @template = GetTemplate($ARGV[1]);
	my $elem_hash = BuildFileHash($ARGV[0], @template);
	foreach $key ( $elem_hash )
	{
		print "$key[0]\n";
	}
}
else
{
	print "Command: perl itemdesc.pl <file-to-clean> (template-file)\n\n	";
}


sub BuildFileHash()
{
	my $file = $_[0];
	my $elem_hash;
	my $elem_key;
	
	open (ITEMDESC, $file) or (print "Cant open $file. Blame Stephen Donald!") && (exit);
	while ( <ITEMDESC> )
	{
		$line = $_;
		chomp($line);
		
		if ( $line =~ /^([a-zA-Z0-9]+)\s+([a-zA-Z0-9 ]+)/i )
		{
			my $type = $1;
			my $name = $2;
						
			#Config elem has begun.
			#print $_;
			if ( $name =~ /0x([a-zA-Z0-9]+)/i )
			{
				#Formats HEX strings to be all capialized.
				$line = "$type\t0x".uc($1);
			}
			$elem_key = $line;
			#print "$line\n";
		}
		elsif ( $line =~ /\s+([a-zA-Z0-9])\s+([a-zA-Z0-9])/i )
		{
			$property = $1;
			$value = $2;
			#print "Putting $_ into $elem_key\n";
			@temp = $elem_hash[$elem_key];
			push (@temp, $line);
			$elem_hash[$elem_key] = $temp;
		}
	}
	close(ITEMDESC);
	
	return $elem_hash;
}

sub GetTemplate
{
	my $file = $_[0];
	if ( !$file )
	{
		$file = "template.cfg";
	}
		
	open (TEMPLATE, "template.cfg") or (print "Cant open template.cfg") && (exit);
	my @template = <TEMPLATE>;
	close(TEMPLATE);
	
	return @template;
}
