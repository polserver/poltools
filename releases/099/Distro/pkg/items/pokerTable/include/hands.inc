/*
        Created by *Edwards
*/

const POKER_HIGH_CARD := 0;
const POKER_PAIR := 1;
const POKER_TWO_PAIRS := 2;
const POKER_THREE_OF_A_KIND := 3;
const POKER_STRAIGHT := 4;
const POKER_FLUSH := 5;
const POKER_FULL_HOUSE := 6;
const POKER_FOUR_OF_A_KIND := 7;
const POKER_STRAIGHT_FLUSH := 8;
const POKER_ROYAL_FLUSH := 9;

function CheckCards( data_elem )

         var table_info := data_elem.GetProp( "Seats" ),
             playerCard1, playerCard2,
             the_cards;

         var i;
         for( i:=1; i<=10; i+=1 )
                 if( table_info[i].money && table_info[i].gotcards )
                         playerCard1 := data_elem.GetProp( "PCardFirst"+i );
                         playerCard2 := data_elem.GetProp( "PCardSecond"+i );

                         the_cards := {playerCard1, playerCard2};

                         ValidateCards( data_elem, the_cards, i );
                 endif
                 SleepMS(2);
         endfor
         
         data_elem.SetProp( "NextDealer", 1 );

         return 1;
endfunction

function ValidateCards( data_elem, player_cards, position )

         //Main Hand
	 var bestHand := POKER_HIGH_CARD;
	 var handValues := array{};
  
	 //The cards
	 var first_card := data_elem.GetProp( "TCard1" ),
	     second_card := data_elem.GetProp( "TCard2" ),
	     third_cards := data_elem.GetProp( "TCard3" ),
	     fourth_card := data_elem.GetProp( "TCard4" ),
	     fifth_card := data_elem.GetProp( "TCard5" ),
	     playerCard1 := player_cards[1],
	     playerCard2 := player_cards[2];

	 //Right now I'm testing with all the 7cards on the table for each players. I might fix later to read
	 //the process flop-turn & || river to append in var the_cards

	 var the_cards := {first_card, second_card, third_cards, fourth_card, fifth_card, playerCard1, playerCard2};

	 var cards := array{0,0,0,0,0,0,0,0,0,0,0,0,0},
             the_card;
      
	 foreach card in the_cards
		the_card := card[2];
		case( card[2] )
			2:      cards[2] += 1;
                                break;
			3:      cards[3] += 1;
                                break;
			4:      cards[4] += 1;
                                break;
			5:      cards[5] += 1;
                                break;
			6:      cards[6] += 1;
                                break;
			7:      cards[7] += 1;
                                break;
			8:      cards[8] += 1;
                                break;
			9:      cards[9] += 1;
                                break;
			10:     cards[10] += 1;
                                break;
			"J":	cards[11] += 1;
                                the_cards[_card_iter][2] := 11;
                                break;
			"Q":	cards[12] += 1;
                                the_cards[_card_iter][2] := 12;
                                break;
			"K":	cards[13] += 1;
                                the_cards[_card_iter][2] := 13;
                                break;
			"A":	cards[1] += 1;
                                the_cards[_card_iter][2] := 14;
                                break;
		endcase

		SleepMS(2);
	 endforeach
  
	 var flush_clubs := array{},
             flush_diamonds := array{},
             flush_hearts := array{},
             flush_spades := array{},
	     flush;

	 foreach card in the_cards
		case( card[1] )
			"clubs":     flush_clubs.Append( card[2] );
                                     break;
			"diamonds":  flush_diamonds.Append( card[2] );
                                     break;
			"hearts":    flush_hearts.Append( card[2] );
                                     break;
			"spades":    flush_spades.Append( card[2] );
                                     break;
		endcase

		SleepMS(2);
	 endforeach
  
         var suitCards := array{flush_clubs, flush_diamonds, flush_hearts, flush_spades};
	 foreach suit in suitCards
		if( suit.Size() >= 5 )
			bestHand := POKER_FLUSH;
			flush := suit;			// we need to remember we have a flush for later evaluation.
                        if( flush == flush_clubs )
                        	 handValues.Insert( 1, "clubs" );
                        elseif( flush == flush_diamonds )
                        	 handValues.Insert( 1, "diamonds" );
                        elseif( flush == flush_hearts )
                        	 handValues.Insert( 1, "hearts" );
                        elseif( flush == flush_spades )
                        	 handValues.Insert( 1, "spades" );
                        endif
		endif

		SleepMS(2);
	 endforeach

	 var lastValue := 0,
	     streak := 0,
             kicker := 2;
      
	 foreach cardAmt in cards

		var card_value := _cardAmt_iter;
		if( card_value == 1 )
			card_value := 14;
		endif
  
		case( cardAmt )
			4:	bestHand := POKER_FOUR_OF_A_KIND;
				handValues.Insert( 1, _cardAmt_iter );
				if( handValues[1] == 1 )
					handValues[1] := 14;		// Make sure aces aren't beatable
				endif
				streak := 0;
				break;
			3:	if( bestHand in array{POKER_PAIR, POKER_TWO_PAIRS} )
					bestHand := POKER_FULL_HOUSE;
					handValues.Insert( 1, _cardAmt_iter );
					if( handValues[1] == 1 )
						handValues[1] := 14;		// Make sure aces aren't beatable
					endif
				elseif( bestHand < POKER_THREE_OF_A_KIND )
					bestHand := POKER_THREE_OF_A_KIND;
					handValues.Insert( 1, _cardAmt_iter );
					if( handValues[1] == 1 )
						handValues[1] := 14;		// Make sure aces aren't beatable
					endif
				endif
    
				if( lastValue && lastValue <= 3 )
					streak += 1;
				endif
				break;
			2:	if( bestHand == POKER_THREE_OF_A_KIND )
					bestHand := POKER_FULL_HOUSE;
					handValues.Insert( 2, _cardAmt_iter );
				elseif( besthand == POKER_PAIR )
					bestHand := POKER_TWO_PAIRS;
					handValues.Insert( 1, _cardAmt_iter );
				elseif( bestHand < POKER_PAIR )
					bestHand := POKER_PAIR;
					handValues.Insert( 1, _cardAmt_iter );
					if( handValues[1] == 1 )
						handValues[1] := 14;		// Make sure aces aren't beatable
					endif
				endif
    
				if( lastValue && lastValue <= 3 )
					streak += 1;
				endif
				break;
			1:	if( lastValue == 1 )
					streak += 1;
				endif

                                if( card_value > kicker )
                                	if( bestHand == POKER_FLUSH )
                                                if( flush[cardAmt] )
                                        	        handValues.Insert( 2, _cardAmt_iter );
                                                endif
                                	elseif( bestHand == POKER_HIGH_CARD )
                                        	handValues.Insert( 1, _cardAmt_iter );
                                	elseif( bestHand == POKER_PAIR )
                                        	handValues.Insert( 2, _cardAmt_iter );
                                	elseif( bestHand == POKER_TWO_PAIRS )
                                        	handValues.Insert( 3, _cardAmt_iter );
                                	elseif( bestHand == POKER_THREE_OF_A_KIND )
                                        	handValues.Insert( 2, _cardAmt_iter );
                                	else
                                        	handValues.Append( _cardAmt_iter );
                                	endif
                                endif
				break;
			0:	streak := 0;
		endcase
  
		if( streak >= 5 || ( streak >= 4 && _cardAmt_iter == 13 && cards[1] ))   // if 5 straight or straight from at least 10 to K and Aces are present (remember aces are 1 in this array)
			if( flush )
				if( cards[1] && cards[10] && cards[11] && cards[12] && cards[13] )
					bestHand := POKER_ROYAL_FLUSH;
					handValues.Insert( 1, 14 );
				else
					besthand := POKER_STRAIGHT_FLUSH;
					if( _cardAmt_iter == 13 && cards[1] )		// make sure we don't forget to count the aces if King is high
						handValues.Insert( 1, 14 );
					else
						handValues.Insert( 1, _cardAmt_iter );
					endif
				endif
			else
				if( bestHand < POKER_STRAIGHT )
					bestHand := POKER_STRAIGHT;
					if( _cardAmt_iter == 13 && cards[1] )		// make sure we don't forget to count the aces if King is high
						handValues.Insert( 1, 14 );
					else
						handValues.Insert( 1, _cardAmt_iter );
					endif
				endif
			endif
		endif
		lastValue := cardAmt;
                SleepMS(2);
	 endforeach

	 case( bestHand )
		POKER_ROYAL_FLUSH:	handValues.Shrink(1);
					break;

		POKER_STRAIGHT_FLUSH:	handValues.Shrink(1);
					break;

		POKER_FOUR_OF_A_KIND:	handValues.Shrink(0);
					break;

		POKER_FULL_HOUSE:	handValues.Shrink(2);
					break;

		POKER_FLUSH:		handValues.Shrink(3);
					break;

		POKER_STRAIGHT:		handValues.Shrink(1);
					break;

		POKER_THREE_OF_A_KIND:	handValues.Shrink(3);
					break;

		POKER_TWO_PAIRS:	handValues.Shrink(3);
					break;

		POKER_PAIR:		handValues.Shrink(4);
		                        break;

		POKER_HIGH_CARD:	handValues.Shrink(5);
		                        break;
	 endcase
         
         var seats := data_elem.GetProp( "Seats" );
         
         seats[position].besthand := bestHand;
         seats[position].handvalue := handValues;
         
         data_elem.SetProp( "Seats", seats );
  
	 return 1;
endfunction

function Check_Winner( npc, data_elem )

         var table_info := data_elem.GetProp( "Seats" ),
             position := 0,
             global_bestHand := 0,
             global_handValue := array{},
             draw := array{};
             
         var i;
         for( i:=1; i<=10; i+=1 )
                 if( table_info[i].gotcards )
                         if( table_info[i].besthand > global_bestHand )
                                 position := i;
                                 global_bestHand := table_info[i].besthand;
                                 global_handValue := table_info[i].handvalue;
                         elseif( table_info[i].besthand == global_bestHand )
                                 var cards;
                                 for( cards:=1; cards<=5; cards+=1 )
                                         if( table_info[i].handvalue[cards] > global_handValue[cards] )
                                                 position := i;
                                                 draw := array{};
                                                 global_bestHand := table_info[i].besthand;
                                                 global_handValue := table_info[i].handvalue;
                                                 break;
                                         else
                                                 draw.Append( i );
                                         endif
                                         
                                         SleepMS(2);
                                 endfor
                         endif
                 endif
                 
                 SleepMS(2);
         endfor
         
         PrintBestHand( npc, table_info, position );
                 
         if( draw.Size() )
                 foreach DrawPosition in draw
                         PrintBestHand( npc, table_info, DrawPosition );
                         SleepMS(2);
                 endforeach
         endif

	 return 1;
endfunction

function PrintBestHand( npc, table_info, position )

         var winner_info := table_info[position],
             winner_name := SystemFindObjectBySerial( winner_info.serial ).name,
             bestHand := winner_info.besthand,
             handValues := winner_info.handvalue;

         foreach value in handValues
         
                  if( value == 14 )
                           value := "A";
                  endif
                  if( value == 13 )
                           value := "K";
                  endif
                  if( value == 12 )
                           value := "Q";
                  endif
                  if( value == 11 )
                           value := "J";
                  endif
                  
                  SleepMS(2);
         endforeach
         
         if( winner_info )
                 case( bestHand )
                         0: PrintTextAbove( npc, winner_name+" won the pot with high card "+handValues[1]+"!", 3, 55 );
                         1: PrintTextAbove( npc, winner_name+" won the pot with one pair of "+handValues[1]+"'s and "+handValues[2]+" kicker!", 3, 55 );
                         2: PrintTextAbove( npc, winner_name+" won the pot with two pairs "+handValues[1]+"'s and "+handValues[2]+"'s with "+handValues[3]+" kicker!", 3, 55 );
                         3: PrintTextAbove( npc, winner_name+" won the pot with three of a kind of "+handValues[1]+"'s with "+handValues[2]+" kicker!", 3, 55 );
                         4: PrintTextAbove( npc, winner_name+" won the pot with straight to "+handValues[1]+"!", 3, 55 );
                         5: PrintTextAbove( npc, winner_name+" won the pot with flush of "+handValues[1]+" to "+handValues[2]+"!", 3, 55 );
                         6: PrintTextAbove( npc, winner_name+" won the pot with full house of "+handValues[1]+"'s to "+handValues[2]+"!", 3, 55 );
                         7: PrintTextAbove( npc, winner_name+" won the pot with four of a kind "+handValues[1]+"'s with "+handValues[2]+" kicker!", 3, 55 );
                         8: PrintTextAbove( npc, winner_name+" won the pot with a straight flush to "+handValues[1]+"!", 3, 55 );
                         9: PrintTextAbove( npc, winner_name+" won the pot with a royal flush!", 3, 55 );
                 endcase
         endif
         
         return 1;
endfunction
