#! /bin/sh
# PenUltima Online Manager
# Original tool by Austin Heilman
# Ported to linux by Axel DominatoR

POL_EXE="pol"
ECOMPILE_EXE="scripts/ecompile"
UOCONVERT_EXE="uoconvert"
MUL_DIR="MUL"

echo
echo -e "\E[31;1mPenUltima Online \E[33mManager \E[0;38mv0.0.1\E[0m"
echo -e "by \E[31mAxel DominatoR ^^^ HC\E[0m aiutato ( suo malgrado ) da \E[36mMatrixina\E[0m ;)"
echo -e "Original made by Austin Heilman, special thanks to him for such a tool!"

Menu_RealmGen()
{
	Build_Britannia()
	{
		echo -e "[ \E[33;1ma\E[0m ] - \E[32mT2A - UOSE Britannia Map\E[0m"
		echo -e "[ \E[33;1mb\E[0m ] - \E[32mMondain's Legacy extended Britannia Map\E[0m"
		echo -n "> "
		read line
		if [ "$line" = "a" ]; then ./$UOCONVERT_EXE map realm=britannia mapid=0 usedif=1 width=6144 height=4096
		elif [ "$line" = "b" ]; then ./$UOCONVERT_EXE map realm=britannia mapid=0 usedif=1 width=7168 height=4096
		else
			Menu_RealmGen
		fi
		./$UOCONVERT_EXE statics realm=britannia
		./$UOCONVERT_EXE maptile realm=britannia
	}

	Build_Britannia_Alt()
	{
		./$UOCONVERT_EXE map     realm=britannia_alt mapid=1 usedif=1 width=6144 height=4096
		./$UOCONVERT_EXE statics realm=britannia_alt
		./$UOCONVERT_EXE maptile realm=britannia_alt
	}

	Build_Ilshenar()
	{
		./$UOCONVERT_EXE map     realm=ilshenar      mapid=2 usedif=1 width=2304 height=1600
		./$UOCONVERT_EXE statics realm=ilshenar
		./$UOCONVERT_EXE maptile realm=ilshenar
	}

	Build_Malas()
	{
		./$UOCONVERT_EXE map     realm=malas         mapid=3 usedif=1 width=2560 height=2048
		./$UOCONVERT_EXE statics realm=malas
		./$UOCONVERT_EXE maptile realm=malas
	}

	Build_Tokuno()
	{
		./$UOCONVERT_EXE map     realm=tokuno        mapid=4 usedif=1 width=1448 height=1448
		./$UOCONVERT_EXE statics realm=tokuno
		./$UOCONVERT_EXE maptile realm=tokuno
	}

	echo ""
	echo -e "============================"
	echo -e "\E[0m[ \E[33;1mRealmGen Menu \E[0m] \E[31;1mPOLManager\E[0m"
	echo -e "============================"
	echo -e "[ \E[33;1ma\E[0m ] - \E[32mCopy needed client files to pol/MUL\E[0m"
	echo ""
	echo -e "[ \E[33;1mb\E[0m ] - \E[32mBuild all needed config files\E[0m"
	echo -e "[ \E[33;1mc\E[0m ] - \E[32mBuild \E[1mlandtiles.cfg\E[0m"
	echo -e "[ \E[33;1md\E[0m ] - \E[32mBuild \E[1mmultis.cfg\E[0m"
	echo -e "[ \E[33;1me\E[0m ] - \E[32mBuild \E[1mtiles.cfg\E[0m"
	echo ""
	echo -e "[ \E[33;1mf\E[0m ] - \E[32mBuild all realms\t\t\E[0m(Takes a very long time!)"
	echo -e "[ \E[33;1mg\E[0m ] - \E[32mBuild \E[1mBritannia\E[0;32m realm\t\t\E[0m(mapid=\E[36;1m0\E[0m)"
	echo -e "[ \E[33;1mh\E[0m ] - \E[32mBuild \E[1mBritannia Alt\E[0;32m realm\t\E[0m(mapid=\E[36;1m1\E[0m)"
	echo -e "[ \E[33;1mi\E[0m ] - \E[32mBuild \E[1mIlshenar\E[0;32m realm\t\t\E[0m(mapid=\E[36;1m2\E[0m)"
	echo -e "[ \E[33;1mj\E[0m ] - \E[32mBuild \E[1mMalas\E[0;32m realm\t\t\E[0m(mapid=\E[36;1m3\E[0m)"
	echo -e "[ \E[33;1mk\E[0m ] - \E[32mBuild \E[1mTokuno\E[0;32m realm\t\t\E[0m(mapid=\E[36;1m4\E[0m)"
	echo ""
	echo -e "[ \E[31;1mx\E[0m ] - \E[31mBack\E[0m"
	echo -n "> "
	read line
	if [ "$line" = "a" ]; then
		echo -n "Path to UO directory> "
		read line
		if [ ! -d "$MUL_DIR" ]; then mkdir "$MUL_DIR"
		fi

		cp "$line/multi.*" "$MUL_DIR"
		cp "$line/map*" "$MUL_DIR"
		cp "$line/staidx*" "$MUL_DIR"
		cp "$line/statics*" "$MUL_DIR"
		cp "$line/tiledata.mul" "$MUL_DIR"
	elif [ "$line" = "b" ]; then
		./$UOCONVERT_EXE landtiles; mv landtiles.cfg config
		./$UOCONVERT_EXE multis; mv multis.cfg config
		./$UOCONVERT_EXE tiles; mv tiles.cfg config
	elif [ "$line" = "c" ]; then ./$UOCONVERT_EXE landtiles; mv landtiles.cfg config
	elif [ "$line" = "d" ]; then ./$UOCONVERT_EXE multis; mv multis.cfg config
	elif [ "$line" = "e" ]; then ./$UOCONVERT_EXE tiles; mv tiles.cfg config
	elif [ "$line" = "f" ]; then Build_Britannia; Build_Britannia_Alt; Build_Ilshenar; Build_Malas; Build_Tokuno
	elif [ "$line" = "g" ]; then Build_Britannia
	elif [ "$line" = "h" ]; then Build_Britannia_Alt
	elif [ "$line" = "i" ]; then Build_Ilshenar
	elif [ "$line" = "j" ]; then Build_Malas
	elif [ "$line" = "k" ]; then Build_Tokuno
	elif [ "$line" = "x" ]; then Menu_Main
	fi

	Menu_RealmGen
}

Menu_Ecompiler()
{
	echo ""
	echo -e "============================="
	echo -e "\E[0m[ \E[33;1mEcompiler Menu \E[0m] \E[31;1mPOLManager\E[0m"
	echo -e "============================="
	echo -e "[ \E[33;1ma\E[0m ] - \E[32mCompile all scripts\E[0m"
	echo -e "[ \E[33;1mb\E[0m ] - \E[32mCompile all scripts and output to ecompile.log\E[0m"
	echo -e "[ \E[33;1mc\E[0m ] - \E[32mCompile updated scripts only\E[0m"
	echo ""
	echo -e "[ \E[33;1md\E[0m ] - \E[32mCompile a directory\E[0m"
	echo -e "[ \E[33;1me\E[0m ] - \E[32mCompile a specific script\E[0m"
	echo ""
	echo -e "[ \E[31;1mx\E[0m ] - \E[31mBack\E[0m"
	echo -n "> "
	read line
	if [ "$line" = "a" ]; then ./$ECOMPILE_EXE -A -b -f
	elif [ "$line" = "b" ]; then  ./$ECOMPILE_EXE -A -b -f >ecompile.log
	elif [ "$line" = "c" ]; then  ./$ECOMPILE_EXE -A -b
	elif [ "$line" = "d" ]; then
		echo -n "Path to directory> "
		read line
		./$ECOMPILE_EXE -b -f -r $line
	elif [ "$line" = "e" ]; then
		echo -n "Path to script> "
		read line
		./$ECOMPILE_EXE $line
	elif [ "$line" = "x" ]; then Menu_Main
	fi

	Menu_Ecompiler
}

Menu_Cleanup()
{
	echo ""
	echo -e "==========================="
	echo -e "\E[0m[ \E[33;1mCleanup Menu \E[0m] \E[31;1mPOLManager\E[0m"
	echo -e "==========================="
	echo -e "[ \E[33;1ma\E[0m ] - \E[32mOne-Shot Cleaner\E[0m\t(Will remove all of the following)"
	echo ""
	echo -e "[ \E[33;1mb\E[0m ] - \E[32mRemove \E[1m*.ecl\E[0;32m files\E[0m\t(Will need to recompile scripts)"
	echo -e "[ \E[33;1mc\E[0m ] - \E[32mRemove \E[1m*.bak\E[0;32m files\E[0m"
	echo -e "[ \E[33;1md\E[0m ] - \E[32mRemove \E[1m*.dep\E[0;32m files\E[0m"
	echo -e "[ \E[33;1me\E[0m ] - \E[32mRemove \E[1m*.log\E[0;32m files\E[0m"
	echo -e "[ \E[33;1mf\E[0m ] - \E[32mRemove \E[1m*.lst\E[0;32m files\E[0m"
	echo -e "[ \E[33;1mg\E[0m ] - \E[32mRemove \E[1m*.dbg\E[0;32m files\E[0m"
	echo ""
	echo -e "[ \E[33;1mh\E[0m ] - \E[32mRemove other files\E[0m\t(Insert file name or wildcard)"
	echo ""
	echo -e "[ \E[31;1mx\E[0m ] - \E[31mBack\E[0m"
	echo -n "> "
	read line
	if [ "$line" = "a" ]; then
		find . -name \*.ecl -exec echo {} \; -exec rm {} \;
		find . -name \*.bak -exec echo {} \; -exec rm {} \;
		find . -name \*.dep -exec echo {} \; -exec rm {} \;
		find . -name \*.log -exec echo {} \; -exec rm {} \;
		find . -name \*.lst -exec echo {} \; -exec rm {} \;
		find . -name \*.dbg -exec echo {} \; -exec rm {} \;
	elif [ "$line" = "b" ]; then find . -name \*.ecl -exec echo {} \; -exec rm {} \;
	elif [ "$line" = "c" ]; then find . -name \*.bak -exec echo {} \; -exec rm {} \;
	elif [ "$line" = "d" ]; then find . -name \*.dep -exec echo {} \; -exec rm {} \;
	elif [ "$line" = "e" ]; then find . -name \*.log -exec echo {} \; -exec rm {} \;
	elif [ "$line" = "f" ]; then find . -name \*.lst -exec echo {} \; -exec rm {} \;
	elif [ "$line" = "g" ]; then find . -name \*.dbg -exec echo {} \; -exec rm {} \;
	elif [ "$line" = "h" ]; then
		echo -n "File Name or Wildcard> "
		read line
		find . -name $line -exec echo {} \; -exec rm {} \;
	elif [ "$line" = "x" ]; then Menu_Main
	fi

	Menu_Cleanup
}

Start_Pol()
{
	clear
	./$POL_EXE
	reset
	clear
	Menu_Main
}

Loop_Pol()
{
	for ((;;))
	do
		clear
		./$POL_EXE
	done
	Menu_Main
}

Quit()
{
	echo -e "\nThanks for using \E[31mPOLManager\E[0m!\n"
	exit
}

Menu_Main()
{
	echo ""
	echo -e "========================"
	echo -e "\E[0m[ \E[33;1mMain Menu \E[0m] \E[31;1mPOLManager\E[0m"
	echo -e "========================"
	echo -e "[ \E[33;1ma\E[0m ] - \E[32mRealmGen menu\E[0m\t\t(Realm building tools)"
	echo -e "[ \E[33;1mb\E[0m ] - \E[32mEcompiler menu\E[0m\t\t(Ecompile tools)"
	echo -e "[ \E[33;1mc\E[0m ] - \E[32mCleanup menu\E[0m\t\t(File removal tools)"
	echo ""
	echo -e "[ \E[33;1md\E[0m ] - \E[32mStart POL.exe\E[0m\t\t(Returns to menu on exit)"
	echo -e "[ \E[33;1me\E[0m ] - \E[32mKeep POL.exe running\E[0m\t(Restarts when it exits. Use CTRL+C to stop)"
	echo ""
	echo -e "[ \E[31;1mx\E[0m ] - \E[31mQuit\E[0m"
	echo -n "> "
	read line
	if [ "$line" = "a" ]; then Menu_RealmGen
	elif [ "$line" = "b" ]; then Menu_Ecompiler
	elif [ "$line" = "c" ]; then Menu_Cleanup
	elif [ "$line" = "d" ]; then Start_Pol
	elif [ "$line" = "e" ]; then Loop_Pol
	elif [ "$line" = "x" ]; then Quit
	else Menu_Main
	fi
}

Menu_Main
