using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer
{
	public class DataAccess
	{
		// Return default connection string for Windows authentication
		public static string GetConnectionString(
			string serverName,
			string databaseName
		)
		{
			return GetConnectionString(serverName, databaseName, string.Empty, string.Empty, true);
		}

		// Return a connection string to get access to the specified database.
		public static string GetConnectionString(
			string serverName,
			string databaseName,
			string password,
			string login,
			bool multipleActiveResultSets
	   )
		{
			string connectionString = "server=" + serverName + ";database=" + databaseName + ";";
			// If a password is provided, add login and password to connection string
			// Otherwise, use Windows authentication
			if (password.Length == 0)
			{
				connectionString += "integrated security=sspi;";
			} else
			{
				connectionString += "User ID=" + login + ";password=" + password + ";";
			}
			if (multipleActiveResultSets)
			{
				connectionString += "MultipleActiveResultSets=True;";
			}
			return connectionString;
		}

		// Use the provided SQL to fill and return a data set
		public static DataSet FillDataSet(
			string sql,
			string connectionString
		)
		{
			DataSet returnDataset = new DataSet();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
					dataAdapter.Fill(returnDataset);
				}
			}
			return returnDataset;
		}
	}
}
