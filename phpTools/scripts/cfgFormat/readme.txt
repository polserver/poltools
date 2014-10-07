Author: Austin Heilman
Contact: AustinHeiman@gmail.com
irc.darkmyst.org port 6667
#POL

=====================================

About:
This is a php script to sort the properties inside of a config elem.
It uses a template file to do this. Any properties not inside the
template get moved to the bottom of sorted configs as custom properties.
This script does not get run as a webpage. You use it from the command line.
This outputs to a CleanedConfig.cfg in the same directory the php script is
found in.

How To Use:
Open a command prompt.
Type
php cfgformat.php <path to config to clean> (path to template config)

The template config path is optional. If one is not set, it will use a
template.cfg in the same directory as cfgformat.php.

Example:
php cfgformat.php E:\UOL\Distro\096\pkg\systems\combat\itemdesc.cfg

=====================================
Notes:

 * If you get php not a valid command, you need to add your PHP directory to 
   the system path.

 * If you want it to also sort the elems by name, edit the script to sort the
   array in CleanUpElems() after it calls GetConfigStringKeys()




