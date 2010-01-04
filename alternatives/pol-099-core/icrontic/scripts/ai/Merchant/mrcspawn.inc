use cfgfile;

var mrcspawn_cfg := ReadConfigFile( "mrcspawn" );


function RestockInventory( merchanttype, byref inventory )
    foreach productgroup in ProductGroups(merchanttype)
        Restock( inventory, productgroup );
    endforeach
endfunction

function ProductGroups( mtype )
    var mtype_elem := FindConfigElem( mrcspawn_cfg, mtype );
    if (!mtype_elem)
        syslog( "Unable to find config elem " + mtype );
        return 0;
    endif
    return GetConfigStringArray( mtype_elem, "Sells" );
endfunction

function Restock( byref inventory, productgroup )
    var pg_elem := FindConfigElem( mrcspawn_cfg, productgroup );
    foreach itemstr in GetConfigStringArray( pg_elem, "Item" )
        sleepms(100);
        SpawnItem( inventory, itemstr );
    endforeach
endfunction

function SpawnItem( byref inventory, itemstr )
    var ii := SplitWords( itemstr );
    var objtype := Cint(ii[1]);
    var count   := CInt(ii[2]);

    count := count - CountExisting( inventory, objtype );

    if (count > 0)
      CreateItemInInventory( inventory, objtype, count);
    endif

    If (Count < 0)
      ConsumeSubstance( inventory, objtype, (count*-1) );
    Endif
endfunction

// This would be nice, and fast, but this doesn't handle multiple stacks
// In particular, it doesn't handle multiple single nonstackable items.
//    local itemref := FindObjtypeInContainer( inventory, objtype );
//    if (itemref)
//        count := count - itemref.amount;
//    endif
function CountExisting( byref container, objtype )
    var count := 0;
    foreach item in EnumerateItemsInContainer( container )
        if (item.objtype = objtype)
            count := count + item.amount;
        endif
    endforeach
    return count;
endfunction


//Return an Array With All MerchantGroups
function MerchantGroups()
    var mrcgroups_elem := FindConfigElem( mrcspawn_cfg, "MerchantGroups" );
    if (!mrcgroups_elem)
        syslog( "Unable to find 'MerchantGroups' element in mrcspawn.cfg" );
        return 0;
    endif
    return GetConfigStringArray( mrcgroups_elem, "MerchantGroup" );
endfunction

function Merchants( MerchantGroup )
    var result := array;
    var mrc := array;
    mrc.+name;
    mrc.+type;

    var group_elem := FindConfigElem( mrcspawn_cfg, MerchantGroup );
    var count := GetConfigInt( group_elem, "MerchantCount" );
    syslog("mercant:GetConfigInt:mrcspawn " GetConfigInt(group_elem, "MerchantCount" ) );
    for i := 1 to count
        mrc.name := GetConfigString( group_elem, "Merchant" + i );
        mrc.type := GetConfigString( group_elem, "MerchantType"+i );
        result[i] := mrc;
    endfor

    return result;
endfunction