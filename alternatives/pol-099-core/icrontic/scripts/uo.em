////////////////////////////////////////////////////////////////
//
//	CONSTANTS
//
////////////////////////////////////////////////////////////////

// CreateMulti flags
// only for house creation:
const CRMULTI_IGNORE_MULTIS     := 0x0001;  // ignore intersecting multis
const CRMULTI_IGNORE_OBJECTS    := 0x0002;  // ignore dynamic objects
const CRMULTI_IGNORE_WORLDZ     := 0x0004;  // ignore standability, relative Z, world height
const CRMULTI_IGNORE_ALL        := 0x0007;  
// only for boat creation:
const CRMULTI_FACING_NORTH      := 0x0000;  
const CRMULTI_FACING_EAST       := 0x0100;
const CRMULTI_FACING_SOUTH      := 0x0200;
const CRMULTI_FACING_WEST       := 0x0300;

//	ListHostiles exclusions

const LH_FLAG_LOS               := 1;       // only include those in LOS
const LH_FLAG_INCLUDE_HIDDEN    := 2;       // include hidden characters

// RegisterForSpeechEvents flags
const LISTENPT_HEAR_GHOSTS      := 0x01;    // hear ghost speech in addition to living speech

//	ListItemsNearLocationEx exclusions

const LISTEX_FLAG_NORMAL := 0x01;
const LISTEX_FLAG_HIDDEN := 0x02;
const LISTEX_FLAG_GHOST := 0x04;


//	Move Options - add together and pass as "flags" param to
//	MoveCharacterToLocation or MoveItemToLocation

const MOVECHAR_FORCELOCATION := 0x40000000;
const MOVEITEM_NORMAL        := 0;
const MOVEITEM_FORCELOCATION := 0x40000000;


//	SendTextEntryGump options

const TE_CANCEL_DISABLE := 0;
const TE_CANCEL_ENABLE  := 1;

const TE_STYLE_DISABLE  := 0;
const TE_STYLE_NORMAL   := 1;
const TE_STYLE_NUMERICAL:= 2;


//	SystemFindObjectBySerial options:

const SYSFIND_SEARCH_OFFLINE_MOBILES := 1;
const SYSFIND_SEARCH_STORAGE_AREAS   := 2;


//	Target Options - add these together and pass as second
//	param to Target()

const TGTOPT_CHECK_LOS   := 0x0001;
const TGTOPT_NOCHECK_LOS := 0x0000;	// to be explicit
const TGTOPT_HARMFUL     := 0x0002;
const TGTOPT_NEUTRAL     := 0x0000;	// to be explicit
const TGTOPT_HELPFUL     := 0x0004;

// POLCLASS_* constants - use with obj.isa(POLCLASS_*)
const POLCLASS_UOBJECT      := 1;
const POLCLASS_ITEM         := 2;
const POLCLASS_MOBILE       := 3;
const POLCLASS_NPC          := 4;
const POLCLASS_LOCKABLE     := 5;
const POLCLASS_CONTAINER    := 6;
const POLCLASS_CORPSE       := 7;
const POLCLASS_DOOR         := 8;
const POLCLASS_SPELLBOOK    := 9;
const POLCLASS_MAP          := 10;
const POLCLASS_MULTI        := 11;
const POLCLASS_BOAT         := 12;
const POLCLASS_HOUSE        := 13;
const POLCLASS_EQUIPMENT    := 14;
const POLCLASS_ARMOR        := 15;
const POLCLASS_WEAPON       := 16;


// Don't use these outside this file, use FONT_* from client.inc
//  (and I don't know what for color)
const _DEFAULT_TEXT_FONT     := 3;
const _DEFAULT_TEXT_COLOR    := 0x3B2;

////////////////////////////////////////////////////////////////
//
//	FUNCTIONS
//
////////////////////////////////////////////////////////////////

Accessible( by_character, item );
AddAmount( item, amount );
AddMenuItem( menu, objtype, text );
AlterAttributeTemporaryMod( character, attrname, delta_tenths );
ApplyConstraint( objlist, configfile, propertyname, minvalue );
ApplyDamage( mobile, damage );
ApplyRawDamage( character, hits ); // raw damage (ignores armor etc)
AssignRectToWeatherRegion( region, xwest, ynorth, xeast, ysouth );
BaseSkillToRawSkill( baseskill );
Broadcast( text, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR );
CheckLineOfSight( object1, object2 );
CheckLosAt( character, x, y, z );
CheckSkill( character, skillid, difficulty, points );
ConsumeMana( who, spellid );
ConsumeReagents( who, spellid );
ConsumeSubstance( container, objtype, amount );
ConsumeVital( who, vital, hundredths );
CreateAccount( acctname, password, enabled );
CreateGuild();
CreateItemAtLocation( x, y, z, objtype, amount );
CreateItemInBackpack( of_character, objtype, amount := 1 );
CreateItemInContainer( container, objtype, amount := 1 );
CreateItemInInventory( container, objtype, amount := 1 );
CreateMenu( title );
CreateMultiAtLocation( x, y, z, objtype, flags := 0 );
CreateNpcFromTemplate( template, x, y, z, override_properties := 0);
CreateRootItemInStorageArea( area, itemname, objtype );
CreateStorageArea( areaname );
Damage( character, hits );	// deprecated, syn. for ApplyRawDamage
DestroyGuild( guild );
DestroyItem( item );
DestroyMulti( multi );
DestroyRootItemInStorageArea( area, itemname );
Detach();
DisableEvents( eventtype );     // eventtype combination of constants from SYSEVENT.INC
DisconnectClient( character );
Distance( obj1, obj2 );
EnableEvents( eventtype, range := -1);  // eventtype combination of constants from SYSEVENT.INC
EnumerateItemsInContainer( container );
EnumerateOnlineCharacters();
EquipFromTemplate( character, template ); // reads from equip.cfg
EquipItem( mobile, item );
EraseGlobalProperty( propname );
EraseObjProperty( object, propname );
FindAccount( acctname );
FindGuild( guildid );
FindObjtypeInContainer( container, objtype );
FindRootItemInStorageArea( area, itemname );
FindStorageArea( areaname );
GameStat(name);		// deprecated
GetAmount( item );
GetAttribute( character, attrname );
GetAttributeBaseValue( character, attrname );
GetAttributeIntrinsicMod( character, attrname );
GetAttributeTemporaryMod( character, attrname );
GetCommandHelp( character, command );
GetEquipmentByLayer( character, layer );
GetGlobalProperty( propname );
GetHarvestDifficulty( resource, x, y, tiletype );
GetMapInfo( x, y );
GetMenuObjTypes( menuname );
GetObjProperty( object, property_name );
GetObjPropertyNames( object );
GetObjType( object );
GetObjtypeByName( name );
GetRegionString( resource, x, y, propertyname );
GetSpellDifficulty( spellid );
GetStandingHeight( x, y, startz );
GetWorldHeight( x, y );
GetVital( character, vitalname );
GetVitalMaximumValue( character, vitalname );
GetVitalRegenRate( character, vitalname );
GrantPrivilege( character, privilege );
HarvestResource( resource, x, y, b, n ); // returns b*a where 0 <= a <= n
HealDamage( character, hits );
ListAccounts( );
ListEquippedItems( who );
ListGhostsNearLocation( x, y, z, range );
ListGuilds();
ListHostiles( character, range := 20, flags := 0 );
ListItemsAtLocation( x, y, z );
ListItemsNearLocation( x, y, z, range );
ListItemsNearLocationOfType( x,y,z, range, objtype );
ListMobilesInLineOfSight( object, range );
ListMobilesNearLocation( x, y, z, range );
ListMobilesNearLocationEx( x,y,z, range, flags );
ListObjectsInBox( x1,y1,z1, x2,y2,z2 );
MoveCharacterToLocation( character, x, y, z, flags := 0 );
MoveItemToContainer( item, container, x := -1, y := -1 );
MoveItemToLocation( item, x, y, z, flags );
OpenPaperdoll( towhom, forwhom );
PerformAction( character, action );
PlayLightningBoltEffect( center_object );
PlayMovingEffect( source, target, effect, speed, loop := 0, explode := 0 );
PlayMovingEffectXYZ( srcx, srcy, srcz, dstx, dsty, dstz, effect, speed, loop := 0, explode := 0 );
PlayObjectCenteredEffect( center, effect, speed, loop := 0 );
PlaySoundEffect( character, effect );
PlaySoundEffectPrivate( character, effect, playfor );
PlayStationaryEffect( x, y, z, effect, speed, loop := 0, explode := 0 );
polcore();
PrintTextAbove( above_object, text, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR ); 
PrintTextAbovePrivate( above_object, text, character, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR );
RawSkillToBaseSkill( rawskill );
ReadGameClock();
RecalcVitals( character );
RegisterForSpeechEvents( at_object, range, flags := 0 );
ReleaseItem( item );
RequestInput( character, item, prompt ); // item is a placeholder, just pass any item
ReserveItem( item );
RestartScript( npc );
Resurrect( mobile );
RevokePrivilege( character, privilege );
SaveWorldState();
SelectColor( character, item );
SelectMenuItem2( character, menuname );
SendBuyWindow( character, container, vendor, items );
SendDialogGump( who, layout, textlines );
SendEvent( npc, event );
SendInstaResDialog( character );
SendOpenBook( character, book );
SendOpenSpecialContainer( character, container );
SendPacket( to_whom, packet_hex_string );
SendSellWindow( character, vendor, i1, i2, i3 );
SendSkillWindow( towhom, forwhom );
SendStringAsTipWindow( character, text );
SendSysMessage( character, text, font := _DEFAULT_TEXT_FONT, color := _DEFAULT_TEXT_COLOR );
SendTextEntryGump( who, line1, cancel := TE_CANCEL_ENABLE, style := TE_STYLE_NORMAL, maximum := 40, line2 := "" );
SendViewContainer( character, container );
SetAttributeBaseValue( character, attrname, basevalue_tenths ); // obsoletes SetRawSkill
SetAttributeTemporaryMod( character, attrname, tempmod_tenths ); // obsoletes mob.strength_mod etc
SetGlobalProperty( propname, propval );
SetName( object, name );
SetObjProperty( object, property_name, property_value_string_only );
SetRegionLightLevel( regionname, lightlevel );
SetRegionWeatherLevel( region, type, severity, aux := 0, lightoverride := -1);
SetScriptController( who );
SetVital( character, vitalname, value );
Shutdown();
SpeakPowerWords( who, spellid );
StartSpellEffect( who, spellid );
SubtractAmount( item, amount );
SystemFindObjectBySerial( serial, sysfind_flags := 0 );
Target( by_character, options := TGTOPT_CHECK_LOS+TGTOPT_NEUTRAL);
TargetCoordinates( by_character );
TargetMultiPlacement( character, objtype, flags := 0 );

