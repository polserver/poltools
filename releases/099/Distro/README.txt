                              README

                            POL Server
                        Escript Environment
                 POL Core099 BETA - POL Distro099


POL is an Ultima Online® emulator that has been in development for over 
12 years and supports most of the features that players have enjoyed 
from Ultima Online: The Second Age® to Ultima Online: Mondain's Legacy®. 
POL is the most flexible and customizable UO emulator around.


=======================================================================
            How To Create Your Own Shard by Kirin 12/13/2006
          Modifications for POL DIstro099 by Edwards 4/29/2011
=======================================================================

Greetings friends !

Thank you to the community who has helped me create my own Shard, now 
I will hope to enlighten all those who seek to create a shard of their 
own. This guide assumes you start with nothing, and know nothing. 
Questions and/or comments are greatly appreciated though I don't 
promise to be prompt on response.

-----------------------------------------------------------------------
Step 1: Creating the Directory and Downloading the necessary files
-----------------------------------------------------------------------

      --> Create a folder somewhere easily reached: i.e.: C:\MyShard

      --> Make sure to save all of the following in this folder !

      --> Download the latest UO client here: 
		ftp://largedownloads.ea.com/pub/uo/setup-1.46.0.3.exe

      --> Download the POL Distro099 project here:                						link

      --> Download UORice here:
		http://stud4.tuwien.ac.at/%7Ee9425109/UO_RICE.zip

      * Do not unzip/install anything yet.

-----------------------------------------------------------------------
Step 2: Extracting/Installing files and Fleshing out the 'MyShard' Directory
-----------------------------------------------------------------------

      --> Unzip the following to your 'MyShard' folder:
          - POL Distro099 project
          - UORice

      --> Make a folder called 'Original Downloads':
	  Or something as equally bland and obvious. Insert all of 
          the .zip files into it so you have a reference in case 
          problems occur ( this is a sure thing my friends! ).

      --> Is UO done being a stupidly large download ??:
          It is! ok good, go ahead and install it to it's default 
          path (C:\Program Files). When this is done move the setup file 
          to the 'Original Downloads' folder for archival purposes.

      --> Open the UORice folder and double click the executable:
	  You've just removed the client side encryption from UO ! 
          awesome work. Navigate to UO directory:

              C:\Program Files\EA Games\Ultima Online Mondain's Legacy 

          and look for two icons 'No_Crypt_Client_2d' + 'No_Crypt_Client_3d'. 
          if you see them then UORice has done it's job!! Lets clean up 
          our work space and delete the 'UORice' folder from our 'MyShard' 
          folder (If for some reason you need it again .. we have the 
          zip file stored!)

-----------------------------------------------------------------------
Step 3: Compiling, Converting and Perhaps.. Utter Confusion!
-----------------------------------------------------------------------

Ok now lets begin compiling the scripts that will maintain the various 
aspects/mechanics of your shard and help setup your 'realm's!

      --> Navigate to the 'scripts' folder:
          In your 'MyShard/scripts' directory. You'll see a file 
          named 'ecompile.cfg'. Go ahead and open this in notepad. The 
          first five lines tell the program 'ecompile' where to look 
          for the various .src files to compile, it will look something 
          like this:

ModuleDirectory    	c:\MyShard\pol\scripts\modules
IncludeDirectory        c:\MyShard\pol\scripts
PolScriptRoot        	c:\MyShard\pol\scripts
PackageRoot      	c:\MyShard\pol\pkg
PackageRoot      	c:\MyShard\pol\devpkg
GenerateListing      	1
GenerateDebugInfo   	1
GenerateDebugTextInfo   1
DisplayWarnings 	1
CompileAspPages 	1
AutoCompileByDefault 	1
UpdateOnlyOnAutoCompile 1
OnlyCompileUpdatedScripts 1
DisplaySummary 		1
GenerateDependencyInfo 	1
DisplayUpToDateScripts 	0

      Note: you may want to edit this file if you rename your "MyShard" 
            directory.
      
      --> Start StartHere.bat:
          In your 'MyShard' folder you'll find a file called 
          'fast compile.bat'. Double click it in order to compile all 
          the .src files you just told ecompile to look for. Close this 
          out when it complete, you now have the barebones scripts for 
          getting your world started !

      --> Create a new folder:
	  In your 'MyShard' folder called 'MUL'. Open it. In a new window 
          navigate to your primary UO directory and copy the following files:

		map0.mul
		multi.idx
		multi.mul
		staidx0.mul
		statics0.mul
		tiledata.mul
		verdata.mul

      --> Into your MUL folder. Run the 'starthere.bat' program again 
          and select 'a' RealmGen tools. A variety of options will come 
          up this example assumes you're creating an emulated version of 
          UO MOndains Legacy, and I will not cover how to do the others - 
          it's straightforward and I'm sure you CAN figure it out. Select 
          option 'e' for Britannia realm, and then select 'b' for the 
          Mondains Legacy extensions. 

                THIS PART TAKES A WHILE SO RELAX AND DONT PRESS ANYTHING! 

          It will pause at "Initializing files:" so wait a bit then it 
          will proceed to slowly slowly slowly convert the files we just 
          put in the  MUL folder to something readable by the emulator. 
          Wait until this is done. After it finishes a 'primary conversion' 
          some things will scroll and then again it will return to the 
          'Converting' dialog, this is normal just wait it out again :). 
          When it says 'press any key to continue... do so !

      --> In your 'MyShard' directory:
          You'll see a file named 'pol.cfg'. Go ahead and open this in 
          notepad. Edit the line named 'UoDataFileRoot=D:\Ultima Online'
          and change the root to your primary UO directory 

	  UoDataFileRoot=C:\Program Files\EA Games\Ultima Online Mondain's Legacy

      --> In your primary UO directory:
          Open the file named 'login.cfg' with notepad. 
          Replace all text by 'LoginServer=127.0.0.1,2593'.


-----------------------------------------------------------------------
Step 4: THE FINALE!
-----------------------------------------------------------------------

      --> Get back to your 'MyShard' folder and start pol.exe program:
          Lines will scroll and it should end all dandy with a line like:

		Listening for HTTP requests on port 5000

          This means as it says.. it's waiting for people to login. 
          Minimize this window.

      --> Open your main UO folder:
          double click the icon produced by UORice entitled:
		No_Crypt_Client_2d.exe

      --> The loading screen for UO will appear and when prompted for a 
          user name and password enter:

		Username: admin
		Password: admin

Select the 'POL Server or whichever appears' and create a character, 
you will load into your barebones shard! Everything is customizable with 
the emulator it's just a matter of figuring it out, but hopefully this document 
will allow you to get on an even and exciting ground where you can SEE results 
in game.

If there are any problems or inquiries I regret to inform you that I doubt I can 
help ! But if you feel the need to ask me directly then do so. The POL community 
is polite and helpful but we are all busy human beings. Patience and perserverance 
are all that is needed to enjoy the enormous work of this community !

Kindest Regards,
Kirin