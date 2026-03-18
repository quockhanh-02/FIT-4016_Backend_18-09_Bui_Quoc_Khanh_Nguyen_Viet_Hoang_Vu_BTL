CREATE DATABASE FleetManagementDB;
GO

USE FleetManagementDB;
GO

------------------------------------------------
-- 1. Vehicles (Phương tiện)
------------------------------------------------
CREATE TABLE Vehicles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    LicensePlate NVARCHAR(20) NOT NULL,
    FuelNorm DECIMAL(18,2) NOT NULL,
    VehicleType NVARCHAR(20) NOT NULL
);
GO

------------------------------------------------
-- 2. Drivers (Tài xế)
------------------------------------------------
CREATE TABLE Drivers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    LicenseNumber NVARCHAR(50) NOT NULL,
    LicenseExpiry DATETIME2 NOT NULL
);
GO

------------------------------------------------
-- 3. Trips (Chuyến đi)
------------------------------------------------
CREATE TABLE Trips (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StartLocation NVARCHAR(200) NOT NULL,
    EndLocation NVARCHAR(200) NOT NULL,
    VehicleId INT NOT NULL,
    DriverId INT NOT NULL
);
GO

------------------------------------------------
-- 4. FuelLogs (Nhật ký nhiên liệu)
------------------------------------------------
CREATE TABLE FuelLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    FuelDate DATETIME2 NOT NULL,
    Odometer INT NOT NULL,
    Liters DECIMAL(18,2) NOT NULL,
    TotalCost DECIMAL(18,2) NOT NULL
);
GO

------------------------------------------------
-- 5. Maintenances (Bảo trì)
------------------------------------------------
CREATE TABLE Maintenances (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    MaintenanceDate DATETIME2 NOT NULL,
    Cost DECIMAL(18,2) NOT NULL,
    Notes NVARCHAR(500),
    Status NVARCHAR(50) NOT NULL
);
GO

------------------------------------------------
-- 6. Users (Tài khoản hệ thống)
------------------------------------------------
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    Role NVARCHAR(50) NOT NULL
);
GO

------------------------------------------------
-- FOREIGN KEY
------------------------------------------------

ALTER TABLE Trips
ADD CONSTRAINT FK_Trips_Vehicles
FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id);

ALTER TABLE Trips
ADD CONSTRAINT FK_Trips_Drivers
FOREIGN KEY (DriverId) REFERENCES Drivers(Id);

ALTER TABLE FuelLogs
ADD CONSTRAINT FK_FuelLogs_Vehicles
FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id);

ALTER TABLE Maintenances
ADD CONSTRAINT FK_Maintenances_Vehicles
FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id);
GO