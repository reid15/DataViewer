using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Data;

namespace DataViewer
{
    public class SQLSchema
	{

		private static StoredProcedure GetStoredProcedure(
			string serverName,
			string databaseName,
            string storedProcedureSchemaName,
            string storedProcedureName
		)
		{
			Database database = DatabaseCommon.DatabaseSchema.GetDatabase(serverName, databaseName);
			return database.StoredProcedures[storedProcedureName, storedProcedureSchemaName];
		}

		public static Dictionary<string, SqlDbType> GetStoredProcedureParameters(
			string serverName,
			string databaseName,
            string storedProcedureSchemaName,
            string storedProcedureName
		)
		{
			StoredProcedure storedProc = GetStoredProcedure(serverName, databaseName, storedProcedureSchemaName, storedProcedureName);
			var parameterList = new Dictionary<string, SqlDbType>();
			foreach (Parameter parameter in storedProc.Parameters)
			{
                var dataTypeName = parameter.DataType.SqlDataType.ToString();
                // Need a SqlDbType enum value to set the stored procedure parameter
                SqlDbType dbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), dataTypeName);
                parameterList.Add(parameter.Name, dbType);
			}
			return parameterList;
		}
	}
}
