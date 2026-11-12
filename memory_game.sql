CREATE DATABASE IF NOT EXISTS memory_game;
USE memory_game;

CREATE TABLE jugador (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50)
);

CREATE TABLE modo_juego (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50),
  descripcion VARCHAR(255),
  disponible BOOLEAN,
  CantidadJugadores INT,
  GuardaRecords BOOLEAN
);

CREATE TABLE tematica (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50),
  carpeta VARCHAR(100)
);

CREATE TABLE nivel (
  id INT PRIMARY KEY AUTO_INCREMENT,
  ModoJuegoId INT,
  nombre VARCHAR(50),
  filas INT,
  columnas INT,
  pares INT GENERATED ALWAYS AS (filas * columnas DIV 2) STORED,
  TiempoLimite INT,
  vidas INT,
  FOREIGN KEY (ModoJuegoId) REFERENCES modo_juego(id)
);

CREATE TABLE partida (
  id INT PRIMARY KEY AUTO_INCREMENT,
  JugadorId INT,
  NivelId INT,
  TematicaId INT,
  fecha DATETIME,
  intentos INT,
  aciertos INT,
  fallos INT,
  TiempoSegundos INT,
  puntuacion INT,
  gano BOOLEAN,
  FOREIGN KEY (JugadorId) REFERENCES jugador(id),
  FOREIGN KEY (NivelId) REFERENCES nivel(id),
  FOREIGN KEY (TematicaId) REFERENCES tematica(id)
);

CREATE TABLE movimiento (
  id INT PRIMARY KEY AUTO_INCREMENT,
  PartidaId INT,
  numero INT,
  fila1 INT,
  columna1 INT,
  fila2 INT,
  columna2 INT,
  acierto BOOLEAN,
  momento DATETIME,
  FOREIGN KEY (PartidaId) REFERENCES partida(id)
);

CREATE TABLE par_descubierto (
  id INT PRIMARY KEY AUTO_INCREMENT,
  PartidaId INT,
  IdValor INT,
  puntos INT,
  FOREIGN KEY (PartidaId) REFERENCES partida(id)
);

CREATE TABLE record (
  id INT PRIMARY KEY AUTO_INCREMENT,
  PartidaId INT,
  JugadorId INT,
  jugador VARCHAR(50),
  modo VARCHAR(50),
  nivel VARCHAR(50),
  tematica VARCHAR(50),
  intentos INT,
  aciertos INT,
  fallos INT,
  TiempoSegundos INT,
  puntuacion INT,
  fecha DATETIME,
  FOREIGN KEY (PartidaId) REFERENCES partida(id),
  FOREIGN KEY (JugadorId) REFERENCES jugador(id)
);

INSERT INTO jugador (id, nombre) VALUES
(1, 'FERM'),
(2, 'PEPE'),
(3, 'P');

INSERT INTO modo_juego (id, nombre, descripcion, disponible, CantidadJugadores, GuardaRecords) VALUES
(1, 'Clasico', 'Sin cronometro: jugas tranquilo hasta encontrar todos los pares', 1, 1, 1),
(2, 'Cronometro', 'Encontra todos los pares antes de que se acabe el tiempo', 1, 1, 1),
(3, 'Contrarreloj', 'Cada par que encontras te regala tiempo extra', 1, 1, 1),
(4, 'Infinito', 'Cada tablero que completas trae otro mas grande. Hasta donde llegas?', 1, 1, 1),
(5, 'Multijugador', '2 a 4 jugadores por turnos: gana quien encuentra mas pares', 1, 2, 0),
(6, 'Supervivencia', 'Tenes una cantidad limitada vidas: cada intento fallido te quita una', 1, 1, 1);

INSERT INTO tematica (id, nombre, carpeta) VALUES
(1, 'Mezcla', 'mezcla');

INSERT INTO nivel (id, ModoJuegoId, nombre, filas, columnas, TiempoLimite, vidas) VALUES
(1, 1, 'Muy Facil', 4, 4, NULL, NULL),
(2, 5, 'Muy Facil', 4, 4, NULL, NULL),
(3, 1, 'Facil', 4, 5, NULL, NULL),
(4, 5, 'Facil', 4, 5, NULL, NULL),
(5, 1, 'Normal', 6, 6, NULL, NULL),
(6, 5, 'Normal', 6, 6, NULL, NULL),
(7, 1, 'Dificil', 7, 8, NULL, NULL),
(8, 5, 'Dificil', 7, 8, NULL, NULL),
(9, 1, 'Muy Dificil', 8, 9, NULL, NULL),
(10, 5, 'Muy Dificil', 8, 9, NULL, NULL),
(11, 1, 'Imposible', 9, 10, NULL, NULL),
(12, 5, 'Imposible', 9, 10, NULL, NULL),
(16, 2, 'Muy Facil', 4, 4, 120, NULL),
(17, 2, 'Facil', 4, 5, 90, NULL),
(18, 2, 'Normal', 6, 6, 90, NULL),
(19, 2, 'Dificil', 7, 8, 150, NULL),
(20, 2, 'Muy Dificil', 8, 9, 170, NULL),
(21, 2, 'Imposible', 9, 10, 220, NULL),
(23, 3, 'Muy Facil', 4, 4, 50, NULL),
(24, 3, 'Facil', 4, 5, 60, NULL),
(25, 3, 'Normal', 6, 6, 70, NULL),
(26, 3, 'Dificil', 7, 8, 80, NULL),
(27, 3, 'Muy Dificil', 8, 9, 90, NULL),
(28, 3, 'Imposible', 9, 10, 100, NULL),
(30, 6, 'Muy Facil', 4, 4, NULL, 6),
(31, 6, 'Facil', 4, 5, NULL, 6),
(32, 6, 'Normal', 6, 6, NULL, 8),
(33, 6, 'Dificil', 7, 8, NULL, 10),
(34, 6, 'Muy Dificil', 8, 9, NULL, 12),
(35, 6, 'Imposible', 9, 10, NULL, 15),
(37, 4, 'Ronda 1', 1, 2, NULL, NULL);

INSERT INTO partida (id, JugadorId, NivelId, TematicaId, fecha, intentos, aciertos, fallos, TiempoSegundos, puntuacion, gano) VALUES
(1, 1, 1, 1, '2026-11-05 20:53:07', 11, 8, 3, 16, 1054, 1),
(2, 2, 1, 1, '2026-11-05 21:03:44', 18, 8, 10, 32, 968, 1);

INSERT INTO movimiento (id, PartidaId, numero, fila1, columna1, fila2, columna2, acierto, momento) VALUES
(1, 1, 1, 0, 0, 1, 1, 0, '2026-11-05 20:52:51'),
(2, 1, 2, 1, 0, 0, 1, 0, '2026-11-05 20:52:52'),
(3, 1, 3, 1, 2, 0, 1, 1, '2026-11-05 20:52:54'),
(4, 1, 4, 0, 2, 0, 3, 1, '2026-11-05 20:52:55'),
(5, 1, 5, 1, 3, 0, 0, 1, '2026-11-05 20:52:57'),
(6, 1, 6, 2, 0, 2, 1, 0, '2026-11-05 20:52:59'),
(7, 1, 7, 2, 2, 1, 1, 1, '2026-11-05 20:53:01'),
(8, 1, 8, 2, 3, 2, 0, 1, '2026-11-05 20:53:02'),
(9, 1, 9, 3, 1, 1, 0, 1, '2026-11-05 20:53:04'),
(10, 1, 10, 3, 0, 2, 1, 1, '2026-11-05 20:53:05'),
(11, 1, 11, 3, 2, 3, 3, 1, '2026-11-05 20:53:07'),
(12, 2, 1, 0, 0, 1, 1, 0, '2026-11-05 21:03:13'),
(13, 2, 2, 1, 0, 0, 1, 0, '2026-11-05 21:03:15'),
(14, 2, 3, 1, 2, 1, 3, 0, '2026-11-05 21:03:16'),
(15, 2, 4, 0, 3, 0, 2, 0, '2026-11-05 21:03:18'),
(16, 2, 5, 1, 1, 0, 2, 1, '2026-11-05 21:03:20'),
(17, 2, 6, 2, 2, 2, 1, 0, '2026-11-05 21:03:21'),
(18, 2, 7, 1, 0, 1, 3, 0, '2026-11-05 21:03:24'),
(19, 2, 8, 1, 2, 2, 1, 1, '2026-11-05 21:03:26'),
(20, 2, 9, 2, 0, 1, 0, 1, '2026-11-05 21:03:27'),
(21, 2, 10, 2, 3, 3, 2, 0, '2026-11-05 21:03:29'),
(22, 2, 11, 1, 3, 3, 2, 1, '2026-11-05 21:03:31'),
(23, 2, 12, 3, 3, 2, 2, 0, '2026-11-05 21:03:33'),
(24, 2, 13, 3, 1, 0, 3, 0, '2026-11-05 21:03:35'),
(25, 2, 14, 3, 0, 0, 3, 1, '2026-11-05 21:03:37'),
(26, 2, 15, 0, 1, 3, 1, 1, '2026-11-05 21:03:39'),
(27, 2, 16, 2, 3, 0, 0, 0, '2026-11-05 21:03:41'),
(28, 2, 17, 2, 2, 0, 0, 1, '2026-11-05 21:03:43'),
(29, 2, 18, 3, 3, 2, 3, 1, '2026-11-05 21:03:44');

INSERT INTO par_descubierto (id, PartidaId, IdValor, puntos) VALUES
(1, 1, 7, 100),
(2, 1, 1, 100),
(3, 1, 5, 100),
(4, 1, 4, 100),
(5, 1, 3, 100),
(6, 1, 0, 100),
(7, 1, 2, 100),
(8, 1, 6, 100),
(9, 2, 7, 100),
(10, 2, 2, 100),
(11, 2, 0, 100),
(12, 2, 1, 100),
(13, 2, 6, 100),
(14, 2, 5, 100),
(15, 2, 4, 100),
(16, 2, 3, 100);

INSERT INTO record (id, PartidaId, JugadorId, jugador, modo, nivel, tematica, intentos, aciertos, fallos, TiempoSegundos, puntuacion, fecha) VALUES
(1, 1, 1, 'FERM', 'Clasico', 'Muy Facil', 'Mezcla', 11, 8, 3, 16, 1054, '2026-11-05 20:53:07'),
(2, 2, 2, 'PEPE', 'Clasico', 'Muy Facil', 'Mezcla', 18, 8, 10, 32, 968, '2026-11-05 21:03:44');

DELIMITER $$
CREATE TRIGGER trg_partida_record AFTER INSERT ON partida FOR EACH ROW
BEGIN
  IF NEW.gano = TRUE THEN
    INSERT INTO record (PartidaId, JugadorId, jugador, modo, nivel, tematica,
                        intentos, aciertos, fallos, TiempoSegundos, puntuacion, fecha)
    SELECT NEW.id, NEW.JugadorId, j.nombre, m.nombre, n.nombre, t.nombre,
           NEW.intentos, NEW.aciertos, NEW.fallos, NEW.TiempoSegundos, NEW.puntuacion, NEW.fecha
    FROM nivel n
    JOIN modo_juego m ON m.id = n.ModoJuegoId
    JOIN jugador j ON j.id = NEW.JugadorId
    JOIN tematica t ON t.id = NEW.TematicaId
    WHERE n.id = NEW.NivelId
      AND m.GuardaRecords = TRUE;
  END IF;
END$$
DELIMITER ;CREATE DATABASE IF NOT EXISTS memory_game;
USE memory_game;

CREATE TABLE jugador (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50)
);

CREATE TABLE modo_juego (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50),
  descripcion VARCHAR(255),
  disponible BOOLEAN,
  CantidadJugadores INT,
  GuardaRecords BOOLEAN
);

CREATE TABLE tematica (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre VARCHAR(50),
  carpeta VARCHAR(100)
);

CREATE TABLE nivel (
  id INT PRIMARY KEY AUTO_INCREMENT,
  ModoJuegoId INT,
  nombre VARCHAR(50),
  filas INT,
  columnas INT,
  pares INT GENERATED ALWAYS AS (filas * columnas DIV 2) STORED,
  TiempoLimite INT,
  vidas INT,
  FOREIGN KEY (ModoJuegoId) REFERENCES modo_juego(id)
);

CREATE TABLE partida (
  id INT PRIMARY KEY AUTO_INCREMENT,
  JugadorId INT,
  NivelId INT,
  TematicaId INT,
  fecha DATETIME,
  intentos INT,
  aciertos INT,
  fallos INT,
  TiempoSegundos INT,
  puntuacion INT,
  gano BOOLEAN,
  FOREIGN KEY (JugadorId) REFERENCES jugador(id),
  FOREIGN KEY (NivelId) REFERENCES nivel(id),
  FOREIGN KEY (TematicaId) REFERENCES tematica(id)
);

CREATE TABLE movimiento (
  id INT PRIMARY KEY AUTO_INCREMENT,
  PartidaId INT,
  numero INT,
  fila1 INT,
  columna1 INT,
  fila2 INT,
  columna2 INT,
  acierto BOOLEAN,
  momento DATETIME,
  FOREIGN KEY (PartidaId) REFERENCES partida(id)
);

CREATE TABLE par_descubierto (
  id INT PRIMARY KEY AUTO_INCREMENT,
  PartidaId INT,
  IdValor INT,
  puntos INT,
  FOREIGN KEY (PartidaId) REFERENCES partida(id)
);

CREATE TABLE record (
  id INT PRIMARY KEY AUTO_INCREMENT,
  PartidaId INT,
  JugadorId INT,
  jugador VARCHAR(50),
  modo VARCHAR(50),
  nivel VARCHAR(50),
  tematica VARCHAR(50),
  intentos INT,
  aciertos INT,
  fallos INT,
  TiempoSegundos INT,
  puntuacion INT,
  fecha DATETIME,
  FOREIGN KEY (PartidaId) REFERENCES partida(id),
  FOREIGN KEY (JugadorId) REFERENCES jugador(id)
);

INSERT INTO jugador (id, nombre) VALUES
(1, 'FERM'),
(2, 'PEPE'),
(3, 'P');

INSERT INTO modo_juego (id, nombre, descripcion, disponible, CantidadJugadores, GuardaRecords) VALUES
(1, 'Clasico', 'Sin cronometro: jugas tranquilo hasta encontrar todos los pares', 1, 1, 1),
(2, 'Cronometro', 'Encontra todos los pares antes de que se acabe el tiempo', 1, 1, 1),
(3, 'Contrarreloj', 'Cada par que encontras te regala tiempo extra', 1, 1, 1),
(4, 'Infinito', 'Cada tablero que completas trae otro mas grande. Hasta donde llegas?', 1, 1, 1),
(5, 'Multijugador', '2 a 4 jugadores por turnos: gana quien encuentra mas pares', 1, 2, 0),
(6, 'Supervivencia', 'Tenes una cantidad limitada vidas: cada intento fallido te quita una', 1, 1, 1);

INSERT INTO tematica (id, nombre, carpeta) VALUES
(1, 'Mezcla', 'mezcla');

INSERT INTO nivel (id, ModoJuegoId, nombre, filas, columnas, TiempoLimite, vidas) VALUES
(1, 1, 'Muy Facil', 4, 4, NULL, NULL),
(2, 5, 'Muy Facil', 4, 4, NULL, NULL),
(3, 1, 'Facil', 4, 5, NULL, NULL),
(4, 5, 'Facil', 4, 5, NULL, NULL),
(5, 1, 'Normal', 6, 6, NULL, NULL),
(6, 5, 'Normal', 6, 6, NULL, NULL),
(7, 1, 'Dificil', 7, 8, NULL, NULL),
(8, 5, 'Dificil', 7, 8, NULL, NULL),
(9, 1, 'Muy Dificil', 8, 9, NULL, NULL),
(10, 5, 'Muy Dificil', 8, 9, NULL, NULL),
(11, 1, 'Imposible', 9, 10, NULL, NULL),
(12, 5, 'Imposible', 9, 10, NULL, NULL),
(16, 2, 'Muy Facil', 4, 4, 120, NULL),
(17, 2, 'Facil', 4, 5, 90, NULL),
(18, 2, 'Normal', 6, 6, 90, NULL),
(19, 2, 'Dificil', 7, 8, 150, NULL),
(20, 2, 'Muy Dificil', 8, 9, 170, NULL),
(21, 2, 'Imposible', 9, 10, 220, NULL),
(23, 3, 'Muy Facil', 4, 4, 50, NULL),
(24, 3, 'Facil', 4, 5, 60, NULL),
(25, 3, 'Normal', 6, 6, 70, NULL),
(26, 3, 'Dificil', 7, 8, 80, NULL),
(27, 3, 'Muy Dificil', 8, 9, 90, NULL),
(28, 3, 'Imposible', 9, 10, 100, NULL),
(30, 6, 'Muy Facil', 4, 4, NULL, 6),
(31, 6, 'Facil', 4, 5, NULL, 6),
(32, 6, 'Normal', 6, 6, NULL, 8),
(33, 6, 'Dificil', 7, 8, NULL, 10),
(34, 6, 'Muy Dificil', 8, 9, NULL, 12),
(35, 6, 'Imposible', 9, 10, NULL, 15),
(37, 4, 'Ronda 1', 1, 2, NULL, NULL);

INSERT INTO partida (id, JugadorId, NivelId, TematicaId, fecha, intentos, aciertos, fallos, TiempoSegundos, puntuacion, gano) VALUES
(1, 1, 1, 1, '2026-11-05 20:53:07', 11, 8, 3, 16, 1054, 1),
(2, 2, 1, 1, '2026-11-05 21:03:44', 18, 8, 10, 32, 968, 1);

INSERT INTO movimiento (id, PartidaId, numero, fila1, columna1, fila2, columna2, acierto, momento) VALUES
(1, 1, 1, 0, 0, 1, 1, 0, '2026-11-05 20:52:51'),
(2, 1, 2, 1, 0, 0, 1, 0, '2026-11-05 20:52:52'),
(3, 1, 3, 1, 2, 0, 1, 1, '2026-11-05 20:52:54'),
(4, 1, 4, 0, 2, 0, 3, 1, '2026-11-05 20:52:55'),
(5, 1, 5, 1, 3, 0, 0, 1, '2026-11-05 20:52:57'),
(6, 1, 6, 2, 0, 2, 1, 0, '2026-11-05 20:52:59'),
(7, 1, 7, 2, 2, 1, 1, 1, '2026-11-05 20:53:01'),
(8, 1, 8, 2, 3, 2, 0, 1, '2026-11-05 20:53:02'),
(9, 1, 9, 3, 1, 1, 0, 1, '2026-11-05 20:53:04'),
(10, 1, 10, 3, 0, 2, 1, 1, '2026-11-05 20:53:05'),
(11, 1, 11, 3, 2, 3, 3, 1, '2026-11-05 20:53:07'),
(12, 2, 1, 0, 0, 1, 1, 0, '2026-11-05 21:03:13'),
(13, 2, 2, 1, 0, 0, 1, 0, '2026-11-05 21:03:15'),
(14, 2, 3, 1, 2, 1, 3, 0, '2026-11-05 21:03:16'),
(15, 2, 4, 0, 3, 0, 2, 0, '2026-11-05 21:03:18'),
(16, 2, 5, 1, 1, 0, 2, 1, '2026-11-05 21:03:20'),
(17, 2, 6, 2, 2, 2, 1, 0, '2026-11-05 21:03:21'),
(18, 2, 7, 1, 0, 1, 3, 0, '2026-11-05 21:03:24'),
(19, 2, 8, 1, 2, 2, 1, 1, '2026-11-05 21:03:26'),
(20, 2, 9, 2, 0, 1, 0, 1, '2026-11-05 21:03:27'),
(21, 2, 10, 2, 3, 3, 2, 0, '2026-11-05 21:03:29'),
(22, 2, 11, 1, 3, 3, 2, 1, '2026-11-05 21:03:31'),
(23, 2, 12, 3, 3, 2, 2, 0, '2026-11-05 21:03:33'),
(24, 2, 13, 3, 1, 0, 3, 0, '2026-11-05 21:03:35'),
(25, 2, 14, 3, 0, 0, 3, 1, '2026-11-05 21:03:37'),
(26, 2, 15, 0, 1, 3, 1, 1, '2026-11-05 21:03:39'),
(27, 2, 16, 2, 3, 0, 0, 0, '2026-11-05 21:03:41'),
(28, 2, 17, 2, 2, 0, 0, 1, '2026-11-05 21:03:43'),
(29, 2, 18, 3, 3, 2, 3, 1, '2026-11-05 21:03:44');

INSERT INTO par_descubierto (id, PartidaId, IdValor, puntos) VALUES
(1, 1, 7, 100),
(2, 1, 1, 100),
(3, 1, 5, 100),
(4, 1, 4, 100),
(5, 1, 3, 100),
(6, 1, 0, 100),
(7, 1, 2, 100),
(8, 1, 6, 100),
(9, 2, 7, 100),
(10, 2, 2, 100),
(11, 2, 0, 100),
(12, 2, 1, 100),
(13, 2, 6, 100),
(14, 2, 5, 100),
(15, 2, 4, 100),
(16, 2, 3, 100);

INSERT INTO record (id, PartidaId, JugadorId, jugador, modo, nivel, tematica, intentos, aciertos, fallos, TiempoSegundos, puntuacion, fecha) VALUES
(1, 1, 1, 'FERM', 'Clasico', 'Muy Facil', 'Mezcla', 11, 8, 3, 16, 1054, '2026-11-05 20:53:07'),
(2, 2, 2, 'PEPE', 'Clasico', 'Muy Facil', 'Mezcla', 18, 8, 10, 32, 968, '2026-11-05 21:03:44');
