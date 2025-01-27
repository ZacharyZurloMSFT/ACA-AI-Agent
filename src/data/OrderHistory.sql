
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

IF OBJECT_ID('dbo.AllProducts') IS NULL
BEGIN
    CREATE TABLE dbo.AllProducts
    (
        ProductID INT NOT NULL CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED,
        ProductName NVARCHAR(100) NOT NULL,
        ProductCategory NVARCHAR(50) NOT NULL,
        Price DECIMAL(10, 2) NOT NULL
    );
END
GO

IF OBJECT_ID('dbo.Orders') IS NULL
BEGIN
    CREATE TABLE dbo.Orders
    (
        OrderID INT NOT NULL CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED,
        CustomerID INT NOT NULL,
        StoreID INT NOT NULL,
        OrderDate DATE NOT NULL,
        CONSTRAINT [FK_Orders_Customer] FOREIGN KEY(CustomerID) REFERENCES dbo.Customer(CustomerID),
        CONSTRAINT [FK_Orders_Store] FOREIGN KEY(StoreID) REFERENCES dbo.Store(StoreID)
    );
END
GO


IF OBJECT_ID('dbo.OrderDetails') IS NULL
BEGIN
    CREATE TABLE dbo.OrderDetails
    (
        OrderDetailID INT NOT NULL CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED,
        OrderID INT NOT NULL,
        ProductID INT NOT NULL,
        Quantity INT NOT NULL,
        Price DECIMAL(10, 2) NOT NULL,
        CONSTRAINT [FK_OrderDetails_Order] FOREIGN KEY(OrderID) REFERENCES dbo.Orders(OrderID),
        CONSTRAINT [FK_OrderDetails_Product] FOREIGN KEY(ProductID) REFERENCES dbo.AllProducts(ProductID)
    );
END
GO


IF OBJECT_ID('dbo.StoreProducts') IS NULL
BEGIN
    CREATE TABLE dbo.StoreProducts
    (
        StoreID INT NOT NULL,
		ProductID INT NOT NULL,
		ProductName  NVARCHAR(100) NOT NULL,
		Price DECIMAL(10, 2) NOT NULL,
		Quantity INT NOT NULL,
        CONSTRAINT [FK_StoreProducts_Store] FOREIGN KEY(StoreID) REFERENCES dbo.Store(StoreID),
        CONSTRAINT [FK_StoreProducts_Product] FOREIGN KEY(ProductID) REFERENCES dbo.AllProducts(ProductID)
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
	(5, 'Briana', 'Hernandez', 1993),
	(6, 'Carter', 'Smith', 2015),
	(7, 'Dylan', 'Johnson', 2016),
	(8, 'Evelyn', 'Williams', 2017),
	(9, 'Faith', 'Brown', 2018),
	(10, 'Grace', 'Jones', 2019),
	(11, 'Hannah', 'Garcia', 2020),
	(12, 'Isaac', 'Martinez', 2021),
	(13, 'Jack', 'Davis', 2022),
	(14, 'Kaitlyn', 'Lopez', 2023),
	(15, 'Liam', 'Gonzalez', 2024),
	(16, 'Mason', 'Wilson', 2025),
	(17, 'Nora', 'Anderson', 2024),
	(18, 'Olivia', 'Thomas', 2024),
	(19, 'Parker', 'Taylor', 2024),
	(20, 'Quinn', 'Moore', 2024),
	(21, 'Riley', 'Jackson', 2024),
	(22, 'Sophia', 'Martin', 2024),
	(23, 'Tyler', 'Lee', 2024),
	(24, 'Uma', 'Perez', 2024),
	(25, 'Victoria', 'Thompson', 2024);
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
	(4, 'Houston', 'USA'),
	(5, 'Phoenix', 'USA'),
	(6, 'Philadelphia', 'USA'),
	(7, 'San Antonio', 'USA'),
	(8, 'San Diego', 'USA'),
	(9, 'Dallas', 'USA'),
	(10, 'San Jose', 'USA');
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM dbo.AllProducts
)
BEGIN
	INSERT INTO dbo.AllProducts
	(
		ProductID,
		ProductName,
		ProductCategory,
		Price
	)
	VALUES
	(1, 'Nike Pegasus', 'Footwear', 120.00),
	(2, 'Adidas Ultraboost', 'Footwear', 180.00),
	(3, 'Brooks Ghost', 'Footwear', 130.00),
	(4, 'Saucony Kinvara', 'Footwear', 110.00),
	(5, 'New Balance Fresh Foam', 'Footwear', 150.00),
	(6, 'Hoka One One Clifton', 'Footwear', 140.00),
	(7, 'Mizuno Wave Rider', 'Footwear', 125.00),
	(8, 'Altra Torin', 'Footwear', 135.00),
	(9, 'On Cloud', 'Footwear', 160.00),
	(10, 'Reebok Floatride', 'Footwear', 100.00),
	(11, 'Under Armour HOVR', 'Footwear', 120.00),
	(12, 'Lululemon running Shorts', 'Apparel', 35.00),
	(13, 'Asics Running Singlet', 'Apparel', 25.00),
	(14, 'Stance Socks', 'Accessories', 10.00),
	(15, 'Running Hat', 'Accessories', 15.00),
	(16, 'Garmin Watch', 'Accessories', 200.00),
	(17, 'Marathon Training', 'Training', 200.00),
	(18, 'Half Marathon Training', 'Training', 150.00),
	(19, '5K Training', 'Training', 100.00),
	(20, '10K Training', 'Training', 120.00),
	(21, 'Trail Running Training', 'Training', 180.00);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM dbo.StoreProducts
)
BEGIN
	INSERT INTO dbo.StoreProducts
	(
		StoreID,
		ProductID,
		ProductName,
		Price,
		Quantity
	)
	VALUES
	(1, 1, 'Nike Pegasus', 120.00, 50),
	(1, 4, 'Saucony Kinvara', 110.00, 30),
	(1, 7, 'Mizuno Wave Rider', 125.00, 20),
	(1, 10, 'Reebok Floatride', 100.00, 40),
	(1, 12, 'Lululemon running Shorts', 35.00, 60),
	(1, 17, 'Marathon Training', 200.00, 10),
	(2, 4, 'Saucony Kinvara', 110.00, 30),
	(2, 1, 'Nike Pegasus', 120.00, 50),
	(2, 16, 'Garmin Watch', 200.00, 15),
	(2, 3, 'Brooks Ghost', 130.00, 25),
	(2, 8, 'Altra Torin', 135.00, 20),
	(2, 18, 'Half Marathon Training', 150.00, 10),
	(3, 2, 'Adidas Ultraboost', 180.00, 10),
	(3, 5, 'New Balance Fresh Foam', 150.00, 20),
	(3, 9, 'On Cloud', 160.00, 15),
	(3, 19, '5K Training', 100.00, 10),
	(3, 11, 'Under Armour HOVR', 120.00, 25),
	(3, 14, 'Stance Socks', 10.00, 100),
	(4, 6, 'Hoka One One Clifton', 140.00, 20),
	(4, 13, 'Asics Running Singlet', 25.00, 50),
	(4, 15, 'Running Hat', 15.00, 40),
	(4, 10, 'Reebok Floatride', 100.00, 40),
	(4, 20, '10K Training', 120.00, 10),
	(4, 7, 'Mizuno Wave Rider', 125.00, 20),
	(5, 1, 'Nike Pegasus', 120.00, 50),
	(5, 2, 'Adidas Ultraboost', 180.00, 10),
	(5, 3, 'Brooks Ghost', 130.00, 25),
	(5, 21, 'Trail Running Training', 180.00, 10),
	(5, 4, 'Saucony Kinvara', 110.00, 30),
	(5, 5, 'New Balance Fresh Foam', 150.00, 20),
	(6, 6, 'Hoka One One Clifton', 140.00, 20),
	(6, 7, 'Mizuno Wave Rider', 125.00, 20),
	(6, 8, 'Altra Torin', 135.00, 20),
	(6, 17, 'Marathon Training', 200.00, 10),
	(6, 9, 'On Cloud', 160.00, 15),
	(6, 10, 'Reebok Floatride', 100.00, 40),
	(7, 11, 'Under Armour HOVR', 120.00, 25),
	(7, 12, 'Lululemon running Shorts', 35.00, 60),
	(7, 13, 'Asics Running Singlet', 25.00, 50),
	(7, 14, 'Stance Socks', 10.00, 100),
	(7, 18, 'Half Marathon Training', 150.00, 10),
	(7, 15, 'Running Hat', 15.00, 40),
	(8, 16, 'Garmin Watch', 200.00, 15),
	(8, 1, 'Nike Pegasus', 120.00, 50),
	(8, 2, 'Adidas Ultraboost', 180.00, 10),
	(8, 19, '5K Training', 100.00, 10),
	(8, 3, 'Brooks Ghost', 130.00, 25),
	(8, 4, 'Saucony Kinvara', 110.00, 30),
	(9, 5, 'New Balance Fresh Foam', 150.00, 20),
	(9, 6, 'Hoka One One Clifton', 140.00, 20),
	(9, 20, '10K Training', 120.00, 10),
	(9, 7, 'Mizuno Wave Rider', 125.00, 20),
	(9, 8, 'Altra Torin', 135.00, 20),
	(9, 9, 'On Cloud', 160.00, 15),
	(10, 10, 'Reebok Floatride', 100.00, 40),
	(10, 11, 'Under Armour HOVR', 120.00, 25),
	(10, 12, 'Lululemon running Shorts', 35.00, 60),
	(10, 13, 'Asics Running Singlet', 25.00, 50),
	(10, 14, 'Stance Socks', 10.00, 100),	
	(10, 21, 'Trail Running Training', 180.00, 10);
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
        OrderDate
	)
	VALUES
	(1, 3, 2, '2023-01-06'),
    (2, 2, 2, '2023-01-19'),
    (3, 3, 4, '2023-03-16'),
    (4, 1, 1, '2023-02-09'),
    (5, 4, 3, '2023-03-11'),
    (6, 1, 4, '2023-05-09'),
    (7, 5, 1, '2023-07-02'),
    (8, 3, 3, '2023-08-23'),
    (9, 5, 1, '2023-09-04'),
    (10, 2, 1, '2024-09-06'),
    (11, 3, 4, '2024-10-06'),
    (12, 4, 4, '2024-10-06'),
    (13, 1, 4, '2025-11-15'),
    (14, 5, 3, '2025-11-06');
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM dbo.OrderDetails
)
BEGIN
	INSERT INTO dbo.OrderDetails
	(
		OrderDetailID,
		OrderID,
		ProductID,
		Quantity,
		Price
	)
	VALUES
	(1, 1, 1, 1, 155.00),
	(2, 2, 12, 1, 35.00),
	(3, 3, 7, 1, 145.00),
	(4, 4, 1, 1, 120.00),
	(5, 5, 13, 1, 25.00),
	(6, 6, 5, 1, 150.00),
	(7, 7, 2, 1, 170.00),
	(8, 8, 13, 1, 25.00),
	(9, 9, 3, 1, 130.00),
	(10, 10, 1, 1, 120.00),
	(11, 11, 7, 1, 145.00),
	(12, 12, 5, 1, 150.00),
	(13, 13, 7, 1, 145.00),
	(14, 14, 2, 1, 170.00),
	(15, 1, 2, 2, 180.00),
	(16, 2, 3, 1, 130.00),
	(17, 3, 4, 1, 110.00),
	(18, 4, 5, 1, 150.00),
	(19, 5, 6, 1, 140.00),
	(20, 6, 7, 1, 125.00),
	(21, 7, 8, 1, 135.00),
	(22, 8, 9, 1, 160.00),
	(23, 9, 10, 1, 100.00),
	(24, 10, 11, 1, 120.00),
	(25, 11, 12, 1, 35.00),
	(26, 12, 13, 1, 25.00),
	(27, 13, 14, 1, 10.00),
	(28, 14, 15, 1, 15.00);
END
GO