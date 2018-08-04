using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DataViewer
{
    public partial class DataViewer : Form
	{
		string connectionString;
        string serverName;
        string databaseName;
        DataTable queryList;
        Dictionary<string, string> parameterDisplayNames;

		public DataViewer()
		{
			InitializeComponent();
            ReadConfigurationData();
            InitializeForm();
		}

        private void ReadConfigurationData()
        {
            string configFileName = "DataViewer_Config.xml";
            DataSet configData = new DataSet();
            configData.ReadXml(configFileName);
            DataRow dataRow = configData.Tables[0].Rows[0];
            serverName = Convert.ToString(dataRow["ServerName"]);
            databaseName = Convert.ToString(dataRow["DatabaseName"]);
            connectionString = DatabaseCommon.DataAccess.GetConnectionString(serverName, databaseName);

            queryList = configData.Tables["Procedure"];
            parameterDisplayNames = SetParameterList(configData.Tables["Parameter"]);
        }

        private void InitializeForm()
		{
			GetQueryList();
			cboProcs.ValueMemberChanged += new EventHandler(cboProcs_SelectedIndexChanged);
			gridParameters.DataError += new DataGridViewDataErrorEventHandler(gridParameters_DataError);
			cboProcs.Focus();
		}

		private void GetQueryList()
		{
            var procList = new List<StoredProcInfo>();
            foreach(DataRow row in queryList.Rows)
            {
                procList.Add(new StoredProcInfo(row["ProcedureSchemaName"].ToString(), row["ProcedureName"].ToString(), row["DisplayName"].ToString()));
            }
            cboProcs.DataSource = procList;
		}

		private void GetParameters()
		{
            gridParameters.Rows.Clear();
            var selectedProc = (StoredProcInfo)cboProcs.SelectedValue;
            Dictionary<string, SqlDbType> parameterList = SQLSchema.GetStoredProcedureParameters(serverName, databaseName,
                selectedProc.SchemaName, selectedProc.ProcName);
            foreach(var item in parameterList)
            {
                string parameterName = item.Key;
                string parameterDisplayName = "";
                SqlDbType parameterDataType = item.Value;
                string parameterValue = "";
                if (!parameterDisplayNames.TryGetValue(parameterName, out parameterDisplayName))
                {
                    parameterDisplayName = parameterName;
                }
                gridParameters.Rows.Add(parameterDisplayName, parameterName, parameterDataType, parameterValue);
            }
            SetParameterDataTypes();
        }

        private List<StoredProcParameter> GetParameterValues()
		{
            var returnList = new List<StoredProcParameter>();

            foreach (DataGridViewRow item in gridParameters.Rows)
			{
				if (!item.IsNewRow)
				{
                    string value = item.Cells[3].Value.ToString();
                    string name = item.Cells[1].Value.ToString();
                    SqlDbType dataType = (SqlDbType)item.Cells[2].Value;
                    var parameter = new StoredProcParameter(name, dataType, value);
                    returnList.Add(parameter);
				}
			}
			return returnList;
		}
        
        private void ClearForm()
        {
            gridParameters.Rows.Clear();
            gridResults.DataSource = null;
        }

		private void ExportResults()
		{
			string queryName = cboProcs.Text;
			DataTable results = (DataTable) ((BindingSource)gridResults.DataSource).DataSource;

			ExportData.ExportResults(queryName, results);
		}

        private Dictionary<string, string> SetParameterList(
            DataTable parameterDataTable
        )
        {
            Dictionary<string, string> parameterListLocal = new Dictionary<string,string>();
            foreach (DataRow dataRow in parameterDataTable.Rows)
            {
                string parameterName = Convert.ToString(dataRow["ParameterName"]);
                string parameterText = Convert.ToString(dataRow["ParameterText"]);
                parameterListLocal.Add(parameterName, parameterText);
            }
            return parameterListLocal;
        }
        
		#region EventHandlers

		private void buttonGo_Click(object sender, EventArgs e)
		{
            try
            {
                GoButtonClick();
            } catch (Exception ex)
            {
                ErrorHandler(ex);
            }
		}

		private void buttonExport_Click(object sender, EventArgs e)
		{
            try
            {
                ExportResults(); 
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
		}

		private void cboProcs_SelectedIndexChanged(object sender, EventArgs e)
		{
			ClearForm();
			GetParameters();
		}

		private void GoButtonClick()
		{
            var selectedProc = (StoredProcInfo)cboProcs.SelectedValue;
            var storedProcName = selectedProc.SchemaName + "." + selectedProc.ProcName;
            var parameterValues = GetParameterValues();
			DataSet queryValue = DataAccess.FillDataSet(storedProcName, parameterValues, connectionString);

			BindingSource bindingSource = new BindingSource();
			bindingSource.DataSource = queryValue.Tables[0];
			gridResults.DataSource = bindingSource;
		}

		private void gridParameters_DataError(
			object sender,
			DataGridViewDataErrorEventArgs e
		)
		{
			string errorMessage = "Parameter Data Validation Error" + Environment.NewLine +
				e.Exception.Message;
			MessageBox.Show(errorMessage);
		}
        #endregion

        private void ErrorHandler(
            Exception e
        )
        {
            var errorMessage = e.Message;
            var caption = "Error";
            MessageBox.Show(errorMessage, caption);
        }
        
        private void SetParameterDataTypes()
        {
            foreach (DataGridViewRow item in gridParameters.Rows)
            {
                if (!item.IsNewRow)
                {
                    SqlDbType dataType = (SqlDbType)item.Cells[2].Value;
                    item.Cells[3].ValueType = GetColumnDataType(dataType);
                }
            }
        }

        private Type GetColumnDataType(
            SqlDbType dataType
        )
        {
            switch (dataType)
            {
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                    return typeof(DateTime);
                case SqlDbType.BigInt:
                case SqlDbType.Int:
                case SqlDbType.SmallInt:
                case SqlDbType.TinyInt:
                    return typeof(Int32);
                default:
                    return typeof(String);
            }
        }

    }
}
