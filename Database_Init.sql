-- PT. XYZ Overtime Management System - Database Initialization Script
-- For Microsoft SQL Server Express

-- Create Database
USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'PTXYZ_OvertimeDB')
BEGIN
    ALTER DATABASE PTXYZ_OvertimeDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PTXYZ_OvertimeDB;
END
GO

CREATE DATABASE PTXYZ_OvertimeDB;
GO

USE PTXYZ_OvertimeDB;
GO

-- Create Department Table
CREATE TABLE Department (
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL
);
GO

-- Create Employee Table
CREATE TABLE Employee (
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
    NIK NVARCHAR(20) NOT NULL UNIQUE,
    FullName NVARCHAR(100) NOT NULL,
    DepartmentId INT NOT NULL,
    Position NVARCHAR(50) NOT NULL,
    LaptopAllowance BIT NOT NULL DEFAULT 0,
    MealAllowance BIT NOT NULL DEFAULT 0,
    Address NVARCHAR(255) NULL,
    PhoneNumber NVARCHAR(20) NULL,
    JoinDate DATE NULL,
    CONSTRAINT FK_Employee_Department FOREIGN KEY (DepartmentId) 
        REFERENCES Department(DepartmentId)
);
GO

-- Create OverTime Table
CREATE TABLE OverTime (
    OverTimeId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    Date DATE NOT NULL,
    TimeStart DATETIME NOT NULL,
    TimeFinish DATETIME NOT NULL,
    ActualOTHours DECIMAL(5,2) NOT NULL,
    CalculatedOTHours DECIMAL(5,2) NOT NULL,
    Remarks NVARCHAR(500) NULL,
    CONSTRAINT FK_OverTime_Employee FOREIGN KEY (EmployeeId) 
        REFERENCES Employee(EmployeeId)
);
GO

-- Insert Sample Data - Departments
INSERT INTO Department (DepartmentName, Description) VALUES
('Production', 'Production Department'),
('Engineering', 'Engineering Department'),
('Quality Control', 'Quality Control Department'),
('Maintenance', 'Maintenance Department'),
('Human Resources', 'HR Department'),
('Finance', 'Finance Department'),
('Information Technology', 'IT Department');
GO

-- Insert Sample Data - Employees
INSERT INTO Employee (NIK, FullName, DepartmentId, Position, LaptopAllowance, MealAllowance, Address, PhoneNumber, JoinDate) VALUES
('EMP001', 'John Anderson', 1, 'Manager', 1, 0, 'Jakarta Selatan', '081234567890', '2020-01-15'),
('EMP002', 'Sarah Wilson', 1, 'Supervisor', 1, 1, 'Jakarta Timur', '081234567891', '2020-03-20'),
('EMP003', 'Michael Brown', 2, 'Leader', 0, 1, 'Bekasi', '081234567892', '2021-02-10'),
('EMP004', 'Emily Davis', 2, 'Technician', 0, 1, 'Tangerang', '081234567893', '2021-05-05'),
('EMP005', 'David Martinez', 3, 'Operator', 0, 1, 'Bogor', '081234567894', '2022-01-20'),
('EMP006', 'Jessica Taylor', 3, 'Supervisor', 1, 1, 'Jakarta Barat', '081234567895', '2021-08-15'),
('EMP007', 'James Johnson', 4, 'Technician', 0, 1, 'Depok', '081234567896', '2022-03-10'),
('EMP008', 'Linda Garcia', 5, 'Manager', 1, 0, 'Jakarta Pusat', '081234567897', '2019-06-01'),
('EMP009', 'Robert Rodriguez', 6, 'Supervisor', 1, 1, 'Jakarta Utara', '081234567898', '2020-09-25'),
('EMP010', 'Maria Hernandez', 7, 'Leader', 0, 1, 'Bekasi Timur', '081234567899', '2021-11-12'),
('EMP011', 'William Moore', 1, 'Operator', 0, 1, 'Tangerang Selatan', '081234567800', '2022-06-20'),
('EMP012', 'Patricia Lee', 2, 'Operator', 0, 1, 'Jakarta Selatan', '081234567801', '2022-08-05'),
('EMP013', 'Christopher Walker', 4, 'Leader', 0, 1, 'Bogor Barat', '081234567802', '2021-04-18'),
('EMP014', 'Barbara Hall', 3, 'Technician', 0, 1, 'Depok Timur', '081234567803', '2022-02-28'),
('EMP015', 'Daniel Allen', 7, 'Technician', 0, 1, 'Jakarta Timur', '081234567804', '2022-05-15');
GO

-- Insert Sample Data - Overtime Entries
INSERT INTO OverTime (EmployeeId, Date, TimeStart, TimeFinish, ActualOTHours, CalculatedOTHours, Remarks) VALUES
(1, '2024-10-01', '2024-10-01 17:00:00', '2024-10-01 19:30:00', 2.5, 5.0, 'Project deadline'),
(2, '2024-10-02', '2024-10-02 17:00:00', '2024-10-02 20:00:00', 3.0, 6.0, 'Production support'),
(3, '2024-10-03', '2024-10-03 17:00:00', '2024-10-03 18:30:00', 1.5, 3.0, 'System maintenance'),
(4, '2024-10-05', '2024-10-05 17:00:00', '2024-10-05 19:00:00', 2.0, 4.0, 'Equipment repair'),
(5, '2024-10-07', '2024-10-07 17:00:00', '2024-10-07 18:00:00', 1.0, 2.0, 'Quality inspection'),
(6, '2024-10-08', '2024-10-08 17:00:00', '2024-10-08 20:00:00', 3.0, 6.0, 'Urgent quality check'),
(7, '2024-10-10', '2024-10-10 17:00:00', '2024-10-10 19:30:00', 2.5, 5.0, 'Machine troubleshooting'),
(2, '2024-10-12', '2024-10-12 17:00:00', '2024-10-12 18:30:00', 1.5, 3.0, 'Team meeting'),
(10, '2024-10-14', '2024-10-14 17:00:00', '2024-10-14 19:00:00', 2.0, 4.0, 'System upgrade'),
(11, '2024-10-15', '2024-10-15 17:00:00', '2024-10-15 18:30:00', 1.5, 3.0, 'Production line support'),
(13, '2024-10-18', '2024-10-18 17:00:00', '2024-10-18 20:00:00', 3.0, 6.0, 'Emergency maintenance'),
(14, '2024-10-20', '2024-10-20 17:00:00', '2024-10-20 19:30:00', 2.5, 5.0, 'Quality audit preparation'),
(3, '2024-10-22', '2024-10-22 17:00:00', '2024-10-22 18:00:00', 1.0, 2.0, 'Documentation update'),
(9, '2024-10-25', '2024-10-25 17:00:00', '2024-10-25 19:00:00', 2.0, 4.0, 'Budget review'),
(15, '2024-10-28', '2024-10-28 17:00:00', '2024-10-28 19:30:00', 2.5, 5.0, 'Network maintenance');
GO

-- Create Indexes for Better Performance
CREATE INDEX IX_Employee_NIK ON Employee(NIK);
CREATE INDEX IX_Employee_DepartmentId ON Employee(DepartmentId);
CREATE INDEX IX_OverTime_EmployeeId ON OverTime(EmployeeId);
CREATE INDEX IX_OverTime_Date ON OverTime(Date);
GO

PRINT 'Database initialization completed successfully!';
PRINT 'Database: PTXYZ_OvertimeDB';
PRINT 'Tables created: Department, Employee, OverTime';
PRINT 'Sample data inserted successfully';
GO
