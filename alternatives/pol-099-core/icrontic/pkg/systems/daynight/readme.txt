I've always hankered for a more realistic night & day system. This system takes into account
various factors while doing the normal night & day cycle. At night, the lightlevel is determined
by the 2 moons Felucca & Trammel, effectively giving the world moonlight. There are also
different weather effects which govern the daylight. A sunny day is obviously brighter than an
overcast or thundery day.
Transition times and levels adjust automatically to the moons & weather so you won't get a
transition to daylight and then suddenly darken to account for the weather, fluid, nice.
Also added seasons to affect weather, more likely for bad weather in winter and good weather in
summer. Spyglasses can be used to determine what kind of light level to expect for that evening
(for budding astronomers, hehe).

This isn't based on any given ultima world. Basically when Trammel phases 8 times (full cycle),
Felucca phases once. When Felucca phases 8 times (full cycle) the season changes. Making the UO
year 256 days long. Also, transition time in the days.cfg is no longer used, it's now 1/4 of the
total day length.

Changes to be made:
You will probably have to either edit or delete the present item.cfg entries for a telescope to
use the spyglass script. The rest sit in a pkg.

Big thanks to Racalac and the rest of the scriptforum for filling the gaps, MatW who's spyglass
script is still in one piece and needed and whomever wrote the original scripts that i have
added to, crucified and generally mullered.

Explanation of Global settings:-

FeluccaPhase: integer 0-7 representing the 8 phases of a moon
TrammelPhase: see above
Day: integer 0-2 used by weather.src to determine if a lighting change for cloud cover can be
made
DayLightSet: integer 0-8 used by daynight.src for smooth transition to daylight according to
weather.src
MoonLightSet: integer 23-27 used by daynight.src to set nightime and smooth transition according
to the light given off my both moons (depending on phase)
PresentLight: integer 0-27 used by weffect.src to return lighting to normal during lightning
effects
PresentWeather: integer 0-4 used by weather.src to determine if a weather change is to be made &
by rainsnow.src as to when to stop rain/snow effects.
Season: integer 0-3 used by weather.src to modify weather severity(increase in possible bad
weather during winter, decrease in possible bad weather during summer)

version 1.01
fixed slight light error when weather changes during transition
fixed kludge with lightning, now runs as seperate script and prevented from calling multiple
times

version 1.02
finally fixed rain & snow! wohoo! make sure your "Background" setting is in the
 \regions\weather.cfg as it uses the "Background" for all weather effects. (AssignRect does not
work, sorry).

version 1.03
fixed players hearing thunder in dungeons
added lightning, roughly 10% chance of a lightning bolt striking NEAR a player, thx Myrathi for
the help :).
complete rewrite to use a single global array instead of lots of seperate globals, form takes...
{FeluccaPhase, TrammelPhase, Day, DayLightSet, MoonLightSet, PresentLight, PresentWeather, Season}
If you are experiencing weather in dungeons you MUST make dungeons&T2A a seperate weather region.
Just copy the Dungeon co-ords in the light.cfg if you don't know them.