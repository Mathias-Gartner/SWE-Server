-- ERP Datenbank

-- Create DB
USE master;
GO

IF DB_ID('erp') IS NOT NULL
BEGIN
	-- terminate existing connections
	ALTER DATABASE erp SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE erp
END

CREATE DATABASE erp
    ON PRIMARY
    (
        NAME       = erp_dat,
        FILENAME   = 'C:\Databases\erp_dat.mdf',
        SIZE       = 100MB,
        MAXSIZE    = 10000MB,
        FILEGROWTH = 50%
    )
    LOG ON
    (
        NAME       = freibier_log,
        FILENAME   = 'C:\Databases\erp_log.ldf',
        SIZE       = 10MB,
        MAXSIZE    = 1000MB,
        FILEGROWTH = 50%
    )
GO
-- End Create DB

USE erp;
GO

-- users
CREATE TABLE [users]
(
	[id]			[int] IDENTITY(1,1)	NOT NULL,
	[username]		[nvarchar](max)		NOT NULL,
	[password]		[nvarchar](128)		NOT NULL,
	[passwordSalt]	[nvarchar](128)		NOT NULL,
	CONSTRAINT [CLIX_PK_users] PRIMARY KEY CLUSTERED ([id])
)


-- address
CREATE TABLE [address]
(
	[id]			[int] IDENTITY(1,1)	NOT NULL,
	[name]			[nvarchar](max)		NULL,
	[street]		[nvarchar](max)		NULL,
	[number]		[nvarchar](max)		NULL,
	[postOfficeBox]	[nvarchar](max)		NULL,
	[postalCode]	[nvarchar](5)		NOT NULL,
	[city]			[nvarchar](max)		NOT NULL,
	[country]		[nvarchar](max)		NULL,
	CONSTRAINT [CLIX_PK_address] PRIMARY KEY CLUSTERED ([id])
)

-- contacts
CREATE TABLE [contacts]
(
	[id]				[int] IDENTITY(1,1)	NOT NULL,
	[addressId]			[int]				NULL REFERENCES [address],
	[invoiceAddressId]	[int]				NULL REFERENCES [address],
	[deliveryAddressId]	[int]				NULL REFERENCES [address],
	[belongsToId]		[int]				NULL REFERENCES [contacts],
	[name]				[nvarchar](max)		NULL,
	[uid]				[nvarchar](14)		NULL,
	[firstname]			[nvarchar](max)		NULL,
	[lastname]			[nvarchar](max)		NULL,
	[prefix]			[nvarchar](max)		NULL,
	[suffix]			[nvarchar](max)		NULL,
	[dateOfBirth]		[date]				NULL,
	[email]				[nvarchar](max)		NULL,
	CONSTRAINT [CLIX_PK_contacts] PRIMARY KEY CLUSTERED ([id])
)

-- employees
CREATE TABLE [employees]
(
	[id]		[int] IDENTITY(1,1) NOT NULL,
	[firstname]	[nvarchar](max)		NOT NULL,
	[lastname]	[nvarchar](max)		NOT NULL,
	[prefix]	[nvarchar](max)		NULL,
	[postfix]	[nvarchar](max)		NULL,
	[birthday]	[date]				NOT NULL,
	[gender]	[bit]				NOT NULL,
	CONSTRAINT [CLIX_PK_employees] PRIMARY KEY CLUSTERED ([id])
)

-- projects
CREATE TABLE [projects]
(
	[id]		[int] IDENTITY(1,1)	NOT NULL,
	[name]		[nvarchar](max)		NOT NULL,
	CONSTRAINT [CLIX_PK_projects] PRIMARY KEY CLUSTERED ([id])
)

-- timetrackingentry
CREATE TABLE [timetrackingentry]
(
	[id]			[int] IDENTITY(1,1)	NOT NULL,
	[projectId]		[int]				NOT NULL	REFERENCES [projects]([id]),
	[employeeId]	[int]				NOT NULL	REFERENCES [employees]([id]),
	[description]	[nvarchar](max)		NULL,
	[hours]			[decimal](2,2)		NOT NULL,
	CONSTRAINT [CLIX_PK_timetrackingentry] PRIMARY KEY CLUSTERED ([id])
)

-- offerts
CREATE TABLE [offerts]
(
	[id]			[int] IDENTITY(1,1)	NOT NULL,
	[customerId]	[int]				NOT NULL REFERENCES [contacts],
	CONSTRAINT [CLIX_PK_offerts] PRIMARY KEY CLUSTERED ([id])
)

CREATE TABLE [invoices]
(
	[id]			[int] IDENTITY(1,1)	NOT NULL,
	[contactId]		[int]				NOT NULL REFERENCES [contacts],
	[outgoingInvoice]	[bit]			NOT NULL,
	[invoiceNumber]	[int]				NOT NULL,
	[invoiceDate]	[date]				NOT NULL DEFAULT getdate(),
	[dueDate]		[date]				NULL,
	[message]		[nvarchar](max)		NULL,
	[comment]		[nvarchar](max)		NULL,
	CONSTRAINT [CLIX_PK_invoices] PRIMARY KEY CLUSTERED ([id])
)

CREATE TABLE [invoiceEntries]
(
	[id]			[int] IDENTITY(1,1)	NOT NULL,
	[invoiceId]		[int]				NOT NULL REFERENCES [invoices],
	[description]	[varchar](max)		NOT NULL,
	[amount]		[int]				NOT NULL DEFAULT 1,
	[price]			[money]				NOT NULL,
	[ustPercent]	[int]				NOT NULL DEFAULT 20,
	CONSTRAINT [CLIX_PK_invoiceentry] PRIMARY KEY CLUSTERED ([id])
)

CREATE TABLE [bankaccount]
(
	[id]			[int] IDENTITY(1,1)	NOT NULL,
	[acountNumber]	[varchar](20)		NULL, -- spanish account numbers are 20 digits
	[bankcode]		[varchar](5)		NULL,
	[iban]			[varchar](34)		NULL,
	[bic]			[varchar](8)		NULL,
	[description]	[varchar](max)		NULL,
	CONSTRAINT [CLIX_PK_bankaccount] PRIMARY KEY CLUSTERED ([id])
)

CREATE TABLE [transaction]
(
	[id]				[int] IDENTITY(1,1)	NOT NULL,
	[sourceAccountId]	[int]				NOT NULL REFERENCES [bankaccount],
	[targetAccountId]	[int]				NOT NULL REFERENCES [bankaccount],
	[amount]			[money]				NOT NULL,
	CONSTRAINT [CLIX_PK_transaction] PRIMARY KEY CLUSTERED ([id])
)
GO