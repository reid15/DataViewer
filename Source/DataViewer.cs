using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace DataViewer
{
    public partial class DataViewer : Form
	{
		string connectionString;
        string serverName;
        string databaseName;
        bool formatObjectName = false;

		public DataViewer()
		{
            try
            {
                InitializeComponent();
                ReadConfigurationData();
                InitializeForm();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
                this.Close();
            }
		}

        private void ReadConfigurationData()
        {
            var settingsReader = new AppSettingsReader();

            serverName = (string)settingsReader.GetValue("DatabaseInstanceName", typeof(string));
            databaseName = (string)settingsReader.GetValue("DatabaseName", typeof(string));
            string formatObjectNameString = (string)settingsReader.GetValue("FormatObjectName", typeof(string));
            if (formatObjectNameString.ToLower() == "true")
            {
                formatObjectName = true;
            }
            connectionString = DatabaseCommon.DataAccess.GetConnectionString(serverName, databaseName);
        }

        private void InitializeForm()
		{
			SetQueryList();
			cboProcs.ValueMemberChanged += new EventHandler(cboProcs_SelectedIndexChanged);
			gridParameters.DataError += new DataGridViewDataErrorEventHandler(gridParameters_DataError);
			cboProcs.Focus();
		}

		private void SetQueryList()
		{
            cboProcs.DataSource = QueryList.GetQueryList(serverName, databaseName, formatObjectName);
		}

		private void GetParameters()
		{
            gridParameters.Rows.Clear();
            var selectedProc = (StoredProcInfo)cboProcs.SelectedValue;
            var parameterList = selectedProc.StoredProcParameters;
            foreach(var item in parameterList)
            {
                string parameterName = item.ParameterName;
                string parameterDisplayName = item.ParameterDisplayName;
                SqlDbType parameterDataType = item.ParameterDataType;
                string defaultParameterValue = "";

                gridParameters.Rows.Add(parameterDisplayName, parameterName, parameterDataType, defaultParameterValue);
            }
            SetParameterDataTypes();
        }

        private List<StoredProcParameterValue> GetParameterValues()
		{
            var returnList = new List<StoredProcParameterValue>();

            foreach (DataGridViewRow item in gridParameters.Rows)
			{
				if (!item.IsNewRow)
				{
                    string value = item.Cells[3].Value.ToString();
                    string name = item.Cells[1].Value.ToString();
                    SqlDbType dataType = (SqlDbType)item.Cells[2].Value;
                    var parameter = new StoredProcParameterValue(name, dataType, value);
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
                    item.Cells[3].ValueType = DatabaseCommon.DataType.GetColumnCSharpDataType(dataType);
                }
            }
        }

    }
}
