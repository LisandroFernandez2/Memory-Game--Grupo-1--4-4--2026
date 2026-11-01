-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 02-11-2026 a las 00:33:28
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
-- Estructura de tabla para la tabla `carta`
--

DROP TABLE IF EXISTS `carta`;
CREATE TABLE `carta` (
  `id` int(11) NOT NULL,
  `partida_id` int(11) NOT NULL,
  `fila` int(11) NOT NULL,
  `columna` int(11) NOT NULL,
  `id_valor` int(11) NOT NULL,
  `esta_descubierta` tinyint(1) NOT NULL DEFAULT 0,
  `esta_emparejada` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `jugador`
--

DROP TABLE IF EXISTS `jugador`;
CREATE TABLE `jugador` (
  `id` int(11) NOT NULL,
  `nombre` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `modo_juego`
--

DROP TABLE IF EXISTS `modo_juego`;
CREATE TABLE `modo_juego` (
  `id` int(11) NOT NULL,
  `nombre` varchar(30) NOT NULL,
  `descripcion` varchar(150) DEFAULT NULL,
  `disponible` tinyint(1) NOT NULL DEFAULT 0,
  `cantidad_jugadores` int(11) NOT NULL DEFAULT 1,
  `guarda_records` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `movimiento`
--

DROP TABLE IF EXISTS `movimiento`;
CREATE TABLE `movimiento` (
  `id` int(11) NOT NULL,
  `partida_id` int(11) NOT NULL,
  `fila1` int(11) NOT NULL,
  `columna1` int(11) NOT NULL,
  `fila2` int(11) NOT NULL,
  `columna2` int(11) NOT NULL,
  `acierto` tinyint(1) NOT NULL,
  `momento` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `nivel`
--

DROP TABLE IF EXISTS `nivel`;
CREATE TABLE `nivel` (
  `id` int(11) NOT NULL,
  `modo_juego_id` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL,
  `filas` int(11) NOT NULL,
  `columnas` int(11) NOT NULL,
  `pares` int(11) NOT NULL,
  `tiempo_limite` int(11) DEFAULT NULL,
  `vidas` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `partida`
--

DROP TABLE IF EXISTS `partida`;
CREATE TABLE `partida` (
  `id` int(11) NOT NULL,
  `jugador_id` int(11) NOT NULL,
  `nivel_id` int(11) NOT NULL,
  `tematica_id` int(11) NOT NULL,
  `fecha` datetime NOT NULL,
  `intentos` int(11) NOT NULL DEFAULT 0,
  `aciertos` int(11) NOT NULL DEFAULT 0,
  `fallos` int(11) NOT NULL DEFAULT 0,
  `tiempo_segundos` int(11) NOT NULL DEFAULT 0,
  `puntuacion` int(11) NOT NULL DEFAULT 0,
  `gano` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `par_descubierto`
--

DROP TABLE IF EXISTS `par_descubierto`;
CREATE TABLE `par_descubierto` (
  `id` int(11) NOT NULL,
  `partida_id` int(11) NOT NULL,
  `id_valor` int(11) NOT NULL,
  `puntos` int(11) NOT NULL DEFAULT 100
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `record`
--

DROP TABLE IF EXISTS `record`;
CREATE TABLE `record` (
  `id` int(11) NOT NULL,
  `partida_id` int(11) NOT NULL,
  `jugador_id` int(11) NOT NULL,
  `nivel` varchar(100) NOT NULL,
  `intentos` int(11) NOT NULL,
  `aciertos` int(11) NOT NULL,
  `fallos` int(11) NOT NULL,
  `tiempo_segundos` int(11) NOT NULL,
  `puntuacion` int(11) NOT NULL,
  `fecha` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tematica`
--

DROP TABLE IF EXISTS `tematica`;
CREATE TABLE `tematica` (
  `id` int(11) NOT NULL,
  `nombre` varchar(30) NOT NULL,
  `carpeta` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `carta`
--
ALTER TABLE `carta`
  ADD PRIMARY KEY (`id`),
  ADD KEY `partida_id` (`partida_id`);

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
  ADD KEY `partida_id` (`partida_id`);

--
-- Indices de la tabla `nivel`
--
ALTER TABLE `nivel`
  ADD PRIMARY KEY (`id`),
  ADD KEY `modo_juego_id` (`modo_juego_id`);

--
-- Indices de la tabla `partida`
--
ALTER TABLE `partida`
  ADD PRIMARY KEY (`id`),
  ADD KEY `jugador_id` (`jugador_id`),
  ADD KEY `nivel_id` (`nivel_id`),
  ADD KEY `tematica_id` (`tematica_id`);

--
-- Indices de la tabla `par_descubierto`
--
ALTER TABLE `par_descubierto`
  ADD PRIMARY KEY (`id`),
  ADD KEY `partida_id` (`partida_id`);

--
-- Indices de la tabla `record`
--
ALTER TABLE `record`
  ADD PRIMARY KEY (`id`),
  ADD KEY `partida_id` (`partida_id`),
  ADD KEY `jugador_id` (`jugador_id`);

--
-- Indices de la tabla `tematica`
--
ALTER TABLE `tematica`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `carta`
--
ALTER TABLE `carta`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `jugador`
--
ALTER TABLE `jugador`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `modo_juego`
--
ALTER TABLE `modo_juego`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `movimiento`
--
ALTER TABLE `movimiento`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `nivel`
--
ALTER TABLE `nivel`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `partida`
--
ALTER TABLE `partida`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `par_descubierto`
--
ALTER TABLE `par_descubierto`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `record`
--
ALTER TABLE `record`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `tematica`
--
ALTER TABLE `tematica`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `carta`
--
ALTER TABLE `carta`
  ADD CONSTRAINT `carta_ibfk_1` FOREIGN KEY (`partida_id`) REFERENCES `partida` (`id`);

--
-- Filtros para la tabla `movimiento`
--
ALTER TABLE `movimiento`
  ADD CONSTRAINT `movimiento_ibfk_1` FOREIGN KEY (`partida_id`) REFERENCES `partida` (`id`);

--
-- Filtros para la tabla `nivel`
--
ALTER TABLE `nivel`
  ADD CONSTRAINT `nivel_ibfk_1` FOREIGN KEY (`modo_juego_id`) REFERENCES `modo_juego` (`id`);

--
-- Filtros para la tabla `partida`
--
ALTER TABLE `partida`
  ADD CONSTRAINT `partida_ibfk_1` FOREIGN KEY (`jugador_id`) REFERENCES `jugador` (`id`),
  ADD CONSTRAINT `partida_ibfk_2` FOREIGN KEY (`nivel_id`) REFERENCES `nivel` (`id`),
  ADD CONSTRAINT `partida_ibfk_3` FOREIGN KEY (`tematica_id`) REFERENCES `tematica` (`id`);

--
-- Filtros para la tabla `par_descubierto`
--
ALTER TABLE `par_descubierto`
  ADD CONSTRAINT `par_descubierto_ibfk_1` FOREIGN KEY (`partida_id`) REFERENCES `partida` (`id`);

--
-- Filtros para la tabla `record`
--
ALTER TABLE `record`
  ADD CONSTRAINT `record_ibfk_1` FOREIGN KEY (`partida_id`) REFERENCES `partida` (`id`),
  ADD CONSTRAINT `record_ibfk_2` FOREIGN KEY (`jugador_id`) REFERENCES `jugador` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
