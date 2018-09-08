DataViewer

Overview:
DataViewer is a program to allow an end user to execute a SQL Server stored procedure to view results in a grid, and to save those results to a spreadsheet, XML document, JSON or a CSV file.

Requirements:
The program requires the .Net Framework 4.0 or later. 
Windows authentication is used. The user must have permission to view the database objects and data that will be compared.
No SQL Server edition specific features are used. All functions were tested using SQL Server 2017.
The source code references the project in the DatabaseCommon repository.

References:
The solution references a OpenXML component to save the query results to an Excel spreadsheet.
https://github.com/ClosedXML/ClosedXML

Repository Contents:
Bin: The compiled program
Source: Visual Studio solution with the C# source code
SQLScripts: SQL Server scripts to set up test data. 
	TestData.sql: Create tables with test data, as well as creating stored procedures to access that test data.
	
Configuration File:
Update the .config file with configuration data.
Set the DatabaseInstanceName and DatabaseName name as the data source.
Set FormatObjectName to true or false - false will display the actual stored procedure and parameter names - true will format the names to a more readable form (Ex. GetObjectName to Get Object Name, @input_parameter to input parameter, etc.)

Query List:
To have a stored procedure show in the list of available queries, add an extended property to the proc:
  Name = DataViewer
  Value = true
The following SQL will add the extended property, in this case for the dbo.GetCustomer proc.
EXEC sys.sp_addextendedproperty @name=N'DataViewer', @value=N'true' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetCustomer';

Creating an extended property named DataViewerName for either a stored procedure or a stored procedure parameter will use the value of that property as the display name for that object. The FormatObjectName value in the .config file must be set to true for these values to be applied.
This SQL will create an extended property for the @CustomerID parameter of the dbo.GetCustomer proc. The text 'Customer ID Number' will be displayed as the parameter name.
EXEC sys.sp_addextendedproperty @name=N'DataViewerName', @value=N'Customer ID Number', 
	@level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'GetCustomer',
	@level2type=N'PARAMETER',@level2name=N'@CustomerId';
	