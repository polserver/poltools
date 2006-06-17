//
// bank.inc: global bankbox functions
//
// public functions:
//      FindBankBox( character)      returns World Bank bankbox for a character
//      WipeBankBox( character )     does what you might think.
//      OpenWorldBank()              returns handle to World Bank.
//

use os;

include "include/objtype";

function FindBankBox( character )
  var worldbank := OpenWorldBank();
  var bank_obj_name := "Bankbox of " + character.serial;
  var bankbox := FindRootItemInStorageArea( worldbank, bank_obj_name );
  if (!bankbox)
    bankbox := CreateRootItemInStorageArea( worldbank, bank_obj_name, UOBJ_BANKBOX );
  endif
  if(!bankbox)
    syslog( "Unable to find or create bankbox for " + character.serial );
  endif
  return bankbox;
endfunction

function WipeBankBox( character )
  var worldbank := OpenWorldBank();
  var bank_obj_name := "Bankbox of " + character.serial;
  return DestroyRootItemInStorageArea( worldbank, bank_obj_name );
endfunction

function OpenWorldBank()
  var bank := FindStorageArea( "World Bank" );
  if (!bank)
    bank := CreateStorageArea( "World Bank" );
  endif
  if (!bank)
    syslog( "Unable to open or create world bank!" );
  endif
  return bank;
endfunction