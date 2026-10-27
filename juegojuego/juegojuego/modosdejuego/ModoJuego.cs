using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace juegojuego
{
    // Un MODO DE JUEGO son las REGLAS con las que se juega: cuantos jugadores hay, si hay tiempo limite,
    // cuando se gana o se pierde, que niveles se ofrecen, que se muestra en pantalla...
    // (No confundir con la clase Modo, que es la TEMATICA: las imagenes y la musica de las cartas.)
    //
    // Game1 no sabe nada de las reglas: le pregunta todo al ModoJuego que se eligio en la pantalla de modos.
    // Por eso, para hacer un modo nuevo SOLO HAY QUE:
    //   1) Crear una clase que herede de ModoJuego (copiar ModoClasico.cs es lo mas facil)
    //   2) Sobreescribir (override) lo que haga distinto a ese modo
    //   3) Agregarla en CatalogoModosJuego.cs (una linea)
    //
    // Todo lo marcado como 'virtual' ya tiene un valor por defecto que sirve para un modo solitario, sin tiempo
    // y que nunca se pierde. Cada modo solo define lo que lo hace distinto.
    public abstract class ModoJuego
    {
        // Nombre que se ve en la pantalla de modos (sin tildes: la fuente no las tiene)
        public string Nombre { get; }
        // Frase corta que explica el modo y se ve debajo del nombre (sin tildes)
        public string Descripcion { get; }

        // Constructor
        // Recibe: nombre y descripcion del modo
        // Devuelve: nada Solo guarda los datos
        protected ModoJuego(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }

        // DATOS DEL MODO

        // false = el modo todavia no esta hecho: aparece en la lista con el cartel PROXIMAMENTE y no se puede elegir
        // Cuando un modo este terminado se cambia a true (o se borra esta linea del modo)
        public virtual bool Disponible => true;

        // Cuantos jugadores juegan la misma partida (1 = solitario). Si es mas de 1 se crean varios Jugador en la Partida
        public virtual int CantidadJugadores => 1;

        // Niveles que ofrece el modo, en el orden en que salen en el menu
        public abstract Nivel[] Niveles { get; }

        // true = al ganar, el puntaje se guarda en la tabla de mejores puntajes
        public virtual bool GuardaRecords => true;



        // Recibe: tematica la tematica elegida (Modo)
        // Devuelve: solo los niveles del modo que se pueden jugar con esa tematica
        // Un nivel necesita una cara distinta por cada par: si la tematica tiene menos caras, ese nivel no se muestra
        public Nivel[] NivelesPara(Modo tematica)
        {
            List<Nivel> jugables = new List<Nivel>();
            foreach (Nivel nivel in Niveles)
            {
                if (nivel.Pares <= tematica.Caras.Count)
                    jugables.Add(nivel);
                else
                    GestorArchivos.RegistrarLog(
                        $"Modo de juego {Nombre}: el nivel {nivel.Nombre} necesita {nivel.Pares} caras y la tematica {tematica.Nombre} tiene {tematica.Caras.Count}, no se muestra");
            }
            return jugables.ToArray();
        }

        // REGLAS: Game1 las llama en el momento justo

        // Se llama una vez cuando el jugador empieza a jugar (al elegir un nivel o al apretar R para reintentar)
        // NO se llama al pasar a la ronda siguiente. Sirve para poner en cero lo propio del modo: vidas, rondas, etc.
        public virtual void AlEmpezarJuego() { }

        // Se llama cada vez que se completa un intento (el jugador da vuelta 2 cartas)
        // Recibe: partida la partida en curso y acierto true si las 2 cartas eran iguales
        // Sirve para pasar el turno (partida.PasarTurno()), restar una vida, sumar tiempo, etc.
        public virtual void AlCompletarIntento(Partida partida, bool acierto) { }

        // Se llama en cada cuadro mientras se juega
        // Devuelve: true si el jugador perdio (ej: se acabo el tiempo o se quedo sin vidas). Por defecto nunca se pierde
        public virtual bool Perdio(Partida partida) => false;

        // Se llama cuando se encuentran todos los pares de un tablero
        // Recibe: actual el nivel que se acaba de completar
        // Devuelve: el nivel de la ronda siguiente (ej: modo infinito) o null si ahi termina el juego y sale el cartel de victoria
        public virtual Nivel SiguienteNivel(Nivel actual) => null;

        // TEXTOS DE PANTALLA

        // Texto del tiempo que se ve arriba a la izquierda ej: TIEMPO 01:30. Si es null no se muestra ningun tiempo
        public virtual string TextoTiempo(Partida partida) => null;

        // true = el tiempo se pinta en rojo (ej: quedan 10 segundos)
        public virtual bool TiempoUrgente(Partida partida) => false;

        // Lineas extra que se ven arriba a la izquierda debajo del tiempo ej: TURNO JUGADOR 1 o VIDAS 3. Poner 2 como maximo
        public virtual string[] LineasInfo(Partida partida) => Array.Empty<string>();

        // Color de cada linea extra (el indice es la posicion en LineasInfo). Por defecto dorado
        public virtual Color ColorLineaInfo(Partida partida, int indice) => Color.Gold;

        // Titulo del cartel cuando se gana ej: GANO JUGADOR 2
        public virtual string TituloVictoria(Partida partida) => "GANASTE!";

        // Titulo del cartel cuando se pierde ej: SE ACABO EL TIEMPO
        public virtual string MotivoDerrota => "PERDISTE";
    }
}
