using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using static DatabaseCommon.DataObject;

namespace DataViewer
{
    public partial class DataViewer : Form
	{
        enum ParameterGridColumnIndex
        {
            DisplayParameterName = 0,
            ParameterName = 1,
            DataType = 2,
            ParameterValue = 3
        }

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
            ClearForm();
            string defaultParameterValue = "";
            var selectedProc = (StoredProcInfo)cboProcs.SelectedValue;
            var parameterList = selectedProc.StoredProcParameters;
            foreach(StoredProcParameter item in parameterList)
            {
                string parameterName = item.ParameterName;
                string parameterDisplayName = item.ParameterDisplayName;
                SqlDbType parameterDataType = item.ParameterDataType;
                var rowIndex = gridParameters.Rows.Add(parameterDisplayName, parameterName, parameterDataType, defaultParameterValue);

                // Add Combo Box for Lookup values, if specified
                if (item.LookupValues.Count > 0)
                {
                    var combo = new DataGridViewComboBoxCell();
                    var source = new BindingSource(item.LookupValues, null);
                    combo.DataSource = source;
                    combo.ValueType = typeof(string);
                    combo.ValueMember = "Key";
                    combo.DisplayMember = "Value";
                    gridParameters.Rows[rowIndex].Cells[(int)ParameterGridColumnIndex.ParameterValue] = combo;
                }

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
                    string value = item.Cells[(int)ParameterGridColumnIndex.ParameterValue].Value.ToString();
                    string name = item.Cells[(int)ParameterGridColumnIndex.ParameterName].Value.ToString();
                    SqlDbType dataType = (SqlDbType)item.Cells[(int)ParameterGridColumnIndex.DataType].Value;
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
            try
            {
                ClearForm();
			    GetParameters();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

		private void GoButtonClick()
		{
            var selectedProc = (StoredProcInfo)cboProcs.SelectedValue;
            var storedProcName = selectedProc.SchemaName + "." + selectedProc.ProcName;
            var parameterValues = GetParameterValues();
			DataSet queryValue = DatabaseCommon.DataAccess.FillDataSet(storedProcName, parameterValues, connectionString);

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
                    SqlDbType dataType = (SqlDbType)item.Cells[(int)ParameterGridColumnIndex.DataType].Value;
                    item.Cells[(int)ParameterGridColumnIndex.ParameterValue].ValueType = DatabaseCommon.DataType.GetColumnCSharpDataType(dataType);
                }
            }
        }

    }
}
