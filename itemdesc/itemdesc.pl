#!perl

# MAIN
print "POL Config organizer by Austin\n";
print "----------\n";

if ( $ARGV[0] =~ /\.cfg/i)
{
	@template = GetTemplate($ARGV[1]);
	BuildFileHash($ARGV[0], @template);
}
else
{
	print "Command: perl itemdesc.pl <file-to-clean> (template-file)";
}


sub BuildFileHash()
{
	my $file = $_[0];
	open (ITEMDESC, $file) or (print "Cant open $file. Blame Stephen Donald!") && (exit);
	while ( <ITEMDESC> )
	{
		#print "$_";
		if ( $_ =~ /^[a-zA-Z0-9]+\s+0x\d+/ )
		{
			#Config elem has begun.
			print $_;
		}
	}
	
	#@template = GetTemplate();
	#for my $line ( @template )
	#{
	#	print $line;
	#}
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
	
	return @template;
}
