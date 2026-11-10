-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 10-11-2026 a las 22:07:00
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `memory_game`
--
CREATE DATABASE IF NOT EXISTS `memory_game` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `memory_game`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `jugador`
--

DROP TABLE IF EXISTS `jugador`;
CREATE TABLE `jugador` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `jugador`
--

INSERT INTO `jugador` (`id`, `nombre`) VALUES
(1, 'FERM'),
(2, 'PEPE'),
(3, 'P');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `modo_juego`
--

DROP TABLE IF EXISTS `modo_juego`;
CREATE TABLE `modo_juego` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) DEFAULT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `disponible` tinyint(1) DEFAULT NULL,
  `CantidadJugadores` int(11) DEFAULT NULL,
  `GuardaRecords` tinyint(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `modo_juego`
--

INSERT INTO `modo_juego` (`id`, `nombre`, `descripcion`, `disponible`, `CantidadJugadores`, `GuardaRecords`) VALUES
(1, 'Clasico', 'Sin cronometro: jugas tranquilo hasta encontrar todos los pares', 1, 1, 1),
(2, 'Cronometro', 'Encontra todos los pares antes de que se acabe el tiempo', 1, 1, 1),
(3, 'Contrarreloj', 'Cada par que encontras te regala tiempo extra', 1, 1, 1),
(4, 'Infinito', 'Cada tablero que completas trae otro mas grande. Hasta donde llegas?', 1, 1, 1),
(5, 'Multijugador', '2 a 4 jugadores por turnos: gana quien encuentra mas pares', 1, 2, 0),
(6, 'Supervivencia', 'Tenes una cantidad limitada vidas: cada intento fallido te quita una', 1, 1, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `movimiento`
--

DROP TABLE IF EXISTS `movimiento`;
CREATE TABLE `movimiento` (
  `id` int(11) NOT NULL,
  `PartidaId` int(11) DEFAULT NULL,
  `numero` int(11) DEFAULT NULL,
  `fila1` int(11) DEFAULT NULL,
  `columna1` int(11) DEFAULT NULL,
  `fila2` int(11) DEFAULT NULL,
  `columna2` int(11) DEFAULT NULL,
  `acierto` tinyint(1) DEFAULT NULL,
  `momento` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `movimiento`
--

INSERT INTO `movimiento` (`id`, `PartidaId`, `numero`, `fila1`, `columna1`, `fila2`, `columna2`, `acierto`, `momento`) VALUES
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

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `nivel`
--

DROP TABLE IF EXISTS `nivel`;
CREATE TABLE `nivel` (
  `id` int(11) NOT NULL,
  `ModoJuegoId` int(11) DEFAULT NULL,
  `nombre` varchar(50) DEFAULT NULL,
  `filas` int(11) DEFAULT NULL,
  `columnas` int(11) DEFAULT NULL,
  `pares` int(11) GENERATED ALWAYS AS (`filas` * `columnas` DIV 2) STORED,
  `TiempoLimite` int(11) DEFAULT NULL,
  `vidas` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `nivel`
--

INSERT INTO `nivel` (`id`, `ModoJuegoId`, `nombre`, `filas`, `columnas`, `TiempoLimite`, `vidas`) VALUES
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

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `partida`
--

DROP TABLE IF EXISTS `partida`;
CREATE TABLE `partida` (
  `id` int(11) NOT NULL,
  `JugadorId` int(11) DEFAULT NULL,
  `NivelId` int(11) DEFAULT NULL,
  `TematicaId` int(11) DEFAULT NULL,
  `fecha` datetime DEFAULT NULL,
  `intentos` int(11) DEFAULT NULL,
  `aciertos` int(11) DEFAULT NULL,
  `fallos` int(11) DEFAULT NULL,
  `TiempoSegundos` int(11) DEFAULT NULL,
  `puntuacion` int(11) DEFAULT NULL,
  `gano` tinyint(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `partida`
--

INSERT INTO `partida` (`id`, `JugadorId`, `NivelId`, `TematicaId`, `fecha`, `intentos`, `aciertos`, `fallos`, `TiempoSegundos`, `puntuacion`, `gano`) VALUES
(1, 1, 1, 1, '2026-11-05 20:53:07', 11, 8, 3, 16, 1054, 1),
(2, 2, 1, 1, '2026-11-05 21:03:44', 18, 8, 10, 32, 968, 1);

--
-- Disparadores `partida`
--
DROP TRIGGER IF EXISTS `trg_partida_record`;
DELIMITER $$
CREATE TRIGGER `trg_partida_record` AFTER INSERT ON `partida` FOR EACH ROW BEGIN
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
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `par_descubierto`
--

DROP TABLE IF EXISTS `par_descubierto`;
CREATE TABLE `par_descubierto` (
  `id` int(11) NOT NULL,
  `PartidaId` int(11) DEFAULT NULL,
  `IdValor` int(11) DEFAULT NULL,
  `puntos` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `par_descubierto`
--

INSERT INTO `par_descubierto` (`id`, `PartidaId`, `IdValor`, `puntos`) VALUES
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

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `record`
--

DROP TABLE IF EXISTS `record`;
CREATE TABLE `record` (
  `id` int(11) NOT NULL,
  `PartidaId` int(11) DEFAULT NULL,
  `JugadorId` int(11) DEFAULT NULL,
  `jugador` varchar(50) DEFAULT NULL,
  `modo` varchar(50) DEFAULT NULL,
  `nivel` varchar(50) DEFAULT NULL,
  `tematica` varchar(50) DEFAULT NULL,
  `intentos` int(11) DEFAULT NULL,
  `aciertos` int(11) DEFAULT NULL,
  `fallos` int(11) DEFAULT NULL,
  `TiempoSegundos` int(11) DEFAULT NULL,
  `puntuacion` int(11) DEFAULT NULL,
  `fecha` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `record`
--

INSERT INTO `record` (`id`, `PartidaId`, `JugadorId`, `jugador`, `modo`, `nivel`, `tematica`, `intentos`, `aciertos`, `fallos`, `TiempoSegundos`, `puntuacion`, `fecha`) VALUES
(1, 1, 1, 'FERM', 'Clasico', 'Muy Facil', 'Mezcla', 11, 8, 3, 16, 1054, '2026-11-05 20:53:07'),
(2, 2, 2, 'PEPE', 'Clasico', 'Muy Facil', 'Mezcla', 18, 8, 10, 32, 968, '2026-11-05 21:03:44');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tematica`
--

DROP TABLE IF EXISTS `tematica`;
CREATE TABLE `tematica` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) DEFAULT NULL,
  `carpeta` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tematica`
--

INSERT INTO `tematica` (`id`, `nombre`, `carpeta`) VALUES
(1, 'Mezcla', 'mezcla');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `jugador`
--
ALTER TABLE `jugador`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `modo_juego`
--
ALTER TABLE `modo_juego`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `movimiento`
--
ALTER TABLE `movimiento`
  ADD PRIMARY KEY (`id`),
  ADD KEY `PartidaId` (`PartidaId`);

--
-- Indices de la tabla `nivel`
--
ALTER TABLE `nivel`
  ADD PRIMARY KEY (`id`),
  ADD KEY `ModoJuegoId` (`ModoJuegoId`);

--
-- Indices de la tabla `partida`
--
ALTER TABLE `partida`
  ADD PRIMARY KEY (`id`),
  ADD KEY `JugadorId` (`JugadorId`),
  ADD KEY `NivelId` (`NivelId`),
  ADD KEY `TematicaId` (`TematicaId`);

--
-- Indices de la tabla `par_descubierto`
--
ALTER TABLE `par_descubierto`
  ADD PRIMARY KEY (`id`),
  ADD KEY `PartidaId` (`PartidaId`);

--
-- Indices de la tabla `record`
--
ALTER TABLE `record`
  ADD PRIMARY KEY (`id`),
  ADD KEY `PartidaId` (`PartidaId`),
  ADD KEY `JugadorId` (`JugadorId`);

--
-- Indices de la tabla `tematica`
--
ALTER TABLE `tematica`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `jugador`
--
ALTER TABLE `jugador`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `modo_juego`
--
ALTER TABLE `modo_juego`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `movimiento`
--
ALTER TABLE `movimiento`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- AUTO_INCREMENT de la tabla `nivel`
--
ALTER TABLE `nivel`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=38;

--
-- AUTO_INCREMENT de la tabla `partida`
--
ALTER TABLE `partida`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `par_descubierto`
--
ALTER TABLE `par_descubierto`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `record`
--
ALTER TABLE `record`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `tematica`
--
ALTER TABLE `tematica`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `movimiento`
--
ALTER TABLE `movimiento`
  ADD CONSTRAINT `movimiento_ibfk_1` FOREIGN KEY (`PartidaId`) REFERENCES `partida` (`id`);

--
-- Filtros para la tabla `nivel`
--
ALTER TABLE `nivel`
  ADD CONSTRAINT `nivel_ibfk_1` FOREIGN KEY (`ModoJuegoId`) REFERENCES `modo_juego` (`id`);

--
-- Filtros para la tabla `partida`
--
ALTER TABLE `partida`
  ADD CONSTRAINT `partida_ibfk_1` FOREIGN KEY (`JugadorId`) REFERENCES `jugador` (`id`),
  ADD CONSTRAINT `partida_ibfk_2` FOREIGN KEY (`NivelId`) REFERENCES `nivel` (`id`),
  ADD CONSTRAINT `partida_ibfk_3` FOREIGN KEY (`TematicaId`) REFERENCES `tematica` (`id`);

--
-- Filtros para la tabla `par_descubierto`
--
ALTER TABLE `par_descubierto`
  ADD CONSTRAINT `par_descubierto_ibfk_1` FOREIGN KEY (`PartidaId`) REFERENCES `partida` (`id`);

--
-- Filtros para la tabla `record`
--
ALTER TABLE `record`
  ADD CONSTRAINT `record_ibfk_1` FOREIGN KEY (`PartidaId`) REFERENCES `partida` (`id`),
  ADD CONSTRAINT `record_ibfk_2` FOREIGN KEY (`JugadorId`) REFERENCES `jugador` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
