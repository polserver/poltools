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
const UOBJ_STONEWALL_EW := 0x0057;

const UOBJ_FISHSTEAK    := 0x097A;

const UOBJ_BOX          := 0x09aa;

const UOBJ_FISH_START   := 0x09CC;
const UOBJ_FISH_END     := 0x09CF;

const UOBJ_SFISH_START  := 0x0DD6;
const UOBJ_SFISH_END    := 0x0DD9;

const UOBJ_KINDLING     := 0x0DE1;
const UOBJ_CAMPFIRE     := 0x0DE3;   
const UOBJ_EMBERS       := 0x0DE9;       
const UOBJ_ASHES        := 0x0DEA;

const UOBJ_BANDAGE      := 0x0E21;
const UOBJ_BLANK_SCROLL := 0x0E34;

const UOBJ_BANKBOX      := 0x0E7C;

const UOBJ_DRUM         := 0x0E9C;
const UOBJ_TAMB         := 0x0E9D;
const UOBJ_HARP         := 0x0EB2;
const UOBJ_LUTE         := 0x0EB3;

const UOBJ_SPELLBOOK        := 0x0EFA;

const UOBJ_POISON_POTION    := 0x0F0A;
const UOBJ_EMPTY_BOTTLE     := 0x0F0E;

const UOBJ_GEM_START        := 0x0F0F;
const UOBJ_GEM_END          := 0x0f30;

const UOBJ_SHOVEL           := 0x0F39;

const UOBJ_XBOW             := 0x0F4F;

const UOBJ_BLUE_MOONGATE    := 0x0F6C;



const UOBJ_CLAY             := 0xee19;
const UOBJ_SAND             := 0xee18;




const UOBJ_BLACK_PEARL      := 0x0F7A;
const UOBJ_BLOOD_MOSS       := 0x0F7B;
const UOBJ_BRIMSTONE        := 0x0F7F;
const UOBJ_FERTILE_DIRT     := 0x0F81;
const UOBJ_EXECUTIONERS_CAP := 0x0F83;
const UOBJ_GARLIC           := 0x0F84;
const UOBJ_GINSENG          := 0x0F85;
const UOBJ_MANDRAKE_ROOT    := 0x0F86;
const UOBJ_NIGHTSHADE       := 0x0F88;
const UOBJ_OBSIDIAN         := 0x0F89;
const UOBJ_PIG_IRON         := 0x0F8A;
const UOBJ_SULFUROUS_ASH    := 0x0F8C; // CHECKME spelling
const UOBJ_SPIDER_SILK      := 0x0F8D;
const UOBJ_VOLCANIC_ASH     := 0x0F8F;


const UOBJ_DEAD_WOOD    := 0x0F90;

Const UOBJ_CLOTH        := 0x0F95;         

const UOBJ_THREAD_START := 0x0FA0;            
const UOBJ_THREAD_END   := 0x0FA1;

const UOBJ_ANVIL1       := 0x0FAF;
const UOBJ_ANVIL2       := 0x0FB0;
const UOBJ_SMALL_FORGE  := 0x0FB1;  // see also UOBJ_FORGE_START..UOBJ_FORGE_END

const UOBJ_COPPER_KEY   := 0x100E;
const UOBJ_GOLD_KEY     := 0x100F;
const UOBJ_IRON_KEY     := 0x1010;
const UOBJ_KEY_RING     := 0x1011;
const UOBJ_MAGIC_KEY    := 0x1012;
const UOBJ_RUSTY_IRON_KEY := 0x1013;

const UOBJ_CLOCK   := 0x104c;
const UOBJ_CLOCK_FRAME   := 0x104e   ;
const UOBJ_CLOCK_PARTS   := 0x104f   ;
const UOBJ_AXLE_AND_GEARS   := 0x1051;
const UOBJ_GEARS   := 0x1053;
const UOBJ_HINGE   := 0x1055;
const UOBJ_SEXTANT   := 0x1057;
const UOBJ_SEXTANT_PARTS   := 0x1059;
const UOBJ_AXLE   := 0x105b;
const UOBJ_SPRINGS   := 0x105d;



const UOBJ_LOOM_START   := 0x105F;
const UOBJ_LOOM_END     := 0x1066;            

const UOBJ_DUMMYSWING_NS     := 0x1071;
const UOBJ_DUMMYFREEZE_NS    := 0x1070;
const UOBJ_DUMMYSWING_EW     := 0x1075;
const UOBJ_DUMMYFREEZE_EW    := 0x1074;

const UOBJ_BLOOD        := 0x122D;

const UOBJ_BOW          := 0x13B2;
const UOBJ_HEAVY_XBOW   := 0x13FD;
const UOBJ_FIRE_BOW     := 0x6050;

const UOBJ_MAP1         := 0x14EB;
const UOBJ_MAP2         := 0x14EC;
const UOBJ_ROLLED_MAP1  := 0x14ED;
const UOBJ_ROLLED_MAP2  := 0x14EE;

const UOBJ_HOUSE_DEED1  := 0x14ef;
const UOBJ_SHIP_DEED1   := 0x14F1;
const UOBJ_SHIP_DEED2   := 0x14F2;

const UOBJ_CHEST        := 0x1940;

const UOBJ_FORGE_START  := 0x197A;  // see also UOBJ_SMALL_FORGE          
const UOBJ_FORGE_END    := 0x19A9;

const UOBJ_FEATHER      := 0x1BD1;
const UOBJ_SHAFTS       := 0x1BD4;
const UOBJ_LOGS         := 0x1BDD;

const UOBJ_GLASS        := 0xee20;

const UOBJ_ARM          := 0x1CEC;
const UOBJ_LEG          := 0x1CE2;
const UOBJ_TRUNK        := 0x1CEE;
const UOBJ_HEART        := 0x1CEF;
const UOBJ_HEAD         := 0x1DAE;

const UOBJ_TINKER_TOOLS := 0x1EBC;


const UOBJ_RUNE         := 0x1F14;

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

const UOBJ_SHOE_START       := 0x170B;
const UOBJ_SHOE_END         := 0x1712;

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

	//
	// TODO: reserve some of these ranges for weapons, armor, wands
	//       explicitly leave an area for end users to play with
	//

// recommendation: start these out somewhere like 0x6300, and reserve
// enough space for N (say, 64) colors of ore and ingots
// they don't all have to be defined yet, just reserved.
const UOBJ_IRON_ORE 		:= 0x6011; 
const UOBJ_TALC_ORE 		:= 0x6019;
const UOBJ_MUSCOVITY_ORE 	:= 0x601a;
const UOBJ_CALCITY_ORE	 	:= 0x601b;
const UOBJ_BRONZE_ORE 	        := 0x6012;
const UOBJ_ICEROCK_ORE 		:= 0x601c;
const UOBJ_FLUORITE_ORE 	:= 0x601d;
const UOBJ_DULLCOPPER_ORE 	:= 0x6013;
const UOBJ_PLATINUM_ORE 	:= 0x601e;
const UOBJ_PYROXENE_ORE		:= 0x601f;
const UOBJ_ORTHOCLASE_ORE	:= 0x6020;
const UOBJ_COPPER_ORE 	 	:= 0x6014;
const UOBJ_KYANITE_ORE  	:= 0x6021;
const UOBJ_SPECTRAL_ORE  	:= 0x6022;
const UOBJ_SILLIMANITE_ORE 	:= 0x6023;
const UOBJ_ONYX_ORE 		:= 0x6015;
const UOBJ_OLIVINE_ORE	 	:= 0x6024;
const UOBJ_PYRITE_ORE 	        := 0x6016;
const UOBJ_QUARTZ_ORE 	        := 0x6025;
const UOBJ_MALACHITE_ORE 	:= 0x6017;
const UOBJ_GARNET_ORE	        := 0x6026;
const UOBJ_AZURITE_ORE	 	:= 0x6018;
const UOBJ_TOURMALINE_ORE 	:= 0x6027;
const UOBJ_TOPAZ_ORE 	        := 0x6028;
const UOBJ_AUGITE_ORE 	        := 0x6029;
const UOBJ_HEMATITE_ORE 	:= 0x602a;
const UOBJ_DOLERITE_ORE 	:= 0x602b;
const UOBJ_OBSIDEAN_ORE 	:= 0x602c;
const UOBJ_CORUNDUM_ORE 	:= 0x602d;
const UOBJ_NEXUS_ORE 	        := 0x602e;
const UOBJ_TITAN_ORE 	        := 0x602f;
                                        
                                     
const UOBJ_INGOTS_START   	:= 0x6031;       
const UOBJ_IRON_INGOT 		:= 0x6031;
const UOBJ_TALC_INGOT 		:= 0x6039;
const UOBJ_MUSCOVITY_INGOT 	:= 0x603a;
const UOBJ_CALCITY_INGOT 	:= 0x603b;
const UOBJ_BRONZE_INGOT 	:= 0x6032; 
const UOBJ_ICEROCK_INGOT 	:= 0x603c;
const UOBJ_FLUORITE_INGOT 	:= 0x603d;
const UOBJ_DULLCOPPER_INGOT 	:= 0x6033;
const UOBJ_PLATINUM_INGOT 	:= 0x603e;
const UOBJ_PYROXENE_INGOT	:= 0x603f;
const UOBJ_ORTHOCLASE_INGOT	:= 0x6040;
const UOBJ_COPPER_INGOT  	:= 0x6034;
const UOBJ_KYANITE_INGOT  	:= 0x6041;
const UOBJ_SPECTRAL_INGOT  	:= 0x6042;
const UOBJ_SILLIMANITE_INGOT 	:= 0x6043;
const UOBJ_ONYX_INGOT 		:= 0x6035;
const UOBJ_OLIVINE_INGOT 	:= 0x6044;
const UOBJ_PYRITE_INGOT 	:= 0x6036;
const UOBJ_QUARTZ_INGOT 	:= 0x6045;
const UOBJ_MALACHITE_INGOT 	:= 0x6037;
const UOBJ_GARNET_INGOT		:= 0x6046;
const UOBJ_AZURITE_INGOT 	:= 0x6038;
const UOBJ_TOURMALINE_INGOT 	:= 0x6047;
const UOBJ_TOPAZ_INGOT	 	:= 0x6048;
const UOBJ_AUGITE_INGOT 	:= 0x6049;
const UOBJ_HEMATITE_INGOT 	:= 0x604a;
const UOBJ_DOLERITE_INGOT 	:= 0x604b;
const UOBJ_OBSIDEAN_INGOT 	:= 0x604c;
const UOBJ_CORUNDUM_INGOT 	:= 0x604d;
const UOBJ_NEXUS_INGOT	 	:= 0x604e;
const UOBJ_TITAN_INGOT	 	:= 0x604f;
const UOBJ_INGOTS_END      	:= 0x604f;   
Const UOBJ_GOLD_INGOT     	:= 0x1be9;

//Darko
const UOBJ_ELVEN_LOG                          := 0x7051;
const UOBJ_MOONBEAM_LOG       	              := 0x7052;
const UOBJ_OAK_LOG          	              := 0x7053;
const UOBJ_WILLOW_LOG	     	              := 0x7054;
const UOBJ_SILVER_LOG          	              := 0x7055;
const UOBJ_JUNGLE_LOG   		      := 0x7056;
const UOBJ_AVATAR_LOG          	              := 0x7057;


const UOBJ_OAK_BOW  	   	              := 0x7061;
const UOBJ_WILLOW_XBOW       	              := 0x7062;
const UOBJ_JUNGLE_HEAVY_XBOW                  := 0x7063;
const UOBJ_MAGIC1_WILLOW_XBOW       	      := 0x7064;
const UOBJ_MAGIC2_WILLOW_XBOW       	      := 0x7065;
const UOBJ_MAGIC3_WILLOW_XBOW       	      := 0x7066;


/////////////////////////////////////////////////////////////////////////////
//
// 0xF000 to 0xFFFF - Reserved by system
//
/////////////////////////////////////////////////////////////////////////////

	//
	// Ship Components
	//
const EXTOBJ_TILLERMAN       := 0xF010;
const EXTOBJ_PORT_PLANK      := 0xF011;
const EXTOBJ_STARBOARD_PLANK := 0xF012;
const EXTOBJ_HOLD            := 0xF013;

