USE Fahrrad

--Abteilungen Table 
IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'ABTEILUNGEN'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'ABTEILUNGEN'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [ABTEILUNGEN]
END 
GO
CREATE TABLE 
[ABTEILUNGEN]
(
   [ABT_NR] numeric(38, 0)  NOT NULL,
   [LEITER] numeric(38, 0)  NOT NULL,
   [NAME] varchar(50)  NOT NULL,
   [ORT] varchar(50)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'ABT_PK' AND type in (N'PK'))
ALTER TABLE [ABTEILUNGEN] DROP CONSTRAINT [ABT_PK]
 GO
ALTER TABLE [ABTEILUNGEN]
 ADD CONSTRAINT [ABT_PK]
 PRIMARY KEY 
   CLUSTERED ([ABT_NR] ASC)

GO

--Angstellte Table 
IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'ANGESTELLTE'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'ANGESTELLTE'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[ANGESTELLTE]
END 
GO

CREATE TABLE 
[dbo].[ANGESTELLTE]
(
   [ANG_NR] numeric(38, 0)  NOT NULL,
   [ABT_NR] numeric(38, 0)  NOT NULL,
   [AUFGABENBESCHREIBUNG] varchar(50)  NOT NULL,
   [BERUF] varchar(50)  NOT NULL,
   [NACHNAME] varchar(50)  NOT NULL,
   [VORNAME] varchar(50)  NOT NULL,
   [GESCHLECHT] char(1)  NOT NULL,
   [EINTRITTSDATUM] datetime2(0)  NULL,
   [GEHALT] numeric(9, 2) DEFAULT 0  NULL,
   [ABZUEGE] numeric(9, 2) DEFAULT 0  NULL,
   [ORT] varchar(50)  NULL,
   [STRASSE] varchar(50)  NULL,
   [ZEITSTEMPEL] datetime2(0)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'ANG_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[ANGESTELLTE] DROP CONSTRAINT [ANG_PK]
 GO

ALTER TABLE [dbo].[ANGESTELLTE]
 ADD CONSTRAINT [ANG_PK]
 PRIMARY KEY 
   CLUSTERED ([ANG_NR] ASC)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'GESCHLECHT'  AND sc.name=N'dbo'  AND type in (N'C'))
ALTER TABLE [dbo].[ANGESTELLTE] DROP CONSTRAINT [GESCHLECHT]
 GO

ALTER TABLE [dbo].[ANGESTELLTE]
 ADD CONSTRAINT [GESCHLECHT]
 CHECK (GESCHLECHT IN ( 'w', 'm' ))

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'ANG_ABT_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[ANGESTELLTE] DROP CONSTRAINT [ANG_ABT_FK]
 GO

ALTER TABLE [dbo].[ANGESTELLTE]
 ADD CONSTRAINT [ANG_ABT_FK]
 FOREIGN KEY 
   ([ABT_NR])
 REFERENCES 
   [dbo].[ABTEILUNGEN]     ([ABT_NR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO


--Artikel Table
IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'ARTIKEL'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'ARTIKEL'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[ARTIKEL]
END 
GO

CREATE TABLE 
[dbo].[ARTIKEL]
(
   [TNR] numeric(38, 0)  NOT NULL,
   [BEZEICHNUNG] varchar(50)  NOT NULL,
   [ARTIKEL_TYP] varchar(50)  NOT NULL,
   [VERKAUFSPREIS] float(53)  NULL,
   [JAHRESUMSATZ] float(53)  NULL,
   [ZEITSTEMPEL] datetime2(0)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AR_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[ARTIKEL] DROP CONSTRAINT [AR_PK]
 GO
 
ALTER TABLE [dbo].[ARTIKEL]
 ADD CONSTRAINT [AR_PK]
 PRIMARY KEY 
   CLUSTERED ([TNR] ASC)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AR_T_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[ARTIKEL] DROP CONSTRAINT [AR_T_FK]
 GO
 
IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AUFTRAEGE'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'AUFTRAEGE'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[AUFTRAEGE]
END 
GO

CREATE TABLE 
[dbo].[AUFTRAEGE]
(
   [AUFTRAGSNR] numeric(38, 0)  NOT NULL,
   [AUFTRAGS_TYP] varchar(50)  NOT NULL,
   [KUN_NR] numeric(38, 0)  NOT NULL,
   [ANG_NR] numeric(38, 0)  NULL,
   [BEREITS_GEZAHLT] float(53)  NULL,
   [BESTELLDATUM] datetime2(0)  NULL,
   [LIEFERDATUM] datetime2(0)  NULL,
   [RECHNUNGSDATUM] datetime2(0)  NULL,
   [ZEITSTEMPEL] datetime2(0)  NOT NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AU_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[AUFTRAEGE] DROP CONSTRAINT [AU_PK]
 GO

ALTER TABLE [dbo].[AUFTRAEGE]
 ADD CONSTRAINT [AU_PK]
 PRIMARY KEY 
   CLUSTERED ([AUFTRAGSNR] ASC)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AUFTRAGS_TYP'  AND sc.name=N'dbo'  AND type in (N'C'))
ALTER TABLE [dbo].[AUFTRAEGE] DROP CONSTRAINT [AUFTRAGS_TYP]
 GO

ALTER TABLE [dbo].[AUFTRAEGE]
 ADD CONSTRAINT [AUFTRAGS_TYP]
 CHECK (AUFTRAGS_TYP IN ( 'Angebot', 'Anfrage', 'Auftrag' ))

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'DATUM'  AND sc.name=N'dbo'  AND type in (N'C'))
ALTER TABLE [dbo].[AUFTRAEGE] DROP CONSTRAINT [DATUM]
 GO

ALTER TABLE [dbo].[AUFTRAEGE]
 ADD CONSTRAINT [DATUM]
 CHECK (BESTELLDATUM <= LIEFERDATUM)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AU_ANG_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[AUFTRAEGE] DROP CONSTRAINT [AU_ANG_FK]
 GO

ALTER TABLE [dbo].[AUFTRAEGE]
 ADD CONSTRAINT [AU_ANG_FK]
 FOREIGN KEY 
   ([ANG_NR])
 REFERENCES 
  [dbo].[ANGESTELLTE]     ([ANG_NR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AU_KUN_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[AUFTRAEGE] DROP CONSTRAINT [AU_KUN_FK]
 GO

IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AUFTRAGSPOSITIONEN'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'AUFTRAGSPOSITIONEN'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[AUFTRAGSPOSITIONEN]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[AUFTRAGSPOSITIONEN]
(
   [TNR] numeric(38, 0)  NOT NULL,
   [AUFTRAGSNR] numeric(38, 0)  NOT NULL,
   [MENGE] float(53)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AR_AU_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[AUFTRAGSPOSITIONEN] DROP CONSTRAINT [AR_AU_PK]
 GO



ALTER TABLE [dbo].[AUFTRAGSPOSITIONEN]
 ADD CONSTRAINT [AR_AU_PK]
 PRIMARY KEY 
   CLUSTERED ([TNR] ASC, [AUFTRAGSNR] ASC)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AR_AU_AR_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[AUFTRAGSPOSITIONEN] DROP CONSTRAINT [AR_AU_AR_FK]
 GO



ALTER TABLE [dbo].[AUFTRAGSPOSITIONEN]
 ADD CONSTRAINT [AR_AU_AR_FK]
 FOREIGN KEY 
   ([TNR])
 REFERENCES 
  [dbo].[ARTIKEL]     ([TNR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'AR_AU_AU_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[AUFTRAGSPOSITIONEN] DROP CONSTRAINT [AR_AU_AU_FK]
 GO



ALTER TABLE [dbo].[AUFTRAGSPOSITIONEN]
 ADD CONSTRAINT [AR_AU_AU_FK]
 FOREIGN KEY 
   ([AUFTRAGSNR])
 REFERENCES 
  [dbo].[AUFTRAEGE]     ([AUFTRAGSNR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO



IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'GEH_KLASSEN'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'GEH_KLASSEN'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[GEH_KLASSEN]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[GEH_KLASSEN]
(
   [GEH_KLASSE] numeric(38, 0)  NOT NULL,
   [MAX_GEHALT] float(53)  NOT NULL,
   [MIN_GEHALT] float(53)  NOT NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'G_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[GEH_KLASSEN] DROP CONSTRAINT [G_PK]
 GO



ALTER TABLE [dbo].[GEH_KLASSEN]
 ADD CONSTRAINT [G_PK]
 PRIMARY KEY 
   CLUSTERED ([GEH_KLASSE] ASC)

GO



IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'KUNDEN'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'KUNDEN'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[KUNDEN]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[KUNDEN]
(
   [KUN_NR] numeric(38, 0)  NOT NULL,
   [NACHNAME] varchar(50)  NOT NULL,
   [VORNAME] varchar(50)  NOT NULL,
   [GESCHLECHT] varchar(1)  NULL,
   [ORT] varchar(50)  NULL,
   [STRASSE] varchar(50)  NULL,
   [TELEFONNR] varchar(50)  NULL,
   [ZEITSTEMPEL] datetime2(0)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'KUN_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[KUNDEN] DROP CONSTRAINT [KUN_PK]
 GO



ALTER TABLE [dbo].[KUNDEN]
 ADD CONSTRAINT [KUN_PK]
 PRIMARY KEY 
   CLUSTERED ([KUN_NR] ASC)

GO



IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LAGER'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'LAGER'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[LAGER]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[LAGER]
(
   [LANR] numeric(38, 0)  NOT NULL,
   [BEZEICHNUNG] varchar(50)  NOT NULL,
   [ORT] varchar(50)  NULL,
   [STRASSE] varchar(50)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LAG_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[LAGER] DROP CONSTRAINT [LAG_PK]
 GO



ALTER TABLE [dbo].[LAGER]
 ADD CONSTRAINT [LAG_PK]
 PRIMARY KEY 
   CLUSTERED ([LANR] ASC)

GO



IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LAGERBESTAND'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'LAGERBESTAND'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[LAGERBESTAND]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[LAGERBESTAND]
(
   [LANR] numeric(38, 0)  NOT NULL,
   [TNR] numeric(38, 0)  NOT NULL,
   [BESTAND] float(53)  NULL,
   [ZEITSTEMPEL] datetime2(0)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LAG_T_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[LAGERBESTAND] DROP CONSTRAINT [LAG_T_PK]
 GO



ALTER TABLE [dbo].[LAGERBESTAND]
 ADD CONSTRAINT [LAG_T_PK]
 PRIMARY KEY 
   CLUSTERED ([LANR] ASC, [TNR] ASC)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LAG_T_LAG_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[LAGERBESTAND] DROP CONSTRAINT [LAG_T_LAG_FK]
 GO



ALTER TABLE [dbo].[LAGERBESTAND]
 ADD CONSTRAINT [LAG_T_LAG_FK]
 FOREIGN KEY 
   ([LANR])
 REFERENCES 
  [dbo].[LAGER]     ([LANR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LAG_T_T_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[LAGERBESTAND] DROP CONSTRAINT [LAG_T_T_FK]
 GO






IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LIEFERANTEN'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'LIEFERANTEN'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[LIEFERANTEN]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[LIEFERANTEN]
(
   [LIEF_NR] numeric(38, 0)  NOT NULL,
   [NAME] varchar(50)  NOT NULL,
   [ORT] varchar(50)  NOT NULL,
   [STRASSE] varchar(50)  NOT NULL,
   [TELEFONNR] varchar(50)  NULL,
   [ZEITSTEMPEL] datetime2(0)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LIEF_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[LIEFERANTEN] DROP CONSTRAINT [LIEF_PK]
 GO



ALTER TABLE [dbo].[LIEFERANTEN]
 ADD CONSTRAINT [LIEF_PK]
 PRIMARY KEY 
   CLUSTERED ([LIEF_NR] ASC)

GO



IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LIEFERPROGRAMME'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'LIEFERPROGRAMME'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[LIEFERPROGRAMME]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[LIEFERPROGRAMME]
(
   [LIEF_NR] numeric(38, 0)  NOT NULL,
   [TNR] numeric(38, 0)  NOT NULL,
   [BESTELLNR] varchar(20)  NULL,
   [EINKAUFSPREIS] float(53)  NULL,
   [GESAMTMENGE] float(53)  NULL,
   [ZEITSTEMPEL] datetime2(0)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LP_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[LIEFERPROGRAMME] DROP CONSTRAINT [LP_PK]
 GO



ALTER TABLE [dbo].[LIEFERPROGRAMME]
 ADD CONSTRAINT [LP_PK]
 PRIMARY KEY 
   CLUSTERED ([LIEF_NR] ASC, [TNR] ASC)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LP_LIEF_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[LIEFERPROGRAMME] DROP CONSTRAINT [LP_LIEF_FK]
 GO



ALTER TABLE [dbo].[LIEFERPROGRAMME]
 ADD CONSTRAINT [LP_LIEF_FK]
 FOREIGN KEY 
   ([LIEF_NR])
 REFERENCES 
  [dbo].[LIEFERANTEN]     ([LIEF_NR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LP_T_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[LIEFERPROGRAMME] DROP CONSTRAINT [LP_T_FK]
 GO






IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LIEFERUNGEN'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'LIEFERUNGEN'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[LIEFERUNGEN]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[LIEFERUNGEN]
(
   [LIEFER_NR] numeric(38, 0)  NOT NULL,
   [LIEF_NR] numeric(38, 0)  NOT NULL,
   [TNR] numeric(38, 0)  NOT NULL,
   [LIEF_DATUM] datetime2(0)  NOT NULL,
   [MENGE] float(53) DEFAULT 1  NOT NULL,
   [ZEITSTEMPEL] datetime2(0)  NOT NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LG_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[LIEFERUNGEN] DROP CONSTRAINT [LG_PK]
 GO



ALTER TABLE [dbo].[LIEFERUNGEN]
 ADD CONSTRAINT [LG_PK]
 PRIMARY KEY 
   CLUSTERED ([LIEFER_NR] ASC, [LIEF_NR] ASC, [TNR] ASC)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LG_LIEF_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[LIEFERUNGEN] DROP CONSTRAINT [LG_LIEF_FK]
 GO



ALTER TABLE [dbo].[LIEFERUNGEN]
 ADD CONSTRAINT [LG_LIEF_FK]
 FOREIGN KEY 
   ([LIEF_NR])
 REFERENCES 
  [dbo].[LIEFERANTEN]     ([LIEF_NR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'LG_T_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[LIEFERUNGEN] DROP CONSTRAINT [LG_T_FK]
 GO





IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'ORTE'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'ORTE'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[ORTE]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[ORTE]
(
   [ORT] varchar(50)  NOT NULL,
   [STRASSE] varchar(50)  NOT NULL,
   [PLZ] numeric(38, 0)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'O_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[ORTE] DROP CONSTRAINT [O_PK]
 GO



ALTER TABLE [dbo].[ORTE]
 ADD CONSTRAINT [O_PK]
 PRIMARY KEY 
   CLUSTERED ([ORT] ASC, [STRASSE] ASC)

GO




IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'STRUKTUR'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'STRUKTUR'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[STRUKTUR]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[STRUKTUR]
(
   [OTEIL] numeric(38, 0)  NOT NULL,
   [UTEIL] numeric(38, 0)  NOT NULL,
   [POSITION] numeric(38, 0)  NOT NULL,
   [MENGE] float(53)  NOT NULL,
   [AUSSCHUSS] float(53)  NULL,
   [ARBEITSGANG] numeric(38, 0)  NULL,
   [ZEITSTEMPEL] datetime2(0)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'S_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[STRUKTUR] DROP CONSTRAINT [S_PK]
 GO



ALTER TABLE [dbo].[STRUKTUR]
 ADD CONSTRAINT [S_PK]
 PRIMARY KEY 
   CLUSTERED ([OTEIL] ASC, [UTEIL] ASC, [POSITION] ASC)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'S_T_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[STRUKTUR] DROP CONSTRAINT [S_T_FK]
 GO




IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'S_T_FK2'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[STRUKTUR] DROP CONSTRAINT [S_T_FK2]
 GO



GO




IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'TEILE'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'TEILE'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[TEILE]
END 
GO


GO

GO
CREATE TABLE 
[dbo].[TEILE]
(
   [TNR] numeric(38, 0)  NOT NULL,
   [ME] varchar(10)  NULL,
   [BEZEICHNUNG] varchar(50)  NULL,
   [TYP] varchar(50)  NULL,
   [HERSTELLKOSTEN] float(53)  NULL,
   [EINKAUFSPREIS] float(53)  NULL,
   [MINDESTBESTAND] float(53)  NULL,
   [BESTAND] float(53)  NULL,
   [LIEFERZEIT] float(53)  NULL,
   [HERSTELLDAUER] float(53)  NULL,
   [GEWICHT] float(53)  NULL,
   [RESERVIERT] float(53)  NULL,
   [VERFUEGBAR] float(53)  NULL,
   [ZEITSTEMPEL] datetime2(0)  NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'TE_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[TEILE] DROP CONSTRAINT [TE_PK]
 GO



ALTER TABLE [dbo].[TEILE]
 ADD CONSTRAINT [TE_PK]
 PRIMARY KEY 
   CLUSTERED ([TNR] ASC)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'TYP'  AND sc.name=N'dbo'  AND type in (N'C'))
ALTER TABLE [dbo].[TEILE] DROP CONSTRAINT [TYP]
 GO



ALTER TABLE [dbo].[TEILE]
 ADD CONSTRAINT [TYP]
 CHECK (TYP IN ( 'Artikel', 'Baugruppe', 'Material' ))

GO


IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'TEILE_WERKE'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'TEILE_WERKE'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[TEILE_WERKE]
END 
GO

CREATE TABLE 
[dbo].[TEILE_WERKE]
(
   [TNR] numeric(38, 0)  NOT NULL,
   [WNR] numeric(38, 0)  NOT NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'T_WE_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[TEILE_WERKE] DROP CONSTRAINT [T_WE_PK]
 GO



ALTER TABLE [dbo].[TEILE_WERKE]
 ADD CONSTRAINT [T_WE_PK]
 PRIMARY KEY 
   CLUSTERED ([TNR] ASC, [WNR] ASC)

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'T_WE_T_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[TEILE_WERKE] DROP CONSTRAINT [T_WE_T_FK]
 GO



ALTER TABLE [dbo].[TEILE_WERKE]
 ADD CONSTRAINT [T_WE_T_FK]
 FOREIGN KEY 
   ([TNR])
 REFERENCES 
  [dbo].[TEILE]     ([TNR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'T_WE_WE_FK'  AND sc.name=N'dbo'  AND type in (N'F'))
ALTER TABLE [dbo].[TEILE_WERKE] DROP CONSTRAINT [T_WE_WE_FK]
 GO




IF  EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'WERKE'  AND sc.name=N'dbo'  AND type in (N'U'))
BEGIN

  DECLARE @drop_statement nvarchar(500)

  DECLARE drop_cursor CURSOR FOR
      SELECT 'alter table '+quotename(schema_name(ob.schema_id))+
      '.'+quotename(object_name(ob.object_id))+ ' drop constraint ' + quotename(fk.name) 
      FROM sys.objects ob INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = ob.object_id
      WHERE fk.referenced_object_id = 
          (
             SELECT so.object_id 
             FROM sys.objects so JOIN sys.schemas sc
             ON so.schema_id = sc.schema_id
             WHERE so.name = N'WERKE'  AND sc.name=N'dbo'  AND type in (N'U')
           )

  OPEN drop_cursor

  FETCH NEXT FROM drop_cursor
  INTO @drop_statement

  WHILE @@FETCH_STATUS = 0
  BEGIN
     EXEC (@drop_statement)

     FETCH NEXT FROM drop_cursor
     INTO @drop_statement
  END

  CLOSE drop_cursor
  DEALLOCATE drop_cursor

  DROP TABLE [dbo].[WERKE]
END 
GO


CREATE TABLE 
[dbo].[WERKE]
(
   [WNR] numeric(38, 0)  NOT NULL,
   [BEZEICHNUNG] varchar(50)  NULL,
   [ORT] varchar(50)  NOT NULL,
   [STRASSE] varchar(50)  NOT NULL
)
GO
IF EXISTS (SELECT * FROM sys.objects so JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name = N'WE_PK'  AND sc.name=N'dbo'  AND type in (N'PK'))
ALTER TABLE [dbo].[WERKE] DROP CONSTRAINT [WE_PK]
 GO


ALTER TABLE [dbo].[WERKE]
 ADD CONSTRAINT [WE_PK]
 PRIMARY KEY 
   CLUSTERED ([WNR] ASC)

GO



IF EXISTS (SELECT * FROM sys.sequences seq JOIN sys.schemas sch ON seq.schema_id=sch.schema_id WHERE seq.name=N'LIEFER_NR'  AND sch.name=N'dbo' )
 DROP SEQUENCE [dbo].[LIEFER_NR]
GO

CREATE SEQUENCE [dbo].[LIEFER_NR]
    AS numeric(28)
    START WITH 61
    INCREMENT BY 1
    MINVALUE 1
    MAXVALUE 9999999999999999999999999999
    NO CYCLE
    CACHE 20

ALTER TABLE [dbo].[ARTIKEL]
 ADD CONSTRAINT [AR_T_FK]
 FOREIGN KEY 
   ([TNR])
 REFERENCES 
  [dbo].[TEILE]     ([TNR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO


ALTER TABLE [dbo].[AUFTRAEGE]
 ADD CONSTRAINT [AU_KUN_FK]
 FOREIGN KEY 
   ([KUN_NR])
 REFERENCES 
  [dbo].[KUNDEN]     ([KUN_NR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

ALTER TABLE [dbo].[LAGERBESTAND]
 ADD CONSTRAINT [LAG_T_T_FK]
 FOREIGN KEY 
   ([TNR])
 REFERENCES 
  [dbo].[TEILE]     ([TNR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

ALTER TABLE [dbo].[LIEFERPROGRAMME]
 ADD CONSTRAINT [LP_T_FK]
 FOREIGN KEY 
   ([TNR])
 REFERENCES 
  [dbo].[TEILE]     ([TNR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO
ALTER TABLE [dbo].[STRUKTUR]
 ADD CONSTRAINT [S_T_FK2]
 FOREIGN KEY 
   ([UTEIL])
 REFERENCES 
  [dbo].[TEILE]     ([TNR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

ALTER TABLE [dbo].[LIEFERUNGEN]
 ADD CONSTRAINT [LG_T_FK]
 FOREIGN KEY 
   ([TNR])
 REFERENCES 
  [dbo].[TEILE]     ([TNR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

ALTER TABLE [dbo].[STRUKTUR]
 ADD CONSTRAINT [S_T_FK]
 FOREIGN KEY 
   ([OTEIL])
 REFERENCES 
  [dbo].[TEILE]     ([TNR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

ALTER TABLE [dbo].[TEILE_WERKE]
 ADD CONSTRAINT [T_WE_WE_FK]
 FOREIGN KEY 
   ([WNR])
 REFERENCES 
  [dbo].[WERKE]     ([WNR])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

