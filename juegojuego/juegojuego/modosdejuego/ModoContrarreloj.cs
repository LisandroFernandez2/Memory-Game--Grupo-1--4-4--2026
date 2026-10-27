using System;

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
        private int _tiempoExtra;

        public override void AlEmpezarJuego() => _tiempoExtra = 0;

        public override void AlCompletarIntento(Partida partida, bool acierto)
        {
            NivelCronometro nivel = partida.NivelActual as NivelCronometro;
            if (acierto)
            {
                _tiempoExtra += 5;
                nivel.TiempoLimiteSegundos = nivel.TiempoLimiteSegundos + _tiempoExtra;
            }
        }

        public override bool Perdio(Partida partida)
        {
            NivelCronometro nivel = partida.NivelActual as NivelCronometro;
            return nivel != null && partida.Cronometro.SeAgotoElTiempo(nivel.TiempoLimiteSegundos);
        }

        // Cuenta regresiva: TIEMPO mm:ss
        public override string TextoTiempo(Partida partida)
        {
            int restante = SegundosRestantes(partida);
            return $"TIEMPO {restante / 60:00}:{restante % 60:00}";
        }
        private static int SegundosRestantes(Partida partida)
        {
            NivelCronometro nivel = partida.NivelActual as NivelCronometro;
            int limite = nivel != null ? nivel.TiempoLimiteSegundos : 0;
            return Math.Max(0, limite - partida.Cronometro.SegundosTranscurridos);
        }
        // El tiempo se pone rojo en los ultimos 10 segundos
        public override bool TiempoUrgente(Partida partida) => SegundosRestantes(partida) <= 10;
        // Todavia no se puede elegir: aparece en la lista como PROXIMAMENTE
        public override bool Disponible => true;

        // Niveles con tiempo limite inicial
        public override Nivel[] Niveles => NivelCronometro.cronometros;
    }
}
