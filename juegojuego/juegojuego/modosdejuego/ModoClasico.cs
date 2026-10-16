namespace juegojuego
{
   // no modifico nada ya que es igual a la clase ModoJuego, simplemente cambio la funcion Disponible de false a true
    public class ModoClasico : ModoJuego
    {
        public ModoClasico()
            : base("Clasico", "Sin cronometro: jugas tranquilo hasta encontrar todos los pares")
        {
        }

        // Todavia no se puede elegir: aparece en la lista como PROXIMAMENTE
        public override bool Disponible => true;

        // Los 6 tableros de siempre pero sin tiempo limite
        public override Nivel[] Niveles => Nivel.TamanosEstandar;
    }
}
