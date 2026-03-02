CREATE DATABASE IF NOT EXISTS `CryptoWalletApiDB`
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

USE `CryptoWalletApiDB`;

CREATE TABLE IF NOT EXISTS `Acciones` (
  `AccionID` INT NOT NULL AUTO_INCREMENT,
  `Accion` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`AccionID`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `Clientes` (
  `ClienteID` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(100) NOT NULL,
  `Email` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`ClienteID`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `Criptos` (
  `CriptoCode` VARCHAR(20) NOT NULL,
  `Nombre` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`CriptoCode`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `Estados` (
  `EstadoID` INT NOT NULL AUTO_INCREMENT,
  `Estado` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`EstadoID`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `Cuentas` (
  `CuentaID` INT NOT NULL AUTO_INCREMENT,
  `ClienteID` INT NOT NULL,
  `EstadoID` INT NOT NULL,
  PRIMARY KEY (`CuentaID`),
  KEY `ClienteID` (`ClienteID`),
  KEY `EstadoID` (`EstadoID`),
  CONSTRAINT `Cuentas_ibfk_1`
    FOREIGN KEY (`ClienteID`) REFERENCES `Clientes` (`ClienteID`),
  CONSTRAINT `Cuentas_ibfk_2`
    FOREIGN KEY (`EstadoID`) REFERENCES `Estados` (`EstadoID`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `HistorialPrecios` (
  `HistorialID` INT NOT NULL AUTO_INCREMENT,
  `CriptoCode` VARCHAR(20) NOT NULL,
  `Precio` DECIMAL(18,2) NOT NULL,
  `Fecha` DATETIME NOT NULL,
  `Fuente` VARCHAR(100) NULL,
  PRIMARY KEY (`HistorialID`),
  KEY `CriptoCode` (`CriptoCode`),
  CONSTRAINT `HistorialPrecios_ibfk_1`
    FOREIGN KEY (`CriptoCode`) REFERENCES `Criptos` (`CriptoCode`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `Operaciones` (
  `OperacionID` INT NOT NULL AUTO_INCREMENT,
  `ClienteID` INT NOT NULL,
  `CriptoCode` VARCHAR(20) NOT NULL,
  `CriptoAmount` DECIMAL(18,8) NOT NULL,
  `Money` DECIMAL(18,2) NOT NULL,
  `Action` VARCHAR(20) NOT NULL,
  `Datetime` DATETIME NOT NULL,
  PRIMARY KEY (`OperacionID`),
  KEY `ClienteID` (`ClienteID`),
  KEY `CriptoCode` (`CriptoCode`),
  CONSTRAINT `Operaciones_ibfk_1`
    FOREIGN KEY (`ClienteID`) REFERENCES `Clientes` (`ClienteID`),
  CONSTRAINT `Operaciones_ibfk_2`
    FOREIGN KEY (`CriptoCode`) REFERENCES `Criptos` (`CriptoCode`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `Movimientos` (
  `MovimientoID` INT NOT NULL AUTO_INCREMENT,
  `OperacionID` INT NOT NULL,
  `CriptoCode` VARCHAR(20) NOT NULL,
  `CantidadCripto` DECIMAL(18,8) NOT NULL,
  `EstadoBilletera` DECIMAL(18,8) NOT NULL,
  `Fecha` DATETIME NOT NULL,
  PRIMARY KEY (`MovimientoID`),
  KEY `OperacionID` (`OperacionID`),
  KEY `CriptoCode` (`CriptoCode`),
  CONSTRAINT `Movimientos_ibfk_1`
    FOREIGN KEY (`OperacionID`) REFERENCES `Operaciones` (`OperacionID`),
  CONSTRAINT `Movimientos_ibfk_2`
    FOREIGN KEY (`CriptoCode`) REFERENCES `Criptos` (`CriptoCode`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;
