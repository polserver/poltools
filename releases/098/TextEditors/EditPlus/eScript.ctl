#TITLE=POL .096
#INFO
Written by Danielle Elora; Nightscape Shard ( www.nightscapeonline.com ) as a FREE aid to writing POL eScript
#SORT=n

#T=UO.EM FUNCTIONS

#T=^^^^^^^^^

#T=use uo;
use uo;
#T= 
 
#T=Accessible
Accessible(^!, item);
#T=AddAmount
AddAmount(^!, amount);
#T=AddMenuItem
AddMenuItem(^!, objtype, text);
#T=AlterAttributeTemporaryMod
AlterAttributeTemporaryMod(^!, attrname, delta_tenths);
#T=ApplyConstraint
ApplyConstraint(^!, configfile, propertyname, minvalue);
#T=ApplyDamage
ApplyDamage(^!, damage);
#T=ApplyRawDamage
ApplyRawDamage(^!, damage);
#T=AssignRectToWeatherRegion
AssignRectToWeatherRegion(^!, xwest, ynorth, xeast, ysouth);
#T=Attach
Attach(^!);
#T=BaseSkillToRawSkill
BaseSkillToRawSkill(^!);
#T=Broadcast
Broadcast(^!, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR);
#T=CheckLineOfSight
CheckLineOfSight(object1, object2);
#T=CheckLosAt
CheckLosAt(^!, x, y, z);
#T=CheckSkill
CheckSkill(^!, skillid, difficulty, points);
#T=ConsumeMana
ConsumeMana(^!, spellid);
#T=ConsumeReagents
ConsumeReagents(^!, spellid);
#T=ConsumeSubstance
ConsumeSubstance(^!, objtype, amount);
#T=ConsumeVital
ConsumeVital(^!, vital, hundredths);
#T=CoordinateDistance
CoordinateDistance(x1, y1, x2, y2);
#T=CreateAccount
CreateAccount(^!, password, enabled);
#T=CreateGuild
CreateGuild();
#T=CreateItemAtLocation
CreateItemAtLocation(x, y, z, objtype, amount := 1, realm := _DEFAULT_REALM);
#T=CreateItemCopyAtLocation
CreateItemCopyAtLocation(x, y, z, item, realm := _DEFAULT_REALM);
#T=CreateItemInBackpack
CreateItemInBackpack(^!, objtype, amount := 1);
#T=CreateItemInContainer
CreateItemInContainer(^!, objtype, amount := 1);
#T=CreateItemInInventory
CreateItemInInventory(^!, objtype, amount := 1);
#T=CreateMenu
CreateMenu(^!);
#T=CreateMultiAtLocation
CreateMultiAtLocation(x, y, z, objtype, flags := 0, realm := _DEFAULT_REALM);
#T=CreateNpcFromTemplate
CreateNpcFromTemplate(^!, x, y, z, override_properties := 0, realm := _DEFAULT_REALM);
#T=CreateRootItemInStorageArea
CreateRootItemInStorageArea(^!, itemname, objtype);
#T=CreateStorageArea
CreateStorageArea(^!);
#T=DestroyGuild
DestroyGuild(^!);
#T=DestroyItem
DestroyItem(^!);
#T=DestroyMulti
DestroyMulti(^!);
#T=DestroyRootItemInStorageArea
DestroyRootItemInStorageArea(^!, itemname);
#T=Detach
Detach();
#T=DisableEvents
DisableEvents(^!);
#T=DisconnectClient
DisconnectClient(^!);
#T=Distance
Distance(obj1, obj2);
#T=EnableEvents
EnableEvents(^!, range := -1);
#T=EnumerateItemsInContainer
EnumerateItemsInContainer(^!, flags := 0);
#T=EnumerateOnlineCharacters
EnumerateOnlineCharacters();
#T=EquipFromTemplate
EquipFromTemplate(^!, template);
#T=EquipItem
EquipItem(^!, item);
#T=EraseGlobalProperty
EraseGlobalProperty(^!);
#T=EraseObjProperty
EraseObjProperty(^!, propname);
#T=FindAccount
FindAccount(^!);
#T=FindGuild
FindGuild(^!);
#T=FindPath
FindObjtypeInContainer(^!, objtype);
#T=FindPath
FindPath(x1, y1, z1, x2, y2, z2, realm := _DEFAULT_REALM, flags := FP_IGNORE_MOBILES, searchskirt := 5);
#T=FindRootItemInStorageArea
FindRootItemInStorageArea(^!, itemname);
#T=FindStorageArea
FindStorageArea(^!);
#T=FindSubstance
FindSubstance(^!, objtype, amount, makeinuse := 0);
#T=GetAmount
GetAmount(^!);
#T=GetAttribute
GetAttribute(^!, attrname);
#T=GetAttributeBaseValue
GetAttributeBaseValue(^!, attrname);
#T=GetAttributeIntrinsicMod
GetAttributeIntrinsicMod(^!, attrname);
#T=GetAttributeTemporaryMod
GetAttributeTemporaryMod(^!, attrname);
#T=GetCommandHelp
GetCommandHelp(^!, command);
#T=CreateStorageArea
GetCoordsInLine(x1, y1, x2, y2);
#T=GetEquipmentByLayer
GetEquipmentByLayer(^!, layer);
#T=GetFacing
GetFacing(from_x, from_y, to_x, to_y);
#T=GetGlobalProperty
GetGlobalProperty(^!);
#T=GetHarvestDifficulty
GetHarvestDifficulty(^!, x, y, tiletype, realm := _DEFAULT_REALM);
#T=GetMenuObjTypes
GetMapInfo(x, y, realm := _DEFAULT_REALM);
#T=GetMenuObjTypes
GetMenuObjTypes(^!);
#T=GetMultiDimensions
GetMultiDimensions(^!);
#T=GetObjProperty
GetObjProperty(^!, property_name);
#T=GetObjPropertyNames
GetObjPropertyNames(^!);
#T=GetObjtypeByName
GetObjType(^!);
#T=GetObjtypeByName
GetObjtypeByName(^!);
#T=GetRegionString
GetRegionString(^!, x, y, propertyname, realm := _DEFAULT_REALM);
#T=GetSpellDifficulty
GetSpellDifficulty(^!);
#T=GetStandingHeight
GetStandingHeight(x, y, startz, realm := _DEFAULT_REALM);
#T=GetStandingLayers
GetStandingLayers(x, y, flags := MAPDATA_FLAG_ALL, realm := _DEFAULT_REALM);
#T=GetWorldHeight
GetWorldHeight(x, y, realm := _DEFAULT_REALM);
#T=GetVital
GetVital(^!, vitalname);
#T=GetVitalMaximumValue
GetVitalMaximumValue(^!, vitalname);
#T=GetVitalRegenRate
GetVitalRegenRate(^!, vitalname);
#T=GrantPrivilege
GrantPrivilege(^!, privilege);
#T=HarvestResource
HarvestResource(^!, x, y, b, n, realm := _DEFAULT_REALM);
#T=HealDamage
HealDamage(^!, hits);
#T=ListAccounts
ListAccounts();
#T=ListEquippedItems
ListEquippedItems(^!);
#T=ListGhostsNearLocation
ListGhostsNearLocation(x, y, z, range, realm := _DEFAULT_REALM);
#T=ListGuilds
ListGuilds();
#T=ListHostiles
ListHostiles(^!, range := 20, flags := 0);
#T=ListItemsAtLocation
ListItemsAtLocation(x, y, z, realm := _DEFAULT_REALM);
#T=ListItemsNearLocation
ListItemsNearLocation(x, y, z, range, realm := _DEFAULT_REALM);
#T=ListItemsNearLocationOfType
ListItemsNearLocationOfType(x, y, z, range, objtype, realm := _DEFAULT_REALM);
#T=ListItemsNearLocationWithFlag
ListItemsNearLocationWithFlag(x, y, z, range, flags, realm := _DEFAULT_REALM);
#T=ListMobilesInLineOfSight
ListMobilesInLineOfSight(^!, range);
#T=ListMobilesNearLocation
ListMobilesNearLocation(x, y, z, range, realm := _DEFAULT_REALM);
#T=ListMobilesNearLocationEx
ListMobilesNearLocationEx(x, y, z, range, flags, realm := _DEFAULT_REALM);
#T=ListObjectsInBox
ListObjectsInBox(x1, y1, z1, x2, y2, z2, realm := _DEFAULT_REALM);
#T=ListMultisInBox
ListMultisInBox(x1, y1, z1, x2, y2, z2, realm := _DEFAULT_REALM);
#T=ListStaticsInBox
ListStaticsInBox(x1, y1, z1, x2, y2, z2, flags := 0, realm := _DEFAULT_REALM);
#T=ListStaticsAtLocation
ListStaticsAtLocation(x, y, z, flags := 0, realm := _DEFAULT_REALM);
#T=ListStaticsNearLocation
ListStaticsNearLocation(x, y, z, range, flags := 0, realm := _DEFAULT_REALM);
#T=MoveCharacterToLocation
MoveCharacterToLocation(^!, x, y, z, flags := 0);
#T=MoveItemToContainer
MoveItemToContainer(^!, container, x := -1, y := -1);
#T=MoveItemToLocation
MoveItemToLocation(^!, x, y, z, flags := 0);
#T=MoveItemToSecureTradeWin
MoveItemToSecureTradeWin(^!, who);
#T=MoveObjectToLocation
MoveObjectToLocation(^!, x, y, z, realm := _DEFAULT_REALM, flags := MOVEOBJECT_NORMAL);
#T=MoveObjectToRealm
MoveObjectToRealm(^!, realm, x, y, z, flags := 0);
#T=OpenPaperdoll
OpenPaperdoll(^!, forwhom);
#T=PerformAction
PerformAction(^!, action);
#T=PlayLightningBoltEffect
PlayLightningBoltEffect(^!);
#T=PlayMovingEffect
PlayMovingEffect(source, target, effect, speed, loop := 0, explode := 0);
#T=PlayMovingEffectXYZ
PlayMovingEffectXYZ(srcx, srcy, srcz, dstx, dsty, dstz, effect, speed, loop := 0, explode := 0, realm := _DEFAULT_REALM);
#T=PlayObjectCenteredEffect
PlayObjectCenteredEffect(center, effect, speed, loop := 0);
#T=PlaySoundEffect
PlaySoundEffect(^!, effect);
#T=PlaySoundEffectPrivate
PlaySoundEffectPrivate(^!, effect, playfor);
#T=PlayStationaryEffect
PlayStationaryEffect(x, y, z, effect, speed, loop := 0, explode := 0, realm := _DEFAULT_REALM);
#T=POLCore
POLCore();
#T=PrintTextAbove
PrintTextAbove(^!, text, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR);
#T=PrintTextAbovePrivate
PrintTextAbovePrivate(^!, text, who, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR);
#T=RawSkillToBaseSkill
RawSkillToBaseSkill(^!);
#T=ReadGameClock
ReadGameClock();
#T=RecalcVitals
RecalcVitals(^!);
#T=RegisterForSpeechEvents
RegisterForSpeechEvents(^!, range, flags := 0);
#T=ReleaseItem
ReleaseItem(^!);
#T=RequestInput
RequestInput(^!, item, prompt);
#T=ReserveItem
ReserveItem(^!);
#T=RestartScript
RestartScript(^!);
#T=Resurrect
Resurrect(^!, flags := 0);
#T=RevokePrivilege
RevokePrivilege(^!, privilege);
#T=SaveWorldState
SaveWorldState();
#T=SecureTradeWin
SecureTradeWin(who, who2);
#T=SelectColor
SelectColor(^!, item);
#T=SelectMenuItem2
SelectMenuItem2(^!, menuname);
#T=SendBuyWindow
SendBuyWindow(^!, container, vendor, items);
#T=SendDialogGump
SendDialogGump(^!, layout, textlines, x := 0, y := 0);
#T=SendEvent
SendEvent(^!, event);
#T=SendInstaResDialog
SendInstaResDialog(^!);
#T=SendOpenBook
SendOpenBook(^!, book);
#T=SendOpenSpecialContainer
SendOpenSpecialContainer(^!, container);
#T=SendPacket
SendPacket(^!, packet_hex_string);
#T=SendQuestArrow
SendQuestArrow(^!, x := -1, y := -1);
#T=SendSellWindow
SendSellWindow(^!, vendor, i1, i2, i3);
#T=SendSkillWindow
SendSkillWindow(^!, forwhom);
#T=SendStatus
SendStatus(^!);
#T=SendSysMessage
SendStringAsTipWindow(^!, text);
#T=SendSysMessage
SendSysMessage(^!, text, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR);
#T=SendTextEntryGump
SendTextEntryGump(^!, line1, cancel := TE_CANCEL_ENABLE, style := TE_STYLE_NORMAL, maximum := 40, line2 := "");
#T=SendViewContainer
SendViewContainer(^!, container);
#T=SetAttributeBaseValue
SetAttributeBaseValue(^!, attrname, basevalue_tenths);
#T=SetAttributeTemporaryMod
SetAttributeTemporaryMod(^!, attrname, tempmod_tenths);
#T=SetGlobalProperty
SetGlobalProperty(^!, propval);
#T=SetName
SetName(^!, name);
#T=SetObjProperty
SetObjProperty(^!, property_name, property_value_string_only);
#T=SetRegionLightLevel
SetRegionLightLevel(^!, lightlevel);
#T=SetRegionWeatherLevel
SetRegionWeatherLevel(^!, type, severity, aux := 0, lightoverride := -1);
#T=SetScriptController
SetScriptController(^!);
#T=SetVital
SetVital(^!, vitalname, value);
#T=Shutdown
Shutdown();
#T=SpeakPowerWords
SpeakPowerWords(^!, spellid);
#T=StartSpellEffect
StartSpellEffect(^!, spellid);
#T=SubtractAmount
SubtractAmount(^!, amount);
#T=SystemFindObjectBySerial
SystemFindObjectBySerial(^!, sysfind_flags := 0);
#T=Target
Target(^!, options := TGTOPT_CHECK_LOS+TGTOPT_NEUTRAL);
#T=TargetCoordinates
TargetCoordinates(^!);
#T=TargetMultiPlacement
TargetMultiPlacement(^!, objtype, flags := 0, xoffset := 0, yoffset := 0);
#T=UseItem
UseItem(^!, who);
#T= 
 
#T=OS.EM FUNCTIONS

#T=^^^^^^^^^

#T=use os;
use os;
#T= 
 
#T=Clear_Event_Queue
Clear_Event_Queue();
#T=Create_Debug_Context
Create_Debug_Context();
#T=Events_Waiting
Events_Waiting();
#T=Is_Critical
Is_Critical();
#T=GetPid
GetPid();
#T=GetProcess
GetProcess(^!);
#T=Run_Script_To_Completion
Run_Script_To_Completion(^!, param := 0);
#T=Set_Critical
Set_Critical(^!);
#T=Set_Debug
Set_Debug(^!);
#T=Set_Event_Queue_Size
Set_Event_Queue_Size(^!);
#T=Set_Priority
Set_Priority(^!);
#T=Set_Script_Option
Set_Script_Option(^!, optval);
#T=Sleep
Sleep(^!);
#T=SleepMS
SleepMS(^!);
#T=Start_Script
Start_Script(^!, param := 0);
#T=SysLog
SysLog(^!);
#T=System_Rpm
System_Rpm();
#T=Unload_Scripts
Unload_Scripts(scriptname := "");
#T=Wait_For_Event
Wait_For_Event(^!);
#T= 
 
#T=UTIL.EM FUNCTIONS

#T=^^^^^^^^^

#T=use util;
use util;
#T= 
 
#T=RandomInt
RandomInt(^!);
#T=RandomFloat
RandomFloat(^!);
#T=RandomDiceRoll
RandomDiceRoll(^!);
#T= 
 
#T=NPC.EM FUNCTIONS

#T=^^^^^^^^^

#T=use npc;
use npc;
#T= 
 
#T=CanMove
CanMove(^!);
#T=IsLegalMove
IsLegalMove(^!, boundingbox);
#T=GetProperty
GetProperty(^!);
#T=MakeBoundingBox
MakeBoundingBox(^!);
#T=Move
Move(^!); 
#T=Position
Position();
#T=RunAwayFrom
RunAwayFrom(^!);
#T=RunAwayFromLocation
RunAwayFromLocation(x, y);
#T=RunToward
RunToward(^!);
#T=RunTowardLocation
RunTowardLocation(x, y);
#T=Say
Say(^!, text_type := SAY_TEXTTYPE_DEFAULT, do_event := SAY_DOEVENT_DISABLE);
#T=Self
Self();
#T=SetAnchor
SetAnchor(centerx, centery, distance_start, percent_subtract);
#T=SetOpponent
SetOpponent(^!);
#T=SetProperty
SetProperty(^!, propertyvalue);
#T=SetWarMode
SetWarMode(^!);
#T=TurnAwayFrom
TurnAwayFrom(^!);
#T=TurnAwayFromLocation
TurnAwayFromLocation(x, y);
#T=TurnToward
TurnToward(^!);
#T=TurnTowardLocation
TurnTowardLocation(x, y);
#T=WalkAwayFrom
WalkAwayFrom(^!);
#T=WalkAwayFromLocation
WalkAwayFromLocation(x, y);
#T=WalkToward
WalkToward(^!);
#T=WalkTowardLocation
WalkTowardLocation(x, y);
#T=Wander
Wander();
#T= 
 
#T=BASIC.EM FUNCTIONS

#T=^^^^^^^^^

#T=use basic;
use basic;
#T= 
 
#T=Bin
Bin(^!);
#T=CAsc
CAsc(^!);
#T=CAscZ
CAscZ(^!);
#T=CChr
CChr(^!);
#T=CChrZ
CChrZ(^!);
#T=CInt
CInt(^!);
#T=CDbl
CDbl(^!);
#T=CStr
CStr(^!);
#T=Find
Find(^!, Search, Start);
#T=Hex
Hex(^!);
#T=Left
Left(^!, Count);
#T=Len
Len(^!);
#T=Lower
Lower(^!);
#T=Pack
Pack(^!);
#T=Print
Print(^!);
#T=SplitWords
SplitWords(^!, delimiter := " ");
#T=SizeOf
SizeOf(^!);
#T=TypeOf
TypeOf(^!);
#T=Unpack
Unpack(^!);
#T=Upper
Upper(^!);
#T= 
 
#T=MATH.EM FUNCTIONS

#T=^^^^^^^^^

#T=use math;
use math;
#T= 
 
#T=Abs
Abs(^!);
#T=ACos
ACos(^!);
#T=ASin
ASin(^!);
#T=ATan
ATan(^!);
#T=Ceil
Ceil(^!);
#T=ConstE
ConstE();
#T=ConstPi
ConstPi();
#T=Cos
Cos(^!);
#T=DegToRad
DegToRad(^!);
#T=Floor
Floor(^!);
#T=FormatRealToString
FormatRealToString(^!, precision);
#T=Log10
Log10(^!);
#T=LogE
LogE(^!);
#T=Pow
Pow(x, y);
#T=RadToDeg
RadToDeg(^!);
#T=Root
Root(x, y);
#T=Sin
Sin(^!);
#T=Sqrt
Sqrt(^!);
#T=Tan
Tan(^!);
#T= 
 
#T=CFGFILE.EM FUNCTIONS

#T=^^^^^^^^^

#T=use cfgfile;
use cfgfile;
#T= 
 
#T=AppendConfigFileElem
AppendConfigFileElem(^!, elemtype, elemkey, properties);
#T=FindConfigElem
FindConfigElem(^!, key);
#T=GetConfigInt
GetConfigInt(^!, property_name);
#T=GetConfigIntKeys
GetConfigIntKeys(^!);
#T=GetConfigMaxIntKey
GetConfigMaxIntKey(^!);
#T=GetConfigReal
GetConfigReal(^!, property_name);
#T=GetConfigString
GetConfigString(^!, property_name);
#T=GetConfigStringArray
GetConfigStringArray(^!, property_name);
#T=GetConfigStringDictionary
GetConfigStringDictionary(^!, property_name);
#T=GetConfigStringKeys
GetConfigStringKeys(^!);
#T=GetElemProperty
GetElemProperty(^!, property_name);
#T=ListConfigElemProps
ListConfigElemProps(^!);
#T=LoadTusScpFile
LoadTusScpFile(^!);
#T=ReadConfigFile
ReadConfigFile(^!);
#T=UnloadConfigFile
UnloadConfigFile(^!);
#T= 
 
#T=DATAFILE.EM FUNCTIONS

#T=^^^^^^^^^

#T=use datafile;
use datafile;
#T= 
 
#T=CreateDataFile
CreateDataFile(^!, flags := DF_KEYTYPE_STRING);
#T=OpenDataFile
OpenDataFile(^!);
#T=UnloadDataFile
UnloadDataFile(^!);
#T= 
 
#T=POLSYS.EM FUNCTIONS

#T=^^^^^^^^^

#T=use polsys;
use polsys;
#T= 
 
#T=CreatePacket
CreatePacket(^!, size);
#T=GetCmdLevelName
GetCmdLevelName(^!);
#T=GetCmdLevelNumber
GetCmdLevelNumber(^!);
#T=GetItemDescriptor
GetItemDescriptor(^!);
#T=GetPackageByName
GetPackageByName(^!);
#T=ListenPoints
ListenPoints();
#T=Packages
Packages();
#T=ReadMillisecondClock
ReadMillisecondClock();
#T=Realms
Realms();
#T=ReloadConfiguration
ReloadConfiguration();
#T=SetSysTrayPopupText
SetSysTrayPopupText(^!);
#T=StorageAreas
StorageAreas();
#T= 
 
#T=BOAT.EM FUNCTIONS

#T=^^^^^^^^^

#T=use boat;
use boat;
#T= 
 
#T=BoatFromItem
BoatFromItem(^!);
#T=MoveBoat
MoveBoat(^!, facing);
#T=MoveBoatRelative
MoveBoatRelative(^!, direction);
#T=MoveBoatXY
MoveBoatXY(^!, x, y);	
#T=RegisterItemWithBoat
RegisterItemWithBoat(^!, item);
#T=SystemFindBoatBySerial
SystemFindBoatBySerial(^!);
#T=TurnBoat
TurnBoat(^!, direction);
#T= 
 
#T=FILE.EM FUNCTIONS

#T=^^^^^^^^^

#T=use file;
use file;
#T= 
 
#T=ReadFile
ReadFile(^!);
#T=AppendToFile
WriteFile(^!, textcontents);
#T=AppendToFile
AppendToFile(^!, textlines);
#T=LogToFile
LogToFile(^!, line, flags := 0);
#T= 
 
#T=UNICODE.EM FUNCTIONS

#T=^^^^^^^^^

#T=use unicode;
use unicode;
#T= 
 
#T=BroadcastUC
BroadcastUC(^!, langcode, font := _DEFAULT_UCFONT, color := _DEFAULT_UCCOLOR);
#T=PrintTextAboveUC
PrintTextAboveUC(^!, uc_text, langcode, font := _DEFAULT_UCFONT, color := _DEFAULT_UCCOLOR); 
#T=PrintTextAbovePrivateUC
PrintTextAbovePrivateUC(^!, uc_text, langcode, who, font := _DEFAULT_UCFONT, color := _DEFAULT_UCCOLOR);
#T=RequestInputUC
RequestInputUC(^!, item, uc_prompt, langcode);
#T=SendSysMessageUC
SendSysMessageUC(^!, uc_text, langcode, font := _DEFAULT_UCFONT, color := _DEFAULT_UCCOLOR);
#T= 
 
#T=HTTP.EM FUNCTIONS

#T=^^^^^^^^^

#T=use http;
use http;
#T= 
 
#T=WriteHtml
WriteHtml(^!);
#T=WriteHtmlRaw
WriteHtmlRaw(^!);
#T=QueryParam
QueryParam(^!);
#T=QueryIP
QueryIP();
#