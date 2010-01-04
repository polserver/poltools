use npc;
use cfgfile;

// Guarda o último lado para qual o NPC estava virado
var lastface;
// Guarda quando foi que o NPC executou a função Procurar camminho, para evitar runaways e
// sobrecarregamento.
var ultimaprocura;

var looping;

////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
//
// FUNÇÃO CLOSEDISTANCE  -  Anda normalmente com o NPC até o alvo, verifica quando o
//                          NPC engalhou e executa a função "Procurecaminho", caso
//                          não ache um caminho, o NPC foge do alvo.
//
////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
function CloseDistance( opponent , distancia := 1)


   
    var d := Distance( me, opponent );
    if (d <= distancia)
        return 1;
    elseif ((d >= 25)or(opponent.concealed)or(opponent.hidden)) // Inimigo fora de alcance...
        return 2;
    else
        sleepms(3);
        RunToward( opponent );
        sleepms(3);
        RunToward( opponent );
        sleepms(3);
        RunToward( opponent );
        if (!(CheckLineOfSight(me, opponent)) and (ultimaprocura < ReadGameClock()))
            ultimaprocura :=  ReadGameClock()+5; // Evita Runaway
            if (Procurecaminho(opponent))
                return 2;
            endif
        endif
    endif

endfunction

////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
//
// FUNÇÃO PROCURECAMINHO  -  Procura o ponto mais perto do npc que tenha LOS para
//                           o NPC e o inimigo ao mesmo tempo.
//
////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
function Procurecaminho(inimigo);
// Varredura do eixo X
var destinoX := 0; // Coordenada X de destino
var destinoY := 0; // Coordenada Y de destino
var distanciaatual := 30; //Distancia entre vc e as coordenadas de destino,
                            //usada para saber qual o melhor caminho
    for X:=1 to 24
// Anti RunAways!
sleepms(1);
        if (CheckLOSat(me, me.x+X-12, me.y, me.z) and CheckLOSat(inimigo, me.x+X-12, me.y, me.z))
            if (coordist( inimigo.x , inimigo.y , me.x+X-12 , me.y ) < distanciaatual)
                destinoX := me.x+X-12;
                destinoY := me.y;
                distanciaatual := coordist( inimigo.x , inimigo.y , me.x+X-12 , me.y );
            endif
        else
// Varredura Y
            for Y:=1 to 24
                if (CheckLOSat(me, me.x+X-12, me.y+Y-12, me.z) and CheckLOSat(inimigo, me.x+X-12, me.y+Y-12, me.z))
                    if (coordist( inimigo.x , inimigo.y , me.x+X-12 , me.y+Y-12 ) < distanciaatual)
                        destinoX := me.x+X-12;
                        destinoY := me.y+Y-12;
                        distanciaatual := coordist( inimigo.x , inimigo.y , me.x+X-12 , me.y );
                    endif
                endif
            endfor
        endif
    endfor
//
// Caso tenha achado um caminho
    if (destinoX)
        return CloseDistancetoLocation( destinoX, destinoY, 0);
    endif
// Caso não...
    return 1;
endfunction

////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
//
// FUNÇÃO CLOSEDISTANCETOLOCATION  -  NPC anda até as coordenadas XY
//
////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
function CloseDistancetoLocation( x, y, distancia := 1)
    var engalhou := 0;
    repeat
// 3 RunToaward consecutivos, pois com apenas 1, o NPC engalha mais facilmente.
// Isso foi testado e comprovado, mas o "pq?" ninguem sabe :P
            RunTowardLocation( x, y );
            RunTowardLocation( x, y );
            RunTowardLocation( x, y );
            var face := me.facing;
            if ((lastface - face) = 4)
                lastface := face;
                engalhou := engalhou+1;
            endif
            lastface := face;
    until ((coordist( me.x , me.y , x , y ) <= distancia) or (engalhou > 1));
    if (engalhou > 1)
        return 1;
    endif
    return 0;
endfunction

////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
//
// FUNÇÃO RUNLIKEHELL  -  Faz o npc correr para longe do oponente.
//
////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
function RunLikeHell(opponent, distancia := 25)
    var ev;
	while ( (opponent) and !(opponent.dead or opponent.hidden or opponent.concealed) and 
	    (dist(me,opponent) < distancia) ) 
        if (dist(me,opponent) < 2)
            repeat
	            case (ev.type)
                    SYSEVENT_DAMAGED:
            		    if ( randomint(3)=1 )
                        	Fight( ev.source );
        		        	return;
            		    endif
                endcase
            until (! (ev := os::wait_for_event(1)) );
        else
            sleepms(3);
            Runawayfrom( opponent );
            sleepms(3);
            Runawayfrom( opponent );
            sleepms(3);
            Runawayfrom( opponent );
            var face := me.facing;
            if ((lastface - face) = 4)
            	var ev := array;
            	ev.+type := EVID_PEACEMADE;
    			SendEvent(me, ev);
                return;
            endif
            lastface := face;
        endif
	endwhile
endfunction

////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
//
// FUNÇÃO HERDING  -  Suporte ao Herding, faz o npc caminhar ate o local, caso o npc
//                    engalhe, ele para.
//
////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////

function herding(alvo);
    PlaySoundEffect(me, DetectarSom());
    var face;
    var runaway := 0;
    repeat
        face := me.facing;
        WalkTowardLocation( alvo.x, alvo.y );
        WalkTowardLocation( alvo.x, alvo.y );
        WalkTowardLocation( alvo.x, alvo.y );
        if ((me.facing - face) = 4)  //NPC trancado
            runaway := runaway+1;
        endif
    until (((me.x = alvo.x) and (me.y = alvo.y)or (runaway > 2)));
endfunction

////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
//
// FUNÇÃO AGRUPAR  -  Verifica se deve agrupar por template, graphic ou não agrupar.
//
////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////

var groupby := npccfg[me.npctemplate].groupby;
// Usando integer no groupby para ocupar menos processamento na função 'agrupar'
case (groupby)
    "template": groupby := 1;
    "graphic": groupby := 2;
    default: groupby := 0;
endcase

function Agrupar()
    var grupo := ListMobilesInLineOfSight( me, 15 );
    if (grupo.size() > 6)
        return;
    endif
    case (groupby)
        0: return;
        1: Agruparportemplate(grupo);
        2: Agruparporgrafico(grupo);
    endcase
endfunction

function Agruparporgrafico(byref grupo)
    foreach animal in grupo
        if ((animal.graphic = me.graphic)and(!animal.warmode))
            if (Distance(me, animal) > 10)
                CaminhePara(animal, 5);
                return;
            endif
        endif
    endforeach
endfunction

function Agruparportemplate(byref grupo)
    foreach animal in grupo
        if ((animal.npctemplate = me.npctemplate)and(!animal.warmode))
            if (Distance(me, animal) > 10)
                CaminhePara(animal, 5);
                return;
            endif
        endif
    endforeach
endfunction
////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
//
// FUNÇÃO CAMINHEPARA  -  O npc caminha para o mobile, caso engalhe, ele para.
//
////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////

function caminhepara(alvo, distancia := 1)
    var face;
    var runaway := 0;
    repeat
        face := me.facing;
        WalkToward( alvo );
        WalkToward( alvo );
        WalkToward( alvo );
        if ((me.facing - face) = 4)  //NPC trancado
            runaway := runaway+1;
        endif
    until ((Distance(me, alvo) <= distancia) or (runaway > 2) or (Distance(me, alvo) > 20));
endfunction