function OpenWebSite(who, url)
	var packet := "A5" + WordPacket(Len(url) + 4);

	foreach ascii_code in CAscZ(url)
		packet:=packet+BytePacket(ascii_code);
	endforeach
	SendPacket(who, packet + "00");

	return;
endfunction


function BytePacket(byte)
	var packet := Hex(byte);
	packet := "00" + packet[3, Len(packet) - 2];

	return (packet[Len(packet) - 1, 2]);
endfunction


function WordPacket(word)
	var packet := Hex(word);
	packet := "0000" + packet[3, Len(packet) - 2];

	return (packet[Len(packet) - 3, 4]);
endfunction


function DoubleWordPacket(doubleword)
	var packet := Hex(doubleword);
	packet := "00000000" + packet[3, Len(packet) - 2];

	return (packet[Len(packet) - 7, 8]);
endfunction
