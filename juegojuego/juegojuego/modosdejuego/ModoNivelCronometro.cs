using System;

namespace juegojuego
{
    // MODO NIVEL CRONOMETRO (el juego de siempre): elegis una dificultad y tenes que encontrar
    // todos los pares antes de que se acabe el tiempo. Cada nivel es un NivelCronometro con su tiempo limite.
    public class ModoNivelCronometro : ModoJuego
    {
        public ModoNivelCronometro()
            : this("Nivel Cronometro", "Encontra todos los pares antes de que se acabe el tiempo")
        {
        }

        // Para los modos que quieren reutilizar el cronometro y los niveles de este modo con otro nombre
        // (ej: ModoCooperativo hereda de esta clase)
        public ModoNivelCronometro(string nombre, string descripcion)
            : base(nombre, descripcion)
        {
        }

        // Los 6 niveles de siempre, cada uno con su tiempo limite
        public override NivelCronometro[] Niveles => NivelCronometro.Estandar;

        // Se pierde cuando el cronometro llega al tiempo limite del nivel
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

        // El tiempo se pone rojo en los ultimos 10 segundos
        public override bool TiempoUrgente(Partida partida) => SegundosRestantes(partida) <= 10;

        public override string MotivoDerrota => "SE ACABO EL TIEMPO";

        // Segundos que faltan para que se acabe el tiempo nunca menos de 0
        private static int SegundosRestantes(Partida partida)
        {
            NivelCronometro nivel = partida.NivelActual as NivelCronometro;
            int limite = nivel != null ? nivel.TiempoLimiteSegundos : 0;
            return Math.Max(0, limite - partida.Cronometro.SegundosTranscurridos);
        }
    }
}
