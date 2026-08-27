-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 27-08-2026 a las 13:47:22
-- Versión del servidor: 8.0.33
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

CREATE DATABASE IF NOT EXISTS `memory_game` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `memory_game`;


DROP TABLE IF EXISTS `conjuntos_de_cartas`;
CREATE TABLE `conjuntos_de_cartas` (
  `ID` int NOT NULL,
  `cantidad_pares` int NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `tema` varchar(100) DEFAULT NULL,
  `sprite_base` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DROP TABLE IF EXISTS `jugadores`;
CREATE TABLE `jugadores` (
  `ID` int NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DROP TABLE IF EXISTS `pares`;
CREATE TABLE `pares` (
  `ID` int NOT NULL,
  `CDC_ID` int NOT NULL,
  `par_id` int NOT NULL,
  `valor_pareja` varchar(100) DEFAULT NULL,
  `imagen` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `partida`;
CREATE TABLE `partida` (
  `ID` int NOT NULL,
  `nivel_dificultad` varchar(50) NOT NULL,
  `fecha_inicio` datetime NOT NULL,
  `fecha_fin` datetime DEFAULT NULL,
  `tiempo_total` time DEFAULT NULL,
  `JUGADORES_ID` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `ranking`;
CREATE TABLE `ranking` (
  `ID` int NOT NULL,
  `PARTIDAS_ID` int NOT NULL,
  `puntaje` decimal(10,2) NOT NULL,
  `dificultad_del_nivel` varchar(50) DEFAULT NULL,
  `mejor_tiempo` time DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `registros_intento_de_volteo`;
CREATE TABLE `registros_intento_de_volteo` (
  `ID` int NOT NULL,
  `PARTIDA_ID` int NOT NULL,
  `carta_id` int DEFAULT NULL,
  `carta1_id` int DEFAULT NULL,
  `timestamp` datetime NOT NULL,
  `fue_acierto` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `resultado_final`;
CREATE TABLE `resultado_final` (
  `ID` int NOT NULL,
  `PARTIDA_ID` int NOT NULL,
  `intentos_totales` int DEFAULT '0',
  `puntaje_eficiencia` decimal(10,2) DEFAULT '0.00',
  `tiempo_tardado` time DEFAULT NULL,
  `fallos` int DEFAULT '0',
  `aciertos` int DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `tableros_generados`;
CREATE TABLE `tableros_generados` (
  `ID` int NOT NULL,
  `CDC_ID` int NOT NULL,
  `PARTIDA_ID` int NOT NULL,
  `estado_inicial` varchar(50) DEFAULT NULL,
  `tamaño_columnas` int NOT NULL,
  `tamaño_filas` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


ALTER TABLE `conjuntos_de_cartas`
  ADD PRIMARY KEY (`ID`);

ALTER TABLE `jugadores`
  ADD PRIMARY KEY (`ID`);

ALTER TABLE `pares`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `FK_PAR_CONJUNTO` (`CDC_ID`);

ALTER TABLE `partida`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `FK_PARTIDA_JUGADOR` (`JUGADORES_ID`);

ALTER TABLE `ranking`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `FK_RANKING_PARTIDA` (`PARTIDAS_ID`);

ALTER TABLE `registros_intento_de_volteo`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `FK_REGISTRO_PARTIDA` (`PARTIDA_ID`);

ALTER TABLE `resultado_final`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `UQ_RESULTADO_PARTIDA` (`PARTIDA_ID`);


ALTER TABLE `tableros_generados`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `UQ_TABLERO_PARTIDA` (`PARTIDA_ID`),
  ADD KEY `FK_TABLERO_CONJUNTO` (`CDC_ID`);

ALTER TABLE `conjuntos_de_cartas`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT;

ALTER TABLE `jugadores`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT;

ALTER TABLE `pares`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT;


ALTER TABLE `partida`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT;

ALTER TABLE `ranking`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT;

ALTER TABLE `registros_intento_de_volteo`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT;

ALTER TABLE `resultado_final`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT;

ALTER TABLE `tableros_generados`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT;


ALTER TABLE `pares`
  ADD CONSTRAINT `FK_PAR_CONJUNTO` FOREIGN KEY (`CDC_ID`) REFERENCES `conjuntos_de_cartas` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;


ALTER TABLE `partida`
  ADD CONSTRAINT `FK_PARTIDA_JUGADOR` FOREIGN KEY (`JUGADORES_ID`) REFERENCES `jugadores` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;


ALTER TABLE `ranking`
  ADD CONSTRAINT `FK_RANKING_PARTIDA` FOREIGN KEY (`PARTIDAS_ID`) REFERENCES `partida` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `registros_intento_de_volteo`
  ADD CONSTRAINT `FK_REGISTRO_PARTIDA` FOREIGN KEY (`PARTIDA_ID`) REFERENCES `partida` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `resultado_final`
  ADD CONSTRAINT `FK_RESULTADO_PARTIDA` FOREIGN KEY (`PARTIDA_ID`) REFERENCES `partida` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `tableros_generados`
  ADD CONSTRAINT `FK_TABLERO_CONJUNTO` FOREIGN KEY (`CDC_ID`) REFERENCES `conjuntos_de_cartas` (`ID`) ON DELETE RESTRICT ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_TABLERO_PARTIDA` FOREIGN KEY (`PARTIDA_ID`) REFERENCES `partida` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
