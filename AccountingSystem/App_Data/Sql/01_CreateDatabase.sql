-- Create database (run as admin if needed)
IF DB_ID('AccountingDb') IS NULL
BEGIN
	CREATE DATABASE AccountingDb;
END
GO

USE AccountingDb;
GO

-- Accounts table
IF OBJECT_ID('dbo.Accounts', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.Accounts
	(
		AccountId INT IDENTITY(1,1) PRIMARY KEY,
		Code NVARCHAR(32) NOT NULL UNIQUE,
		Name NVARCHAR(200) NOT NULL,
		Type INT NOT NULL,
		IsActive BIT NOT NULL CONSTRAINT DF_Accounts_IsActive DEFAULT(1),
		CreatedAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_Accounts_CreatedAt DEFAULT(SYSUTCDATETIME())
	);
END
GO

-- Journal entries
IF OBJECT_ID('dbo.JournalEntries', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.JournalEntries
	(
		JournalEntryId INT IDENTITY(1,1) PRIMARY KEY,
		EntryDate DATE NOT NULL,
		Reference NVARCHAR(64) NULL,
		Description NVARCHAR(400) NULL,
		CreatedAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_JournalEntries_CreatedAt DEFAULT(SYSUTCDATETIME())
	);
END
GO

-- Journal lines
IF OBJECT_ID('dbo.JournalLines', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.JournalLines
	(
		JournalLineId INT IDENTITY(1,1) PRIMARY KEY,
		JournalEntryId INT NOT NULL FOREIGN KEY REFERENCES dbo.JournalEntries(JournalEntryId),
		AccountId INT NOT NULL FOREIGN KEY REFERENCES dbo.Accounts(AccountId),
		Debit DECIMAL(18,2) NOT NULL CONSTRAINT DF_JournalLines_Debit DEFAULT(0),
		Credit DECIMAL(18,2) NOT NULL CONSTRAINT DF_JournalLines_Credit DEFAULT(0)
	);
END
GO

-- TVP for journal lines
IF TYPE_ID('dbo.JournalLineType') IS NULL
BEGIN
	CREATE TYPE dbo.JournalLineType AS TABLE
	(
		AccountId INT NOT NULL,
		Debit DECIMAL(18,2) NOT NULL,
		Credit DECIMAL(18,2) NOT NULL
	);
END
GO

-- Stored procedure to create a journal entry with lines
IF OBJECT_ID('dbo.sp_CreateJournalEntry', 'P') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.sp_CreateJournalEntry;
END
GO

CREATE PROCEDURE dbo.sp_CreateJournalEntry
	@EntryDate DATE,
	@Reference NVARCHAR(64) = NULL,
	@Description NVARCHAR(400) = NULL,
	@Lines dbo.JournalLineType READONLY,
	@NewId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	-- Validate balance
	DECLARE @TotalDebit DECIMAL(18,2) = (SELECT SUM(Debit) FROM @Lines);
	DECLARE @TotalCredit DECIMAL(18,2) = (SELECT SUM(Credit) FROM @Lines);
	IF (@TotalDebit IS NULL OR @TotalCredit IS NULL OR @TotalDebit <> @TotalCredit)
	BEGIN
		RAISERROR('Journal entry not balanced.', 16, 1);
		RETURN;
	END

	BEGIN TRANSACTION;
	BEGIN TRY
		INSERT INTO dbo.JournalEntries(EntryDate, Reference, Description)
		VALUES(@EntryDate, @Reference, @Description);
		SET @NewId = SCOPE_IDENTITY();

		INSERT INTO dbo.JournalLines(JournalEntryId, AccountId, Debit, Credit)
		SELECT @NewId, AccountId, Debit, Credit
		FROM @Lines;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
GO