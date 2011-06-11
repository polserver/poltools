/*
        Maintened by *Edwards
        
        2008-06-01
        
        Thanks to ZuluHotel for the original Idea and also the development of PolServer
        For this unique "base" race system :)
*/
use uo;
use os;

include ":races:races";

program setup_Race( mobile, race )

        var setting := RACE_GetSettingsCfgFile();
        if( setting.errortext )
                  SysLog( "Error cannot read races.cfg --> "+setting.errortext );
                  return 0;
        endif
        
        return 1;
endprogram
