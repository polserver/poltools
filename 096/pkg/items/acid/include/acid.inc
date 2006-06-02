/* $Id$
 *
 */
use uo;
use cfgfile;
use util;

include ":brainAI:npcUtil";
include "include/packets";

/*
 * SpitAced(spitter, spit_on)
 *
 * Purpose
 * Creates an acid tile where the spit_on is and handles dealing initial
 * damage to the spit_on and affects armor if applicable.
 *
 * Parameters
 * spitter:	The npc spitting the acid
 * spit_on:	The mobile the npc is spitting on.
 *
 * Return value
 * Returns an item reference to the tile created.
 *
 */
function SpitAcid(spitter, spit_on)
	if( !CheckLineOfSight(spitter, spit_on) )
		return 0;
	endif
	PlayMovingEffectXYZHued(spitter, spit_on, 0x36d4, 5, 1, 1, 0x47, 0);
	var acid_pool := CreateItemAtLocation(spit_on.x, spit_on.y, GetMapInfo(spit_on.x, spit_on.y).z, "acid",  1, spit_on.realm);
	var dice_dealt := DealAcidDamage(spitter, spit_on, acid_pool);
	if( TypeOf(dice_dealt) == "String" && !GetObjProperty(acid_pool, "%AcidDice%") )
		SetObjProperty(acid_pool, "%AcidDice%", dice_dealt);
	endif
	return acid_pool;
endfunction


/*
 * DealAcidDamage(npc, mobile)
 *
 * Purpose
 * Deals damage to a mob based on an npc's AcidDamage entry in it's npctemplate
 * if 0 is sent for acid_damage. Otherwise uses the Dice Roll sent. Also checks
 * for a cprop on the acid itself. The CProp is used by walkOn/Timer scripts.
 * Also deals damage randomly to the mobile's armor or weapon.
 *
 * Parameters
 * npc:		The npc spitting the acid
 * mobile:	The mobile the npc is spitting on.
 * acid_damage:	Optional. If 0/Not sent, then it defaults to
 *		to the npc's AcidDamage setting in template.
 *
 * Return value
 * Returns dice roll string.
 *
 */
function DealAcidDamage(npc, mobile, acid_pool, acid_damage:=0)
	var acid_dice := GetAcidDiceFromTemplate(npc);
	if( TypeOf(acid_dice) != "String" )
		acid_dice := GetAcidDiceFromItem(acid_pool);
	endif
	if( acid_damage )
		acid_damage := RandomDiceRoll(acid_damage);
	elseif( TypeOf(acid_dice) == "String" )
		acid_damage := RandomDiceRoll(acid_dice);
	else
		return 0;
	endif
	ApplyRawDamage(mobile, acid_damage);
	var equipped_items := ListEquippedItems(mobile);
	if( RandomInt(10) > 5 )
		var acid_burned := equipped_items[RandomInt(equipped_items.Size())+1];
		if( acid_burned.isA(POLCLASS_WEAPON) || acid_burned.isA(POLCLASS_ARMOR) )
			var armor_damage := RandomInt(4) + 1;
			if( acid_burned.hp > armor_damage )
				acid_burned.hp := acid_burned.hp - armor_damage;
				PrintTextAbovePrivate( mobile, "*Acid damages your equipment*", mobile, _DEFAULT_TEXT_FONT, 72);
			else
				DestroyItem(acid_burned);
			endif
		endif
	endif
	return acid_dice;
endfunction


function GetAcidDiceFromTemplate(npc)
	if( npc.isA(POLCLASS_NPC) )
		var npc_info := NPC_GetNPCConfig(npc.npctemplate);
		if( !npc_info.AcidDamage )
			// Can't Deal Acid Damage
			return 0;
		else
			return npc_info.AcidDamage;
		endif
	else
		return 0;
	endif
endfunction


function GetAcidDiceFromItem(acid_pool)
	if( acid_pool.isA(POLCLASS_ITEM) )
		if( TypeOf(GetObjProperty(acid_pool, "%AcidDice%")) == "String" )
			return GetObjProperty(acid_pool, "%AcidDice%");

		else
			// Can't Deal Acid Damage
			return 0;
		endif
	else
		return 0;
	endif
endfunction