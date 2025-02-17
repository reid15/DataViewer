
-- Test data for Data Viewer

-- Cleanup

DROP PROCEDURE IF EXISTS dbo.GetCustomer;
DROP PROCEDURE IF EXISTS dbo.get_customer_by_state;
DROP PROCEDURE IF EXISTS dbo.GetSalesForCustomer;
DROP PROCEDURE IF EXISTS dbo.NotForDataViewer;
DROP PROCEDURE IF EXISTS dbo.GetSalesByState;

DROP VIEW IF EXISTS dbo.vwState;

DROP TABLE IF EXISTS dbo.Sales;
DROP TABLE IF EXISTS dbo.Customer;
DROP TABLE IF EXISTS dbo.[State];

-- Tables

CREATE TABLE dbo.[State](
	StateCode char(2) not null primary key,
	StateName varchar(30) not null
);

CREATE TABLE dbo.Customer(
	CustomerId smallint not null primary key,
	CustomerName varchar(50) not null,
	CustomerState char(2) not null references dbo.[State](StateCode)
);

CREATE TABLE dbo.Sales(
	SalesId int not null identity(1,1) primary key,
	CustomerId smallint not null references dbo.Customer(CustomerId),
	SalesAmount decimal(6,2) not null,
	SalesDescription varchar(50) not null,
	SalesDate date not null
);

GO

-- Stored Procedures

GO
CREATE OR ALTER PROCEDURE dbo.GetCustomer
	@CustomerId smallint
as

select c.CustomerId, c.CustomerName
from dbo.Customer as c
where c.CustomerId = @CustomerId;

GO
CREATE OR ALTER PROCEDURE dbo.get_customer_by_state
	@state_code char(2)
as

select c.CustomerId, c.CustomerName, c.CustomerState
from dbo.Customer as c
where c.CustomerState = @state_code;

GO
CREATE OR ALTER PROCEDURE dbo.GetSalesForCustomer
	@CustomerId smallint
as

select c.CustomerId, c.CustomerName, s.SalesAmount, s.SalesDescription, s.SalesDate
from dbo.Sales as s
join dbo.Customer as c
	on c.CustomerId = s.CustomerId
where c.CustomerId = @CustomerId;

GO
CREATE OR ALTER PROCEDURE dbo.GetSalesByState
	@state_code char(2),
	@MinSalesAmount decimal(6,2)
as

select c.CustomerId, c.CustomerName, s.SalesAmount, s.SalesDescription, s.SalesDate,
	st.StateName
from dbo.Sales as s
join dbo.Customer as c
	on c.CustomerId = s.CustomerId
join dbo.[State] as st
	on st.StateCode = c.CustomerState
where c.CustomerState = @state_code
	and s.SalesAmount >= @MinSalesAmount;

GO
CREATE OR ALTER PROCEDURE dbo.NotForDataViewer
as

select [Name]
from sys.tables;

GO

-- View

create or alter view dbo.vwState
as

select StateCode as KeyValue, StateName as DisplayValue
from dbo.[State];
GO

-- Extended Properties
 
EXEC sys.sp_addextendedproperty @name=N'DataViewer', @value=N'true' , @level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'GetCustomer';
EXEC sys.sp_addextendedproperty @name=N'DataViewer', @value=N'true' , @level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'get_customer_by_state';
EXEC sys.sp_addextendedproperty @name=N'DataViewer', @value=N'true' , @level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'GetSalesForCustomer';
EXEC sys.sp_addextendedproperty @name=N'DataViewer', @value=N'true' , @level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'GetSalesByState';

EXEC sys.sp_addextendedproperty @name=N'DataViewerName', @value=N'Sales: Get Customer', @level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'GetCustomer';

EXEC sys.sp_addextendedproperty @name=N'DataViewerName', @value=N'Sales: Customer ID', 
	@level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'GetCustomer',
	@level2type=N'PARAMETER',@level2name=N'@CustomerId';

EXEC sys.sp_addextendedproperty @name=N'DataViewerLookup', @value=N'dbo.vwState', 
	@level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'get_customer_by_state',
	@level2type=N'PARAMETER',@level2name=N'@state_code';

EXEC sys.sp_addextendedproperty @name=N'DataViewerLookup', @value=N'dbo.vwState', 
	@level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'GetSalesByState',
	@level2type=N'PARAMETER',@level2name=N'@state_code';

GO

-- Data

INSERT INTO dbo.[State](StateCode, StateName) VALUES ('AL', 'Alabama');
INSERT INTO dbo.[State](StateCode, StateName) VALUES ('AK', 'Alaska');
INSERT INTO dbo.[State](StateCode, StateName) VALUES ('AR', 'Arkansas');

INSERT INTO dbo.Customer(CustomerId, CustomerName, CustomerState)
VALUES (1, 'Acme Inc.', 'AL');

INSERT INTO dbo.Customer(CustomerId, CustomerName, CustomerState)
VALUES (2, 'Doug Smith', 'AK');

INSERT INTO dbo.Customer(CustomerId, CustomerName, CustomerState)
VALUES (3, 'Jane Doe', 'AR');

INSERT INTO dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
VALUES (1, 350.00, 'Tires', dateadd(day, -3, getdate()));

INSERT INTO dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
VALUES (2, 50.00, 'Muffler', dateadd(day, -2, getdate()));

INSERT INTO dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
VALUES (1, 500.00, 'Bumper', dateadd(day, -1, getdate()));
 
INSERT INTO dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
VALUES (2, 35.00, 'Oil Change', getdate());

GO
