CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Accounts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Accounts" PRIMARY KEY AUTOINCREMENT,
    "Code" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Type" INTEGER NOT NULL,
    "NormalBalance" INTEGER NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "ParentAccountId" INTEGER NULL,
    CONSTRAINT "FK_Accounts_Accounts_ParentAccountId" FOREIGN KEY ("ParentAccountId") REFERENCES "Accounts" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "JournalEntries" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_JournalEntries" PRIMARY KEY AUTOINCREMENT,
    "Date" TEXT NOT NULL,
    "Reference" TEXT NULL,
    "Description" TEXT NULL
);

CREATE TABLE "JournalEntryLines" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_JournalEntryLines" PRIMARY KEY AUTOINCREMENT,
    "JournalEntryId" INTEGER NOT NULL,
    "AccountId" INTEGER NOT NULL,
    "Debit" TEXT NOT NULL,
    "Credit" TEXT NOT NULL,
    "Memo" TEXT NULL,
    CONSTRAINT "FK_JournalEntryLines_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_JournalEntryLines_JournalEntries_JournalEntryId" FOREIGN KEY ("JournalEntryId") REFERENCES "JournalEntries" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Accounts_Code" ON "Accounts" ("Code");

CREATE INDEX "IX_Accounts_ParentAccountId" ON "Accounts" ("ParentAccountId");

CREATE INDEX "IX_JournalEntryLines_AccountId" ON "JournalEntryLines" ("AccountId");

CREATE INDEX "IX_JournalEntryLines_JournalEntryId" ON "JournalEntryLines" ("JournalEntryId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250814183024_InitialCreate', '8.0.8');

COMMIT;

