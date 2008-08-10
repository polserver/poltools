uoconvert multis
copy multis.cfg config

uoconvert tiles
copy tiles.cfg config

uoconvert landtiles
copy landtiles.cfg config

rem Mondain's Legacy use "width=7168" here
uoconvert map     realm=britannia mapid=0 usedif=1 width=6144 height=4096
uoconvert statics realm=britannia
uoconvert maptile realm=britannia