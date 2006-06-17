include "include/client";
include "include/objtype";

function IsLocked(me, door, options := "no", closed := "no")
  const KEYSTART := 0x100e;
  const KEYEND := 0x1013;
  const KEYRING := 0x1011;
    var doorlockid;
    var keylockid;
    var insidering;
    if((!door.locked) || ((GetObjProperty(door,"WhenOpened")) && (closed == "yes")))
      return 0;
    else
      if(((me.multi) && (options != "no")) || (GetObjProperty(door, "UseFlow")))
        if(options == "house")
          var params := array;
          params[0] := me;
          params[1] := door;
          var innerlock := run_script_to_completion(":housing:innerLock", params);
          if(innerlock)
            return 0;
          endif
          return 1;
        endif
        if(((options == "yes") || ((options == "front") && (GetObjProperty(door, "Front")))) || (GetObjProperty(door, "UseFlow")))
          case (GetObjProperty(door, "Flow"))
            "S": if(door.y < me.y)
                   PrintTextAbovePrivate(door, "The door is locked, but you open it from the inside.", me);
                   return 0;
                 endif
            "E": if(door.x < me.x)
                   PrintTextAbovePrivate(door, "The door is locked, but you open it from the inside.", me);
                   return 0;
                 endif
            "W": if(door.x > me.x)
                   PrintTextAbovePrivate(door, "The door is locked, but you open it from the inside.", me);
                   return 0;
                 endif
            "N": if(door.y > me.y)
                   PrintTextAbovePrivate(door, "The door is locked, but you open it from the inside.", me);
                   return 0;
                 endif
          endcase
        endif
      endif
      doorlockid := GetObjProperty(door, "lockid");
      if(!doorlockid)
         return 1;
      endif
      var mykeys := EnumerateItemsInContainer(me.backpack);
      foreach key in mykeys
        if((key.objtype >= KEYSTART) && (key.objtype <= KEYEND) &&  (key.objtype != KEYRING))
          keylockid := GetObjProperty(key, "lockid");
          if((keylockid) && (keylockid == doorlockid))
            PrintTextAbovePrivate(door, "You quickly unlock, use and re-lock the door", me);
            return 0;
          endif
        elseif(key.objtype == KEYRING)
          insidering := FindMyPack(key.serial);
          foreach otherkey in EnumerateItemsInContainer(insidering)
          keylockid := GetObjProperty( otherkey, "lockid" );
          if(keylockid == doorlockid)
            PrintTextAbovePrivate( door, "You quickly unlock, use and re-lock the door", me);
            return 0;
          endif
        endforeach
      endif
    endforeach
  endif
  return 1;
endfunction

function OpenTamedStorageAreas()
  var bank := FindStorageArea("Tamed Storage");
  if(!bank)
    bank := CreateStorageArea("Tamed Storage");
  endif
  return bank;
endfunction

function FindMyPack(myserial)
  var mybank := OpenTamedStorageAreas();
  var bank_obj_name := "Bankbox  " + Hex(myserial);
  var bankbox := FindRootItemInStorageArea(mybank, bank_obj_name);
  if(!bankbox)
     bankbox := CreateRootItemInStorageArea(mybank, bank_obj_name, UOBJ_BANKBOX);
  endif
  return bankbox;
endfunction