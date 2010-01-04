// CIDADES IDS
Const BRITAIN   := 1;
Const YEW       := 2;
Const SKARA     := 3;
Const MAGINCIA  := 4;
Const MOONGLOW  := 5;
Const BUC       := 6;
Const NUJELM    := 7;
Const OCCLO     := 8;
Const MINOC     := 9;
Const VESPER    := 10;
Const COVE      := 11;
Const TRINSIC   := 12;
Const SERPENT   := 13;
Const JHELOM    := 14;
Const WIND		:= 15;
Const PAPUA		:= 16;
Const DELUCIA	:= 17;

Function Guarded(byref chr)
  //  1244 - 1530 - 1704 - 1758
    If ( chr.x>=1244 and chr.y>=1510 and chr.x <= 1704 and chr.y <= 1758)
        If ( chr.x>=1210 and chr.y>=1465 and chr.x <= 1420 and chr.y <= 1152)
            return 0;
        endif
        Return BRITAIN;     // britain

	Elseif ( chr.x>=1296 and chr.y>=1560 and chr.x <= 1406 and chr.y <= 1694)
        Return BRITAIN;     // LB Castle

	Elseif ( chr.x>=1500 and chr.y>=1400 and chr.x <= 1550 and chr.y <= 1480)
        Return BRITAIN;     // LBT Castle

	Elseif ( chr.x>=4379 and chr.y>=1045 and chr.x <= 4487 and chr.y <= 1178)
        Return MOONGLOW;     // moonglow

    Elseif ( chr.x>=5638 and chr.y>=3083 and chr.x<=5799 and chr.y<=3328)
        Return PAPUA;   // papua
      
    Elseif ( chr.x>=5799 and chr.y>=3121 and chr.x<=5822 and chr.y<=3327)
        Return PAPUA;   // papua
      
    Elseif ( chr.x>=5137 and chr.y>=3948 and chr.x<=5196 and chr.y<=4082)
        Return DELUCIA; // delucia
      
    Elseif ( chr.x>=5196 and chr.y>=3923 and chr.x<=5322 and chr.y<=4085)
        Return DELUCIA; // delucia

    Elseif ( chr.x>=1294 and chr.y>=3664 and chr.x <= 1506 and chr.y <= 3886)
        if ( chr.x>=1384 and chr.y>=3728 and chr.x <= 1414 and chr.y <= 3758)   // arena
            return 0;
        endif
        Return JHELOM;  // jhelom

    Elseif ( chr.x>=494 and chr.y>=926 and chr.x <= 600 and chr.y <= 1034)
        Return YEW;    //Yew

    Elseif ( chr.x>=590 and chr.y>=796 and chr.x <= 670 and chr.y <= 886)
        Return YEW;     //Empath Abbey

    Elseif ( chr.x>=2394 and chr.y>=370 and chr.x <= 2624 and chr.y <= 632)
        Return MINOC;     // minoc

    Elseif ( chr.x>=1794 and chr.y>=2638 and chr.x <= 2136 and chr.y <= 2902)
        Return TRINSIC;     // trinsic

    Elseif ( chr.x>=538 and chr.y>=2106 and chr.x <= 686 and chr.y <= 2274)
        Return SKARA;     // skara brae

    Elseif ( chr.x>=3646 and chr.y>=2040 and chr.x <= 3814 and chr.y <= 2280)
        Return MAGINCIA;     // magincia

    Elseif ( chr.x>=3588 and chr.y>=2446 and chr.x <= 3762 and chr.y <= 2690)
        Return OCCLO;     // occlo

    Elseif ( chr.x>=2612 and chr.y>=2082 and chr.x <= 2772 and chr.y <= 2256)
        Return BUC;     // buc

    Elseif ( chr.x>=3496 and chr.y>=1062 and chr.x <= 3808 and chr.y <= 1400)
        Return NUJELM;     // nujelm

    Elseif ( chr.x>=2794 and chr.y>=634 and chr.x <= 3026 and chr.y <= 1000)
        Return VESPER;     // vesper

    Elseif ( chr.x>=2206 and chr.y>=1116 and chr.x <= 2290 and chr.y <= 1236)
        Return COVE;     // cove

    Elseif ( chr.x>=5120 and chr.y>=0 and chr.x <= 5374 and chr.y <= 254)
        Return WIND;     // wind
      
    Else
        Return 0;
Endif

Return 0;
EndFunction
