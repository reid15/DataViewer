using System;
using System.Windows.Forms;
using System.Data;
using ClosedXML.Excel;
using System.Text;
using System.IO;

namespace DataViewer
{

	public class ExportData
	{
        enum FileFormat
        {
            Excel = 1,
            XML = 2,
            CSV = 3
        }

        public static void ExportResults(
			string queryName,
			DataTable results
		)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Excel WorkBook (*.xlsx)|.xlsx|Extensible Markup Language (*.xml)|.xml|CSV (*.csv)|.csv";
			saveFileDialog.FileName = queryName;
			DialogResult dialogResult = saveFileDialog.ShowDialog();
			if (dialogResult == DialogResult.Cancel)
			{
				return;
			}
			Application.DoEvents();

			// Filter index should match the order of the file types in the Filter
			switch (saveFileDialog.FilterIndex)
			{
				case (int)FileFormat.Excel: 
					SaveResultsToExcel(saveFileDialog.FileName, results);
					break;
				case (int)FileFormat.XML: 
					SaveResultsToXml(saveFileDialog.FileName, results);
					break;
                case (int)FileFormat.CSV: 
                    SaveResultsToCsv(saveFileDialog.FileName, results);
                    break;
                default:
					throw new ApplicationException("Unhandled ExportData type");
			}
		}

		private static void SaveResultsToXml(
			string fileName,
			DataTable results
		)
		{
			results.WriteXml(fileName);
		}

		private static void SaveResultsToExcel(
			string fileName,
			DataTable results
		)
		{
			var wb = new XLWorkbook();
			wb.Worksheets.Add(results);
            wb.SaveAs(fileName);
		}

        private static void SaveResultsToCsv(
            string fileName,
            DataTable results
        )
        {
            var sb = new StringBuilder();
            bool firstColumn = true;
            foreach (var column in results.Columns)
            {
                if (!firstColumn)
                {
                    sb.Append(",");
                } else
                {
                    firstColumn = false;
                }
                sb.Append("\"" + column.ToString() + "\"");
            }
            sb.AppendLine();
            foreach (DataRow row in results.Rows)
            {
                firstColumn = true;
                foreach (var item in row.ItemArray)
                {
                    if (!firstColumn)
                    {
                        sb.Append(",");
                    }
                    else
                    {
                        firstColumn = false;
                    }
                    sb.Append("\"" + item.ToString() + "\"");
                }
                sb.AppendLine();
            }
            File.WriteAllText(fileName, sb.ToString());
        }
    }
}
