/////////////////////////////////////////////////////////////////////////////
//
// include/objtype.inc - Object Type definitions
//
// TODO: consider moving these into include/client.inc
//       or, moving CID_XX CONSTants in here
//
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
//
// 0x0000 to 0x3FFF - "Standard" objects
//
/////////////////////////////////////////////////////////////////////////////

CONST UOBJ_STONEWALL_NS := 0x005f;
CONST UOBJ_STONEWALL_EW := 0x0060;
CONST UOBJ_GUARDS       := 0xa340;
CONST UOBJ_FISHSTEAK    := 0x097a;

CONST UOBJ_BOX          := 0x09aa;

CONST UOBJ_FISH_START   := 0x09cc;
CONST UOBJ_FISH_END     := 0x09cf;

CONST UOBJ_SFISH_START  := 0x0dd6;
CONST UOBJ_SFISH_END    := 0x0dd9;

CONST UOBJ_KINDLING     := 0x0de1;
CONST UOBJ_CAMPFIRE     := 0x0de3;
CONST UOBJ_EMBERS       := 0x0de9;
CONST UOBJ_ASHES        := 0x0dea;

CONST UOBJ_BANDAGE      := 0x0e21;
CONST UOBJ_BLANK_SCROLL := 0x0e34;

CONST UOBJ_BANKBOX      := 0x0e40;

CONST UOBJ_DRUM         := 0x0e9c;
CONST UOBJ_TAMB         := 0x0e9d;
CONST UOBJ_HARP         := 0x0eb2;
CONST UOBJ_LUTE         := 0x0eb3;

CONST UOBJ_SPELLBOOK        := 0x0efa;

CONST UOBJ_POISON_POTION    := 0x0f0a;
CONST UOBJ_EMPTY_BOTTLE     := 0x0f0e;

CONST UOBJ_GEM_START        := 0x0f0f;
CONST UOBJ_GEM_END          := 0x0f30;

CONST UOBJ_SHOVEL           := 0x0f39;

CONST UOBJ_XBOW             := 0x0f4f;

CONST UOBJ_BLUE_MOONGATE    := 0x0f6c;



CONST UOBJ_CLAY             := 0xee19;
CONST UOBJ_SAND             := 0xee18;




CONST UOBJ_BLACK_PEARL      := 0x0f7a;
CONST UOBJ_BLOOD_MOSS       := 0x0f7b;
CONST UOBJ_BRIMSTONE        := 0x0f7f;
CONST UOBJ_FERTILE_DIRT     := 0x0f81;
CONST UOBJ_EXECUTIONERS_CAP := 0x0f83;
CONST UOBJ_GARLIC           := 0x0f84;
CONST UOBJ_GINSENG          := 0x0f85;
CONST UOBJ_MANDRAKE_ROOT    := 0x0f86;
CONST UOBJ_NIGHTSHADE       := 0x0f88;
CONST UOBJ_OBSIDIAN         := 0x0f89;
CONST UOBJ_PIG_IRON         := 0x0f8a;
CONST UOBJ_SULFUROUS_ASH    := 0x0f8c; // CHECKME spelling
CONST UOBJ_SPIDER_SILK      := 0x0f8d;
CONST UOBJ_VOLCANIC_ASH     := 0x0f8f;

CONST UOBJ_DEAD_WOOD    := 0x0f90;

CONST UOBJ_CLOTH        := 0x0f95;
CONST UOBJ_CLOTH_STARTS := 0x175d;
CONST UOBJ_CLOTH_END    := 0x1768;
CONST UOBJ_LOOM_START   := 0x105f;
CONST UOBJ_LOOM_END     := 0x1066;
CONST UOBJ_BOLT_START   := 0x0f95;
CONST UOBJ_BOLT_END     := 0x0f9c;
CONST UOBJ_THREAD_START := 0x0fa0;
CONST UOBJ_THREAD_END   := 0x0fa1;
CONST UOBJ_MUTLI_CLOTH  := 0x1766;
CONST UOBJ_SEWING_KIT   := 0x0f9d;

CONST UOBJ_ANVIL1       := 0x0faf;
CONST UOBJ_ANVIL2       := 0x0fb0;
CONST UOBJ_SMALL_FORGE  := 0xfb1;  // see also UOBJ_FORGE_START..UOBJ_FORGE_END
CONST UOBJ_FORGE1       := 0x19a2;
CONST UOBJ_FORGE2       := 0x19a6;
CONST UOBJ_FORGE3       := 0x1982;
CONST UOBJ_FORGE4       := 0x1996;
CONST UOBJ_FORGE5       := 0x199a;
CONST UOBJ_FORGE6       := 0x198e;
CONST UOBJ_FORGE7       := 0x1992;
CONST UOBJ_FORGE8       := 0x1986;
CONST UOBJ_FORGE9       := 0x198a;

CONST UOBJ_COPPER_KEY   := 0x100e;
CONST UOBJ_GOLD_KEY     := 0x100f;
CONST UOBJ_IRON_KEY     := 0x1010;
CONST UOBJ_KEY_RING     := 0x1011;
CONST UOBJ_MAGIC_KEY    := 0x1012;
CONST UOBJ_RUSTY_IRON_KEY := 0x1013;

CONST UOBJ_CLOCK   := 0x104c;
CONST UOBJ_CLOCK_FRAME   := 0x104e;
CONST UOBJ_CLOCK_PARTS   := 0x104f;
CONST UOBJ_AXLE_AND_GEARS   := 0x1051;
CONST UOBJ_GEARS   := 0x1053;
CONST UOBJ_HINGE   := 0x1055;
CONST UOBJ_SEXTANT   := 0x1057;
CONST UOBJ_SEXTANT_PARTS   := 0x1059;
CONST UOBJ_AXLE   := 0x105b;
CONST UOBJ_SPRINGS   := 0x105d;

CONST UOBJ_TDUMMYSWING_NS     := 0x1071;
CONST UOBJ_TDUMMYFREEZE_NS    := 0x1070;
CONST UOBJ_TDUMMYSWING_EW     := 0x1075;
CONST UOBJ_TDUMMYFREEZE_EW    := 0x1074;

CONST UOBJ_PDUMMYSWING_EW       := 0x1EC1;
CONST UOBJ_PDUMMYFREEZE_EW      := 0x1EC0;
CONST UOBJ_PDUMMYSWING_NS       := 0x1EC4;
CONST UOBJ_PDUMMYFREEZE_NS      := 0x1EC3;

CONST UOBJ_BLOOD        := 0x122d;

CONST UOBJ_BOW          := 0x13b2;
CONST UOBJ_HEAVY_XBOW   := 0x13fd;

CONST UOBJ_MAP1         := 0x14eb;
CONST UOBJ_MAP2         := 0x14ec;
CONST UOBJ_ROLLED_MAP1  := 0x14ed;
CONST UOBJ_ROLLED_MAP2  := 0x14ee;

CONST UOBJ_HOUSE_DEED1  := 0x14ef;
CONST UOBJ_SHIP_DEED1   := 0x14f1;
CONST UOBJ_SHIP_DEED2   := 0x14f2;

CONST UOBJ_CHEST        := 0x1940;

CONST UOBJ_FORGE_START  := 0x197a;  // see also UOBJ_SMALL_FORGE
CONST UOBJ_FORGE_END    := 0x19a9;

CONST UOBJ_FEATHER      := 0x1bd1;
CONST UOBJ_SHAFTS       := 0x1bd4;
CONST UOBJ_LOGS         := 0x1bdd;

CONST UOBJ_GLASS        := 0xee20;

CONST UOBJ_ARM          := 0x1cec;
CONST UOBJ_LEG          := 0x1ce2;
CONST UOBJ_TRUNK        := 0x1cee;
CONST UOBJ_HEART        := 0x1cef;
CONST UOBJ_HEAD         := 0x1dae;

CONST UOBJ_TINKER_TOOLS := 0x1ebc;


CONST UOBJ_RUNE         := 0x1f14;

CONST UOBJ_CORPSE       := 0x2006;

CONST UOBJ_PARAFIELD_NS := 0x3967;
CONST UOBJ_PARAFIELD_EW := 0x3979;
CONST UOBJ_POISONFIELD_NS   := 0x3914;
CONST UOBJ_POISONFIELD_EW   := 0x3920;
CONST UOBJ_EFIELD_NS        := 0x3947;
CONST UOBJ_EFIELD_EW        := 0x3956;

CONST GID_PLANK_EXTENDED_FACING_WEST   := 0x3ed5;
CONST GID_PLANK_RETRACTED_FACING_WEST  := 0x3eb1;
CONST GID_PLANK_EXTENDED_FACING_EAST   := 0x3ed4;
CONST GID_PLANK_RETRACTED_FACING_EAST  := 0x3eb2;
CONST GID_PLANK_EXTENDED_FACING_SOUTH  := 0x3e84;
CONST GID_PLANK_RETRACTED_FACING_SOUTH := 0x3e85;
CONST GID_PLANK_EXTENDED_FACING_NORTH  := 0x3e89;
CONST GID_PLANK_RETRACTED_FACING_NORTH := 0x3e8a;

// ick! these are fine for humans, but not true for items..
// these are more of a CID.
CONST UOBJ_HUMAN_MALE   := 0x190;
CONST UOBJ_HUMAN_FEMALE := 0x191;

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
CONST EXTOBJ_TILLERMAN       := 0xf010;
CONST EXTOBJ_PORT_PLANK      := 0xf011;
CONST EXTOBJ_STARBOARD_PLANK := 0xf012;
CONST EXTOBJ_HOLD            := 0xf013;
