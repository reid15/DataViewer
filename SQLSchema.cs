using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Smo;
using System.Text;

namespace DataViewer
{
	public class SQLSchema
	{
		// Return Database SMO object for an existing database.
		private static Database GetDatabase(
			string serverName,
			string databaseName
		)
		{
			Server server = new Server(serverName);
			return server.Databases[databaseName];
		}

		// Return StoredProcedure SMO object.
		private static StoredProcedure GetStoredProcedure(
			string serverName,
			string databaseName,
			string storedProcedureName
		)
		{
			Database database = GetDatabase(serverName, databaseName);
			return database.StoredProcedures[storedProcedureName];
		}

		public static Dictionary<string, string> GetStoredProcedureParameters(
			string serverName,
			string databaseName,
			string storedProcedureName
		)
		{
			StoredProcedure storedProc = GetStoredProcedure(serverName, databaseName, storedProcedureName);
			Dictionary<string, string> parameterList = new Dictionary<string, string>();
			foreach (Parameter parameter in storedProc.Parameters)
			{
				parameterList.Add(parameter.Name, parameter.DataType.Name);
			}
			return parameterList;
		}
	}
}
