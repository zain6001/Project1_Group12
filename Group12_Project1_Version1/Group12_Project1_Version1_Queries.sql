-- Create Products Table
use InventoryManagement

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    SKU NVARCHAR(50) UNIQUE NOT NULL,
    Category NVARCHAR(50) NULL,
    Quantity INT DEFAULT 0,
    UnitPrice DECIMAL(10, 2) NULL,
    Barcode NVARCHAR(50) NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- Create Suppliers Table
CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName NVARCHAR(100) NOT NULL,
    ContactName NVARCHAR(100) NULL,
    Phone NVARCHAR(15) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(255) NULL
);

-- Create PurchaseOrders Table
CREATE TABLE PurchaseOrders (
    PurchaseOrderID INT PRIMARY KEY IDENTITY(1,1),
    SupplierID INT FOREIGN KEY REFERENCES Suppliers(SupplierID),
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) CHECK (Status IN ('Pending', 'Completed', 'Cancelled')),
    TotalAmount DECIMAL(10, 2) NULL
);

-- Create PurchaseOrderDetails Table
CREATE TABLE PurchaseOrderDetails (
    PODetailID INT PRIMARY KEY IDENTITY(1,1),
    PurchaseOrderID INT FOREIGN KEY REFERENCES PurchaseOrders(PurchaseOrderID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL
);

-- Create SalesOrders Table
CREATE TABLE SalesOrders (
    SalesOrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerName NVARCHAR(100) NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) CHECK (Status IN ('Pending', 'Shipped', 'Cancelled')),
    TotalAmount DECIMAL(10, 2) NULL
);

-- Create SalesOrderDetails Table
CREATE TABLE SalesOrderDetails (
    SODetailID INT PRIMARY KEY IDENTITY(1,1),
    SalesOrderID INT FOREIGN KEY REFERENCES SalesOrders(SalesOrderID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL
);

-- Create StockMovements Table
CREATE TABLE StockMovements (
    MovementID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    MovementType NVARCHAR(20) CHECK (MovementType IN ('IN', 'OUT', 'ADJUSTMENT')),
    Quantity INT NOT NULL,
    MovementDate DATETIME DEFAULT GETDATE(),
    Description NVARCHAR(255) NULL
);

-- Create Users Table
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) CHECK (Role IN ('Admin', 'Manager', 'Staff')),
    CreatedAt DATETIME DEFAULT GETDATE()
);



-- Create AuditLogs Table
CREATE TABLE AuditLogs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    Action NVARCHAR(100) NOT NULL,
    TableAffected NVARCHAR(50) NULL,
    ActionTime DATETIME DEFAULT GETDATE(),
    Description NVARCHAR(255) NULL
);

-- Create Categories Table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) UNIQUE NOT NULL,
    Description NVARCHAR(255) NULL
);


-- Insert data into Categories
INSERT INTO Categories (CategoryName, Description)
VALUES 
('Electronics', 'Devices and gadgets'),
('Furniture', 'Home and office furniture'),
('Clothing', 'Apparel and accessories');



-- Insert Data into Categories
INSERT INTO Categories (CategoryName, Description)
VALUES 
('Electronics', 'Devices and gadgets'),
('Furniture', 'Home and office furniture'),
('Clothing', 'Apparel and accessories');

-- Insert Data into Products
INSERT INTO Products (Name, SKU, Category, Quantity, UnitPrice, Barcode)
VALUES 
('Product A', 'SKU001', 'Electronics', 100, 19.99, '1234567890123'),
('Product B', 'SKU002', 'Furniture', 50, 99.99, '1234567890124'),
('Product C', 'SKU003', 'Clothing', 200, 29.99, '1234567890125');

-- Insert Data into Suppliers
INSERT INTO Suppliers (SupplierName, ContactName, Phone, Email, Address)
VALUES 
('Supplier One', 'Alice Johnson', '123-456-7890', 'alice@supplierone.com', '123 Main St, City'),
('Supplier Two', 'Bob Smith', '234-567-8901', 'bob@suppliertwo.com', '456 Elm St, City');

-- Insert Data into PurchaseOrders
INSERT INTO PurchaseOrders (SupplierID, OrderDate, Status, TotalAmount)
VALUES 
(1, '2024-12-15', 'Pending', 1999.50),
(2, '2024-12-16', 'Completed', 4999.99);

-- Insert Data into PurchaseOrderDetails
INSERT INTO PurchaseOrderDetails (PurchaseOrderID, ProductID, Quantity, UnitPrice)
VALUES 
(1, 1, 100, 19.99),
(1, 2, 20, 99.99),
(2, 3, 200, 29.99);

-- Insert Data into SalesOrders
INSERT INTO SalesOrders (CustomerName, OrderDate, Status, TotalAmount)
VALUES 
('John Doe', '2024-12-17', 'Pending', 59.98),
('Jane Smith', '2024-12-18', 'Shipped', 299.97);

-- Insert Data into SalesOrderDetails
INSERT INTO SalesOrderDetails (SalesOrderID, ProductID, Quantity, UnitPrice)
VALUES 
(1, 1, 2, 19.99),
(2, 3, 3, 29.99);

-- Insert Data into StockMovements
INSERT INTO StockMovements (ProductID, MovementType, Quantity, MovementDate, Description)
VALUES 
(1, 'IN', 100, '2024-12-15', 'Initial stock'),
(2, 'IN', 20, '2024-12-15', 'Initial stock'),
(3, 'IN', 200, '2024-12-16', 'Initial stock'),
(1, 'OUT', 2, '2024-12-17', 'Sold to customer');

-- Insert Data into Users
INSERT INTO Users (Username, PasswordHash, Role)
VALUES 
('admin', 'hashed_password_1', 'Admin'),
('manager', 'hashed_password_2', 'Manager'),
('staff', 'hashed_password_3', 'Staff');

-- Insert Data into AuditLogs
INSERT INTO AuditLogs (UserID, Action, TableAffected, ActionTime, Description)
VALUES 
(1, 'INSERT', 'Products', '2024-12-15', 'Added initial stock'),
(1, 'UPDATE', 'Products', '2024-12-17', 'Updated stock levels after sale');



SELECT * FROM Categories;
SELECT * FROM Products;
SELECT * FROM Suppliers;
SELECT * FROM PurchaseOrders;
SELECT * FROM PurchaseOrderDetails;
SELECT * FROM SalesOrders;
SELECT * FROM SalesOrderDetails;
SELECT * FROM StockMovements;
SELECT * FROM Users;
SELECT * FROM AuditLogs;
