use uo;



function YesNo(me, prompt)



	var layout := array(

                        "page 0",

			"nodispose",

                        "gumppic 0 0 2070",

                        "button 30 75 2130 2129 1 0 1",

                        "button 90 75 2119 2120 1 0 0",

                        "text 40 30 502 0",

                       

	);



	var data := array(  prompt  );

          

	var res := SendDialogGump( me, layout, data );



	if (res)

		return 1;

	else

		return 0;

	endif



endfunction
