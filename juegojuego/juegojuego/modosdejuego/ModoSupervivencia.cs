namespace juegojuego
{
    // MODO SUPERVIVENCIA (todavia no disponible): el jugador tiene 3 vidas y cada intento fallido le quita una.
    // Sin cronometro: si se queda sin vidas pierde, si encuentra todos los pares gana.
    //
    // PARA TERMINARLO (los metodos estan explicados en ModoJuego.cs):
    //   - Agregar un campo para las vidas:  private int _vidas;
    //   - AlEmpezarJuego():  _vidas = 3;
    //   - AlCompletarIntento(Partida partida, bool acierto):  if (!acierto) _vidas--;
    //   - Perdio(Partida partida):  return _vidas <= 0;
    //   - LineasInfo(Partida partida):  return new[] { $"VIDAS {_vidas}" };
    //   - MotivoDerrota:  "TE QUEDASTE SIN VIDAS"
    //   - Cuando este listo: borrar la linea de Disponible (o poner true)
    public class ModoSupervivencia : ModoJuego
    {
        public ModoSupervivencia()
            : base("Supervivencia", "Tenes 3 vidas: cada intento fallido te quita una")
        {
        }

        // Todavia no se puede elegir: aparece en la lista como PROXIMAMENTE
        public override bool Disponible => false;

        // Los 6 tableros de siempre pero sin tiempo limite
        public override Nivel[] Niveles => Nivel.TamanosEstandar;
    }
}
