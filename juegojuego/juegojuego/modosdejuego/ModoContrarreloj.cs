namespace juegojuego
{
    // MODO CONTRARRELOJ (todavia no disponible): cada par que se encuentra regala tiempo extra,
    // asi que jugar bien alarga la partida.
    //
    // PARA TERMINARLO (los metodos estan explicados en ModoJuego.cs; copiar de ModoNivelCronometro.cs el cronometro):
    //   - Agregar un campo para el tiempo extra:  private int _tiempoExtra;
    //   - AlEmpezarJuego():  _tiempoExtra = 0;
    //   - AlCompletarIntento(Partida partida, bool acierto):  if (acierto) _tiempoExtra += 5;
    //   - Perdio, TextoTiempo y TiempoUrgente:  igual que en ModoNivelCronometro pero con el limite + _tiempoExtra
    //   - Cuando este listo: borrar la linea de Disponible (o poner true)
    public class ModoContrarreloj : ModoJuego
    {
        public ModoContrarreloj()
            : base("Contrarreloj", "Cada par que encontras te regala tiempo extra")
        {
        }

        // Todavia no se puede elegir: aparece en la lista como PROXIMAMENTE
        public override bool Disponible => true;

        // Niveles con tiempo limite inicial
        public override NivelCronometro[] Niveles => NivelCronometro.Estandar;
    }
}
