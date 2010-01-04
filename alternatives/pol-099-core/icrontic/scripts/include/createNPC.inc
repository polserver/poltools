use uo;
use cfgfile;

function CreateNpc(template, x, y, z, props := 0)
  var npc := CreateNpcFromTemplate(template, x, y, z, props);
  SetObjProperty(npc, "AiSerial", npc.serial);
  return npc;
endfunction

function TemplateName(pkgtemplate)
    var start := find(pkgtemplate, ":", 2) + 1;
    if (!start)
        return pkgtemplate;
    endif
    return pkgtemplate[start, len(pkgtemplate)];
endfunction