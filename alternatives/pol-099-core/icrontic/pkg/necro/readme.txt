Necromancy v2.0
By: Sigismund

======================================================================
Installation Instructions:

- Unzip this package into /pol/pkg/necromancy, preserving directories
- Move the inscription.* files to /pol/pkg/std/inscription
- Move the circles.cfg file to /pol/config
- Move the chaoseffects.inc into /pol/scripts/include
- Move the math.inc into /pol/scripts/include
- Compile the necromancy files and the inscription files

======================================================================
How Necromancy Works:

Necromantic spells function basically like regular spells, with a few
exceptions.  First of all, the spells must be stored in a specific
book designed for necromancy, the Codex Damnorum (0xA100).  In order
to place a necromantic spell into the Codex, it must be inscribed.
Use the Inscription skill, then select the necromantic scroll, then
select the spellbook.

Necromantic spells can take certain parameters when casting, to change
the requirements for each spell.   These parameters are passed as a
custom property setting, "NecroOpts", set on the caster.   After each
spell, this property is removed.  Currently, the available options are
as follows: NOREGS, NOMANA, NOWORDS, NODELAY, NOSKILL (they all mean
just what they say).  So, for example, to allow the casting of a spell
with no reagents or skill check, you would set the "NecroOpts" prop
on the caster to be "NOREGS NOSKILL".

Also included in this package are several items designed with the
necromantic spells in mind.   Use them at your discretion.

The chaoseffects.inc file included in this package is for the Winds of
Chaos v1.1 package.   If you have a higher version of Winds of Chaos,
you do not need to install this include.

The math.inc in this file is v1.0 of the new math include.  Once
again, if you have a higher version, you do not need to copy this file
over.

======================================================================
Questions?  Comments?  Vapid hostilities?   Contact me at:

        e-mail: prostheticdelirium@worldnet.att.net
        ICQ: 10520050

Sigismund
