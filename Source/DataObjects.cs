using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataViewer
{
    public class StoredProcInfo
    {
        public string SchemaName { get; set; }
        public string ProcName { get; set; }
        public string DisplayName { get; set; }
        public List<StoredProcParameter> StoredProcParameters { get; set; }

        public StoredProcInfo(
            string schemaName,
            string procName,
            string displayName,
            List<StoredProcParameter> storedProcParameters
        )
        {
            SchemaName = schemaName;
            ProcName = procName;
            DisplayName = displayName;
            StoredProcParameters = storedProcParameters;
        }
        public override string ToString()
        {
            return DisplayName;
        }
    }

    public class StoredProcParameter
    {
        public string ParameterName { get; set; }
        public SqlDbType ParameterDataType { get; set; }
        public string ParameterValue { get; set; }
        public string ParameterDisplayName { get; set; }
        public Dictionary<string, string> LookupValues { get; set; }

        public StoredProcParameter(
            string parameterName,
            SqlDbType parameterDataType,
            string parameterValue,
            string parameterDisplayName,
            Dictionary<string, string> lookupValues
        )
        {
            ParameterName = parameterName;
            ParameterDataType = parameterDataType;
            ParameterValue = parameterValue;
            ParameterDisplayName = parameterDisplayName;
            LookupValues = lookupValues;
        }
    }

    public class StoredProcParameterValue
    {
        public string ParameterName { get; set; }
        public SqlDbType ParameterDataType { get; set; }
        public string ParameterValue { get; set; }

        public StoredProcParameterValue(
            string parameterName,
            SqlDbType parameterDataType,
            string parameterValue
        )
        {
            ParameterName = parameterName;
            ParameterDataType = parameterDataType;
            ParameterValue = parameterValue;
        }
    }

}
