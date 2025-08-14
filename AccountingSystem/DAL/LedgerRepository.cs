using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AccountingSystem.DAL
{
	public class LedgerRepository
	{
		public IEnumerable<(DateTime EntryDate, string Reference, string Description, decimal Debit, decimal Credit)> GetAccountLedger(int accountId, DateTime from, DateTime to)
		{
			var results = new List<(DateTime, string, string, decimal, decimal)>();
			using (SqlConnection connection = SqlConnectionFactory.Create())
			using (SqlCommand command = new SqlCommand(@"SELECT je.EntryDate, je.Reference, je.Description, jl.Debit, jl.Credit
				FROM JournalLines jl
				INNER JOIN JournalEntries je ON jl.JournalEntryId = je.JournalEntryId
				WHERE jl.AccountId = @AccountId AND je.EntryDate BETWEEN @From AND @To
				ORDER BY je.EntryDate, je.JournalEntryId, jl.JournalLineId", connection))
			{
				command.Parameters.AddWithValue("@AccountId", accountId);
				command.Parameters.AddWithValue("@From", from.Date);
				command.Parameters.AddWithValue("@To", to.Date);
				connection.Open();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						results.Add((reader.GetDateTime(0),
							reader.IsDBNull(1) ? null : reader.GetString(1),
							reader.IsDBNull(2) ? null : reader.GetString(2),
							reader.GetDecimal(3),
							reader.GetDecimal(4)));
					}
				}
			}
			return results;
		}
	}
}