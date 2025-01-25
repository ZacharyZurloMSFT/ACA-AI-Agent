-- Run against the ZurlocloudRunningStore database.

IF OBJECT_ID('dbo.Customer') IS NULL
BEGIN
	CREATE TABLE dbo.Customer
	(
		CustomerID INT NOT NULL CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED,
		FirstName NVARCHAR(75) NOT NULL,
		LastName NVARCHAR(75) NOT NULL,
		MembershipBeginYear INT NOT NULL
	);
END
GO

IF OBJECT_ID('dbo.Store') IS NULL
BEGIN
	CREATE TABLE dbo.Store
	(
		StoreID INT NOT NULL CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED,
		City NVARCHAR(75) NOT NULL,
		Country NVARCHAR(75) NOT NULL
	);
END
GO

IF OBJECT_ID('dbo.Product') IS NULL
BEGIN
	CREATE TABLE dbo.Product
	(
		ProductID INT NOT NULL CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED,
		ProductName NVARCHAR(100) NOT NULL,
		ProductCategory NVARCHAR(50) NOT NULL,
		Price DECIMAL(10, 2) NOT NULL,
		Quantity INT NOT NULL
	);
END
GO

IF OBJECT_ID('dbo.Orders') IS NULL
BEGIN
	CREATE TABLE dbo.Orders
	(
		OrderID INT NOT NULL CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED,
		CustomerID INT NOT NULL,
		StoreID INT NOT NULL,
		OrderDate DATE NOT NULL,
		TotalAmount DECIMAL(10, 2) NOT NULL,
		CONSTRAINT [FK_Order_Customer] FOREIGN KEY(CustomerID) REFERENCES dbo.Customer(CustomerID),
		CONSTRAINT [FK_Order_Store] FOREIGN KEY(StoreID) REFERENCES dbo.Store(StoreID)
	);
END
GO

IF OBJECT_ID('dbo.ProductOrder') IS NULL
BEGIN
	CREATE TABLE dbo.ProductOrder
	(
		ProductOrderID INT NOT NULL CONSTRAINT [PK_ProductOrder] PRIMARY KEY CLUSTERED,
		OrderID INT NOT NULL,
		ProductID INT NOT NULL,
		Quantity INT NOT NULL,
		CONSTRAINT [FK_ProductOrder_Order] FOREIGN KEY(OrderID) REFERENCES dbo.Orders(OrderID),
		CONSTRAINT [FK_ProductOrder_Product] FOREIGN KEY(ProductID) REFERENCES dbo.Product(ProductID)
	);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM dbo.Customer
)
BEGIN
	INSERT INTO dbo.Customer
	(
		CustomerID,
		FirstName,
		LastName,
		MembershipBeginYear
	)
	VALUES
	(1, 'Amber', 'Rodriguez', 2013),
	(2, 'Ana', 'Bowman', 2013),
	(3, 'Dakota', 'Sanchez', 2000),
	(4, 'Amari', 'Rivera', 1992),
	(5, 'Briana', 'Hernandez', 1993);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM dbo.Store
)
BEGIN
	INSERT INTO dbo.Store
	(
		StoreID,
		City,
		Country
	)
	VALUES
	(1, 'New York', 'USA'),
	(2, 'Los Angeles', 'USA'),
	(3, 'Chicago', 'USA'),
	(4, 'Houston', 'USA');
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM dbo.Product
)
BEGIN
	INSERT INTO dbo.Product
	(
		ProductID,
		ProductName,
		ProductCategory,
		Price,
		Quantity
	)
	VALUES
	(1, 'Nike Pegasus', 'Footwear', 120.00, 100),
	(2, 'Adidas Ultraboost', 'Footwear', 180.00, 100),
	(3, 'Brooks Ghost', 'Footwear', 130.00, 100),
	(4, 'Saucony Kinvara', 'Footwear', 110.00, 100),
	(5, 'New Balance Fresh Foam', 'Footwear', 150.00, 100),
	(6, 'Hoka One One Clifton', 'Footwear', 140.00, 100),
	(7, 'Mizuno Wave Rider', 'Footwear', 125.00, 100),
	(8, 'Altra Torin', 'Footwear', 135.00, 100),
	(9, 'On Cloud', 'Footwear', 160.00, 100),
	(10, 'Reebok Floatride', 'Footwear', 100.00, 100),
	(11, 'Under Armour HOVR', 'Footwear', 120.00, 100),
	(12, 'Lululemon running Shorts', 'Apparel', 35.00, 100),
	(13, 'Asics Running Singlet', 'Apparel', 25.00, 100),
	(14, 'Stance Socks', 'Accessories', 10.00, 100),
	(15, 'Running Hat', 'Accessories', 15.00, 100),
	(16, 'Garmin Watch', 'Accessories', 200.00, 100);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM dbo.Orders
)
BEGIN
	INSERT INTO dbo.Orders
	(
		OrderID,
		CustomerID,
		StoreID,
		OrderDate,
		TotalAmount
	)
	VALUES
	(1, 3, 2, '2023-01-06', 155.00),
	(2, 2, 2, '2023-01-19', 35.00),
	(3, 3, 4, '2023-03-16', 145.00),
	(4, 1, 1, '2023-02-09', 120.00),
	(5, 4, 3, '2023-03-11', 25.00),
	(6, 1, 4, '2023-05-09', 150.00),
	(7, 5, 1, '2023-07-02', 170.00),
	(8, 3, 3, '2023-08-23', 25.00),
	(9, 5, 1, '2023-09-04', 130.00),
	(10, 2, 1, '2024-09-06', 120.00),
	(11, 3, 4, '2024-10-06', 145.00),
	(12, 4, 4, '2024-10-06', 150.00),
	(13, 1, 4, '2025-11-15', 145.00),
	(14, 5, 3, '2025-11-06', 170.00),
	(15, 1, 3, '2025-12-29', 145.00);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM dbo.ProductOrder
)
BEGIN
	INSERT INTO dbo.ProductOrder
	(
		ProductOrderID,
		OrderID,
		ProductID,
		Quantity
	)
	VALUES
	(1, 1, 2, 1),
	(2, 1, 3, 1),
	(3, 1, 4, 1),
	(4, 2, 2, 1),
	(5, 3, 1, 1),
	(6, 3, 4, 1),
	(7, 4, 1, 1),
	(8, 5, 3, 1),
	(9, 6, 1, 1),
	(10, 6, 4, 1),
	(11, 7, 1, 1),
	(12, 7, 2, 1),
	(13, 7, 3, 1),
	(14, 8, 3, 1),
	(15, 9, 1, 1),
	(16, 10, 1, 1);
END
GO
