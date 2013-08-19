using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataViewer
{
    public class QueryParameter
    {
        Dictionary<string, string> parameterDisplayNames;

        public QueryParameter(
            Dictionary<string, string> parameterDisplayNamesInput
        )
        {
            parameterDisplayNames = parameterDisplayNamesInput;
        }

        public QueryParameterDataSet.QueryParameterDataTableDataTable GetQueryParameterDataSet(
            string serverName,
            string databaseName,
            string storedProcedureName
        )
        {
            QueryParameterDataSet dataSet = new QueryParameterDataSet();
			// Return Dictionary of ParameterName and Data Type Name
			Dictionary<string, string> parameterList = SQLSchema.GetStoredProcedureParameters(serverName, databaseName, storedProcedureName);
			foreach (KeyValuePair<string, string> item in parameterList)
            {
                string parameter = item.Key;
                string displayName = string.Empty;
                parameterDisplayNames.TryGetValue(parameter, out displayName);
                if (displayName == null || displayName == string.Empty)
                {
                    displayName = parameter;
                }
                dataSet.QueryParameterDataTable.AddQueryParameterDataTableRow(parameter, displayName, item.Value);
            }
            return dataSet.QueryParameterDataTable;
        }
    }
}
