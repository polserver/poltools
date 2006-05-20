use uo;
use os;

var guildinitlayout := {
	"page 0",
	"resizepic 30 30 3000 400 200",
	"text 163 43 5 0",
	"text 50 65 5 1",
	"text 80 85 35 2",
	"text 50 105 5 3",
	"text 80 125 35 4",
	"text 50 145 5 5",
	"text 80 165 35 6",
	"button 230 195 2119 2120 1 0 1",
	"button 165 195 2130 2129 1 0 2"
};

var guildinitdata := {
	"Guild Information",
	"Guild Name:",
	"Guildname",
	"Guild Abbreviation:",
	"Guildabbr",
	"Guildmaster:",
	"Guildmaster"
};
// ResetGuildStone(stone)
//
// This function will initialize a guildstone.  It will wipe all
// existing guild data from the stone, queue all of the guild's
// players so their guild info is removed, and remove the guild from
// the registry.

function ResetGuildStone(stone)

	var i, x;
	var empty := {};

	var guildmembers := GetObjProperty(stone, "MemberSerials");

	SetObjProperty(stone, "GuildName", "NONE");
	SetObjProperty(stone, "GuildAbbr", "NONE");
	SetObjProperty(stone, "GuildAlign", "NONE");
	SetObjProperty(stone, "MemberSerials", empty);
	SetObjProperty(stone, "MemberNames", empty);
	SetObjProperty(stone, "MemberTitles", empty);
	SetObjProperty(stone, "Enemies", empty);
	SetObjProperty(stone, "Allies", empty);
	SetObjProperty(stone, "PendingAction", empty);

	set_critical(1);
	var guildregistry := GetGlobalProperty("GuildRegistry");
	var newregistry := {};
	var newreglen := 0;
	for (i := 1; i <= len(guildregistry); i := i + 1);
		if (stone.serial != guildregistry[i])
			newreglen := newreglen + 1;
			newregistry[newreglen] := guildregistry[i];
		endif
	endfor
	SetGlobalProperty("GuildRegistry", newregistry);
	set_critical(0);

	foreach x in guildmembers
		GuildAction(x.serial, stone, "Disband");
	endforeach

	PrintTextAbove(stone, "Guildstone has been reset.");

endfunction


// GuildAction(destination, source, action)
//
// This function adds a PendingAction to a guildstone.  It can be used
// to declare war, peace, or alliances with other guildstones, (in
// which case destination = target guild's stone serial, source = 
// the declarer's guildstone, and the action = "War", "Peace", or 
// "Ally"), or it can be used to remove or retitle other members of
// the guild  (in which case destination = target's serial, source =
// the stone, and action = "Remove", "Disband", "Add", etc).  If a 
// player is changed, and the player is not online, then the action
// is posted to the global queue.  The function returns 1 on success, 
// 2 if the target was offline, and 0 for failure.

function GuildAction(destination, source, action)

	var i, x;
	var targobj;
	var pendingaction;

	var guildname := GetObjProperty(source, "GuildName");

	if ( (action == "War") || (action == "Peace") || (action == "Ally") )
		targobj := SystemFindObjectBySerial(destination);
		pendingaction := GetObjProperty(targobj, "PendingAction");
		x := len(pendingaction) + 1;
		pendingaction[x] := source.serial;
		x := x + 1;
		pendingaction[x] := action;
		SetObjProperty(targobj, "PendingAction", pendingaction);
		return 1;
	elseif ( (action == "Remove") || (action == "Disband") || (action == "Retitle") || (action == "Add"))
		i := 0;
		foreach x in EnumerateOnlineCharacters()
			if (x.serial == destination)
				targobj := x;
				i := 1;
			endif
		endforeach
		if (i == 1)
			case(action)
				"Remove":
				SendSysMessage(targobj, "You have been removed from " + guildname + "!");
				EraseObjProperty(targobj, "Guild");
				targobj.name := GetObjProperty(targobj, "OldName");
				EraseObjProperty(targobj, "GuildTitle");
				EraseObjProperty(targobj, "OldName");
				return 1;

				"Disband":
				SendSysMessage(targobj, "Your guild, " + guildname + ", has disbanded.");
				EraseObjProperty(targobj, "Guild");
				targobj.name := GetObjProperty(targobj, "OldName");
				EraseObjProperty(targobj, "GuildTitle");
				EraseObjProperty(targobj, "OldName");
				return 1;

				"Retitle":
				SendSysMessage(targobj, "You have been granted a new guild title.  Go to the guildstone and update your information.");
				return 1;

				"Add":
				SendSysMessage(targobj, "You have been recruited into " + guildname + ".");
				SetObjProperty(targobj, "OldName", targobj.name);
				SetObjProperty(targobj, "Guild", source.serial);
				SetObjProperty(targobj, "GuildTitle", "Initiate");
				return 1;

			endcase
		else
			set_critical(1);
			var guildpendingaction := GetGlobalProperty("GuildPendingAction");
			i := len(guildpendingaction) + 1;
			guildpendingaction[x] := destination;
			x := x + 1;
			guildpendingaction[x] := action;
			SetGlobalProperty("GuildPendingAction", guildpendingaction);
			set_critical(0);
			return 2;
		endif	
	else
		print("Error selecting action type.");
		return 0;
	endif

endfunction


// FindPosition(who, stone)
//
// This function will find the array position in the guild data for
// member <who>.  If the person is not in the guild, the function 
// returns a 0.

function FindPosition(who, stone)

	var memberserials := GetObjProperty(stone, "MemberSerials");
	var i;
	var guildslot := 0;

	for (i := 1; i <= len(memberserials); i := i + 1)
		if (memberserials[i] == who.serial)
			guildslot := i;
		endif
	endfor

	return guildslot;

endfunction


// RemoveMember(who, stone)
//
// This function will remove an individual from a guildstone.  In this
// case, <who> is the SERIAL NUMBER of the person to be removed.

function RemoveMember(who, stone)

	var i := 0;
	var x := 0;
	var newserials := {};
	var newnames := {};
	var newtitles := {};

	var memberserials := GetObjProperty(stone, "MemberSerials");
	var membernames := GetObjProperty(stone, "MemberNames");
	var membertitles := GetObjProperty(stone, "MemberTitles");

	for (i := 1; i <= len(memberserials); i := i + 1)
		if (i != who)
			x := x + 1;
			newserials[x] := memberserials[i];
			x := x + 1;
			newnames[x] := membernames[i];
			x := x + 1;
			newtitles[x] := membertitles[i];
		endif
	endfor

	SetObjProperty(stone, "MemberSerials", newserials);
	SetObjProperty(stone, "MemberNames", newnames);
	SetObjProperty(stone, "MemberTitles", newtitles);

	GuildAction(who, stone, "Remove");

endfunction


// AddMember(who, stone)
//
// This function will add a member to a guildstone.  If the player is
// already a member, the function will return 0.  The "title" field
// is optional, and will return "Initiate" if omitted.

function AddMember(who, stone, title := "Initiate")

	if ( FindPosition(who, stone) > 0 )
		PrintTextAbove(stone, "That person is already a member!");
		return 0;
	endif

	var memberserials := GetObjProperty(stone, "MemberSerials");
	var membernames := GetObjProperty(stone, "MemberNames");
	var membertitles := GetObjProperty(stone, "MemberTitles");

	var i := len(memberserials) + 1;

	memberserials[i] := who.serial;
	membernames[i] := who.name;
	membertitles[i] := title;

	SetObjProperty(stone, "MemberSerials", memberserials);
	SetObjProperty(stone, "MemberNames", membernames);
	SetObjProperty(stone, "MemberTitles", membertitles);

	GuildAction(who.serial, stone, "Add");

endfunction


// InitializeGuild(who, guildstone)
//
// This function runs when the guildstone is first placed, or after
// it has been wiped and the guild disbanded.   The first person to
// use the stone after it is placed becomes the guildmaster.  This
// function just sets the name and abbreviation of the guild, and
// sets the title of the guildmaster.   It will then return to the
// main guildstone program, where the guildmaster can add members
// and make adjustments as needed.

function InitializeGuild(who, stone)

	var success := 0;
	var guildname, guildabbr, guildtitle;

	while (success == 0)
		guildname := RequestInput(who, stone, "Enter a your Guild's name (5-40 characters):");
		if ( (len(guildname) > 40) || (len(guildname) < 5) )
			SendSysMessage(who, "Invalid Name.");
		else
			success := 1;
		endif
	endwhile
	guildinitdata[3] := guildname;

	success := 0;
	while (success == 0)
		guildabbr := RequestInput(who, stone, "Enter a your Guild's short name, displayed in your members' titles (3-16 characters):");
		SendSysMessage(who, guildabbr);
		if ( (len(guildabbr) > 16) || (len(guildname) < 3) )
			SendSysMessage(who, "Invalid Name.");
		else
			success := 1;
		endif
	endwhile
	guildinitdata[5] := guildabbr;

	success := 0;
	while (success == 0)
		guildtitle := RequestInput(who, stone, "Enter your title (3-16 characters):");
		SendSysMessage(who, guildtitle);
		if ( (len(guildtitle) > 16) || (len(guildname) < 3) )
			SendSysMessage(who, "Invalid Name.");
		else
			success := 1;
		endif
	endwhile
	guildinitdata[7] := guildtitle;

	SendSysMessage(who, "Is this information correct?");
	var res := SendDialogGump(who, guildinitlayout, guildinitdata);
	if (res[0] == 2)

		set_critical(1);
		var masterlist := GetGlobalProperty("GuildRegistry");
		success := len(masterlist) + 1;
		masterlist[success] := stone.serial;	
		SetGlobalProperty("GuildRegistry", masterlist);
		set_critical(0);

		var memberserials := { who.serial };
		var membernames := { who.name };
		var membertitles := { guildtitle };
		var empty := { };

		SetObjProperty(stone, "GuildName", guildname);
		SetObjProperty(stone, "GuildAbbr", guildabbr);
		SetObjProperty(stone, "GuildAlign", "NONE");
		SetObjProperty(stone, "MemberSerials", memberserials);
		SetObjProperty(stone, "MemberNames", membernames);
		SetObjProperty(stone, "MemberTitles", membertitles);
		SetObjProperty(stone, "GuildEnemies", empty);
		SetObjProperty(stone, "GuildAllies", empty);
		SetObjProperty(stone, "PendingAction", empty);

		SetObjProperty(who, "Guild", stone.serial);
		SetObjProperty(who, "OldName", who.name);
		SetObjProperty(who, "GuildTitle", guildtitle);
		who.name := who.name + ", " + guildtitle + " [" + guildabbr + "]";
		SendSysMessage(who, "Guildstone initialized.");
		return 1;
	else
		return 0;
	endif

endfunction