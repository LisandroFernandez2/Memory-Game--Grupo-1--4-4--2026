using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using juegojuego.BasedeDatos;
using Microsoft.EntityFrameworkCore;

namespace juegojuego
{
    // Puente entre el juego, el JSON de partidas (datos/partidas.json) y la base de datos MySQL (memory_game).
    // Flujo: la partida se guarda primero en el JSON y de ahi se pasa a MySQL.
    // El juego sigue funcionando aunque MySQL no este disponible: si algo falla, se anota en el log y se sigue.
    internal static class RepositorioBD
    {
        // ---- Copia de la partida en el archivo JSON (datos/partidas.json) ----
        // El JSON es lo primero que se guarda y de ahi se pasa a MySQL. Si MySQL esta apagado, la partida queda
        // pendiente (Guardada = false) en el JSON y se manda a la base la proxima vez que se pueda.
        private class PartidaJson
        {
            public string Jugador { get; set; } = "";
            public string Modo { get; set; } = "";
            public string Nivel { get; set; } = "";
            public int Filas { get; set; }
            public int Columnas { get; set; }
            public int? TiempoLimite { get; set; }
            public int? Vidas { get; set; }
            public string Tematica { get; set; } = "";
            public DateTime Fecha { get; set; }
            public int Intentos { get; set; }
            public int Aciertos { get; set; }
            public int Fallos { get; set; }
            public int TiempoSegundos { get; set; }
            public int Puntuacion { get; set; }
            public bool Gano { get; set; }
            // true cuando ya se paso a la base de datos
            public bool Guardada { get; set; }
            public List<MovimientoJson> Movimientos { get; set; } = new List<MovimientoJson>();
            public List<ParJson> Pares { get; set; } = new List<ParJson>();
        }

        private class MovimientoJson
        {
            public int Numero { get; set; }
            public int Fila1 { get; set; }
            public int Columna1 { get; set; }
            public int Fila2 { get; set; }
            public int Columna2 { get; set; }
            public bool Acierto { get; set; }
            public DateTime Momento { get; set; }
        }

        private class ParJson
        {
            public int IdValor { get; set; }
            public int Puntos { get; set; }
        }

        private static readonly string archivoPartidas = Path.Combine(AppContext.BaseDirectory, "datos", "partidas.json");
        private static readonly JsonSerializerOptions opcionesJson = new JsonSerializerOptions { WriteIndented = true };

        // Lee la lista de partidas del JSON. Devuelve: lista vacia si el archivo no existe, null si esta daniado (para no pisarlo)
        private static List<PartidaJson> LeerPartidasJson()
        {
            try
            {
                if (!File.Exists(archivoPartidas)) return new List<PartidaJson>();
                return JsonSerializer.Deserialize<List<PartidaJson>>(File.ReadAllText(archivoPartidas)) ?? new List<PartidaJson>();
            }
            catch (Exception ex)
            {
                GestorArchivos.RegistrarLog($"BD: no se pudo leer partidas.json (se deja como esta): {Detalle(ex)}");
                return null;
            }
        }

        private static void EscribirPartidasJson(List<PartidaJson> lista)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(archivoPartidas));
            File.WriteAllText(archivoPartidas, JsonSerializer.Serialize(lista, opcionesJson));
        }

        // Guarda una partida terminada (ganada o perdida): primero en el JSON y despues la pasa a MySQL.
        // Recibe: partida la partida del juego, modoJuego ej: "Clasico", tematica ej: "Mezcla" y gano true/false
        // Devuelve: true si quedo guardada en la base false si no (queda pendiente en el JSON)
        public static bool GuardarPartida(Partida partida, string modoJuego, string tematica, bool gano)
        {
            try
            {
                int segundos = partida.Cronometro.SegundosTranscurridos;
                Nivel nivel = partida.NivelActual;
                var registro = new PartidaJson
                {
                    Jugador = partida.NombreJugador,
                    Modo = modoJuego,
                    Nivel = nivel.Nombre,
                    Filas = nivel.Filas,
                    Columnas = nivel.Columnas,
                    TiempoLimite = nivel is NivelCronometro c ? c.TiempoLimiteSegundos : (int?)null,
                    Vidas = nivel is NivelSupervivencia sv ? sv.vidas : (int?)null,
                    Tematica = tematica,
                    Fecha = DateTime.Now,
                    Intentos = partida.Contador.Intentos,
                    Aciertos = partida.Contador.Aciertos,
                    Fallos = partida.Contador.Fallos,
                    TiempoSegundos = segundos,
                    Puntuacion = partida.Contador.CalcularPuntuacion(segundos),
                    Gano = gano
                };

                // El historial es una pila (el ultimo intento sale primero): se da vuelta para numerarlos desde 1
                List<Movimiento> movimientos = partida.Historial.ObtenerHistorialCompleto().ToList();
                movimientos.Reverse();
                for (int i = 0; i < movimientos.Count; i++)
                {
                    Movimiento m = movimientos[i];
                    registro.Movimientos.Add(new MovimientoJson
                    {
                        Numero = i + 1,
                        Fila1 = m.Fila1,
                        Columna1 = m.Columna1,
                        Fila2 = m.Fila2,
                        Columna2 = m.Columna2,
                        Acierto = m.Acierto,
                        Momento = m.Momento
                    });
                }

                foreach (ListaParesDescubiertos.ParDescubierto par in partida.ParesDescubiertos.ObtenerPares())
                    registro.Pares.Add(new ParJson { IdValor = par.IdValor, Puntos = par.Puntos });

                // 1) JSON
                List<PartidaJson> lista = LeerPartidasJson();
                if (lista == null) return false;
                lista.Add(registro);
                EscribirPartidasJson(lista);
            }
            catch (Exception ex)
            {
                GestorArchivos.RegistrarLog($"BD: error al guardar la partida en el JSON: {Detalle(ex)}");
                return false;
            }

            // 2) MySQL, leyendo del JSON
            return SincronizarConBaseDeDatos();
        }

        // Pasa a MySQL todas las partidas del JSON que todavia no estan en la base (partida + movimientos + pares en un SaveChanges cada una)
        // y las marca como guardadas. Se llama despues de cada partida y al abrir el juego.
        // Devuelve: true si no quedo ninguna pendiente false si MySQL fallo (siguen pendientes y se reintenta despues)
        public static bool SincronizarConBaseDeDatos()
        {
            try
            {
                List<PartidaJson> lista = LeerPartidasJson();
                if (lista == null) return false;
                if (!lista.Any(p => !p.Guardada)) return true;

                using var db = new AppDbContext();
                bool todoOk = true;

                foreach (PartidaJson p in lista.Where(x => !x.Guardada))
                {
                    try
                    {
                        // Jugador: lo busca por nombre y si no existe lo crea
                        JugadorDB jugador = db.Jugador.FirstOrDefault(j => j.Nombre == p.Jugador);
                        if (jugador == null)
                        {
                            jugador = new JugadorDB { Nombre = p.Jugador };
                            db.Jugador.Add(jugador);
                        }

                        NivelDB nivel = BuscarOCrearNivel(db, p);
                        if (nivel == null)
                        {
                            GestorArchivos.RegistrarLog($"BD: no se guardo la partida, el modo de juego '{p.Modo}' no existe en la tabla modo_juego");
                            db.ChangeTracker.Clear();
                            todoOk = false;
                            continue;
                        }

                        var partidaDb = new PartidaDB
                        {
                            Jugador = jugador,
                            Nivel = nivel,
                            Tematica = BuscarOCrearTematica(db, p.Tematica),
                            Fecha = p.Fecha,
                            Intentos = p.Intentos,
                            Aciertos = p.Aciertos,
                            Fallos = p.Fallos,
                            TiempoSegundos = p.TiempoSegundos,
                            Puntuacion = p.Puntuacion,
                            Gano = p.Gano
                        };
                        foreach (MovimientoJson m in p.Movimientos)
                            partidaDb.Movimientos.Add(new MovimientoDB
                            {
                                Numero = m.Numero,
                                Fila1 = m.Fila1,
                                Columna1 = m.Columna1,
                                Fila2 = m.Fila2,
                                Columna2 = m.Columna2,
                                Acierto = m.Acierto,
                                Momento = m.Momento
                            });
                        foreach (ParJson par in p.Pares)
                            partidaDb.ParesDescubiertos.Add(new Par_descubiertoDB { IdValor = par.IdValor, Puntos = par.Puntos });

                        db.Partida.Add(partidaDb);
                        db.SaveChanges();

                        p.Guardada = true;
                        GestorArchivos.RegistrarLog($"BD: partida guardada (id {partidaDb.Id}, gano={p.Gano})");
                    }
                    catch (Exception ex)
                    {
                        // Si MySQL falla no tiene sentido probar con las demas: quedan pendientes en el JSON
                        GestorArchivos.RegistrarLog($"BD: error al guardar la partida: {Detalle(ex)}");
                        db.ChangeTracker.Clear();
                        todoOk = false;
                        break;
                    }
                }

                EscribirPartidasJson(lista);
                return todoOk;
            }
            catch (Exception ex)
            {
                GestorArchivos.RegistrarLog($"BD: error al sincronizar con la base: {Detalle(ex)}");
                return false;
            }
        }

        // Lee los records guardados en la base de datos (tabla record) ordenados de mayor a menor puntuacion.
        // Devuelve: la lista de Puntaje o null si no se pudo leer (ej: MySQL apagado), para usar el archivo records.bin
        public static List<Puntaje> CargarRecords()
        {
            try
            {
                using var db = new AppDbContext();
                List<RecordDB> filas = LeerRecordsDB(db);

                var puntajes = new List<Puntaje>();
                foreach (RecordDB r in filas)
                {
                    // Misma etiqueta que arma Game1.EtiquetaRecord: aclara tematica y modo solo si hay mas de uno
                    string etiqueta = r.Nivel;
                    if (CatalogoModos.Todos.Length > 1) etiqueta = $"{r.Tematica} {etiqueta}";
                    if (CatalogoModosJuego.CantidadDisponibles > 1) etiqueta = $"{r.Modo} {etiqueta}";

                    puntajes.Add(new Puntaje(
                        r.Jugador, r.Fecha, etiqueta,
                        r.Intentos, r.Aciertos, r.Fallos,
                        TimeSpan.FromSeconds(r.TiempoSegundos), r.Puntuacion));
                }
                return puntajes;
            }
            catch (Exception ex)
            {
                GestorArchivos.RegistrarLog($"BD: error al leer los records: {Detalle(ex)}");
                return null;
            }
        }

        // Lee la tabla record ordenada de mayor a menor puntuacion.
        // Recibe: db la base abierta Devuelve: la lista de RecordDB (lanza la excepcion si MySQL falla)
        public static List<RecordDB> LeerRecordsDB(AppDbContext db)
        {
            return db.Record.AsNoTracking().OrderByDescending(r => r.Puntuacion).ToList();
        }

        // Recibe: ex la excepcion capturada Devuelve: el mensaje completo incluyendo las excepciones internas
        // EF Core tapa el error real de MySQL ("Unknown column ...") dentro de InnerException:
        // sin esto el log solo diria "See the inner exception for details"
        public static string Detalle(Exception ex)
        {
            string texto = ex.Message;
            for (Exception interna = ex.InnerException; interna != null; interna = interna.InnerException)
                texto += " -> " + interna.Message;
            return texto;
        }

        // Busca el nivel del modo (por nombre y tamano de tablero). Si no existe (ej: "Ronda 5" del Infinito) lo crea.
        // Devuelve: el nivel o null si el modo de juego no esta cargado en la tabla modo_juego
        private static NivelDB BuscarOCrearNivel(AppDbContext db, PartidaJson p)
        {
            Modo_JuegoDB modoDb = db.Modo_Juego.FirstOrDefault(m => m.Nombre == p.Modo);
            if (modoDb == null) return null;

            NivelDB nivelDb = db.Nivel.FirstOrDefault(n => n.ModoJuegoId == modoDb.Id
                                                        && n.Nombre == p.Nivel
                                                        && n.Filas == p.Filas
                                                        && n.Columnas == p.Columnas);
            if (nivelDb != null) return nivelDb;

            // "pares" lo calcula MySQL solo, por eso no se asigna
            nivelDb = new NivelDB
            {
                ModoJuegoId = modoDb.Id,
                Nombre = p.Nivel,
                Filas = p.Filas,
                Columnas = p.Columnas,
                TiempoLimite = p.TiempoLimite,
                Vidas = p.Vidas
            };
            db.Nivel.Add(nivelDb);
            return nivelDb;
        }

        // Busca la tematica por nombre o carpeta. Si no existe la crea.
        private static TematicaDB BuscarOCrearTematica(AppDbContext db, string tematica)
        {
            TematicaDB tematicaDb = db.Tematica.FirstOrDefault(t => t.Nombre == tematica || t.Carpeta == tematica);
            if (tematicaDb != null) return tematicaDb;

            tematicaDb = new TematicaDB { Nombre = tematica, Carpeta = tematica };
            db.Tematica.Add(tematicaDb);
            return tematicaDb;
        }
    }
}
