// math.inc
//
// Siggy's Improved Math Include, v1.0
//
// Questions, comments, flames?   Send to:
// EMail: prostheticdelirium@worldnet.att.net
// ICQ: 10520050

const CONST_E := 2.718282;
const CONST_PI := 3.141592;


function Sin(b, c)

// Returns the Sine of angle Theta, given either the measure of the
// angle or the opposite side and hypotenuse of a triangle.  If !c,
// then we're assuming that b = measure of angle Theta in degrees.
// (We convert to Rads)  Given the angle, sin is computed as:
//      sin(x) = x - x^3/3! + x^5/5! - x^7/7! + x^9/9! - x^11/11!
// which is pretty accurate for all intensive purposes, at least to
// four decimal places.  If b and c are given, b = opposite side, 
// c = hypotenuse.

	var i, x, theta;

	if (c==0)
		b := DegToRad(b);
		theta := b;	
		for (i:=1; i < 6; i:=i + 1)
			x := (2 * i) + 1;
			x := Exp(b, x) / Fac(x);

			if ( (i==1) or (i==3) or (i=5) )
				theta := theta - x;
			else
				theta := theta + x;
			endif
		endfor
	else
		theta := (b / c);
	endif

	return theta;


endfunction


function Cos(a, c)

// Returns the Cosine of angle Theta, given either the measure of the
// angle or the adjacent side and hypotenuse of a triangle.  If !c,
// then we're assuming that a = measure of angle Theta in degrees.
// (We convert to Rads)  Given the angle, cos is computed as:
//      cos(x) = 1 - x^2/2! + x^4/4! - x^6/6! + x^8/8! - x^10/10!
// which is pretty accurate for all intensive purposes, at least to
// four decimal places.  If a and c are given, a = adjacent side, 
// c = hypotenuse.

	var i, x, theta := 1;

	if (c==0)
		a := DegToRad(a);
		for (i:=1; i < 6; i:=i + 1)
			x := 2 * i;
			x := Exp(a, x) / Fac(x);

			if ( (i==1) or (i==3) or (i=5) )
				theta := theta - x;
			else
				theta := theta + x;
			endif
		endfor
	else
		theta := (a / c);
	endif

	return theta;

endfunction


function Tan(a, b)

// Returns the tangent of an angle given the measure of the angle
// in degrees, or the lengths of the sides opposite and adjacent
// in a triangle.

	var theta := 0;

	if (b==0)
		a := DegToRad(a);
		theta := ( Cos(a,0) / Sin(a,0) );
	else
		theta := a / b;
	endif

	return theta;

endfunction


function DegToRad(deg)

// Converts an angle in degrees to radians.

	var rad := (deg * 180.000) / CONST_PI;
	return rad;

endfunction


function RadToDeg(rad)

// Converts an angle in radians to degrees.

	var deg := (rad * CONST_PI) / 180.000;
	return deg;

endfunction


function Log(base, answer)

// Finds the logarithm to <base> of <answer>.

	var x := 1.000;
	var logcount := 0.000;
	var diff := 1.000;
	var success := 0;

	While (success = 0)

		x := x * base;

		If (Abs(answer - x) < 0.001)
			logcount := logcount + 1;
			success := 1;
		else
			If (x > answer)
				diff := diff / 2;
				logcount := logcount - diff;
				x := Exp(base, logcount);
			else
				logcount := logcount + diff;
			endif
		endif
	
		Count := Count + 1;

		If (Count > 200) 
			success := 1;
		endif

	endwhile

	return logcount;

endfunction


function Abs(x)

// Returns the absolute value (distance from zero) of x.

	if (x < 0)
		x := 0 - x;
	endif
	return x;

endfunction


function Mod(Numerator, Denominator)

// Returns the modulus (remainder) of the fraction:
//
//		 Numerator
//		-----------
//		Denominator

	While (Numerator > Denominator)
		Numerator := Numerator - Denominator;
	endwhile

	return Numerator;

endfunction


function Trunc(number, places)

// Truncates a dbl to a fixed amount of decimal places.

	var base := Exp(10, places);

	number := number * base;
	number := Cint(number);
	number := CDbl(number);
	number := number / base;

	return number;

endfunction


function Fac(factorial);

// Returns the factorial of a number:
// Fac(10) = 10 * 9 * 8 * 7 * 6 * 5 * 4 * 3 * 2 * 1

	var x;
	While (factorial > 0)
		x := x * factorial;
		factorial := factorial - 1;
	endwhile
	return x;

endfunction


function Exp(base, power)

// Returns the exponent of <base> raised to <power>:
// Exp(2, 5) = 2^5 = 32

	var i;
	var x := 1;
	var check := Cint(power);
	if (power = check)
		for(i:=1;i<=power;i:=i+1)
			x := x * base;
		endfor
		return x;
	else
		var m, n;
		m := power - check;
		m := m * 1000;
		n := YRoot(base, 1000);
		x := Exp(n, m);
		return x;
	endif

endfunction


function YRoot(x, y)

// Returns the <y> root of a number <x>:
	
	var i := 0.000;
	var n := 0.000;
	var root := 2.000;
	var a := CInt(Sqr(x));

	if (a < 4)
		a := 4;
	endif

	var success := 0;
	
	
	While (success = 0)		

		n := Exp(root, y);
		i := x - n;
		i := i / (a * root);
		i := Trunc(i, 3);
		root := root + i;
		if (i = 0)
			success := 1;
			if ( Exp(Root, y) > x )
				root := root - 0.001;
			endif
			if ( Exp((root + 0.001), y) = x)
				root := root + 0.001;
			endif
		endif
	endwhile

	return root;

endfunction


function Sqr(x)

// Returns the square root of x.

	var i := 0.000;
	var n := 0.000;
	var root := 2.000;
	var success := 0;
	
	While (success = 0)		

		n := Exp(root, 2);
		i := x - n;
		i := i / (2 * root);
		i := Trunc(i, 3);
		root := root + i;
		if (i = 0)
			success := 1;
			if ( Exp(Root, 2) > x )
				root := root - 0.001;
			endif
		endif
	endwhile

	return root;

endfunction


function Mean(ArrayX)

// Returns the arithmatic mean (average) of an array of numbers.

	var n, i:=0.000;

	foreach n in ArrayX
		i := i + ArrayX[n];
	endforeach

	n := CDbl(i / Len(ArrayX) );

	return n;

endfunction


function StdDev(ArrayX)

// Returns the Standard Deviation of an array of numbers.

	var m, n, i := 0.000;
	var ArrayMean := Mean(ArrayX);
	var ArrayLen := Len(ArrayX);

	foreach m in ArrayX
		n := ArrayX[m] - ArrayMean;
		n := Exp(n, 2);
		i := i + n;
	endforeach

	i := i / (ArrayLen - 1);
	n := Sqr (i);

	return n;

endfunction


function ZTest(ArrayX, mu, sigma)

// Returns the test statistic for a ZTest.   This is used to show
// the likelihood that a sample is drawn from the population.  The 
// mean and standard deviation for the population must be known.
// The z-statistic is computed as follows:
//
//	z = (mean - mu) / (sigma / (Sqr(n))
//
// mean = the mean of the ArrayX data
// mu = the mean of the entire population
// sigma = the standard deviation of the population
// n = the number of samples in the data

	var i, n;
	var z:= 0.000;
	var ArrayMean := Mean(ArrayX);
	var ArrayLen := Len(ArrayX);

	i := ArrayMean - mu;
	n := sigma / Sqr(ArrayLen);

	z := i/n;

	return := z;

endfunction


function TTest(ArrayX, mu)

// Returns the test statistic for a TTest.   This is used to show
// the likelihood that a sample is drawn from the population, when 
// the mean and standard deviation for the entire population are
// unknown.  The t-statistic is computed as follows:
//
//	t = (mean - mu) / (s / (Sqr(n))
//
// mean = the mean of the ArrayX data
// mu = the estimated mean of the entire population
// s = the standard deviation of the samples
// n = the number of samples in the data


	var i, n, s;
	var t:= 0.000;
	var ArrayMean := Mean(ArrayX);
	var ArrayLen := Len(ArrayX);
	var s := StdDev(ArrayX);

	i := ArrayMean - mu;
	n := s / Sqr(ArrayLen);

	t := i/n;

	return := t;

endfunction

function BitIn(ArrayX)

// Takes an array of 8 bits and converts them into an integer.
// This is used for the flag support.  IMPORTANT: bit positon is
// *NOT* the same thing as the array position!   The first bit in 
// the flag is the 8th bit in the array, and vice versa.  This is
// because it processes the bit values in the following order:
// 128, 64, 32, 16, 8, 4, 2, 1.

	var i := 8;
	var n;
	var loop := 1;
	var bitvalue := 0;

	While (i >= 0)

		n := 8 - i;
		if (ArrayX[i] = 1)
			bitvalue := bitvalue + Exp(2, n);
		endif
		i := i - 1;

	endwhile

	return bitvalue;

endfunction


function BitOut(x)

// This function takes an unsigned, short integer (0-255) and returns
// an array containing the individual bit values.   IMPORTANT: the 
// first element of the array is NOT the first bit, it is the 8th
// bit, and so on down.   See the desc for BitIn for the details.

	x := Cint(x);
	var loop := 1;
	var bitvalue := 0;
	var ArrayX := {0,0,0,0,0,0,0,0};
	var n;

	while (loop <= 8)

		n := 8 - loop;
		bitvalue := Exp(2, n);
		if ( (x - bitvalue) >= 0)
			ArrayX[loop] := 1;
			x := x - bitvalue;
		endif
		loop := loop + 1;

	endwhile

	return ArrayX;

endfunction


function BitAnd(x, y)

// Performs a bitwise logical conjunction for two unsigned, short
// integers (0-255).  If both values for a position are set, then
// that position in the result is set.   If either are reset, then
// the position in the result is reset.

	var ArrayX := BitOut(x);
	var ArrayY := BitOut(y);
	var z := 0;
	var loop := 1;
	var n;

	while (loop <= 8)
	
		n := loop - 1;
		if ( (ArrayX[loop] = 1) AND (ArrayY[loop] = 1) )
			z := z + Exp(2, n);
		endif
		loop := loop + 1;

	endwhile

	return z;

endfunction


function BitOr(x, y)

// Performs a bitwise logical disjunction for two unsigned, short
// integers (0-255).  If either value (or both values) for a 
// position are set, then that position in the result is set.   If 
// both are reset, then the position in the result is reset.

	var ArrayX := BitOut(x);
	var ArrayY := BitOut(y);
	var z := 0;
	var loop := 1;
	locan n;

	while (loop <= 8)

		n := loop - 1;
		if ( (ArrayX[loop] = 1) OR (ArrayY[loop] = 1) )
			z := z + Exp(2, n);
		endif
		loop := loop + 1;

	endwhile

	return z;

endfunction


function BitXor(x, y)

// Performs a bitwise logical exclusion for two unsigned, short
// integers (0-255).  If one and ONLY one value for a position is
// set, then that position in the result is set.   If both are 
// either set or reset, then the position in the result is reset.

	var ArrayX := BitOut(x);
	var ArrayY := BitOut(y);
	var z := 0;
	var loop := 1;
	var n;

	while (loop <= 8)

		n := loop - 1;
		if (ArrayX[loop] - ArrayY[loop] <> 0)
			z := z + Exp(2, n);
		endif
		loop := loop + 1;

	endwhile

	return z;

endfunction


function BitEqv(x, y)

// Performs a bitwise logical equivalence for two unsigned, short
// integers (0-255).  If one and ONLY one value for a position is
// set, then that position in the result is reset.   If both are 
// either set or reset, then the position in the result is set.

	var ArrayX := BitOut(x);
	var ArrayY := BitOut(y);
	var z := 0;
	var loop := 1;
	var n;

	while (loop <= 8)

		n := loop - 1;
		if (ArrayX[loop] - ArrayY[loop] = 0)
			z := z + Exp(2, n);
		endif
		loop := loop + 1;

	endwhile

	return z;

endfunction


function BitImp(x, y)

// Performs a bitwise logical implication for two unsigned, short
// integers (0-255).  If the a position in the first integer is set,
// and the corresponding position for the second integer is reset,
// then that position in the result is reset.  All other combinations
// will result in the value being set.

	var ArrayX := BitOut(x);
	var ArrayY := BitOut(y);
	var z := 0;
	var loop := 1;
	var n;

	while (loop <= 8)

		n := loop - 1;
		if (ArrayX[loop] - ArrayY[loop] < 1)
			z := z + Exp(2, n);
		endif
		loop := loop + 1;

	endwhile

	return z;

endfunction


function BitNot(x)

// Performs a logical negation on a short, unsigned integer (0-255).
// If a position in the integer is set, then that position in the
// result is reset.  Conversely, if a position in the integer is 
// reset, then that position in the result is set.

	var ArrayX := BitOut(x);
	var z := 0;
	var loop := 1;
	var n;

	while (loop <= 8)

		n := loop - 1;
		if (ArrayX[loop] = 0)
			ArrayX[loop] := 1;
			z := z + Exp(2, n);
		else
			ArrayX[loop] := 0;
		endif

		loop := loop + 1;

	endwhile

	return z;

endfunction


function FlagSet(flag, bit);

// This function will set the <bit> position in the <flag> register.
// Both <flag> and <bit> are integers.

	bit := 9 - bit;
	
	var FlagArray := BitOut(flag);
	FlagArray[bit] := 1;

	bit := BitIn(FlagArray);

	return bit;

endfunction


function FlagReset(flag, bit);

// This function will reset the <bit> position in the <flag> register.
// Both <flag> and <bit> are integers.

	bit := 9 - bit;
	
	var FlagArray := BitOut(flag);
	FlagArray[bit] := 0;

	bit := BitIn(FlagArray);

	return bit;

endfunction


function FlagCheck(flag, bit);

// This function will check the flag in the <bit> position of the
// <flag> register.  It will return a 1 if the flag is set, and a 0
// if the flag is reset.  Both <flag> and <bit> are integers.

	var FlagArray := BitOut(flag);
	bit := 9 - bit;

	if (FlagArray[bit] = 1)
		return 1;
	else
		return 0;
	endif

endfunction
