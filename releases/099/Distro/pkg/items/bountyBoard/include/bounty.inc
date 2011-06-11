/*
        Maintened by *Edwards
        For FantasiaShard.com

        Edwards@FantasiaShard.com

        2008-12-13
        
        Last Modifications 2009-10-25
             * Cleaned codes for better performances
             * Fixed all codes to work now...
*/
use uo;
use os;

include ":datafile:datafile";
include ":timeUtils:time";
include ":security:report";

var g_datafile := DFOpenDataFile( ":bountyBoard:bounty", DF_CREATE );

function GetBounty( mobile )

         var data_elem := DFFindElement( g_datafile, mobile.serial );
         if( !data_elem )
                return 0;
         endif
         
         var holder := array;
                
         var name := data_elem.GetProp( "Name" );
         if( !name )
                  name := mobile.name;
         endif
         holder.Append( name );
                
         var bounty := data_elem.GetProp( "Bounty" );
         if( !bounty )
                  bounty := 0;
         endif
         holder.Append( bounty );

         var murders := data_elem.GetProp( "Murders" );
         if( !murders )
                  murders := 0;
         endif
         holder.Append( murders );

         var expires := data_elem.GetProp( "Expires" );
         if( !expires )
                  expires := 0;
         endif
         holder.Append( expires );
         
         return holder;
endfunction

function SetBounty( mobile, amount )

         var data_elem := DFFindElement( g_datafile, mobile.serial, DF_CREATE );

         data_elem.SetProp( "Name", mobile.name );
         
         var bounty := CInt( data_elem.GetProp( "Bounty" ));
         if( !bounty )
                  bounty := 0;
         endif
         
         data_elem.SetProp( "Bounty", bounty + amount );

         data_elem.SetProp( "Expires", ReadGameClock() + SEC_WEEK );
         
         return 1;
endfunction

function GetMurders( mobile )

         var data_elem := DFFindElement( g_datafile, mobile.serial );
         if( !data_elem )
                return 0;
         endif
         
         var expire := CInt( data_elem.GetProp( "Expires" ));
         if( ReadGameClock() > expire )
                return 0;
         endif
         
         var murders := CInt( data_elem.GetProp( "Murders" ));
         if( !murders )
                  murders := 0;
         endif
         
         return murders;
endfunction

function AddMurder( mobile )

         var data_elem := DFFindElement( g_datafile, mobile.serial, DF_CREATE );

         var murders := CInt( data_elem.GetProp( "Murders" ));
         if( !murders )
                  murders := 0;
         endif
         
         data_elem.SetProp( "Murders", murders + 1 );
         data_elem.SetProp( "Expires", ReadGameClock() + SEC_WEEK );
         
         return 1;
endfunction

function EraseBounty( serial )

         var data_elem := DFFindElement( g_datafile, serial );

         data_elem.EraseProp( "Name" );
         data_elem.EraseProp( "Bounty" );
         data_elem.EraseProp( "Murders" );
         data_elem.EraseProp( "Expires" );

         return 1;
endfunction

function GetBounties()

         var names := g_datafile.keys(),
             data := array;
             
         foreach entry in names
                var data_elem := DFFindElement( g_datafile, entry );

                var holder := array;
                holder.Append( data_elem.GetProp( "Name" ));
                holder.Append( data_elem.GetProp( "Bounty" ));
                holder.Append( data_elem.GetProp( "Murders" ));
                holder.Append( data_elem.GetProp( "Expires" ));
                holder.Append( entry );

                data.Append( holder );
                SleepMS(10);
         endforeach
         
         return data;
endfunction
