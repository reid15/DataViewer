using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

		private void InitializeForm()
		{
			GetQueryList();
			cboProcs.ValueMemberChanged += new EventHandler(cboProcs_SelectedIndexChanged);
			gridParameters.DataError += new DataGridViewDataErrorEventHandler(gridParameters_DataError);
			cboProcs.Focus();
		}

		private void GetQueryList()
		{
			cboProcs.DisplayMember = "DisplayName";
            cboProcs.ValueMember = "ProcedureName";
            cboProcs.DataSource = queryList;
		}

		private void GetParameters()
		{
			gridParameters.Columns.Clear();
			string procedureName = cboProcs.SelectedValue.ToString();
            QueryParameter queryParameter = new QueryParameter(parameterDisplayNames);

			BindingSource bindingSource = new BindingSource();
			bindingSource.DataSource = queryParameter.GetQueryParameterDataSet(serverName, databaseName, procedureName);
			gridParameters.DataSource = bindingSource;

            // Hide the Parameter Name column
            gridParameters.Columns[0].Visible = false;

			gridParameters.Columns[1].Visible = true;
			gridParameters.Columns[1].HeaderText = "Parameter";
			gridParameters.Columns[1].Width = 150;
			gridParameters.Columns[1].ReadOnly = true;

            // Hide the Data Type column
			gridParameters.Columns[2].Visible = false;

			// Add a column for the user to enter the parameter value
			DataGridViewColumn column = new DataGridViewTextBoxColumn();
			column.HeaderText = "Input Value";
			gridParameters.Columns.Add(column);
			SetParameterDataTypes();
		}

		private string GetParameterValues()
		{
			string returnValues = "";
			foreach (DataGridViewRow item in gridParameters.Rows)
			{
				if (!item.IsNewRow)
				{
					if (returnValues.Length > 0)
					{
						returnValues += ", ";
					}
					string value = item.Cells[3].Value.ToString();

					returnValues += "'" + value + "'";
				}
			}
			return returnValues;
		}

        // After a new procedure has been chosen, clear the results and the parameters
        private void ClearForm()
        {
            gridParameters.DataSource = null;
            gridResults.DataSource = null;
        }

		private void ExportResults()
		{
			string queryName = cboProcs.Text;
			DataTable results = (DataTable) ((BindingSource)gridResults.DataSource).DataSource;

			ExportData.ExportResults(queryName, results);
		}

        private void ReadConfigurationData()
        {
            string configFileName = "DataViewer_Config.xml";
            DataSet configData = new DataSet();
            configData.ReadXml(configFileName);
            DataRow dataRow = configData.Tables[0].Rows[0];
            serverName = Convert.ToString(dataRow["ServerName"]);
            databaseName = Convert.ToString(dataRow["DatabaseName"]);
			connectionString = DataAccess.GetConnectionString(serverName, databaseName);

            queryList = configData.Tables["Procedure"];
            parameterDisplayNames = SetParameterList(configData.Tables["Parameter"]);
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

		private void SetParameterDataTypes()
		{
			foreach (DataGridViewRow item in gridParameters.Rows)
			{
				if (!item.IsNewRow)
				{
					string dataValue = item.Cells[2].Value.ToString();
					item.Cells[3].ValueType = GetColumnDataType(dataValue);
				}
			}
		}

		private Type GetColumnDataType(
			string dataTypeName
		)
		{
			switch (dataTypeName.ToLower())
			{
				case "datetime":
					return typeof(DateTime);
				default:
					return typeof(String);
			}
		}

		#region EventHandlers

		private void buttonGo_Click(object sender, EventArgs e)
		{
			GoButtonClick();
		}

		private void buttonExport_Click(object sender, EventArgs e)
		{
			ExportResults();
		}

		private void cboProcs_SelectedIndexChanged(object sender, EventArgs e)
		{
			ClearForm();
			GetParameters();
		}

		private void GoButtonClick()
		{
			string procedureName = cboProcs.SelectedValue.ToString();
			string parameterValues = GetParameterValues();
			string sql = "EXEC " + procedureName + " " + parameterValues + ";";
			DataSet queryValue = DataAccess.FillDataSet(sql, connectionString);

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
	}
}
