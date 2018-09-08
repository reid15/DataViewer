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

		public static DataSet FillDataSet(
            string storedProcName,
            List<StoredProcParameterValue> parameterList,
            string connectionString
		)
		{
			DataSet returnDataset = new DataSet();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(storedProcName, connection))
				{
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(SetStoredProcParameters(command, parameterList));
					dataAdapter.Fill(returnDataset);
				}
			}
			return returnDataset;
		}

        private static SqlCommand SetStoredProcParameters(
            SqlCommand command,
            List<StoredProcParameterValue> parameterList
        )
        {
            foreach(var item in parameterList)
            {
                command.Parameters.Add(item.ParameterName, item.ParameterDataType).Value = item.ParameterValue;
            }
            return command;
        }
        
	}
}
