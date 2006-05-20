// None of these need to be changed unless the UO client is reprogrammed to use different packet structure...
// So these will never need to be changed.

const MSGTYPE_PARTY := 0xBF;
const SUBCMD_PARTY := 6;

const OFFSET_PARTY_MSGLEN := 1;
const OFFSET_PARTY_SUBCMD := 3;
const OFFSET_PARTY_SUBSUBCMD := 5;