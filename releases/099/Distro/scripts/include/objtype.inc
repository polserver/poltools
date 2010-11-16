/////////////////////////////////////////////////////////////////////////////
//
// include/objtype.inc - Object Type definitions
//
// TODO: consider moving these into include/client.inc
//       or, moving CID_XX constants in here
//
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
//
// 0x0000 to 0x3FFF - "Standard" objects
//
/////////////////////////////////////////////////////////////////////////////

const UOBJ_STONEWALL_NS := 0x005f;
const UOBJ_STONEWALL_EW := 0x0060;
const UOBJ_GUARDS       := 0xa340;
const UOBJ_FISHSTEAK    := 0x097a;

const UOBJ_BOX          := 0x09aa;

const UOBJ_FISH_START   := 0x09cc;
const UOBJ_FISH_END     := 0x09cf;

const UOBJ_SFISH_START  := 0x0dd6;
const UOBJ_SFISH_END    := 0x0dd9;

const UOBJ_KINDLING     := 0x0de1;
const UOBJ_CAMPFIRE     := 0x0de3;
const UOBJ_EMBERS       := 0x0de9;
const UOBJ_ASHES        := 0x0dea;

const UOBJ_BANDAGE      := 0x0e21;
const UOBJ_BLANK_SCROLL := 0x0e34;

const UOBJ_BANKBOX      := 0x0e40;

const UOBJ_DRUM         := 0x0e9c;
const UOBJ_TAMB         := 0x0e9d;
const UOBJ_HARP         := 0x0eb2;
const UOBJ_LUTE         := 0x0eb3;

const UOBJ_SPELLBOOK        := 0x0efa;

const UOBJ_POISON_POTION    := 0x0f0a;
const UOBJ_EMPTY_BOTTLE     := 0x0f0e;

const UOBJ_GEM_START        := 0x0f0f;
const UOBJ_GEM_END          := 0x0f30;

const UOBJ_SHOVEL           := 0x0f39;

const UOBJ_XBOW             := 0x0f4f;

const UOBJ_BLUE_MOONGATE    := 0x0f6c;



const UOBJ_CLAY             := 0xee19;
const UOBJ_SAND             := 0xee18;




const UOBJ_BLACK_PEARL      := 0x0f7a;
const UOBJ_BLOOD_MOSS       := 0x0f7b;
const UOBJ_BRIMSTONE        := 0x0f7f;
const UOBJ_FERTILE_DIRT     := 0x0f81;
const UOBJ_EXECUTIONERS_CAP := 0x0f83;
const UOBJ_GARLIC           := 0x0f84;
const UOBJ_GINSENG          := 0x0f85;
const UOBJ_MANDRAKE_ROOT    := 0x0f86;
const UOBJ_NIGHTSHADE       := 0x0f88;
const UOBJ_OBSIDIAN         := 0x0f89;
const UOBJ_PIG_IRON         := 0x0f8a;
const UOBJ_SULFUROUS_ASH    := 0x0f8c; // CHECKME spelling
const UOBJ_SPIDER_SILK      := 0x0f8d;
const UOBJ_VOLCANIC_ASH     := 0x0f8f;


const UOBJ_DEAD_WOOD    := 0x0f90;

const UOBJ_CLOTH        := 0x0f95;

const UOBJ_THREAD_START := 0x0fa0;
const UOBJ_THREAD_END   := 0x0fa1;

const UOBJ_ANVIL1       := 0x0faf;
const UOBJ_ANVIL2       := 0x0fb0;
const UOBJ_SMALL_FORGE  := 0xfb1;  // see also UOBJ_FORGE_START..UOBJ_FORGE_END
const UOBJ_FORGE1       := 0x19a2;
const UOBJ_FORGE2       := 0x19a6;
const UOBJ_FORGE3       := 0x1982;
const UOBJ_FORGE4       := 0x1996;
const UOBJ_FORGE5       := 0x199a;
const UOBJ_FORGE6       := 0x198e;
const UOBJ_FORGE7       := 0x1992;
const UOBJ_FORGE8       := 0x1986;
const UOBJ_FORGE9       := 0x198a;

const UOBJ_COPPER_KEY   := 0x100e;
const UOBJ_GOLD_KEY     := 0x100f;
const UOBJ_IRON_KEY     := 0x1010;
const UOBJ_KEY_RING     := 0x1011;
const UOBJ_MAGIC_KEY    := 0x1012;
const UOBJ_RUSTY_IRON_KEY := 0x1013;

const UOBJ_CLOCK   := 0x104c;
const UOBJ_CLOCK_FRAME   := 0x104e;
const UOBJ_CLOCK_PARTS   := 0x104f;
const UOBJ_AXLE_AND_GEARS   := 0x1051;
const UOBJ_GEARS   := 0x1053;
const UOBJ_HINGE   := 0x1055;
const UOBJ_SEXTANT   := 0x1057;
const UOBJ_SEXTANT_PARTS   := 0x1059;
const UOBJ_AXLE   := 0x105b;
const UOBJ_SPRINGS   := 0x105d;



const UOBJ_LOOM_START   := 0x105f;
const UOBJ_LOOM_END     := 0x1066;

const UOBJ_DUMMYSWING_NS     := 0x1071;
const UOBJ_DUMMYFREEZE_NS    := 0x1070;
const UOBJ_DUMMYSWING_EW     := 0x1075;
const UOBJ_DUMMYFREEZE_EW    := 0x1074;

const UOBJ_BLOOD        := 0x122d;

const UOBJ_BOW          := 0x13b2;
const UOBJ_HEAVY_XBOW   := 0x13fd;

const UOBJ_MAP1         := 0x14eb;
const UOBJ_MAP2         := 0x14ec;
const UOBJ_ROLLED_MAP1  := 0x14ed;
const UOBJ_ROLLED_MAP2  := 0x14ee;

const UOBJ_HOUSE_DEED1  := 0x14ef;
const UOBJ_SHIP_DEED1   := 0x14f1;
const UOBJ_SHIP_DEED2   := 0x14f2;

const UOBJ_CHEST        := 0x1940;

const UOBJ_FORGE_START  := 0x197a;  // see also UOBJ_SMALL_FORGE
const UOBJ_FORGE_END    := 0x19a9;

const UOBJ_FEATHER      := 0x1bd1;
const UOBJ_SHAFTS       := 0x1bd4;
const UOBJ_LOGS         := 0x1bdd;

const UOBJ_GLASS        := 0xee20;

const UOBJ_ARM          := 0x1cec;
const UOBJ_LEG          := 0x1ce2;
const UOBJ_TRUNK        := 0x1cee;
const UOBJ_HEART        := 0x1cef;
const UOBJ_HEAD         := 0x1dae;

const UOBJ_TINKER_TOOLS := 0x1ebc;


const UOBJ_RUNE         := 0x1f14;

const UOBJ_CORPSE       := 0x2006;

const UOBJ_PARAFIELD_NS := 0x3967;
const UOBJ_PARAFIELD_EW := 0x3979;
const UOBJ_POISONFIELD_NS   := 0x3914;
const UOBJ_POISONFIELD_EW   := 0x3920;
const UOBJ_EFIELD_NS        := 0x3947;
const UOBJ_EFIELD_EW        := 0x3956;

const GID_PLANK_EXTENDED_FACING_WEST   := 0x3ed5;
const GID_PLANK_RETRACTED_FACING_WEST  := 0x3eb1;
const GID_PLANK_EXTENDED_FACING_EAST   := 0x3ed4;
const GID_PLANK_RETRACTED_FACING_EAST  := 0x3eb2;
const GID_PLANK_EXTENDED_FACING_SOUTH  := 0x3e84;
const GID_PLANK_RETRACTED_FACING_SOUTH := 0x3e85;
const GID_PLANK_EXTENDED_FACING_NORTH  := 0x3e89;
const GID_PLANK_RETRACTED_FACING_NORTH := 0x3e8a;

// ick! these are fine for humans, but not true for items..
// these are more of a CID.
const UOBJ_HUMAN_MALE   := 0x190;
const UOBJ_HUMAN_FEMALE := 0x191;

/////////////////////////////////////////////////////////////////////////////
//
// 0x4000 to 0x4FFF - Multis
//
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
//
// 0x5000 to 0xEFFF - To Be Determined
//
/////////////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////////
//
// 0xF000 to 0xFFFF - Reserved by system
//
/////////////////////////////////////////////////////////////////////////////

	//
	// Ship Components
	//
const EXTOBJ_TILLERMAN       := 0xf010;
const EXTOBJ_PORT_PLANK      := 0xf011;
const EXTOBJ_STARBOARD_PLANK := 0xf012;
const EXTOBJ_HOLD            := 0xf013;
