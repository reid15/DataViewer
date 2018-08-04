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
	
Configuration:
Update the DataViewer_Config.xml file with configuration data.
Set the Server and Database name as the data source.
Under 'Procedures', list the Procedure Name and the Display Name to show as a name to the end user.
'Parameters' are optional settings, it will allow different text to be displayed instead of @ParameterName.
