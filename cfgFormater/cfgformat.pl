#!perl

# MAIN
print "\n\n";
print "POL Config organizer by Austin\n";
print "----------\n";

if ( $ARGV[0] =~ /\.cfg/i)
{
	my @template = GetTemplate($ARGV[1]);
	BuildFileHash($ARGV[0]);
}
else
{
	print "Command: perl cfgformat.pl <file-to-clean> (template-file)\n\n	";
}


sub BuildFileHash()
{
	my $file = $_[0];
	my %elem_hash = ();
	my $elem_key;

	open (CFGFILE, "<$file") or (print "Cant open $file ($!). Blame Stephen Donald.") && (exit);
	while ( <CFGFILE> )
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
				#Formats HEX strings to be all capitalized.
				$line = "$type\t0x".uc($1);
			}
			$elem_key = $line;
			#print "$line\n";
		}
		elsif ( $line =~ /\s+([a-zA-Z0-9]+)\s+(.+)/i )
		{
			my $property = $1;
			my $value = $2;
			my @value_list;
			if ( exists($elem_hash{$elem_key}{$property}) )
			{
				@value_list = $elem_hash{$elem_key}{$property};
			}
			push(@value_list, $value);
			$elem_hash{$elem_key}{$property} = @value_list;
		}
	}
	close(CFGFILE);
	sort keys (%elem_hash);

	foreach my $elem ( keys(%elem_hash) )
	{
		#print "$elem\n";
		foreach my $property ( $elem_hash{$elem} )
		{
			#print "$elem_hash{$elem}{$property}\n";
		}
	}

	return %elem_hash;
}

sub GetTemplate
{
	my $file = $_[0];
	if ( !$file )
	{
		$file = "template.cfg";
	}

	open (TEMPLATE, "<$file") or (print "Cant open template.cfg ($!)") && (exit);
	my @template = <TEMPLATE>;
	close(TEMPLATE);

	return @template;
}
