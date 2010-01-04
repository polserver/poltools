///////////////////
//  This function returns a dictionary of all foodstuffs used in cooking.  the dictionary is used to look
//  up the objtypes of names given to food.  it's used to make defining recipes easier.
///////////////////

function define_food_dictionary ()
	var food := dictionary;

	//some tools used in cooking
	food [ "ITEM_MIXING_BOWL" ]		:= 0x15f8;
	food [ "ITEM_ROLLING_PIN" ]		:= 0x1043;
	food [ "ITEM_FRYING_PAN" ]		:= 0x097f;
	food [ "ITEM_EMPTY_PITCHER" ]		:= 0x09a7;
	food [ "ITEM_EMPTY_GLASS" ]		:= 0x1f81;
	food [ "ITEM_SPIT" ]		        := 0x1e96;
	food [ "ITEM_LARGEWOODEN" ]	        := 0x1605;
 
	//bottles, pitchers and glasses of liquid
	food [ "ITEM_BOTTLE_OF_LIQUOR" ]	:= 0x099B;
	food [ "ITEM_BOTTLE_OF_ALE" ]		:= 0x099F;
	food [ "ITEM_BOTTLE_OF_WINE" ]	:= 0x09C7;
	food [ "ITEM_JUG_OF_CIDER" ]		:= 0x09c8;

	food [ "ITEM_PITCHER_OF_LIQUOR" ]	:= 0x1f9a;
	food [ "ITEM_PITCHER_OF_ALE" ]	:= 0x1f96;
	food [ "ITEM_PITCHER_OF_WINE" ]	:= 0x1f9c;
	food [ "ITEM_PITCHER_OF_CIDER" ]	:= 0x1f98;
	food [ "ITEM_PITCHER_OF_WATER" ]	:= 0x1f9e;
	food [ "ITEM_PITCHER_OF_MILK" ]	:= 0x09ad;

	food [ "ITEM_GLASS_OF_LIQUOR" ]	:= 0x1F85;
	food [ "ITEM_GLASS_OF_ALE" ]		:= 0x09EE;
	food [ "ITEM_GLASS_OF_WINE" ]		:= 0x1F8D;
	food [ "ITEM_GLASS_OF_CIDER" ]	:= 0x1F7D;
	food [ "ITEM_GLASS_OF_WATER" ]	:= 0x1F91;
	food [ "ITEM_GLASS_OF_MILK" ]		:= 0x1F89;

	//These items are special.  If they're called for in a recipe, it will call a special function that will
	//permit multiple forms of that item
	food [ "ITEM_GENERAL_WATER" ]		:= 0x1f9e;
	food [ "ITEM_GENERAL_MILK" ]		:= 0x09ad;
	food [ "ITEM_GENERAL_CHEESE" ]	:= 0x097d;

	//primary food ingredients
	food [ "ITEM_FLOUR" ]			:= 0x1045;
	food [ "ITEM_CORNMEAL" ]			:= 0xc800;
	food [ "ITEM_RAW_RIB" ]			:= 0x09f1;
	food [ "ITEM_RAW_FISHSTEAK" ]		:= 0x097a;
	food [ "ITEM_RAW_BIRD" ]			:= 0x09b9;
	food [ "ITEM_RAW_HAM" ]			:= 0x09c9;
	food [ "ITEM_BACON_SLAB" ]		:= 0x0976;
	food [ "ITEM_RAW_SAUSAGE" ]		:= 0x09c0;
	food [ "ITEM_GREEN_FISH" ]		:= 0x09cc;
	food [ "ITEM_BROWN_FISH" ]		:= 0x09cd;
	food [ "ITEM_BLUE_FISH" ]		:= 0x09ce;
	food [ "ITEM_YELLOW_FISH" ]		:= 0x09cf;
	food [ "ITEM_LARD" ]			:= 0x1e8a;
	food [ "ITEM_EGG" ]				:= 0x09b5;
	food [ "ITEM_GARLIC" ]			:= 0x0f84;
	food [ "ITEM_HONEY" ]			:= 0x09ec;
	food [ "ITEM_CARROT" ]			:= 0x0c77;
	food [ "ITEM_APPLE" ]			:= 0x09d0;
	food [ "ITEM_PEACH" ]			:= 0x172c;
	food [ "ITEM_LEMON" ]			:= 0x1728;
	food [ "ITEM_LIME" ]			:= 0x172a;
	food [ "ITEM_BANANA" ]			:= 0x171f;
	food [ "ITEM_PEAR" ]			:= 0x172d;
	food [ "ITEM_GRAPES" ]			:= 0x0d1a;
	food [ "ITEM_DATES" ]			:= 0x1727;
	food [ "ITEM_PUMPKIN" ]			:= 0x0c6a;
	food [ "ITEM_YELLOW_GOURD" ]		:= 0x0c65;
	food [ "ITEM_GREEN_GOURD" ]		:= 0x0c66;
	food [ "ITEM_SQUASH" ]			:= 0x0c72;
	food [ "ITEM_WATERMELON" ]		:= 0x0c5c;
	food [ "ITEM_HONEYDEW_MELON" ]	        := 0x0c75;
	food [ "ITEM_CANTELOUPE" ]		:= 0x0c7a;
	food [ "ITEM_LETTUCE" ]			:= 0x0c70;
	food [ "ITEM_CABBAGE" ]			:= 0x0c7c;
	food [ "ITEM_ONION" ]			:= 0x0c6d;
	food [ "ITEM_TURNIP" ]			:= 0x0d3a;
	food [ "ITEM_COCONUT" ]			:= 0x1726;
	food [ "ITEM_RAW_CHICKEN_LEG" ]	        := 0x1607;
	food [ "ITEM_RAW_LEG_OF_LAMB" ]	        := 0x1609;
	food [ "ITEM_SKINNEDRABBIT" ]		:= 0x1e93;
 
	//products that are used as ingredients in other recipes
	food [ "ITEM_BALL_OF_DOUGH" ]		:= 0xc805;
	food [ "ITEM_SWEET_DOUGH" ]		:= 0xc801;
	food [ "ITEM_CAKE_BATTER" ]		:= 0xc802;
	food [ "ITEM_PIZZA_CRUST" ]		:= 0xc803;
	food [ "ITEM_PIE_CRUST" ]		:= 0xc804;
	food [ "ITEM_BATTER" ]			:= 0xc806;
	food [ "ITEM_COOKED_BIRD" ]		:= 0x09b7;
	return food;
endfunction




///////////////////
//  This function returns a dictionary of all foodstuffs used in cooking relating them to their names.  its used in
//  the cookbook script to translate names
///////////////////

function define_food_name_dictionary ()
	var food := dictionary;

	//some tools used in cooking
	food [ "ITEM_MIXING_BOWL" ]		:= "mixing bowl";
	food [ "ITEM_ROLLING_PIN" ]		:= "rolling pin";
	food [ "ITEM_FRYING_PAN" ]		:= "frying pan";
	food [ "ITEM_EMPTY_PITCHER" ]		:= "empty pitcher";
	food [ "ITEM_EMPTY_GLASS" ]		:= "empty glass";
	food [ "ITEM_SPIT" ]		        := "a spit";
	food [ "ITEM_LARGEWOODEN" ]	        := "large wooden bowl";
 
	//bottles, pitchers and glasses of liquid
	food [ "ITEM_BOTTLE_OF_LIQUOR" ]	:= "bottle of liquor";
	food [ "ITEM_BOTTLE_OF_ALE" ]		:= "bottle of ale";
	food [ "ITEM_BOTTLE_OF_WINE" ]	:= "bottle of wine";
	food [ "ITEM_JUG_OF_CIDER" ]		:= "jug of cider";

	food [ "ITEM_PITCHER_OF_LIQUOR" ]	:= "pitcher of liquor";
	food [ "ITEM_PITCHER_OF_ALE" ]	:= "pitcher of ale";
	food [ "ITEM_PITCHER_OF_WINE" ]	:= "pitcher of wine";
	food [ "ITEM_PITCHER_OF_CIDER" ]	:= "pitcher of cider";
	food [ "ITEM_PITCHER_OF_WATER" ]	:= "pitcher of water";
	food [ "ITEM_PITCHER_OF_MILK" ]	:= "pitcher of milk";

	food [ "ITEM_GLASS_OF_LIQUOR" ]	:= "glass of liquor";
	food [ "ITEM_GLASS_OF_ALE" ]		:= "glass of ale";
	food [ "ITEM_GLASS_OF_WINE" ]		:= "glass of wine";
	food [ "ITEM_GLASS_OF_CIDER" ]	:= "glass of cider";
	food [ "ITEM_GLASS_OF_WATER" ]	:= "glass of water";
	food [ "ITEM_GLASS_OF_MILK" ]		:= "glass of milk";

	//These items are special.  If they're called for in a recipe, it will call a special function that will
	//permit multiple forms of that item
	food [ "ITEM_GENERAL_WATER" ]		:= "cup of water";
	food [ "ITEM_GENERAL_MILK" ]		:= "cup of milk";
	food [ "ITEM_GENERAL_CHEESE" ]	:= "wedge of cheese";

	//primary food ingredients
	food [ "ITEM_FLOUR" ]			:= "cup of flour";
	food [ "ITEM_CORNMEAL" ]			:= "cup of cornmeal";
	food [ "ITEM_RAW_RIB" ]			:= "raw rib";
	food [ "ITEM_RAW_FISHSTEAK" ]		:= "raw fishsteak";
	food [ "ITEM_RAW_BIRD" ]			:= "raw bird";
	food [ "ITEM_RAW_HAM" ]			:= "raw ham";
	food [ "ITEM_BACON_SLAB" ]		:= "bacon slab";
	food [ "ITEM_RAW_SAUSAGE" ]		:= "raw sausage";
	food [ "ITEM_GREEN_FISH" ]		:= "green fish";
	food [ "ITEM_BROWN_FISH" ]		:= "brown fish";
	food [ "ITEM_BLUE_FISH" ]		:= "blue fish";
	food [ "ITEM_YELLOW_FISH" ]		:= "yellow fish";
	food [ "ITEM_LARD" ]			:= "glob of lard";
	food [ "ITEM_EGG" ]				:= "egg";
	food [ "ITEM_GARLIC" ]			:= "clove of garlic";
	food [ "ITEM_HONEY" ]			:= "honey";
	food [ "ITEM_CARROT" ]			:= "carrot";
	food [ "ITEM_APPLE" ]			:= "apple";
	food [ "ITEM_PEACH" ]			:= "peach";
	food [ "ITEM_LEMON" ]			:= "lemon";
	food [ "ITEM_LIME" ]			:= "lime";
	food [ "ITEM_BANANA" ]			:= "banana";
	food [ "ITEM_PEAR" ]			:= "pear";
	food [ "ITEM_GRAPES" ]			:= "bunch of grapes";
	food [ "ITEM_DATES" ]			:= "dates";
	food [ "ITEM_PUMPKIN" ]			:= "pumpkin";
	food [ "ITEM_YELLOW_GOURD" ]		:= "yellow gourd";
	food [ "ITEM_GREEN_GOURD" ]		:= "green gourd";
	food [ "ITEM_SQUASH" ]			:= "squash";
	food [ "ITEM_WATERMELON" ]		:= "watermelon";
	food [ "ITEM_HONEYDEW_MELON" ]	:= "honeydew melon";
	food [ "ITEM_CANTELOUPE" ]		:= "canteloupe";
	food [ "ITEM_LETTUCE" ]			:= "head of lettuce";
	food [ "ITEM_CABBAGE" ]			:= "head of cabbage";
	food [ "ITEM_ONION" ]			:= "onion";
	food [ "ITEM_TURNIP" ]			:= "turnip";
	food [ "ITEM_COCONUT" ]			:= "coconut";
	food [ "ITEM_RAW_CHICKEN_LEG" ]	:= "raw chicken leg";
	food [ "ITEM_RAW_LEG_OF_LAMB" ]	:= "raw leg of lamb";
	food [ "ITEM_SKINNEDRABBIT" ]		:= "a skinned rabbit";

	//products that are used as ingredients in other recipes
	food [ "ITEM_BALL_OF_DOUGH" ]		:= "ball of dough";
	food [ "ITEM_SWEET_DOUGH" ]		:= "sweet dough";
	food [ "ITEM_CAKE_BATTER" ]		:= "cake batter";
	food [ "ITEM_PIZZA_CRUST" ]		:= "pizza crust";
	food [ "ITEM_PIE_CRUST" ]		:= "pie crust";
	food [ "ITEM_BATTER" ]			:= "batter";
	food [ "ITEM_COOKED_BIRD" ]		:= "a cooked bird";

	return food;
endfunction
///////////////////
//  This function searches for water in the player's backpack.  It starts by looking for a glass of water.  If that's
//  not found, it then looks for a pitcher of water
///////////////////

function special_find_water (character, amount)
	var found_ingredient;

	//first check to see if amount is a positive number
	if (amount < 0)
		return 0;

	//then check to see if only 1 unit of water is needed.  If so, we can check for it a much faster way
	elseif (amount = 1 or amount = 0)
		//first try searching for a glass of water
		var found_ingredient := FindObjtypeInContainer (character.backpack, food_dictionary ["ITEM_GLASS_OF_WATER"] );
		if (found_ingredient)
			if (!ReserveItem(found_ingredient)) return 0; endif
			return 1;
		endif

		//otherwise try looking for a pitcher of water
		found_ingredient := FindObjtypeInContainer (character.backpack, food_dictionary ["ITEM_PITCHER_OF_WATER"] );
		if (found_ingredient)
			if (!ReserveItem(found_ingredient)) return 0; endif
			return 1;
		endif

	//if more than 1 unit of water is needed, we'll have to do it a slower way
	else
		var water_count := 0;
		foreach item in enumerateitemsincontainer(character.backpack)
			if (item.objtype = food_dictionary ["ITEM_GLASS_OF_WATER"])
				water_count := water_count + 1;
				if (!ReserveItem(item)) return 0; endif
			elseif (item.objtype = food_dictionary ["ITEM_PITCHER_OF_WATER"])
				if ( GetObjProperty (item, "fullness") )
					water_count := water_count + GetObjProperty (item, "fullness");
				else
					water_count := water_count + 4;
				endif
				if (!ReserveItem(item)) return 0; endif
			endif
			if (water_count >= amount)
				return 1;
			endif
		endforeach
	endif

	//if we still haven't found what we're looking for, fail
	return 0;

endfunction




///////////////////
//  similar to the previous function, this does pretty much the same thing with milk (doesn't yet do multiples)
///////////////////

function special_find_milk (character)

	//first try searching for a glass of milk
	var found_ingredient := FindObjtypeInContainer (character.backpack, food_dictionary ["ITEM_GLASS_OF_MILK"] );
	if (found_ingredient)
		if (!ReserveItem(found_ingredient)) return 0; endif
		return 1;
	endif

	//otherwise try looking for a pitcher of water
	found_ingredient := FindObjtypeInContainer (character.backpack, food_dictionary ["ITEM_PITCHER_OF_MILK"] );
	if (found_ingredient)
		if (!ReserveItem(found_ingredient)) return 0; endif
		return 1;
	endif

	//otherwise fail
	return 0;

endfunction




///////////////////
//  again for cheese
///////////////////

function special_find_cheese (character, amount)

	//count of all cheese found;
	var cheese_count := 0;

	//search for a wheel of cheese
	var found_ingredient := FindObjtypeInContainer (character.backpack, 0x97e );
	if (found_ingredient)
		if (!ReserveItem(found_ingredient)) return 0; endif
		cheese_count := found_ingredient.amount * 3;
	endif

	//then look for a large wedge of cheese
	found_ingredient := FindObjtypeInContainer (character.backpack, 0x97d );
	if (found_ingredient)
		if (!ReserveItem(found_ingredient)) return 0; endif
		cheese_count := cheese_count + found_ingredient.amount * 2;
	endif

	//then look for a small wedge of cheese
	found_ingredient := FindObjtypeInContainer (character.backpack, 0x97c );
	if (found_ingredient)
		if (!ReserveItem(found_ingredient)) return 0; endif
		cheese_count := cheese_count + found_ingredient.amount;
	endif

	//now check to see if enough cheese was found
	if (cheese_count >= amount)
		return 1;
	else
		return 0;
	endif

endfunction




///////////////////
//  This function searches the player's backpack for water and destroys it.
///////////////////

function special_destroy_water (character, amount)
	var found_ingredient;
	var i := 0;

	//check to make sure amount is at least 1
	if (amount <= 0)
		return;
	endif

	repeat
		//first see if they have a glass of water
		found_ingredient := FindObjtypeInContainer (character.backpack, food_dictionary ["ITEM_GLASS_OF_WATER"] );
		if (found_ingredient)
			//if so, create an empty glass
			var the_glass := CreateItemInBackpack (character, food_dictionary ["ITEM_EMPTY_GLASS"] );

			//if the full glass has a saved name, rename the new glass
			if (GetObjProperty (found_ingredient, "EmptyName") )
				the_glass.name := GetObjProperty (found_ingredient, "EmptyName");
			endif

			//destroy the drink
			SubtractAmount( found_ingredient, 1 );

			i :=  i + 1;

		else
			//otherwise try looking for a pitcher of water
			found_ingredient := FindObjtypeInContainer (character.backpack, food_dictionary ["ITEM_PITCHER_OF_WATER"] );
			if (found_ingredient)

				//if we found one, reduce the amount of water in the pitcher by 1
				reduce_container_fullness (character, found_ingredient);

				i :=  i + 1;

			//emergency escape exit.  if it couldn't find a pitcher or a glass of water and it still needed more.
			//this should prevent an infinite loop.  Since the water items were already found and reserved, 
			//this really shouldn't happen.  Better safe than sorry...
			else
				return;
			endif

		endif
	until (i >= amount);
endfunction




///////////////////
//  Once again similar to the previous function, this one used up milk
///////////////////

function special_destroy_milk (character)

	//first see if they have a glass of milk
	var found_ingredient := FindObjtypeInContainer (character.backpack, food_dictionary ["ITEM_GLASS_OF_MILK"] );
	if (found_ingredient)
		//if so, create an empty glass
		var the_glass := CreateItemInBackpack (character, food_dictionary ["ITEM_EMPTY_GLASS"] );

		//if the full glass has a saved name, rename the new glass
		if (GetObjProperty (found_ingredient, "EmptyName") )
			the_glass.name := GetObjProperty (found_ingredient, "EmptyName");
		endif

		//destroy the drink
		SubtractAmount( found_ingredient, 1 );
		return;

	endif

	//otherwise try looking for a pitcher of milk
	found_ingredient := FindObjtypeInContainer (character.backpack, food_dictionary ["ITEM_PITCHER_OF_MILK"]);
	if (found_ingredient)
		//if we found one, reduce the amount of water in the pitcher by 1
		reduce_container_fullness (character, found_ingredient);
	endif
endfunction




///////////////////
//  again for cheese
///////////////////

function special_destroy_cheese (character, amount)

	repeat
		//start with a small wedge of cheese
		var found_ingredient := FindObjtypeInContainer (character.backpack, 0x97c );
		if (found_ingredient)
			SubtractAmount (found_ingredient, 1);
			amount := amount - 1;

		//search for a large wedge of cheese
		else
			found_ingredient := FindObjtypeInContainer (character.backpack, 0x97d );
			if (found_ingredient)
				SubtractAmount (found_ingredient, 1);
				CreateItemInBackpack (character, 0x97c);
				amount := amount - 1;

			//then look for a wheel of cheese
			else
				found_ingredient := FindObjtypeInContainer (character.backpack, 0x97e );
				if (found_ingredient)
					SubtractAmount (found_ingredient, 1);
					CreateItemInBackpack (character, 0x97d);
					amount := amount - 1;
				endif
			endif
		endif
	until (amount <= 0);
endfunction




///////////////////
//  This function decreases the amount of liquid in a container, such as a bottle or pitcher.  If the container
//  is empty, it destroys the container.  If the item has the property return_type defined, one of that item is
//  then created
///////////////////

function reduce_container_fullness (character, container)
	//get how full the container is
	var fullness := GetObjProperty (container, "fullness");

	//load some data about the container from itemdesc
	var cfgfile := ReadConfigFile( "itemdesc" );
	var elem := FindConfigElem( cfgfile, container.objtype );
	var drink_name := GetConfigString( elem, "drink_name" );
	var return_type := GetConfigInt (elem, "return_type");

	//if the container has no fullness, that means its new.  Give it 3 more uses and rename it
	if (! fullness)
		SetObjProperty (container, "fullness", 3);
		container.name := "a mostly full " + drink_name;

	//otherwise, reduce it's fullness
	elseif (fullness = 3)
		SetObjProperty (container, "fullness", 2);
		container.name := "half a " + drink_name;
	elseif (fullness = 2)
		SetObjProperty (container, "fullness", 1);
		container.name := "an almost empty " + drink_name;

	//if it has no more uses left, return something,  if applicable, and destroy the item
	else
		if (return_type)
			var empty_container := CreateItemInBackpack (character, return_type );

			//if the full container has a saved name, rename the new container
			if (GetObjProperty (container, "EmptyName") )
				empty_container.name := GetObjProperty (container, "EmptyName");
			endif
		endif
		destroyitem (container);
	endif
endfunction
