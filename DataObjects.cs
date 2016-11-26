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

        public StoredProcInfo(
            string schemaName,
            string procName,
            string displayName
        )
        {
            SchemaName = schemaName;
            ProcName = procName;
            DisplayName = displayName;
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

        public StoredProcParameter(
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
