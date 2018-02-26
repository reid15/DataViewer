
-- Test data for Data Viewer

drop table if exists dbo.Sales;
drop table if exists dbo.Customer;

create table dbo.Customer(
CustomerId smallint not null primary key,
CustomerName varchar(50) not null
);

create table dbo.Sales(
SalesId int not null identity(1,1) primary key,
CustomerId smallint not null references dbo.Customer(CustomerId),
SalesAmount decimal(6,2) not null,
SalesDescription varchar(50) not null,
SalesDate date not null
);

go

create or alter procedure dbo.GetCustomer
	@CustomerId smallint
as

select c.CustomerId, c.CustomerName
from dbo.Customer as c
where c.CustomerId = @CustomerId;

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

insert into dbo.Customer(CustomerId, CustomerName)
values (1, 'Acme Inc.');

insert into dbo.Customer(CustomerId, CustomerName)
values (2, 'Doug Smith');

insert into dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
values (1, 350.00, 'Tires', dateadd(day, -3, getdate()));

insert into dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
values (2, 50.00, 'Muffler', dateadd(day, -2, getdate()));

insert into dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
values (1, 500.00, 'Bumper', dateadd(day, -1, getdate()));
 
insert into dbo.Sales(CustomerId, SalesAmount, SalesDescription, SalesDate) 
values (2, 35.00, 'Oil Change', getdate());

select * from dbo.Customer;
select * from dbo.Sales;

exec dbo.GetSalesForCustomer 1;

go
