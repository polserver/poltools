#!perl

Main();

sub Main()
{
	print "\n\n";
	print "POL Config organizer by Austin\n";
	print "----------\n";
	
	if ( $ARGV[0] =~ /\.cfg/i)
	{
		my $elem_hash = BuildFileHash($ARGV[0]);
		my @template = GetTemplate($ARGV[1]);
	}
	else
	{
		print "Command: perl cfgformat.pl <file-to-clean> (template-file)\n\n";
	}
	return 1;
}

sub BuildFileHash()
{
	my $file = $_[0];
	my %elem_hash;
	my $elem_key;

	open (CFGFILE, "<$file") or (print "Cant open $file ($!). Blame Stephen Donald.") && (exit);
	while ( <CFGFILE> )
	{
		my $line = $_;
		chomp($line);

		if ( $line =~ /^([a-zA-Z0-9]+)\s+([a-zA-Z0-9 ]+)/i )
		{
			my $type = $1;
			my $name = $2;

			#Config elem has begun.
			if ( $name =~ /0x([a-zA-Z0-9]+)/i )
			{
				#Formats HEX strings to be all capitalized.
				$line = "$type\t0x".uc($1);
			}
			
			$elem_key = $line;
		}
		elsif ( $line =~ /^\s+([a-zA-Z0-9]+)\s+(.+)/i && $elem_key )
		{
			my $property = $1;
			my $prop_value = $2;
			my @value_list;
			if ( exists($elem_hash{$elem_key}{$property}) )
			{
				@value_list = $elem_hash{$elem_key}{$property};
				print @value_list[0];
				print "\n";
			}
			push(@value_list, $prop_value);
			$elem_hash{$elem_key}{$property} = @value_list;
		}
		elsif ( $line =~ /^\}/ )
		{
			$elem_key = 0;
		}
	}
	close(CFGFILE);
	sort keys (%elem_hash);

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
