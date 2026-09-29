using System;
using System.Collections.Generic;
using System.IO;

namespace juegojuego
{
    // ESTRUCTURA: Archivos de texto/binarios.
    // Se encarga de la persistencia ligera del juego:
    //   Configuración del juego (nivel, sonido, etc.) en un archivo de TEXTO plano,
    //    fácil de leer/editar manualmente.
    //   Tabla de récords en un archivo BINARIO, más compacto y rápido
    //    de leer/escribir que texto cuando son muchos registros.
    //   Log de depuración en texto, para dejar rastro de eventos durante el desarrollo
    public static class GestorArchivos
    {
        private static readonly string carpetaDatos = Path.Combine(AppContext.BaseDirectory, "datos");
        private static readonly string archivoConfig = Path.Combine(carpetaDatos, "configuracion.txt");
        private static readonly string archivoRecords = Path.Combine(carpetaDatos, "records.bin");
        private static readonly string archivoLog = Path.Combine(carpetaDatos, "debug.log");

        private static void AsegurarCarpeta()
        {
            if (!Directory.Exists(carpetaDatos))
                Directory.CreateDirectory(carpetaDatos);
        }


        // Guarda la configuración del juego como pares clave=valor en texto plano
        public static void GuardarConfiguracionTexto(Dictionary<string, string> configuracion)
        {
            AsegurarCarpeta();
            using (StreamWriter escritor = new StreamWriter(archivoConfig, false))
            {
                foreach (var par in configuracion)
                    escritor.WriteLine($"{par.Key}={par.Value}");
            }
        }

        // Carga la configuración del juego desde el archivo de texto
        public static Dictionary<string, string> CargarConfiguracionTexto()
        {
            var configuracion = new Dictionary<string, string>();
            if (!File.Exists(archivoConfig))
                return configuracion;

            foreach (string linea in File.ReadAllLines(archivoConfig))
            {
                if (string.IsNullOrWhiteSpace(linea) || !linea.Contains('='))
                    continue;

                string[] partes = linea.Split('=', 2);
                configuracion[partes[0]] = partes[1];
            }
            return configuracion;
        }


        // Guarda la lista de puntajes en un archivo binario
        public static void GuardarRecordsBinario(List<Puntaje> puntajes)
        {
            AsegurarCarpeta();
            using (FileStream flujo = new FileStream(archivoRecords, FileMode.Create))
            using (BinaryWriter escritor = new BinaryWriter(flujo))
            {
                escritor.Write(puntajes.Count);
                foreach (Puntaje p in puntajes)
                {
                    escritor.Write(p.Jugador);
                    escritor.Write(p.Fecha.ToBinary());
                    escritor.Write(p.Nivel);
                    escritor.Write(p.Intentos);
                    escritor.Write(p.Aciertos);
                    escritor.Write(p.Fallos);
                    escritor.Write(p.Tiempo.Ticks);
                    escritor.Write(p.Puntuacion);
                }
            }
        }

        // Lee la lista de puntajes desde el archivo binario
        public static List<Puntaje> CargarRecordsBinario()
        {
            var puntajes = new List<Puntaje>();
            if (!File.Exists(archivoRecords))
                return puntajes;

            using (FileStream flujo = new FileStream(archivoRecords, FileMode.Open))
            using (BinaryReader lector = new BinaryReader(flujo))
            {
                int cantidad = lector.ReadInt32();
                for (int i = 0; i < cantidad; i++)
                {
                    string jugador = lector.ReadString();
                    DateTime fecha = DateTime.FromBinary(lector.ReadInt64());
                    string nivel = lector.ReadString();
                    int intentos = lector.ReadInt32();
                    int aciertos = lector.ReadInt32();
                    int fallos = lector.ReadInt32();
                    TimeSpan tiempo = new TimeSpan(lector.ReadInt64());
                    int puntuacion = lector.ReadInt32();

                    puntajes.Add(new Puntaje(jugador, fecha, nivel, intentos, aciertos, fallos, tiempo, puntuacion));
                }
            }
            return puntajes;
        }


        // Agrega una línea con marca de tiempo al log de depuración
        public static void RegistrarLog(string mensaje)
        {
            AsegurarCarpeta();
            using (StreamWriter escritor = new StreamWriter(archivoLog, true))
            {
                escritor.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {mensaje}");
            }
        }
    }
}
