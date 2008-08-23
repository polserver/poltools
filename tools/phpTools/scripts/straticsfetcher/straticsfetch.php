#! /usr/bin/php
<?php
/*
	Stratics Fetcher v0.0.1
	by Axel DominatoR ^^^ HC

	Ok, that is a quick, dirty and ugly script made to fetch info from
	online stratics database. Actually it fetches Locations only!
	I'll code the creatures section as soon as possible.
	You need to have PHP CLI to run this. ( it should be easily adaptable to
	a web page, tough...
*/


/*
	Generic I/O functions
*/

function printt
(
	$output
)
{
	system('echo -e "' . $output . '"');
}

function readt()
{
	echo '>>> ';
	return rtrim( fgets( STDIN ));
}

/*
	Main fetch/parse routines
*/

function Fetch_Locations()
{
	$url = 'http://uo.stratics.com/database/view.php?db_content=atlas&id=';
	$outfile = 'Locations.dat';

	if (( $fd = fopen( $outfile, 'a')) !== false )
	{
		for ( $i = 1536; $i < 9999; $i++ )
		{
			$tmpfile = $i . '.loc';
			exec('wget -q "' . $url . $i . '" -O "' . $tmpfile . '"');

			if (( $tmpfd = fopen( $tmpfile, 'r')) !== false )
			{
				$cont_loc = str_replace( array("\n", "\r", "\t"), '', fread( $tmpfd, filesize( $tmpfile )));
				fclose( $tmpfd );
			}

			if ( strpos( $cont_loc, 'MYSQL ERROR' ) !== false )
			{
				echo "Location: ", $i, " does not exists!\n";
			}
			else
			{
				preg_match('|class="top"\>(.+)\<|U', $cont_loc, $raw_data );
				$loc_name = $raw_data[ 1 ];
				preg_match('|X: (.+), Y: (.+)\<|U', $cont_loc, $raw_data );
				$coord_x = $raw_data[ 1 ];
				$coord_y = $raw_data[ 2 ];
				preg_match('|Facet:(.+)width="20|U', $cont_loc, $raw_data );
				$loc_facet = trim( strip_tags( $raw_data[ 1 ]));
				$loc_critters = array();
				preg_match_all('|<li>(.+)\<|U', $cont_loc, $raw_data );
				foreach ( $raw_data[ 1 ] as $rd )
				{
					preg_match('|id=(.+)"|U', $rd, $tmp_res );
					$loc_critters[] = $tmp_res[ 1 ];
				}
				echo 'ID: ', $i, ' Name -', $loc_name, '- X: ', $coord_x, ' Y: ', $coord_y, ' Facet -', $loc_facet, "-\n";
				fwrite( $fd, $i . ',' . $loc_name . ',' . $coord_x . ',' . $coord_y . ',' . $loc_facet . ',');
				foreach ( $loc_critters as $critter )
				{
					fwrite( $fd, $critter . ',');
				}
				fwrite( $fd, "\n");
			}
			exec('rm -f ' . $tmpfile );
		}
		fclose( $fd );
	}
}

function Fetch_Creatures()
{
	$url = 'http://uo.stratics.com/database/view.php?db_content=hunters&id=';
	$outfile = 'Creatures.dat';

	if (( $fd = fopen( $outfile, 'a')) !== false )
	{
		for ( $i = 0; $i < 9999; $i++ )
		{
			$tmpfile = $i . '.crt';
/*			exec('wget -q "' . $url . $i . '" -O "' . $tmpfile . '"');*/

			if (( $tmpfd = fopen( $tmpfile, 'r')) !== false )
			{
				$cont_loc = str_replace( array("\n", "\r", "\t"), '', fread( $tmpfd, filesize( $tmpfile )));
				fclose( $tmpfd );
			}

			if ( strpos( $cont_loc, 'MYSQL ERROR' ) !== false )
			{
				echo "Creature: ", $i, " does not exists!\n";
			}
			else
			{
/*
				preg_match('|class="top"\>(.+)\<|U', $cont_loc, $raw_data );
				$loc_name = $raw_data[ 1 ];
				preg_match('|X: (.+), Y: (.+)\<|U', $cont_loc, $raw_data );
				$coord_x = $raw_data[ 1 ];
				$coord_y = $raw_data[ 2 ];
				preg_match('|Facet:(.+)width="20|U', $cont_loc, $raw_data );
				$loc_facet = trim( strip_tags( $raw_data[ 1 ]));
				$loc_critters = array();
				preg_match_all('|<li>(.+)\<|U', $cont_loc, $raw_data );
				foreach ( $raw_data[ 1 ] as $rd )
				{
					preg_match('|id=(.+)"|U', $rd, $tmp_res );
					$loc_critters[] = $tmp_res[ 1 ];
				}
				echo 'ID: ', $i, ' Name -', $loc_name, '- X: ', $coord_x, ' Y: ', $coord_y, ' Facet -', $loc_facet, "-\n";
				fwrite( $fd, $i . ',' . $loc_name . ',' . $coord_x . ',' . $coord_y . ',' . $loc_facet . ',');
				foreach ( $loc_critters as $critter )
				{
					fwrite( $fd, $critter . ',');
				}
				fwrite( $fd, "\n");
*/
			}
/*			exec('rm -f ' . $tmpfile );*/
		}
		fclose( $fd );
	}
}

/*
	Menus
*/

function Menu_Main()
{
	printt('========================');
	printt('\E[0m[ \E[33;1mMain Menu \E[0m] \E[31;1mSpawn Fetcher\E[0m');
	printt('========================');
	printt('[ \E[33;1ma\E[0m ] - \E[32mFetch Locations\E[0m');
	printt('[ \E[33;1mb\E[0m ] - \E[32mFetch Creatures\E[0m');
	echo "\n";
	printt('[ \E[31;1mx\E[0m ] - \E[31mQuit\E[0m');

	$reply = readt();
	switch ( $reply )
	{
		case 'a':
			Fetch_Locations();
			break;
		case 'b':
/*			Fetch_Creatures();*/
			echo "Fetch_Creatures still under development, sorry!\n";
			break;
		case 'x':
			Quit();
			break;
	}
	Menu_Main();
}

function Quit()
{
	printt('\nThanks for using \E[31mSpawn Fetcher\E[0m!');
	exit;
}

Menu_Main();

?>
