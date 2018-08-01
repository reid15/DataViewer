DataViewer

Overview:
DataViewer is a program to allow an end user to execute stored procedure to view results in a grid, and to save those results to a spreadsheet, XML document, JSON or a CSV file.

References:
The solution references a OpenXML component to save the query results to an Excel spreadsheet.
https://github.com/ClosedXML/ClosedXML

Repository Contents:
Bin: The compiled program
Source: Visual Studio solution with the C# source code
SQLScripts: SQL Server scripts to set up test databases. 
	TestData.sql: Create tables with test data, as well as creating stored procedures to access that test data.
	
Configuration:
Update the DataViewer_Config.xml file with configuration data.
Set the Server and Database name as the data source - assumes integrated security is used.
Under 'Procedures', list the Procedure Name and the Display Name to show as a name to the end user.
'Parameters' are optional settings, it will allow different text to be displayed instead of @ParameterName.
