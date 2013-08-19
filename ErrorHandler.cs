using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataViewer
{
	public class ErrorHandler
	{
		// Error handler for a windows form application - Return error message
		public static string WinFormErrorHandler(
			Exception e
		)
		{
			return WinFormErrorHandler(e, string.Empty);
		}

		// Error handler for a windows form application
		// Allow for an additional message to be added to error text
		// Return error message
		public static string WinFormErrorHandler(
			Exception e,
			string message
		)
		{
			StringBuilder errorMessage = new StringBuilder();
			errorMessage.Append("Error: ");
			errorMessage.AppendLine(e.Message.ToString());
			errorMessage.AppendLine("");
			if (message.Length > 0)
			{
				errorMessage.Append("Message: ");
				errorMessage.AppendLine(message);
				errorMessage.AppendLine("");
			}
			errorMessage.Append("Stack Trace: ");
			errorMessage.AppendLine(e.StackTrace.ToString());
			return errorMessage.ToString();
		}
	}
}
