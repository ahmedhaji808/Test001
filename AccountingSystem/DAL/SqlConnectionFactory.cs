using System.Configuration;
using System.Data.SqlClient;

namespace AccountingSystem.DAL
{
	public static class SqlConnectionFactory
	{
		public static SqlConnection Create()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["AccountingDb"].ConnectionString;
			return new SqlConnection(connectionString);
		}
	}
}