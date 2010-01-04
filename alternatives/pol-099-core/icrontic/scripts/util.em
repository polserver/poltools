RandomInt( below_this_number );     // returns integer R such that 0 <= R < B
// RandomFloat( below_this_float ); // returns float R such that 0.0 <= R < B

// formats for RandomDiceRoll dice_string parameter:
//      4
//      d4
//      d6+6
//      d6-2
//      2d12
//      2d8+12
//      2d8-4
// Returns an integer >= 0
RandomDiceRoll( dice_string );      
