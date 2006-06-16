!TCL=349, Danielle Elora - http://www.nightscapeonline.com
!TITLE=POL .96 ClipText File
!SORT=n
!CHARSET=DEFAULT

!TEXT=_________________
 
!

!TEXT=UO.EM FUNCTIONS
 
!
!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!
!TEXT=use uo;
use uo;
!

!TEXT=Accessible
Accessible(^!, item);
!
!TEXT=AddAmount
AddAmount(^!, amount);
!
!TEXT=AddMenuItem
AddMenuItem(^!, objtype, text);
!
!TEXT=AlterAttributeTemporaryMod
AlterAttributeTemporaryMod(^!, attrname, delta_tenths);
!
!TEXT=ApplyConstraint
ApplyConstraint(^!, configfile, propertyname, minvalue);
!
!TEXT=ApplyDamage
ApplyDamage(^!, damage);
!
!TEXT=ApplyRawDamage
ApplyRawDamage(^!, damage);
!
!TEXT=AssignRectToWeatherRegion
AssignRectToWeatherRegion(^!, xwest, ynorth, xeast, ysouth);
!
!TEXT=Attach
Attach(^!);
!
!TEXT=BaseSkillToRawSkill
BaseSkillToRawSkill(^!);
!
!TEXT=Broadcast
Broadcast(^!, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR);
!
!TEXT=CheckLineOfSight
CheckLineOfSight(object1, object2);
!
!TEXT=CheckLosAt
CheckLosAt(^!, x, y, z);
!
!TEXT=CheckSkill
CheckSkill(^!, skillid, difficulty, points);
!
!TEXT=ConsumeMana
ConsumeMana(^!, spellid);
!
!TEXT=ConsumeReagents
ConsumeReagents(^!, spellid);
!
!TEXT=ConsumeSubstance
ConsumeSubstance(^!, objtype, amount);
!
!TEXT=ConsumeVital
ConsumeVital(^!, vital, hundredths);
!
!TEXT=CoordinateDistance
CoordinateDistance(x1, y1, x2, y2);
!
!TEXT=CreateAccount
CreateAccount(^!, password, enabled);
!
!TEXT=CreateGuild
CreateGuild();
!
!TEXT=CreateItemAtLocation
CreateItemAtLocation(x, y, z, objtype, amount := 1, realm := _DEFAULT_REALM);
!
!TEXT=CreateItemCopyAtLocation
CreateItemCopyAtLocation(x, y, z, item, realm := _DEFAULT_REALM);
!
!TEXT=CreateItemInBackpack
CreateItemInBackpack(^!, objtype, amount := 1);
!
!TEXT=CreateItemInContainer
CreateItemInContainer(^!, objtype, amount := 1);
!
!TEXT=CreateItemInInventory
CreateItemInInventory(^!, objtype, amount := 1);
!
!TEXT=CreateMenu
CreateMenu(^!);
!
!TEXT=CreateMultiAtLocation
CreateMultiAtLocation(x, y, z, objtype, flags := 0, realm := _DEFAULT_REALM);
!
!TEXT=CreateNpcFromTemplate
CreateNpcFromTemplate(^!, x, y, z, override_properties := 0, realm := _DEFAULT_REALM);
!
!TEXT=CreateRootItemInStorageArea
CreateRootItemInStorageArea(^!, itemname, objtype);
!
!TEXT=CreateStorageArea
CreateStorageArea(^!);
!
!TEXT=DestroyGuild
DestroyGuild(^!);
!
!TEXT=DestroyItem
DestroyItem(^!);
!
!TEXT=DestroyMulti
DestroyMulti(^!);
!
!TEXT=DestroyRootItemInStorageArea
DestroyRootItemInStorageArea(^!, itemname);
!
!TEXT=Detach
Detach();
!
!TEXT=DisableEvents
DisableEvents(^!);
!
!TEXT=DisconnectClient
DisconnectClient(^!);
!
!TEXT=Distance
Distance(obj1, obj2);
!
!TEXT=EnableEvents
EnableEvents(^!, range := -1);
!
!TEXT=EnumerateItemsInContainer
EnumerateItemsInContainer(^!, flags := 0);
!
!TEXT=EnumerateOnlineCharacters
EnumerateOnlineCharacters();
!
!TEXT=EquipFromTemplate
EquipFromTemplate(^!, template);
!
!TEXT=EquipItem
EquipItem(^!, item);
!
!TEXT=EraseGlobalProperty
EraseGlobalProperty(^!);
!
!TEXT=EraseObjProperty
EraseObjProperty(^!, propname);
!
!TEXT=FindAccount
FindAccount(^!);
!
!TEXT=FindGuild
FindGuild(^!);
!
!TEXT=FindPath
FindObjtypeInContainer(^!, objtype);
!
!TEXT=FindPath
FindPath(x1, y1, z1, x2, y2, z2, realm := _DEFAULT_REALM, flags := FP_IGNORE_MOBILES, searchskirt := 5);
!
!TEXT=FindRootItemInStorageArea
FindRootItemInStorageArea(^!, itemname);
!
!TEXT=FindStorageArea
FindStorageArea(^!);
!
!TEXT=FindSubstance
FindSubstance(^!, objtype, amount, makeinuse := 0);
!
!TEXT=GetAmount
GetAmount(^!);
!
!TEXT=GetAttribute
GetAttribute(^!, attrname);
!
!TEXT=GetAttributeBaseValue
GetAttributeBaseValue(^!, attrname);
!
!TEXT=GetAttributeIntrinsicMod
GetAttributeIntrinsicMod(^!, attrname);
!
!TEXT=GetAttributeTemporaryMod
GetAttributeTemporaryMod(^!, attrname);
!
!TEXT=GetCommandHelp
GetCommandHelp(^!, command);
!
!TEXT=CreateStorageArea
GetCoordsInLine(x1, y1, x2, y2);
!
!TEXT=GetEquipmentByLayer
GetEquipmentByLayer(^!, layer);
!
!TEXT=GetFacing
GetFacing(from_x, from_y, to_x, to_y);
!
!TEXT=GetGlobalProperty
GetGlobalProperty(^!);
!
!TEXT=GetHarvestDifficulty
GetHarvestDifficulty(^!, x, y, tiletype, realm := _DEFAULT_REALM);
!
!TEXT=GetMenuObjTypes
GetMapInfo(x, y, realm := _DEFAULT_REALM);
!
!TEXT=GetMenuObjTypes
GetMenuObjTypes(^!);
!
!TEXT=GetMultiDimensions
GetMultiDimensions(^!);
!
!TEXT=GetObjProperty
GetObjProperty(^!, property_name);
!
!TEXT=GetObjPropertyNames
GetObjPropertyNames(^!);
!
!TEXT=GetObjtypeByName
GetObjType(^!);
!
!TEXT=GetObjtypeByName
GetObjtypeByName(^!);
!
!TEXT=GetRegionString
GetRegionString(^!, x, y, propertyname, realm := _DEFAULT_REALM);
!
!TEXT=GetSpellDifficulty
GetSpellDifficulty(^!);
!
!TEXT=GetStandingHeight
GetStandingHeight(x, y, startz, realm := _DEFAULT_REALM);
!
!TEXT=GetStandingLayers
GetStandingLayers(x, y, flags := MAPDATA_FLAG_ALL, realm := _DEFAULT_REALM);
!
!TEXT=GetWorldHeight
GetWorldHeight(x, y, realm := _DEFAULT_REALM);
!
!TEXT=GetVital
GetVital(^!, vitalname);
!
!TEXT=GetVitalMaximumValue
GetVitalMaximumValue(^!, vitalname);
!
!TEXT=GetVitalRegenRate
GetVitalRegenRate(^!, vitalname);
!
!TEXT=GrantPrivilege
GrantPrivilege(^!, privilege);
!
!TEXT=HarvestResource
HarvestResource(^!, x, y, b, n, realm := _DEFAULT_REALM);
!
!TEXT=HealDamage
HealDamage(^!, hits);
!
!TEXT=ListAccounts
ListAccounts();
!
!TEXT=ListEquippedItems
ListEquippedItems(^!);
!
!TEXT=ListGhostsNearLocation
ListGhostsNearLocation(x, y, z, range, realm := _DEFAULT_REALM);
!
!TEXT=ListGuilds
ListGuilds();
!
!TEXT=ListHostiles
ListHostiles(^!, range := 20, flags := 0);
!
!TEXT=ListItemsAtLocation
ListItemsAtLocation(x, y, z, realm := _DEFAULT_REALM);
!
!TEXT=ListItemsNearLocation
ListItemsNearLocation(x, y, z, range, realm := _DEFAULT_REALM);
!
!TEXT=ListItemsNearLocationOfType
ListItemsNearLocationOfType(x, y, z, range, objtype, realm := _DEFAULT_REALM);
!
!TEXT=ListItemsNearLocationWithFlag
ListItemsNearLocationWithFlag(x, y, z, range, flags, realm := _DEFAULT_REALM);
!
!TEXT=ListMobilesInLineOfSight
ListMobilesInLineOfSight(^!, range);
!
!TEXT=ListMobilesNearLocation
ListMobilesNearLocation(x, y, z, range, realm := _DEFAULT_REALM);
!
!TEXT=ListMobilesNearLocationEx
ListMobilesNearLocationEx(x, y, z, range, flags, realm := _DEFAULT_REALM);
!
!TEXT=ListObjectsInBox
ListObjectsInBox(x1, y1, z1, x2, y2, z2, realm := _DEFAULT_REALM);
!
!TEXT=ListMultisInBox
ListMultisInBox(x1, y1, z1, x2, y2, z2, realm := _DEFAULT_REALM);
!
!TEXT=ListStaticsInBox
ListStaticsInBox(x1, y1, z1, x2, y2, z2, flags := 0, realm := _DEFAULT_REALM);
!
!TEXT=ListStaticsAtLocation
ListStaticsAtLocation(x, y, z, flags := 0, realm := _DEFAULT_REALM);
!
!TEXT=ListStaticsNearLocation
ListStaticsNearLocation(x, y, z, range, flags := 0, realm := _DEFAULT_REALM);
!
!TEXT=MoveCharacterToLocation
MoveCharacterToLocation(^!, x, y, z, flags := 0);
!
!TEXT=MoveItemToContainer
MoveItemToContainer(^!, container, x := -1, y := -1);
!
!TEXT=MoveItemToLocation
MoveItemToLocation(^!, x, y, z, flags := 0);
!
!TEXT=MoveItemToSecureTradeWin
MoveItemToSecureTradeWin(^!, who);
!
!TEXT=MoveObjectToLocation
MoveObjectToLocation(^!, x, y, z, realm := _DEFAULT_REALM, flags := MOVEOBJECT_NORMAL);
!
!TEXT=MoveObjectToRealm
MoveObjectToRealm(^!, realm, x, y, z, flags := 0);
!
!TEXT=OpenPaperdoll
OpenPaperdoll(^!, forwhom);
!
!TEXT=PerformAction
PerformAction(^!, action);
!
!TEXT=PlayLightningBoltEffect
PlayLightningBoltEffect(^!);
!
!TEXT=PlayMovingEffect
PlayMovingEffect(source, target, effect, speed, loop := 0, explode := 0);
!
!TEXT=PlayMovingEffectXYZ
PlayMovingEffectXYZ(srcx, srcy, srcz, dstx, dsty, dstz, effect, speed, loop := 0, explode := 0, realm := _DEFAULT_REALM);
!
!TEXT=PlayObjectCenteredEffect
PlayObjectCenteredEffect(center, effect, speed, loop := 0);
!
!TEXT=PlaySoundEffect
PlaySoundEffect(^!, effect);
!
!TEXT=PlaySoundEffectPrivate
PlaySoundEffectPrivate(^!, effect, playfor);
!
!TEXT=PlayStationaryEffect
PlayStationaryEffect(x, y, z, effect, speed, loop := 0, explode := 0, realm := _DEFAULT_REALM);
!
!TEXT=POLCore
POLCore();
!
!TEXT=PrintTextAbove
PrintTextAbove(^!, text, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR);
!
!TEXT=PrintTextAbovePrivate
PrintTextAbovePrivate(^!, text, who, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR);
!
!TEXT=RawSkillToBaseSkill
RawSkillToBaseSkill(^!);
!
!TEXT=ReadGameClock
ReadGameClock();
!
!TEXT=RecalcVitals
RecalcVitals(^!);
!
!TEXT=RegisterForSpeechEvents
RegisterForSpeechEvents(^!, range, flags := 0);
!
!TEXT=ReleaseItem
ReleaseItem(^!);
!
!TEXT=RequestInput
RequestInput(^!, item, prompt);
!
!TEXT=ReserveItem
ReserveItem(^!);
!
!TEXT=RestartScript
RestartScript(^!);
!
!TEXT=Resurrect
Resurrect(^!, flags := 0);
!
!TEXT=RevokePrivilege
RevokePrivilege(^!, privilege);
!
!TEXT=SaveWorldState
SaveWorldState();
!
!TEXT=SecureTradeWin
SecureTradeWin(who, who2);
!
!TEXT=SelectColor
SelectColor(^!, item);
!
!TEXT=SelectMenuItem2
SelectMenuItem2(^!, menuname);
!
!TEXT=SendBuyWindow
SendBuyWindow(^!, container, vendor, items);
!
!TEXT=SendDialogGump
SendDialogGump(^!, layout, textlines, x := 0, y := 0);
!
!TEXT=SendEvent
SendEvent(^!, event);
!
!TEXT=SendInstaResDialog
SendInstaResDialog(^!);
!
!TEXT=SendOpenBook
SendOpenBook(^!, book);
!
!TEXT=SendOpenSpecialContainer
SendOpenSpecialContainer(^!, container);
!
!TEXT=SendPacket
SendPacket(^!, packet_hex_string);
!
!TEXT=SendQuestArrow
SendQuestArrow(^!, x := -1, y := -1);
!
!TEXT=SendSellWindow
SendSellWindow(^!, vendor, i1, i2, i3);
!
!TEXT=SendSkillWindow
SendSkillWindow(^!, forwhom);
!
!TEXT=SendStatus
SendStatus(^!);
!
!TEXT=SendSysMessage
SendStringAsTipWindow(^!, text);
!
!TEXT=SendSysMessage
SendSysMessage(^!, text, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR);
!
!TEXT=SendTextEntryGump
SendTextEntryGump(^!, line1, cancel := TE_CANCEL_ENABLE, style := TE_STYLE_NORMAL, maximum := 40, line2 := "");
!
!TEXT=SendViewContainer
SendViewContainer(^!, container);
!
!TEXT=SetAttributeBaseValue
SetAttributeBaseValue(^!, attrname, basevalue_tenths);
!
!TEXT=SetAttributeTemporaryMod
SetAttributeTemporaryMod(^!, attrname, tempmod_tenths);
!
!TEXT=SetGlobalProperty
SetGlobalProperty(^!, propval);
!
!TEXT=SetName
SetName(^!, name);
!
!TEXT=SetObjProperty
SetObjProperty(^!, property_name, property_value_string_only);
!
!TEXT=SetRegionLightLevel
SetRegionLightLevel(^!, lightlevel);
!
!TEXT=SetRegionWeatherLevel
SetRegionWeatherLevel(^!, type, severity, aux := 0, lightoverride := -1);
!
!TEXT=SetScriptController
SetScriptController(^!);
!
!TEXT=SetVital
SetVital(^!, vitalname, value);
!
!TEXT=Shutdown
Shutdown();
!
!TEXT=SpeakPowerWords
SpeakPowerWords(^!, spellid);
!
!TEXT=StartSpellEffect
StartSpellEffect(^!, spellid);
!
!TEXT=SubtractAmount
SubtractAmount(^!, amount);
!
!TEXT=SystemFindObjectBySerial
SystemFindObjectBySerial(^!, sysfind_flags := 0);
!
!TEXT=Target
Target(^!, options := TGTOPT_CHECK_LOS+TGTOPT_NEUTRAL);
!
!TEXT=TargetCoordinates
TargetCoordinates(^!);
!
!TEXT=TargetMultiPlacement
TargetMultiPlacement(^!, objtype, flags := 0, xoffset := 0, yoffset := 0);
!
!TEXT=UseItem
UseItem(^!, who);
!

!TEXT=_________________
 
!

!TEXT=OS.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use os;
use os;
!

!TEXT=Clear_Event_Queue
Clear_Event_Queue();
!
!TEXT=Create_Debug_Context
Create_Debug_Context();
!
!TEXT=Events_Waiting
Events_Waiting();
!
!TEXT=Is_Critical
Is_Critical();
!
!TEXT=GetPid
GetPid();
!
!TEXT=GetProcess
GetProcess(^!);
!
!TEXT=Run_Script_To_Completion
Run_Script_To_Completion(^!, param := 0);
!
!TEXT=Set_Critical
Set_Critical(^!);
!
!TEXT=Set_Debug
Set_Debug(^!);
!
!TEXT=Set_Event_Queue_Size
Set_Event_Queue_Size(^!);
!
!TEXT=Set_Priority
Set_Priority(^!);
!
!TEXT=Set_Script_Option
Set_Script_Option(^!, optval);
!
!TEXT=Sleep
Sleep(^!);
!
!TEXT=SleepMS
SleepMS(^!);
!
!TEXT=Start_Script
Start_Script(^!, param := 0);
!
!TEXT=SysLog
SysLog(^!);
!
!TEXT=System_Rpm
System_Rpm();
!
!TEXT=Unload_Scripts
Unload_Scripts(scriptname := "");
!
!TEXT=Wait_For_Event
Wait_For_Event(^!);
!

!TEXT=__________________
 
!

!TEXT=UTIL.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use util;
use util;
!

!TEXT=RandomInt
RandomInt(^!);
!
!TEXT=RandomFloat
RandomFloat(^!);
!
!TEXT=RandomDiceRoll
RandomDiceRoll(^!);
!

!TEXT=__________________
 
!

!TEXT=NPC.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use npc;
use npc;
!

!TEXT=CanMove
CanMove(^!);
!
!TEXT=IsLegalMove
IsLegalMove(^!, boundingbox);
!
!TEXT=GetProperty
GetProperty(^!);
!
!TEXT=MakeBoundingBox
MakeBoundingBox(^!);
!
!TEXT=Move
Move(^!);
! 
!TEXT=Position
Position();
!
!TEXT=RunAwayFrom
RunAwayFrom(^!);
!
!TEXT=RunAwayFromLocation
RunAwayFromLocation(x, y);
!
!TEXT=RunToward
RunToward(^!);
!
!TEXT=RunTowardLocation
RunTowardLocation(x, y);
!
!TEXT=Say
Say(^!, text_type := SAY_TEXTTYPE_DEFAULT, do_event := SAY_DOEVENT_DISABLE);
!
!TEXT=Self
Self();
!
!TEXT=SetAnchor
SetAnchor(centerx, centery, distance_start, percent_subtract);
!
!TEXT=SetOpponent
SetOpponent(^!);
!
!TEXT=SetProperty
SetProperty(^!, propertyvalue);
!
!TEXT=SetWarMode
SetWarMode(^!);
!
!TEXT=TurnAwayFrom
TurnAwayFrom(^!);
!
!TEXT=TurnAwayFromLocation
TurnAwayFromLocation(x, y);
!
!TEXT=TurnToward
TurnToward(^!);
!
!TEXT=TurnTowardLocation
TurnTowardLocation(x, y);
!
!TEXT=WalkAwayFrom
WalkAwayFrom(^!);
!
!TEXT=WalkAwayFromLocation
WalkAwayFromLocation(x, y);
!
!TEXT=WalkToward
WalkToward(^!);
!
!TEXT=WalkTowardLocation
WalkTowardLocation(x, y);
!
!TEXT=Wander
Wander();
!

!TEXT=___________________
 
!

!TEXT=BASIC.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use basic;
use basic;
!

!TEXT=Bin
Bin(^!);
!
!TEXT=CAsc
CAsc(^!);
!
!TEXT=CAscZ
CAscZ(^!);
!
!TEXT=CChr
CChr(^!);
!
!TEXT=CChrZ
CChrZ(^!);
!
!TEXT=CInt
CInt(^!);
!
!TEXT=CDbl
CDbl(^!);
!
!TEXT=CStr
CStr(^!);
!
!TEXT=Find
Find(^!, Search, Start);
!
!TEXT=Hex
Hex(^!);
!
!TEXT=Left
Left(^!, Count);
!
!TEXT=Len
Len(^!);
!
!TEXT=Lower
Lower(^!);
!
!TEXT=Pack
Pack(^!);
!
!TEXT=Print
Print(^!);
!
!TEXT=SplitWords
SplitWords(^!, delimiter := " ");
!
!TEXT=SizeOf
SizeOf(^!);
!
!TEXT=TypeOf
TypeOf(^!);
!
!TEXT=Unpack
Unpack(^!);
!
!TEXT=Upper
Upper(^!);
!

!TEXT=___________________
 
!

!TEXT=MATH.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use math;
use math;
!

!TEXT=Abs
Abs(^!);
!
!TEXT=ACos
ACos(^!);
!
!TEXT=ASin
ASin(^!);
!
!TEXT=ATan
ATan(^!);
!
!TEXT=Ceil
Ceil(^!);
!
!TEXT=ConstE
ConstE();
!
!TEXT=ConstPi
ConstPi();
!
!TEXT=Cos
Cos(^!);
!
!TEXT=DegToRad
DegToRad(^!);
!
!TEXT=Floor
Floor(^!);
!
!TEXT=FormatRealToString
FormatRealToString(^!, precision);
!
!TEXT=Log10
Log10(^!);
!
!TEXT=LogE
LogE(^!);
!
!TEXT=Pow
Pow(x, y);
!
!TEXT=RadToDeg
RadToDeg(^!);
!
!TEXT=Root
Root(x, y);
!
!TEXT=Sin
Sin(^!);
!
!TEXT=Sqrt
Sqrt(^!);
!
!TEXT=Tan
Tan(^!);
!

!TEXT=_____________________
 
!

!TEXT=CFGFILE.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use cfgfile;
use cfgfile;
!

!TEXT=AppendConfigFileElem
AppendConfigFileElem(^!, elemtype, elemkey, properties);
!
!TEXT=FindConfigElem
FindConfigElem(^!, key);
!
!TEXT=GetConfigInt
GetConfigInt(^!, property_name);
!
!TEXT=GetConfigIntKeys
GetConfigIntKeys(^!);
!
!TEXT=GetConfigMaxIntKey
GetConfigMaxIntKey(^!);
!
!TEXT=GetConfigReal
GetConfigReal(^!, property_name);
!
!TEXT=GetConfigString
GetConfigString(^!, property_name);
!
!TEXT=GetConfigStringArray
GetConfigStringArray(^!, property_name);
!
!TEXT=GetConfigStringDictionary
GetConfigStringDictionary(^!, property_name);
!
!TEXT=GetConfigStringKeys
GetConfigStringKeys(^!);
!
!TEXT=GetElemProperty
GetElemProperty(^!, property_name);
!
!TEXT=ListConfigElemProps
ListConfigElemProps(^!);
!
!TEXT=LoadTusScpFile
LoadTusScpFile(^!);
!
!TEXT=ReadConfigFile
ReadConfigFile(^!);
!
!TEXT=UnloadConfigFile
UnloadConfigFile(^!);
!

!TEXT=______________________
 
!

!TEXT=DATAFILE.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use datafile;
use datafile;
!

!TEXT=CreateDataFile
CreateDataFile(^!, flags := DF_KEYTYPE_STRING);
!
!TEXT=OpenDataFile
OpenDataFile(^!);
!
!TEXT=UnloadDataFile
UnloadDataFile(^!);
!

!TEXT=_____________________
 
!

!TEXT=POLSYS.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use polsys;
use polsys;
!

!TEXT=CreatePacket
CreatePacket(^!, size);
!
!TEXT=GetCmdLevelName
GetCmdLevelName(^!);
!
!TEXT=GetCmdLevelNumber
GetCmdLevelNumber(^!);
!
!TEXT=GetItemDescriptor
GetItemDescriptor(^!);
!
!TEXT=GetPackageByName
GetPackageByName(^!);
!
!TEXT=ListenPoints
ListenPoints();
!
!TEXT=Packages
Packages();
!
!TEXT=ReadMillisecondClock
ReadMillisecondClock();
!
!TEXT=Realms
Realms();
!
!TEXT=ReloadConfiguration
ReloadConfiguration();
!
!TEXT=SetSysTrayPopupText
SetSysTrayPopupText(^!);
!
!TEXT=StorageAreas
StorageAreas();
!

!TEXT=___________________
 
!

!TEXT=BOAT.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use boat;
use boat;
!

!TEXT=BoatFromItem
BoatFromItem(^!);
!
!TEXT=MoveBoat
MoveBoat(^!, facing);
!
!TEXT=MoveBoatRelative
MoveBoatRelative(^!, direction);
!
!TEXT=MoveBoatXY
MoveBoatXY(^!, x, y);
!	
!TEXT=RegisterItemWithBoat
RegisterItemWithBoat(^!, item);
!
!TEXT=SystemFindBoatBySerial
SystemFindBoatBySerial(^!);
!
!TEXT=TurnBoat
TurnBoat(^!, direction);
!

!TEXT=_________________
 
!

!TEXT=FILE.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use file;
use file;
!

!TEXT=ReadFile
ReadFile(^!);
!
!TEXT=AppendToFile
WriteFile(^!, textcontents);
!
!TEXT=AppendToFile
AppendToFile(^!, textlines);
!
!TEXT=LogToFile
LogToFile(^!, line, flags := 0);
!

!TEXT=______________________
 
!

!TEXT=UNICODE.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use unicode;
use unicode;
!

!TEXT=BroadcastUC
BroadcastUC(^!, langcode, font := _DEFAULT_UCFONT, color := _DEFAULT_UCCOLOR);
!
!TEXT=PrintTextAboveUC
PrintTextAboveUC(^!, uc_text, langcode, font := _DEFAULT_UCFONT, color := _DEFAULT_UCCOLOR);
! 
!TEXT=PrintTextAbovePrivateUC
PrintTextAbovePrivateUC(^!, uc_text, langcode, who, font := _DEFAULT_UCFONT, color := _DEFAULT_UCCOLOR);
!
!TEXT=RequestInputUC
RequestInputUC(^!, item, uc_prompt, langcode);
!
!TEXT=SendSysMessageUC
SendSysMessageUC(^!, uc_text, langcode, font := _DEFAULT_UCFONT, color := _DEFAULT_UCCOLOR);
!

!TEXT=___________________
 
!

!TEXT=HTTP.EM FUNCTIONS
 
!

!TEXT=¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
!

!TEXT=use http;
use http;
!

!TEXT=WriteHtml
WriteHtml(^!);
!
!TEXT=WriteHtmlRaw
WriteHtmlRaw(^!);
!
!TEXT=QueryParam
QueryParam(^!);
!
!TEXT=QueryIP
QueryIP();
!