//include "objtype";

function OpenTamedStorageAreas()
    var bank := FindStorageArea( "Tamed Storage" );
    if (!bank)
        bank := CreateStorageArea( "Tamed Storage" );
    endif

    if (!bank)
    endif

    return bank;
endfunction

function FindMyPack(myserial)
    var mybank := OpenTamedStorageAreas();

    var bank_obj_name := "Bankbox  " + hex(myserial);

    var bankbox := FindRootItemInStorageArea( mybank, bank_obj_name );

    if (!bankbox)
        bankbox := CreateRootItemInStorageArea(mybank, bank_obj_name, UOBJ_BANKBOX  );

    endif
    
    return bankbox;

endfunction

function FindExistingPack(myserial)
    var mybank := OpenTamedStorageAreas();

    var bank_obj_name := "Bankbox  " + hex(myserial);

    var bankbox := FindRootItemInStorageArea( mybank, bank_obj_name );
    
    if (!bankbox)
        print( "I cannot find it!");
    	return 0;
    endif
    
    //print( "Deleting box " + hex(myserial) + "!");
    
    return bankbox;

endfunction

function MoveBackpackToCorpse(container)
        
	var myserial := GetObjProperty(container, "serial");
        
	if (!myserial)
		return;
	endif
	
	var mybank := OpenTamedStorageAreas();
        var bank_obj_name := "Bankbox  " + hex(myserial);
        var bankbox := FindRootItemInStorageArea( mybank, bank_obj_name );

        var mypack := bankbox;
        
        if (!mypack)
                return 0;
        endif
         if( GetObjProperty( container , "summoned" ) )
                foreach thingie in EnumerateItemsInContainer(mypack)
			if (distance(thingie,container) != 0)
			        moveitemtolocation(thingie, container.x, container.y, container.z,0);
			endif
                endforeach
                
                if(bankbox)
                  DestroyRootItemInStorageArea( mybank, bank_obj_name );
                  print( "Deleting box " + hex(myserial) + "!");
                else
        	  print( "I cannot find it!");
                endif
        
                return 0;
        else
                foreach thingie in EnumerateItemsInContainer(mypack)
			if (distance(thingie,container) != 0)
				moveitemtocontainer(thingie, container);
			endif
                endforeach
                
                if(bankbox)
                  DestroyRootItemInStorageArea( mybank, bank_obj_name );
                  print( "Deleting box " + hex(myserial) + "!");
                else
        	  print( "I cannot find it!");
                endif
                
                return 1;
        endif
endfunction
