////////////////////////////////////////////////////////////////
//
//	CONSTANTS
//
////////////////////////////////////////////////////////////////

// Don't use these outside this file, use FONT_* from client.inc
const _DEFAULT_UCFONT  := 3;
const _DEFAULT_UCCOLOR := 0x3B2;

////////////////////////////////////////////////////////////////
//
//	FUNCTIONS
//
////////////////////////////////////////////////////////////////

// 'uc_text' is an Array of 2-byte integers,
//           where each integer is a Unicode character!
//
// 'langcode' is a 3-character "Originating Language" code
//       e.g. ENG, ENU, CHT, DEU, FRA, JPN, RUS, KOR (etc??)

BroadcastUC(uc_text, langcode, font:=_DEFAULT_UCFONT, color:=_DEFAULT_UCCOLOR);
PrintTextAboveUC(above_object, uc_text, langcode, font:=_DEFAULT_UCFONT, color:=_DEFAULT_UCCOLOR); 
PrintTextAbovePrivateUC(above_object, uc_text, langcode, character, font:=_DEFAULT_UCFONT, color:=_DEFAULT_UCCOLOR);
SendSysMessageUC(character, uc_text, langcode, font:=_DEFAULT_UCFONT, color:=_DEFAULT_UCCOLOR);

RequestInputUC(character, item, uc_prompt, langcode); // item is a placeholder, pass anything
