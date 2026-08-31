-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 31-08-2026 a las 22:15:41
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
-- Estructura de tabla para la tabla `conjuntos_de_cartas`
--

DROP TABLE IF EXISTS `conjuntos_de_cartas`;
CREATE TABLE `conjuntos_de_cartas` (
  `ID` int(11) NOT NULL,
  `cantidad_pares` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `tema` varchar(100) DEFAULT NULL,
  `sprite_base` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `conjuntos_de_cartas`
--

INSERT INTO `conjuntos_de_cartas` (`ID`, `cantidad_pares`, `nombre`, `tema`, `sprite_base`) VALUES
(1, 8, 'Animales', 'Fauna', 'animales.png'),
(2, 10, 'Videojuegos', 'Gaming', 'videojuegos.png'),
(3, 12, 'Frutas', 'Alimentos', 'frutas.png');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `jugadores`
--

DROP TABLE IF EXISTS `jugadores`;
CREATE TABLE `jugadores` (
  `ID` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `jugadores`
--

INSERT INTO `jugadores` (`ID`, `nombre`, `apellido`) VALUES
(1, 'Nicolas', 'Gomez'),
(2, 'Lucas', 'Martinez'),
(3, 'Sofia', 'Rodriguez');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pares`
--

DROP TABLE IF EXISTS `pares`;
CREATE TABLE `pares` (
  `ID` int(11) NOT NULL,
  `CDC_ID` int(11) NOT NULL,
  `par_id` int(11) NOT NULL,
  `valor_pareja` varchar(100) DEFAULT NULL,
  `imagen` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pares`
--

INSERT INTO `pares` (`ID`, `CDC_ID`, `par_id`, `valor_pareja`, `imagen`) VALUES
(1, 1, 1, 'Perro', 'perro.png'),
(2, 2, 1, 'Mario', 'mario.png'),
(3, 3, 1, 'Manzana', 'manzana.png');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `partida`
--

DROP TABLE IF EXISTS `partida`;
CREATE TABLE `partida` (
  `ID` int(11) NOT NULL,
  `nivel_dificultad` varchar(50) NOT NULL,
  `fecha_inicio` datetime NOT NULL,
  `fecha_fin` datetime DEFAULT NULL,
  `tiempo_total` time DEFAULT NULL,
  `JUGADORES_ID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `partida`
--

INSERT INTO `partida` (`ID`, `nivel_dificultad`, `fecha_inicio`, `fecha_fin`, `tiempo_total`, `JUGADORES_ID`) VALUES
(1, 'Facil', '2026-08-31 10:00:00', '2026-08-31 10:05:30', '00:05:30', 1),
(2, 'Medio', '2026-08-31 11:00:00', '2026-08-31 11:08:45', '00:08:45', 2),
(3, 'Dificil', '2026-08-31 12:00:00', '2026-08-31 12:12:20', '00:12:20', 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ranking`
--

DROP TABLE IF EXISTS `ranking`;
CREATE TABLE `ranking` (
  `ID` int(11) NOT NULL,
  `PARTIDAS_ID` int(11) NOT NULL,
  `puntaje` decimal(10,2) NOT NULL,
  `dificultad_del_nivel` varchar(50) DEFAULT NULL,
  `mejor_tiempo` time DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ranking`
--

INSERT INTO `ranking` (`ID`, `PARTIDAS_ID`, `puntaje`, `dificultad_del_nivel`, `mejor_tiempo`) VALUES
(1, 1, 850.50, 'Facil', '00:05:30'),
(2, 2, 780.00, 'Medio', '00:08:45'),
(3, 3, 802.50, 'Dificil', '00:12:20');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `registros_intento_de_volteo`
--

DROP TABLE IF EXISTS `registros_intento_de_volteo`;
CREATE TABLE `registros_intento_de_volteo` (
  `ID` int(11) NOT NULL,
  `PARTIDA_ID` int(11) NOT NULL,
  `carta_id` int(11) DEFAULT NULL,
  `carta1_id` int(11) DEFAULT NULL,
  `timestamp` datetime NOT NULL,
  `fue_acierto` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `registros_intento_de_volteo`
--

INSERT INTO `registros_intento_de_volteo` (`ID`, `PARTIDA_ID`, `carta_id`, `carta1_id`, `timestamp`, `fue_acierto`) VALUES
(1, 1, 1, 2, '2026-08-31 10:01:15', 1),
(2, 2, 1, 3, '2026-08-31 11:02:30', 0),
(3, 3, 2, 4, '2026-08-31 12:03:45', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `resultado_final`
--

DROP TABLE IF EXISTS `resultado_final`;
CREATE TABLE `resultado_final` (
  `ID` int(11) NOT NULL,
  `PARTIDA_ID` int(11) NOT NULL,
  `intentos_totales` int(11) DEFAULT 0,
  `puntaje_eficiencia` decimal(10,2) DEFAULT 0.00,
  `tiempo_tardado` time DEFAULT NULL,
  `fallos` int(11) DEFAULT 0,
  `aciertos` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `resultado_final`
--

INSERT INTO `resultado_final` (`ID`, `PARTIDA_ID`, `intentos_totales`, `puntaje_eficiencia`, `tiempo_tardado`, `fallos`, `aciertos`) VALUES
(1, 1, 12, 85.50, '00:05:30', 4, 8),
(2, 2, 20, 78.00, '00:08:45', 10, 10),
(3, 3, 30, 80.25, '00:12:20', 6, 24);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tableros_generados`
--

DROP TABLE IF EXISTS `tableros_generados`;
CREATE TABLE `tableros_generados` (
  `ID` int(11) NOT NULL,
  `CDC_ID` int(11) NOT NULL,
  `PARTIDA_ID` int(11) NOT NULL,
  `estado_inicial` varchar(50) DEFAULT NULL,
  `tamaño_columnas` int(11) NOT NULL,
  `tamaño_filas` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tableros_generados`
--

INSERT INTO `tableros_generados` (`ID`, `CDC_ID`, `PARTIDA_ID`, `estado_inicial`, `tamaño_columnas`, `tamaño_filas`) VALUES
(1, 1, 1, 'Oculto', 4, 4),
(2, 2, 2, 'Oculto', 5, 4),
(3, 3, 3, 'Oculto', 6, 4);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `conjuntos_de_cartas`
--
ALTER TABLE `conjuntos_de_cartas`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `jugadores`
--
ALTER TABLE `jugadores`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `pares`
--
ALTER TABLE `pares`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `FK_PAR_CONJUNTO` (`CDC_ID`);

--
-- Indices de la tabla `partida`
--
ALTER TABLE `partida`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `FK_PARTIDA_JUGADOR` (`JUGADORES_ID`);

--
-- Indices de la tabla `ranking`
--
ALTER TABLE `ranking`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `FK_RANKING_PARTIDA` (`PARTIDAS_ID`);

--
-- Indices de la tabla `registros_intento_de_volteo`
--
ALTER TABLE `registros_intento_de_volteo`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `FK_REGISTRO_PARTIDA` (`PARTIDA_ID`);

--
-- Indices de la tabla `resultado_final`
--
ALTER TABLE `resultado_final`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `UQ_RESULTADO_PARTIDA` (`PARTIDA_ID`);

--
-- Indices de la tabla `tableros_generados`
--
ALTER TABLE `tableros_generados`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `UQ_TABLERO_PARTIDA` (`PARTIDA_ID`),
  ADD KEY `FK_TABLERO_CONJUNTO` (`CDC_ID`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `conjuntos_de_cartas`
--
ALTER TABLE `conjuntos_de_cartas`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `jugadores`
--
ALTER TABLE `jugadores`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `pares`
--
ALTER TABLE `pares`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `partida`
--
ALTER TABLE `partida`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `ranking`
--
ALTER TABLE `ranking`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `registros_intento_de_volteo`
--
ALTER TABLE `registros_intento_de_volteo`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `resultado_final`
--
ALTER TABLE `resultado_final`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `tableros_generados`
--
ALTER TABLE `tableros_generados`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `pares`
--
ALTER TABLE `pares`
  ADD CONSTRAINT `FK_PAR_CONJUNTO` FOREIGN KEY (`CDC_ID`) REFERENCES `conjuntos_de_cartas` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `partida`
--
ALTER TABLE `partida`
  ADD CONSTRAINT `FK_PARTIDA_JUGADOR` FOREIGN KEY (`JUGADORES_ID`) REFERENCES `jugadores` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `ranking`
--
ALTER TABLE `ranking`
  ADD CONSTRAINT `FK_RANKING_PARTIDA` FOREIGN KEY (`PARTIDAS_ID`) REFERENCES `partida` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `registros_intento_de_volteo`
--
ALTER TABLE `registros_intento_de_volteo`
  ADD CONSTRAINT `FK_REGISTRO_PARTIDA` FOREIGN KEY (`PARTIDA_ID`) REFERENCES `partida` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `resultado_final`
--
ALTER TABLE `resultado_final`
  ADD CONSTRAINT `FK_RESULTADO_PARTIDA` FOREIGN KEY (`PARTIDA_ID`) REFERENCES `partida` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `tableros_generados`
--
ALTER TABLE `tableros_generados`
  ADD CONSTRAINT `FK_TABLERO_CONJUNTO` FOREIGN KEY (`CDC_ID`) REFERENCES `conjuntos_de_cartas` (`ID`) ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_TABLERO_PARTIDA` FOREIGN KEY (`PARTIDA_ID`) REFERENCES `partida` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
