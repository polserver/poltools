//////////////////////////////////////////////
//
//   ErrorMessages Include v1.0 (2001/01/07)
//
//     Author: Shinigami
//     eMail : Shinigami@gmx.net
//
//   Modifications:
//
//////////////////////////////////////////////
//
// Usage:
//   u've to declare following Constants in your Script
//     ERRORMSGS_GERMAN
//     ERRORMSGS_ENGLISH
//     (This include supports 2 Languages now - u can enhance this.)
//
//   Examples
//     Const ERRORMSGS_GERMAN:=
//       "{}MRCSpawn: Das Item %% konnte nicht erzeugt werden!"+
//       "{}MRCSpawn: Das Item %% in der Gruppe '%%' ist fehlerhaft definiert!";
//     Const ERRORMSGS_ENGLISH:=
//       "{}MRCSpawn: Unable to create Item %% !"+
//       "{}MRCSpawn: Items %% in Group '%%' is definied in a wrong way!";
//
//     // "{}" divides the Constant String into SubStrings
//     // "%%" is a Placeholder for a Parameter
//
//   Call-Examples
//     PrintErrorMessage(1, {Hex(123)});
//     PrintErrorMessage(2, {Hex(123), "BlahGroup"});
//     PrintErrorMessage(1, {Hex(123)}, player);

//////////////
// Constants
//////////////

Const MSGLANGUAGE:=0; // 1==German, 0==English

/////////////////////////////////////////////////////////////////
// PrintErrorMessage - Prints the ErrorMessage in your Language
/////////////////////////////////////////////////////////////////
//
// msgid  - SubString # inside ConstantString
// params - to replace "%%"
// who    - 0  = SysLog(...);
//          !0 = SendSysMessage(who, ...);

Function PrintErrorMessage(msgid, params:={ }, who:=0)
  var errormsgsraw;
  If (MSGLANGUAGE)
    errormsgsraw:=ERRORMSGS_GERMAN;
  Else
    errormsgsraw:=ERRORMSGS_ENGLISH;
  EndIf

  var errormsgs:={ };
  var endpos;

  var startpos:=Find(errormsgsraw, "{}", 1)+2;
  While (startpos>2)
    endpos:=Find(errormsgsraw, "{}", startpos);
    If (endpos)
      errormsgs.append(errormsgsraw[startpos, endpos-startpos]);
    Else
      errormsgs.append(errormsgsraw[startpos, Len(errormsgsraw)-startpos]);
    EndIf

    startpos:=endpos+2;
  EndWhile

  If (msgid<=errormsgs.size())
    var message:=errormsgs[msgid];

    var index:=1;
    startpos:=Find(message, "%%", 1);
    While (startpos And (index<=params.size()))
      message[startpos, 2]:=CStr(params[index]);

      index:=index+1;
      startpos:=Find(message, "%%", 1);
    EndWhile

    If (Find(message, "%%", 1))
      If (MSGLANGUAGE)
        SysLog("ErrorMsg: Zu wenige Parameter uebergeben!");
      Else
        SysLog("ErrorMsg: Not enough Params passed!");
      EndIf
    EndIf

    If (who)
      SendSysMessage(who, message, _DEFAULT_TEXT_FONT, 38);
    Else
      SysLog(message);
    EndIf
  Else
    If (MSGLANGUAGE)
      SysLog("ErrorMsg: In '"+errormsgsraw+"' gibt es keine "+msgid+". Meldung!");
    Else
      SysLog("ErrorMsg: In '"+errormsgsraw+"' isn't definied a "+msgid+". Message!");
    EndIf
  EndIf
EndFunction
