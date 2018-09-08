
-- Test data for Data Viewer

-- Tables

drop table if exists dbo.Sales;
drop table if exists dbo.Customer;
drop table if exists dbo.[State];

create table dbo.[State](
StateCode char(2) not null primary key,
StateName varchar(30) not null
);

create table dbo.Customer(
CustomerId smallint not null primary key,
CustomerName varchar(50) not null,
CustomerState char(2) not null references dbo.[State](StateCode)
);

create table dbo.Sales(
SalesId int not null identity(1,1) primary key,
CustomerId smallint not null references dbo.Customer(CustomerId),
SalesAmount decimal(6,2) not null,
SalesDescription varchar(50) not null,
SalesDate date not null
);

go

-- Stored Procedures

go
create or alter procedure dbo.GetCustomer
	@CustomerId smallint
as

select c.CustomerId, c.CustomerName
from dbo.Customer as c
where c.CustomerId = @CustomerId;

go
create or alter procedure dbo.GetCustomerByState
	@state_code char(2)
as

select c.CustomerId, c.CustomerName, c.CustomerState
from dbo.Customer as c
where c.CustomerState = @state_code;

go
create or alter procedure dbo.GetSalesForCustomer
	@CustomerId smallint
as

select c.CustomerId, c.CustomerName, s.SalesAmount, s.SalesDescription, s.SalesDate
from dbo.Sales as s
join dbo.Customer as c
	on c.CustomerId = s.CustomerId
where c.CustomerId = @CustomerId;

go
create or alter procedure dbo.NotForDataViewer
as

select [Name]
from sys.tables;

go

-- View

create or alter view dbo.vwState
as

select StateCode as KeyValue, StateName as DisplayValue
from dbo.[State];
go

-- Extended Properties
 
EXEC sys.sp_addextendedproperty @name=N'DataViewer', @value=N'true' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetCustomer';
EXEC sys.sp_addextendedproperty @name=N'DataViewer', @value=N'true' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetCustomerByState';
EXEC sys.sp_addextendedproperty @name=N'DataViewer', @value=N'true' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetSalesForCustomer';

EXEC sys.sp_addextendedproperty @name=N'DataViewerName', @value=N'ZZ Get Customer', @level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'GetCustomer';

EXEC sys.sp_addextendedproperty @name=N'DataViewerName', @value=N'ZZZ Customer ID', 
	@level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'GetCustomer',
	@level2type=N'PARAMETER',@level2name=N'@CustomerId';

EXEC sys.sp_addextendedproperty @name=N'DataViewerLookup', @value=N'dbo.vwState', 
	@level0type=N'SCHEMA',@level0name=N'dbo', 
	@level1type=N'PROCEDURE',@level1name=N'GetCustomerByState',
	@level2type=N'PARAMETER',@level2name=N'@state_code';

GO

-- Data

insert into dbo.[State](StateCode, StateName) values ('AL', 'Alabama');
insert into dbo.[State](StateCode, StateName) values ('AK', 'Alaska');
insert into dbo.[State](StateCode, StateName) values ('AR', 'Arkansas');

insert into dbo.Customer(CustomerId, CustomerName, CustomerState)
values (1, 'Acme Inc.', 'AL');

insert into dbo.Customer(CustomerId, CustomerName, CustomerState)
values (2, 'Doug Smith', 'AK');

insert into dbo.Customer(CustomerId, CustomerName, CustomerState)
values (3, 'Jane Doe', 'AR');

insert into dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
values (1, 350.00, 'Tires', dateadd(day, -3, getdate()));

insert into dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
values (2, 50.00, 'Muffler', dateadd(day, -2, getdate()));

insert into dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
values (1, 500.00, 'Bumper', dateadd(day, -1, getdate()));
 
insert into dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
values (2, 35.00, 'Oil Change', getdate());

go
