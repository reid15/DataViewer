DataViewer:
DataViewer is a program to allow an end user to execute stored procedure to view results in a grid, and to save those results to a spreadsheet.

References:
The solution references a OpenXML component to save the query results to an Excel spreadsheet.
http://closedxml.codeplex.com/

Configuration:
Update the DataViewer_Config.xml file with configuration data.
Set the Server and Database name as the data source - assumes integrated security is used.
Under 'Procedures', list the Procedure Name and the Display Name to show as a name to the end user.
'Parameters' are optional settings, it will allow different text to be displayed instead of @ParameterName.
