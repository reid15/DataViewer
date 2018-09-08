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
            List<StoredProcedure> procList = DatabaseCommon.DatabaseSchema.GetStoredProcedures(serverName, databaseName);
            foreach(StoredProcedure proc in procList)
            {
                if (GetExtendedPropertyValue(proc, "DataViewer") == "true")
                {
                    string objectName = GetObjectName(proc.Name, formatObjectName);
                    returnList.Add(new StoredProcInfo(proc.Schema, proc.Name, objectName, GetStoredProcParameters(proc, formatObjectName)));
                }
            }
            
            return returnList;
        }

        private static string GetExtendedPropertyValue(
            StoredProcedure proc,
            string propertyName
        )
        {
            string returnValue = "";
            if (proc.ExtendedProperties.Contains(propertyName))
            {
                returnValue = proc.ExtendedProperties[propertyName].Value.ToString().ToLower();
            }
            return returnValue;
        }

        private static string GetObjectName(
            string objectName,
            bool formatObjectName
        )
        {
            if (!formatObjectName)
            {
                return objectName;
            }
            string returnName = objectName.Replace("_", " ");
            returnName = returnName.Replace("@", " ");
            // Add a space in front of each capital letter
            returnName = Regex.Replace(returnName, "[A-Z]", " $0").Trim();

            return returnName;
        }

        private static List<StoredProcParameter> GetStoredProcParameters(
            StoredProcedure proc,
            bool formatObjectName
        )
        {
            string defaultParameterValue = "";
            var returnList = new List<StoredProcParameter>();
            foreach (StoredProcedureParameter parameter in proc.Parameters)
            {
                var dataTypeName = parameter.DataType.SqlDataType.ToString();
                // Need a SqlDbType enum value to set the stored procedure parameter for the SQL Command
                SqlDbType dataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), dataTypeName);
                var displayName = GetObjectName(parameter.Name, formatObjectName);
                var parameterInfo = new StoredProcParameter(parameter.Name, dataType, defaultParameterValue, displayName);
                returnList.Add(parameterInfo);
            }

            return returnList;
        }

    }
}
