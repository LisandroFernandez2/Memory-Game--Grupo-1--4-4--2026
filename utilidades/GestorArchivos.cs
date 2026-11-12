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
    // Clase estatica no se crea con new para guardar y leer archivos:
    // configuracion texto records binario y log texto
    public static class GestorArchivos
    {
        // Carpeta 'datos' que se crea junto al ejecutable; ahi se guarda todo
        private static readonly string carpetaDatos = Path.Combine(AppContext.BaseDirectory, "datos");
        // Ruta del archivo de configuracion texto
        private static readonly string archivoConfig = Path.Combine(carpetaDatos, "configuracion.txt");
        // Ruta del archivo de records binario
        private static readonly string archivoRecords = Path.Combine(carpetaDatos, "records.bin");
        // Ruta del archivo de log texto
        private static readonly string archivoLog = Path.Combine(carpetaDatos, "debug.log");

        // Recibe: nada Devuelve: nada
        // Crea la carpeta 'datos' si todavia no existe
        private static void AsegurarCarpeta()
        {
            if (!Directory.Exists(carpetaDatos))
                Directory.CreateDirectory(carpetaDatos);
        }


        // Guarda la configuración del juego como pares clave=valor en texto plano
        // Recibe: configuracion diccionario clave -> valor
        // Devuelve: nada
        // Como funciona: escribe una linea 'clave=valor' por cada par reemplazando el archivo anterior
        public static void GuardarConfiguracionTexto(Dictionary<string, string> configuracion)
        {
            AsegurarCarpeta();
            // false = pisa el archivo en vez de agregar al final
            using (StreamWriter escritor = new StreamWriter(archivoConfig, false))
            {
                foreach (var par in configuracion)
                    escritor.WriteLine($"{par.Key}={par.Value}");
            }
        }

        // Carga la configuración del juego desde el archivo de texto
        // Recibe: nada
        // Devuelve: diccionario clave -> valor con la configuracion leida vacio si el archivo no existe
        public static Dictionary<string, string> CargarConfiguracionTexto()
        {
            var configuracion = new Dictionary<string, string>();
            // Si no hay archivo devuelve el diccionario vacio
            if (!File.Exists(archivoConfig))
                return configuracion;

            // Lee el archivo linea por linea
            foreach (string linea in File.ReadAllLines(archivoConfig))
            {
                // Salta las lineas vacias o que no tengan '='
                if (string.IsNullOrWhiteSpace(linea) || !linea.Contains('='))
                    continue;

                // Separa en clave y valor por el primer '='
                string[] partes = linea.Split('=', 2);
                configuracion[partes[0]] = partes[1];
            }
            return configuracion;
        }


        // Guarda la lista de puntajes en un archivo binario
        // Recibe: puntajes lista de Puntaje
        // Devuelve: nada
        // Como funciona: escribe en binario la cantidad de puntajes y despues los datos de cada uno
        public static void GuardarRecordsBinario(List<Puntaje> puntajes)
        {
            AsegurarCarpeta();
            // Create = crea el archivo o lo reemplaza si ya existia
            using (FileStream flujo = new FileStream(archivoRecords, FileMode.Create))
            using (BinaryWriter escritor = new BinaryWriter(flujo))
            {
                // Primero la cantidad para saber cuantos leer despues
                escritor.Write(puntajes.Count);
                // Despues los campos de cada puntaje siempre en este mismo orden
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
        // Recibe: nada
        // Devuelve: lista de Puntaje leida del archivo vacia si el archivo no existe
        // Los campos se leen en el mismo orden en que se guardaron
        public static List<Puntaje> CargarRecordsBinario()
        {
            var puntajes = new List<Puntaje>();
            // Si no hay archivo devuelve la lista vacia
            if (!File.Exists(archivoRecords))
                return puntajes;

            using (FileStream flujo = new FileStream(archivoRecords, FileMode.Open))
            using (BinaryReader lector = new BinaryReader(flujo))
            {
                // Lee cuantos puntajes hay guardados
                int cantidad = lector.ReadInt32();
                // Lee un puntaje por vuelta campo por campo
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

                    // Arma el Puntaje con lo leido y lo agrega a la lista
                    puntajes.Add(new Puntaje(jugador, fecha, nivel, intentos, aciertos, fallos, tiempo, puntuacion));
                }
            }
            return puntajes;
        }


        // Agrega una línea con marca de tiempo al log de depuración
        // Recibe: mensaje texto
        // Devuelve: nada
        // Como funciona: agrega al final del log una linea con la fecha y hora + el mensaje
        public static void RegistrarLog(string mensaje)
        {
            AsegurarCarpeta();
            // true = agrega al final sin borrar lo anterior
            using (StreamWriter escritor = new StreamWriter(archivoLog, true))
            {
                escritor.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {mensaje}");
            }
        }
    }
}
