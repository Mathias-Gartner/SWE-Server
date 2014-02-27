USE Master

IF EXISTS(SELECT * FROM sysdatabases WHERE name = 'Fahrrad')
  DROP DATABASE Fahrrad
GO

CREATE DATABASE Fahrrad

ON PRIMARY (
  Name = 'Fahrrad_DATA',
  Filename='C:\Databases\Fahrrad_DATA.mdf',
  Size = 10MB,
  Filegrowth = 10%
)
LOG ON (
  Name = 'Fahrrad_Log',
  Filename='C:\Databases\Fahrrad_LOG.ldf',
  Size = 10MB,
  Filegrowth = 10%
)
