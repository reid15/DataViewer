using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DataViewer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new DataViewer());
			} catch (Exception e){
				ErrorHandler.WinFormErrorHandler(e);
			}
		}
	}
}
