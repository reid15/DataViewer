using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace DataViewer
{
    public class QueryList
    {
        public static List<StoredProcInfo> GetQueryList(
            string serverName,
            string databaseName,
            bool formatObjectName
        )
        {
            var returnList = new List<StoredProcInfo>();
            string connectionString = DatabaseCommon.DataAccess.GetConnectionString(serverName, databaseName);
            List<StoredProcedure> procList = DatabaseCommon.DatabaseSchema.GetStoredProcedures(serverName, databaseName);
            foreach(StoredProcedure proc in procList)
            {
                if (GetExtendedPropertyValue(proc.ExtendedProperties, "DataViewer") == "true")
                {
                    string dataViewerName = GetExtendedPropertyValue(proc.ExtendedProperties, "DataViewerName");
                    string objectName = GetObjectName(proc.Name, formatObjectName, dataViewerName);
                    var procParameters = GetStoredProcParameters(proc, formatObjectName, connectionString);
                    returnList.Add(new StoredProcInfo(proc.Schema, proc.Name, objectName, procParameters));
                }
            }
            
            return returnList;
        }

        private static string GetExtendedPropertyValue(
            ExtendedPropertyCollection extendedProperties,
            string propertyName
        )
        {
            string returnValue = "";
            if (extendedProperties.Contains(propertyName))
            {
                returnValue = extendedProperties[propertyName].Value.ToString().ToLower();
            }
            return returnValue;
        }

        private static string GetObjectName(
            string objectName,
            bool formatObjectName,
            string dataViewerName
        )
        {
            if (!formatObjectName)
            {
                return objectName;
            }
            if (dataViewerName.Length > 0)
            {
                return dataViewerName;
            }
            string returnName = objectName.Replace("_", " ");
            returnName = returnName.Replace("@", " ");
            // Add a space in front of each capital letter
            returnName = Regex.Replace(returnName, "[A-Z]", " $0").Trim();

            return returnName;
        }

        private static List<StoredProcParameter> GetStoredProcParameters(
            StoredProcedure proc,
            bool formatObjectName,
            string connectionString
        )
        {
            string defaultParameterValue = "";
            var returnList = new List<StoredProcParameter>();
            foreach (StoredProcedureParameter parameter in proc.Parameters)
            {
                var dataTypeName = parameter.DataType.SqlDataType.ToString();
                // Need a SqlDbType enum value to set the stored procedure parameter for the SQL Command
                SqlDbType dataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), dataTypeName);
                string dataViewerName = GetExtendedPropertyValue(parameter.ExtendedProperties, "DataViewerName");
                string lookupTableName = GetExtendedPropertyValue(parameter.ExtendedProperties, "DataViewerLookup");
                var lookupValues = GetLookupValues(lookupTableName, connectionString);
                var displayName = GetObjectName(parameter.Name, formatObjectName, dataViewerName);
                var parameterInfo = new StoredProcParameter(parameter.Name, dataType, defaultParameterValue, displayName, lookupValues);
                returnList.Add(parameterInfo);
            }

            return returnList;
        }

        private static Dictionary<string, string> GetLookupValues(
            string tableName,
            string connectionString
        )
        {
            var returnDictionary = new Dictionary<string, string>();
            if (tableName.Length == 0)
            {
                return returnDictionary;
            }

            string sql = string.Format("select KeyValue, DisplayValue from {0} order by KeyValue", tableName);
            var data = DatabaseCommon.DataAccess.GetDataTable(connectionString, sql);
            
            foreach(DataRow item in data.Rows)
            {
                returnDictionary.Add(item["KeyValue"].ToString(), item["DisplayValue"].ToString());
            }

            return returnDictionary;
        }

    }
}
