using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AccountingSystem.Models;

namespace AccountingSystem.DAL
{
	public class JournalRepository
	{
		public int CreateEntry(JournalEntry entry)
		{
			using (SqlConnection connection = SqlConnectionFactory.Create())
			using (SqlCommand command = new SqlCommand("sp_CreateJournalEntry", connection))
			{
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@EntryDate", entry.EntryDate);
				command.Parameters.AddWithValue("@Reference", (object)entry.Reference ?? DBNull.Value);
				command.Parameters.AddWithValue("@Description", (object)entry.Description ?? DBNull.Value);
				SqlParameter idParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
				command.Parameters.Add(idParam);

				DataTable linesTable = new DataTable();
				linesTable.Columns.Add("AccountId", typeof(int));
				linesTable.Columns.Add("Debit", typeof(decimal));
				linesTable.Columns.Add("Credit", typeof(decimal));
				foreach (JournalLine line in entry.Lines)
				{
					linesTable.Rows.Add(line.AccountId, line.Debit, line.Credit);
				}
				SqlParameter tvp = new SqlParameter("@Lines", SqlDbType.Structured)
				{
					TypeName = "dbo.JournalLineType",
					Value = linesTable
				};
				command.Parameters.Add(tvp);

				connection.Open();
				command.ExecuteNonQuery();
				return (int)idParam.Value;
			}
		}

		public IEnumerable<JournalEntry> GetEntries(DateTime from, DateTime to)
		{
			var results = new List<JournalEntry>();
			using (SqlConnection connection = SqlConnectionFactory.Create())
			using (SqlCommand command = new SqlCommand(@"SELECT je.JournalEntryId, je.EntryDate, je.Reference, je.Description,
				jl.JournalLineId, jl.AccountId, jl.Debit, jl.Credit
				FROM JournalEntries je
				LEFT JOIN JournalLines jl ON je.JournalEntryId = jl.JournalEntryId
				WHERE je.EntryDate BETWEEN @From AND @To
				ORDER BY je.EntryDate, je.JournalEntryId, jl.JournalLineId", connection))
			{
				command.Parameters.AddWithValue("@From", from.Date);
				command.Parameters.AddWithValue("@To", to.Date);
				connection.Open();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					JournalEntry current = null;
					int? currentId = null;
					while (reader.Read())
					{
						int journalEntryId = reader.GetInt32(0);
						if (!currentId.HasValue || currentId.Value != journalEntryId)
						{
							current = new JournalEntry
							{
								JournalEntryId = journalEntryId,
								EntryDate = reader.GetDateTime(1),
								Reference = reader.IsDBNull(2) ? null : reader.GetString(2),
								Description = reader.IsDBNull(3) ? null : reader.GetString(3)
							};
							results.Add(current);
							currentId = journalEntryId;
						}

						if (!reader.IsDBNull(4))
						{
							current.Lines.Add(new JournalLine
							{
								JournalLineId = reader.GetInt32(4),
								JournalEntryId = journalEntryId,
								AccountId = reader.GetInt32(5),
								Debit = reader.GetDecimal(6),
								Credit = reader.GetDecimal(7)
							});
						}
					}
				}
			}
			return results;
		}

		public IEnumerable<(int AccountId, decimal Debit, decimal Credit)> GetTrialBalance(DateTime to)
		{
			var results = new List<(int, decimal, decimal)>();
			using (SqlConnection connection = SqlConnectionFactory.Create())
			using (SqlCommand command = new SqlCommand(@"SELECT AccountId,
				SUM(Debit) AS TotalDebit,
				SUM(Credit) AS TotalCredit
				FROM JournalLines jl
				INNER JOIN JournalEntries je ON jl.JournalEntryId = je.JournalEntryId
				WHERE je.EntryDate <= @To
				GROUP BY AccountId
				ORDER BY AccountId", connection))
			{
				command.Parameters.AddWithValue("@To", to.Date);
				connection.Open();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						results.Add((reader.GetInt32(0), reader.GetDecimal(1), reader.GetDecimal(2)));
					}
				}
			}
			return results;
		}
	}
}