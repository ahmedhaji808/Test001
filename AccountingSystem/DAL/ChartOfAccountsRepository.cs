using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AccountingSystem.Models;

namespace AccountingSystem.DAL
{
	public class ChartOfAccountsRepository
	{
		public IEnumerable<Account> GetAll()
		{
			var results = new List<Account>();
			using (SqlConnection connection = SqlConnectionFactory.Create())
			using (SqlCommand command = new SqlCommand("SELECT AccountId, Code, Name, Type, IsActive, CreatedAtUtc FROM Accounts ORDER BY Code", connection))
			{
				connection.Open();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						results.Add(new Account
						{
							AccountId = reader.GetInt32(0),
							Code = reader.GetString(1),
							Name = reader.GetString(2),
							Type = (AccountType)reader.GetInt32(3),
							IsActive = reader.GetBoolean(4),
							CreatedAtUtc = reader.GetDateTime(5)
						});
					}
				}
			}
			return results;
		}

		public void Create(Account account)
		{
			using (SqlConnection connection = SqlConnectionFactory.Create())
			using (SqlCommand command = new SqlCommand(@"INSERT INTO Accounts(Code, Name, Type, IsActive, CreatedAtUtc)
VALUES(@Code, @Name, @Type, @IsActive, SYSUTCDATETIME());", connection))
			{
				command.Parameters.Add("@Code", SqlDbType.NVarChar, 32).Value = account.Code;
				command.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = account.Name;
				command.Parameters.Add("@Type", SqlDbType.Int).Value = (int)account.Type;
				command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = account.IsActive;
				connection.Open();
				command.ExecuteNonQuery();
			}
		}
	}
}