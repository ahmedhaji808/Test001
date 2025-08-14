# Accounting System (ASP.NET Web Forms)

A minimal accounting system scaffold built with ASP.NET Web Forms (.NET Framework). Includes core modules:

- Chart of Accounts
- Journal Entry
- General Ledger
- Trial Balance

## Requirements

- Windows with Visual Studio 2019 or later (Community is fine)
- .NET Framework 4.8 Developer Pack
- SQL Server (LocalDB or full SQL Server)

## Getting Started

1. Clone or download this folder to your Windows machine.
2. Open `AccountingSystem.csproj` in Visual Studio. A solution will be created automatically.
3. Create the database:
   - Open `App_Data/Sql/01_CreateDatabase.sql` in SQL Server Management Studio (or Visual Studio's SQL Server Object Explorer) and run it.
4. Update the connection string if needed in `Web.config` under `connectionStrings` (`AccountingDb`).
5. Build and run (F5). The app starts at `Default.aspx`.

## Connection String

The project defaults to SQL Server LocalDB. If you use a named SQL Server instance, update the `Data Source` and `Initial Catalog` in `Web.config` accordingly.

```xml
<connectionStrings>
	<add name="AccountingDb" connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AccountingDb;Integrated Security=True;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" />
</connectionStrings>
```

## Project Structure

- `Site.Master`: layout and navigation
- `Default.aspx`: dashboard/landing
- `Accounts.aspx`: manage the chart of accounts
- `Journal.aspx`: enter journal entries (debits/credits)
- `Ledger.aspx`: view ledger by account and date range
- `TrialBalance.aspx`: see trial balance
- `Models/`: domain models
- `DAL/`: ADO.NET data access layer
- `App_Data/Sql/`: SQL scripts to create the database and tables

## Notes

- This is a learning scaffolding, not production-ready. Add authentication/authorization, validation, and error handling before real use.
- Web Forms is Windows-only. Run and debug on Windows with Visual Studio.