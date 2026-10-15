namespace juegojuego
{
    // MODO CLASICO (todavia no disponible): sin cronometro ni tiempo limite.
    // El jugador juega tranquilo y nunca pierde: la partida termina cuando encuentra todos los pares.
    //
    // PARA ACTIVARLO: borrar la linea de Disponible (o poner true). No hay que programar nada mas:
    // las reglas por defecto de ModoJuego (sin tiempo limite, nunca se pierde) ya son las del clasico.
    // Esta clase tambien sirve de plantilla para copiar y armar un modo nuevo.
    public class ModoClasico : ModoJuego
    {
        public ModoClasico()
            : base("Clasico", "Sin cronometro: jugas tranquilo hasta encontrar todos los pares")
        {
        }

        // Todavia no se puede elegir: aparece en la lista como PROXIMAMENTE
        public override bool Disponible => false;

        // Los 6 tableros de siempre pero sin tiempo limite
        public override Nivel[] Niveles => Nivel.TamanosEstandar;
    }
}
