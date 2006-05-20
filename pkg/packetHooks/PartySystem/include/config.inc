// Turn on/off debug messages. 1=yes/0=no
const DEBUG := 0;

// This turns Set_Critical() on or off for the handle scripts.
// Normally packet hook scripts are run-to-completion, having Set_Critical( 1 ), will emulate this.
const DEBUG_SET_CRITICAL := 1;

// Object property names. These can be changed.
// Note the leading "#" denotes a property that is not saved during server shutdown
const PARTY_JOINING_PROP := "#JoiningParty";
const PARTY_PROP := "#Party";
const PARTY_LOOT_PROP	:= "#PartyLoot";

// This can be NO MORE than 10, because of the client gump
const PARTY_MAX_SIZE := 10;

// The number of seconds which a user has to accept
const PARTY_INVITE_TIMEOUT := 10;

// Characters must be at least this close to other party members to get stats update packets.
// My client grey's out the other party member's status window after moving 19 tiles away.
const PARTY_STATUS_UPDATE_DISTANCE := 19;

// This font and color are used for the SendSysMessage()s to the clients.
const PARTY_FONT := 3; // normal
const PARTY_FONT_COLOR := 68; // green
