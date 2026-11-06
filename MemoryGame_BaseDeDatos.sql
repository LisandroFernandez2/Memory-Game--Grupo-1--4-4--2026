CREATE DATABASE memory_game;
USE memory_game;

CREATE TABLE jugador (
    id      INT AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50)
);

CREATE TABLE tematica (
    id       INT AUTO_INCREMENT PRIMARY KEY,
    nombre   VARCHAR(50),
    carpeta  VARCHAR(100)
);

CREATE TABLE modo_juego (
    id                  INT AUTO_INCREMENT PRIMARY KEY,
    nombre              VARCHAR(50),
    descripcion         VARCHAR(255),
    disponible          BOOLEAN,
    CantidadJugadores  INT,
    GuardaRecords      BOOLEAN
);

CREATE TABLE nivel (
    id             INT AUTO_INCREMENT PRIMARY KEY,
    ModoJuegoId  INT,
    nombre         VARCHAR(50),
    filas          INT,
    columnas       INT,
    pares          INT GENERATED ALWAYS AS ((filas * columnas) DIV 2) STORED,
    TiempoLimite  INT,
    vidas          INT,
    FOREIGN KEY (ModoJuegoId) REFERENCES modo_juego(id)
);

CREATE TABLE partida (
    id               INT AUTO_INCREMENT PRIMARY KEY,
    JugadorId       INT,
    NivelId         INT,
    TematicaId      INT,
    fecha            DATETIME,
    intentos         INT,
    aciertos         INT,
    fallos           INT,
    TiempoSegundos  INT,
    puntuacion       INT,
    gano             BOOLEAN,
    FOREIGN KEY (JugadorId)  REFERENCES jugador(id),
    FOREIGN KEY (NivelId)    REFERENCES nivel(id),
    FOREIGN KEY (TematicaId) REFERENCES tematica(id)
);

CREATE TABLE movimiento (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    PartidaId  INT,
    numero      INT,
    fila1       INT,
    columna1    INT,
    fila2       INT,
    columna2    INT,
    acierto     BOOLEAN,
    momento     DATETIME,
    FOREIGN KEY (PartidaId) REFERENCES partida(id)
);

CREATE TABLE par_descubierto (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    PartidaId  INT,
    IdValor    INT,
    puntos      INT,
    FOREIGN KEY (PartidaId) REFERENCES partida(id)
);

CREATE TABLE record (
    id               INT AUTO_INCREMENT PRIMARY KEY,
    PartidaId        INT,
    JugadorId        INT,
    jugador          VARCHAR(50),
    modo             VARCHAR(50),
    nivel            VARCHAR(50),
    tematica         VARCHAR(50),
    intentos         INT,
    aciertos         INT,
    fallos           INT,
    TiempoSegundos   INT,
    puntuacion       INT,
    fecha            DATETIME,
    FOREIGN KEY (PartidaId) REFERENCES partida(id),
    FOREIGN KEY (JugadorId) REFERENCES jugador(id)
);

DELIMITER $$
CREATE TRIGGER trg_partida_record
AFTER INSERT ON partida
FOR EACH ROW
BEGIN
    IF NEW.gano = TRUE THEN
        INSERT INTO record (PartidaId, JugadorId, jugador, modo, nivel, tematica,
                            intentos, aciertos, fallos, TiempoSegundos, puntuacion, fecha)
        SELECT NEW.id, NEW.JugadorId, j.nombre, m.nombre, n.nombre, t.nombre,
               NEW.intentos, NEW.aciertos, NEW.fallos, NEW.TiempoSegundos, NEW.puntuacion, NEW.fecha
        FROM nivel n
        JOIN modo_juego m ON m.id = n.ModoJuegoId
        JOIN jugador    j ON j.id = NEW.JugadorId
        JOIN tematica   t ON t.id = NEW.TematicaId
        WHERE n.id = NEW.NivelId
          AND m.GuardaRecords = TRUE;
    END IF;
END$$
DELIMITER ;

INSERT INTO tematica (nombre, carpeta) VALUES
('Mezcla', 'mezcla');

INSERT INTO modo_juego (nombre, descripcion, disponible, CantidadJugadores, GuardaRecords) VALUES
('Clasico',       'Sin cronometro: jugas tranquilo hasta encontrar todos los pares',        TRUE, 1, TRUE),
('Cronometro',    'Encontra todos los pares antes de que se acabe el tiempo',               TRUE, 1, TRUE),
('Contrarreloj',  'Cada par que encontras te regala tiempo extra',                          TRUE, 1, TRUE),
('Infinito',      'Cada tablero que completas trae otro mas grande. Hasta donde llegas?',   TRUE, 1, TRUE),
('Multijugador',  '2 a 4 jugadores por turnos: gana quien encuentra mas pares',             TRUE, 2, FALSE),
('Supervivencia', 'Tenes una cantidad limitada vidas: cada intento fallido te quita una',   TRUE, 1, TRUE);

INSERT INTO nivel (ModoJuegoId, nombre, filas, columnas)
SELECT m.id, t.nombre, t.filas, t.columnas
FROM modo_juego m
JOIN (
    SELECT 'Muy Facil'   AS nombre, 4 AS filas, 4  AS columnas UNION ALL
    SELECT 'Facil',        4, 5  UNION ALL
    SELECT 'Normal',       6, 6  UNION ALL
    SELECT 'Dificil',      7, 8  UNION ALL
    SELECT 'Muy Dificil',  8, 9  UNION ALL
    SELECT 'Imposible',    9, 10
) t
WHERE m.nombre IN ('Clasico', 'Multijugador');

INSERT INTO nivel (ModoJuegoId, nombre, filas, columnas, TiempoLimite)
SELECT m.id, t.nombre, t.filas, t.columnas, t.tiempo
FROM modo_juego m
JOIN (
    SELECT 'Muy Facil'   AS nombre, 4 AS filas, 4  AS columnas, 120 AS tiempo UNION ALL
    SELECT 'Facil',        4, 5,  90  UNION ALL
    SELECT 'Normal',       6, 6,  90  UNION ALL
    SELECT 'Dificil',      7, 8,  150 UNION ALL
    SELECT 'Muy Dificil',  8, 9,  170 UNION ALL
    SELECT 'Imposible',    9, 10, 220
) t
WHERE m.nombre = 'Cronometro';

INSERT INTO nivel (ModoJuegoId, nombre, filas, columnas, TiempoLimite)
SELECT m.id, t.nombre, t.filas, t.columnas, t.tiempo
FROM modo_juego m
JOIN (
    SELECT 'Muy Facil'   AS nombre, 4 AS filas, 4  AS columnas, 50  AS tiempo UNION ALL
    SELECT 'Facil',        4, 5,  60  UNION ALL
    SELECT 'Normal',       6, 6,  70  UNION ALL
    SELECT 'Dificil',      7, 8,  80  UNION ALL
    SELECT 'Muy Dificil',  8, 9,  90  UNION ALL
    SELECT 'Imposible',    9, 10, 100
) t
WHERE m.nombre = 'Contrarreloj';

INSERT INTO nivel (ModoJuegoId, nombre, filas, columnas, vidas)
SELECT m.id, t.nombre, t.filas, t.columnas, t.vidas
FROM modo_juego m
JOIN (
    SELECT 'Muy Facil'   AS nombre, 4 AS filas, 4  AS columnas, 6  AS vidas UNION ALL
    SELECT 'Facil',        4, 5,  6  UNION ALL
    SELECT 'Normal',       6, 6,  8  UNION ALL
    SELECT 'Dificil',      7, 8,  10 UNION ALL
    SELECT 'Muy Dificil',  8, 9,  12 UNION ALL
    SELECT 'Imposible',    9, 10, 15
) t
WHERE m.nombre = 'Supervivencia';

INSERT INTO nivel (ModoJuegoId, nombre, filas, columnas)
SELECT id, 'Ronda 1', 1, 2 FROM modo_juego WHERE nombre = 'Infinito';
