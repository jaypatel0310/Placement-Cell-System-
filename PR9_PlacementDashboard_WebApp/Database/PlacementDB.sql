-- OPTIONAL. The website creates this database automatically the first time you open it,
-- so you do NOT have to run this script.
-- Run it only if you want to RESET the database (WARNING: it deletes PlacementDB and recreates it).
-- PR9 : Placement Cell Dashboard  -  run this WHOLE script once in SSMS / VS SQL Server Object Explorer
IF DB_ID('PlacementDB') IS NOT NULL
BEGIN
    ALTER DATABASE PlacementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PlacementDB;
END
GO
CREATE DATABASE PlacementDB;
GO
USE PlacementDB;
GO
CREATE TABLE Students(
    StudentId  INT IDENTITY(1,1) PRIMARY KEY,
    FullName   VARCHAR(100) NOT NULL,
    Department VARCHAR(50)  NOT NULL,
    CGPA       DECIMAL(3,2) NOT NULL,
    Email      VARCHAR(100) NULL);
CREATE TABLE Companies(
    CompanyId   INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName VARCHAR(100) NOT NULL,
    Sector      VARCHAR(50)  NOT NULL,
    PackageLPA  DECIMAL(5,2) NOT NULL);
CREATE TABLE Placements(
    PlacementId   INT IDENTITY(1,1) PRIMARY KEY,
    StudentId     INT NOT NULL REFERENCES Students(StudentId),
    CompanyId     INT NOT NULL REFERENCES Companies(CompanyId),
    Role          VARCHAR(80) NOT NULL,
    PlacementDate DATE NOT NULL,
    Status        VARCHAR(20) NOT NULL CHECK (Status IN ('Placed','Offered','Pending')));
GO
INSERT INTO Students(FullName,Department,CGPA,Email) VALUES
('Aarav Patel','Computer',8.90,'aarav@mail.com'),
('Diya Shah','Computer',9.10,'diya@mail.com'),
('Kabir Mehta','IT',8.20,'kabir@mail.com'),
('Isha Joshi','IT',7.80,'isha@mail.com'),
('Rohan Desai','Electronics',7.50,'rohan@mail.com'),
('Meera Trivedi','Computer',8.60,'meera@mail.com'),
('Jay Vyas','IT',8.00,'jay@mail.com'),
('Priya Raval','Mechanical',7.20,'priya@mail.com'),
('Harsh Parmar','Computer',9.30,'harsh@mail.com'),
('Nidhi Bhatt','Electronics',8.40,'nidhi@mail.com');
INSERT INTO Companies(CompanyName,Sector,PackageLPA) VALUES
('TCS','IT Services',3.60),
('Infosys','IT Services',4.00),
('Wipro','IT Services',3.80),
('Accenture','Consulting',6.50),
('Zoho','Product',8.00),
('L&T','Engineering',5.20);
INSERT INTO Placements(StudentId,CompanyId,Role,PlacementDate,Status) VALUES
(1,5,'Software Developer','2026-08-10','Placed'),
(2,4,'Associate Engineer','2026-08-12','Placed'),
(3,1,'System Engineer','2026-08-15','Placed'),
(4,2,'Systems Specialist','2026-08-18','Offered'),
(5,6,'Graduate Engineer Trainee','2026-08-20','Placed'),
(6,3,'Project Engineer','2026-08-22','Placed'),
(7,1,'System Engineer','2026-09-01','Offered'),
(8,6,'Design Trainee','2026-09-03','Pending'),
(9,5,'Backend Developer','2026-09-05','Placed'),
(10,2,'Systems Engineer','2026-09-08','Placed'),
(4,3,'Analyst','2026-09-10','Pending'),
(7,4,'Technology Analyst','2026-09-12','Offered');
GO
CREATE VIEW vw_PlacementReport AS
SELECT p.PlacementId, s.StudentId, s.FullName, s.Department, s.CGPA,
       c.CompanyId, c.CompanyName, c.Sector, c.PackageLPA,
       p.Role, p.PlacementDate, p.Status
FROM Placements p
JOIN Students  s ON s.StudentId = p.StudentId
JOIN Companies c ON c.CompanyId = p.CompanyId;
GO
SELECT * FROM vw_PlacementReport;   -- you should see 12 rows
