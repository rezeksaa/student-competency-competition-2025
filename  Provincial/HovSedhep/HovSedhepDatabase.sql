USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'HovSedhepDatabase')
BEGIN
  DROP DATABASE HovSedhepDatabase;
END;
GO

CREATE DATABASE HovSedhepDatabase;
GO
USE HovSedhepDatabase;
GO

-- Employee Table
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Role] NVARCHAR(50) NOT NULL CHECK ([Role] IN ('Waitress', 'Cashier', 'Chef')),
    Phone NVARCHAR(20) UNIQUE,
    Email NVARCHAR(100) UNIQUE,
    HireDate DATE NOT NULL
);

-- Restaurant Tables Table
CREATE TABLE RestaurantTables (
    TableID INT IDENTITY(1,1) PRIMARY KEY,
    [Name] VARCHAR(10) UNIQUE NOT NULL,
    Capacity INT CHECK (Capacity IN (2, 4, 6)) NOT NULL
);

-- Categories Table
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL UNIQUE
);

-- Menu Items Table
CREATE TABLE MenuItems (
    MenuItemID INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL CHECK (Price > 0),
    [Description] NVARCHAR(255),
    CategoryID INT NOT NULL,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Transactions Table (One transaction per table per session)
CREATE TABLE Transactions (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY,
    TableID INT NOT NULL,
    CustomerName NVARCHAR(100) NOT NULL,
    TransactionDate DATETIME DEFAULT GETDATE(),
    [Status] NVARCHAR(20) CHECK ([Status] IN ('Ongoing', 'Completed', 'Cancelled')) DEFAULT 'Ongoing',
    FOREIGN KEY (TableID) REFERENCES RestaurantTables(TableID) ON DELETE CASCADE
);

-- Orders Table (Each transaction can have multiple orders)
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    TransactionID INT NOT NULL,
    EmployeeID INT NOT NULL,  -- The waiter who entered the order
    OrderTime DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TransactionID) REFERENCES Transactions(TransactionID) ON DELETE CASCADE,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE CASCADE
);

-- Order Details Table (Each order can have multiple menu items ordered)
CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    MenuItemID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    Price DECIMAL(10,2) NOT NULL CHECK (Price > 0),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (MenuItemID) REFERENCES MenuItems(MenuItemID)
);


SET IDENTITY_INSERT Employees ON;
INSERT INTO Employees (EmployeeID, [Name], [Role], Phone, Email, HireDate) VALUES
(1, 'Nuni Santika', 'Waitress', '081234567890', 'budi.santoso@gmail.com', '2024-01-10'),
(2, 'Siti Aminah', 'Waitress', '081298765432', 'siti.aminah@yahoo.com', '2023-12-15'),
(3, 'Dewi Kusuma', 'Cashier', '082134567891', 'dewi.kusuma@outlook.com', '2023-11-20'),
(4, 'Rizky Hidayat', 'Waitress', '081312345678', 'rizky.hidayat@gmail.com', '2024-02-01'),
(5, 'Ahmad Fauzi', 'Chef', '085678901234', 'ahmad.fauzi@yahoo.com', '2022-09-25'),
(6, 'Lisa Permata', 'Waitress', '081423456789', 'lisa.permata@gmail.com', '2023-10-05'),
(7, 'Tono Subagio', 'Chef', '087654321098', 'tono.subagio@hotmail.com', '2021-07-14'),
(8, 'Farah Wijaya', 'Waitress', '081556677889', 'farah.wijaya@yahoo.com', '2024-03-12'),
(9, 'Guntur Pratama', 'Cashier', '082233445566', 'guntur.pratama@gmail.com', '2023-08-30'),
(10, 'Yusuf Aditya', 'Chef', '081998877665', 'yusuf.aditya@outlook.com', '2020-05-18');
SET IDENTITY_INSERT Employees OFF;


SET IDENTITY_INSERT RestaurantTables ON;
INSERT INTO RestaurantTables (TableID, [Name], Capacity) VALUES
(1, 'A1', 2),
(2, 'A2', 2),
(3, 'A3', 2),
(4, 'A4', 2),
(5, 'B1', 4),
(6, 'B2', 4),
(7, 'C1', 6),
(8, 'C2', 6);
SET IDENTITY_INSERT RestaurantTables OFF;


SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (CategoryID, [Name]) VALUES
(1, 'Rice Dishes'),
(2, 'Noodles'),
(3, 'Soups & Stews'),
(4, 'Grilled & Fried'),
(5, 'Side Dishes'),
(6, 'Hot Drinks'),
(7, 'Cold Drinks'),
(8, 'Traditional Beverages');
SET IDENTITY_INSERT Categories OFF;


SET IDENTITY_INSERT MenuItems ON;
INSERT INTO MenuItems (MenuItemID, [Name], Price, [Description], CategoryID) VALUES
(1, 'Nasi Goreng Spesial', 35000, 'Indonesian-style fried rice with egg, chicken, and shrimp', 1),
(2, 'Nasi Uduk Komplit', 30000, 'Coconut rice with fried chicken, omelette, and tempeh', 1),
(3, 'Nasi Padang Rendang', 40000, 'Steamed rice with spicy beef rendang', 1),
(4, 'Nasi Liwet Ayam Suwir', 32000, 'Rice cooked in coconut milk, served with shredded chicken', 1),
(5, 'Mie Goreng Jawa', 28000, 'Javanese-style fried noodles with chicken and vegetables', 2),
(6, 'Bakmi Ayam Pangsit', 30000, 'Egg noodles with seasoned chicken and wontons', 2),
(7, 'Kwetiau Goreng Seafood', 38000, 'Stir-fried flat noodles with seafood and soy sauce', 2),
(8, 'Bihun Goreng Sapi', 33000, 'Stir-fried vermicelli with beef and vegetables', 2),
(9, 'Soto Ayam Lamongan', 27000, 'Yellow chicken soup with shredded chicken and rice cakes', 3),
(10, 'Rawon Daging Sapi', 40000, 'Black beef soup with keluak seasoning', 3),
(11, 'Sayur Asem Betawi', 22000, 'Tamarind vegetable soup with peanuts and corn', 3),
(12, 'Gulai Kambing', 45000, 'Rich lamb curry with coconut milk', 3),
(13, 'Ayam Goreng Kremes', 32000, 'Crispy fried chicken with crunchy batter flakes', 4),
(14, 'Ikan Bakar Jimbaran', 50000, 'Grilled fish with Balinese spices', 4),
(15, 'Bebek Goreng Sambal Matah', 45000, 'Fried duck with Balinese sambal', 4),
(16, 'Tahu Tempe Bacem', 20000, 'Sweet marinated tofu and tempeh', 4),
(17, 'Tempe Mendoan', 18000, 'Crispy fried tempeh with soy dipping sauce', 5),
(18, 'Tahu Isi Pedas', 20000, 'Stuffed spicy tofu fritters', 5),
(19, 'Perkedel Kentang', 15000, 'Fried mashed potato patties', 5),
(20, 'Bakwan Jagung', 15000, 'Corn fritters with flour batter', 5),
(21, 'Teh Manis Panas', 10000, 'Hot sweet tea', 6),
(22, 'Kopi Tubruk Jawa', 15000, 'Javanese-style strong black coffee', 6),
(23, 'Wedang Jahe', 12000, 'Hot ginger tea with palm sugar', 6),
(24, 'Es Teh Manis', 12000, 'Iced sweet tea', 7),
(25, 'Es Jeruk Nipis', 15000, 'Fresh squeezed lime juice with ice', 7),
(26, 'Es Cincau Segar', 18000, 'Refreshing grass jelly drink with palm sugar', 7),
(27, 'Bajigur Bandung', 18000, 'Hot coconut milk drink with ginger', 8),
(28, 'Bir Pletok Betawi', 22000, 'Betawi herbal drink with spices', 8),
(29, 'Es Doger', 20000, 'Pink coconut milk dessert with sticky rice', 8),
(30, 'Air Mineral', 8000, 'Bottled mineral water', 8);
SET IDENTITY_INSERT MenuItems OFF;


SET IDENTITY_INSERT Transactions ON;
INSERT INTO Transactions (TransactionID, TableID, CustomerName, TransactionDate, [Status]) VALUES
-- Transactions
(1, 5, 'David Lee', '2025-04-20 11:20:00', 'Completed'),
(2, 7, 'Michael Brown', '2025-04-20 11:57:00', 'Completed'),
(3, 5, 'Sophia Martinez', '2025-04-20 14:33:00', 'Completed'),
(4, 2, 'Sarah Johnson', '2025-04-20 15:40:00', 'Completed'),
(5, 4, 'Alice Smith', '2025-04-20 16:22:00', 'Completed'),
(6, 1, 'Michael Brown', '2025-04-20 17:51:00', 'Completed'),
(7, 5, 'John Doe', '2025-04-20 18:29:00', 'Completed'),
(8, 3, 'Emily Davis', '2025-04-20 12:43:00', 'Completed'),
(9, 7, 'Sophia Martinez', '2025-04-20 12:51:00', 'Completed'),
(10, 6, 'James Wilson', '2025-04-20 19:15:00', 'Cancelled');
SET IDENTITY_INSERT Transactions OFF;


SET IDENTITY_INSERT Orders ON;
INSERT INTO Orders (OrderID, TransactionID, EmployeeID, OrderTime) VALUES
-- Orders for Transaction 1
(1, 1, 8, '2025-04-20 11:25:00'),
-- Orders for Transaction 2
(2, 2, 1, '2025-04-20 12:00:00'),
(3, 2, 4, '2025-04-20 12:10:00'),
-- Orders for Transaction 3
(4, 3, 8, '2025-04-20 14:40:00'),
-- Orders for Transaction 4
(5, 4, 1, '2025-04-20 15:45:00'),
-- Orders for Transaction 5
(6, 5, 1, '2025-04-20 16:30:00'),
(7, 5, 6, '2025-04-20 16:40:00'),
-- Orders for Transaction 6
(8, 6, 4, '2025-04-20 17:55:00'),
-- Orders for Transaction 7
(9, 7, 2, '2025-04-20 18:35:00'),
(10, 7, 2, '2025-04-20 18:45:00'),
-- Orders for Transaction 8
(11, 8, 1, '2025-04-20 12:50:00'),
-- Orders for Transaction 9
(12, 9, 2, '2025-04-20 13:00:00'),
(13, 9, 6, '2025-04-20 13:10:00');
-- Orders for Transaction 10
-- No Orders because it status is Cancelled
SET IDENTITY_INSERT Orders OFF;


SET IDENTITY_INSERT OrderDetails ON;
INSERT INTO OrderDetails (OrderDetailID, OrderID, MenuItemID, Quantity, Price) VALUES
-- Order Details for Transaction 1 (Table Capacity: 4)
(1, 1, 1, 2, 35000),
(2, 1, 6, 2, 30000),
(3, 1, 22, 2, 15000),
(4, 1, 26, 2, 18000),
-- Order Details for Transaction 2 (Table Capacity: 6)
(5, 2, 3, 3, 40000),
(6, 2, 8, 3, 33000),
(7, 2, 23, 3, 12000),
(8, 3, 28, 3, 22000),
-- Order Details for Transaction 3 (Table Capacity: 4)
(9, 4, 2, 2, 30000),
(10, 4, 7, 2, 38000),
(11, 4, 24, 2, 12000),
(12, 4, 27, 2, 18000),
-- Order Details for Transaction 4 (Table Capacity: 2)
(13, 5, 10, 1, 40000),
(14, 5, 12, 1, 45000),
(15, 5, 21, 1, 10000),
(16, 5, 25, 1, 15000),
-- Order Details for Transaction 5 (Table Capacity: 4)
(17, 6, 14, 2, 50000),
(18, 6, 18, 2, 20000),
(19, 6, 29, 2, 20000),
(20, 7, 11, 2, 22000),
-- Order Details for Transaction 6 (Table Capacity: 2)
(21, 8, 5, 1, 28000),
(22, 8, 9, 1, 27000),
(23, 8, 20, 1, 15000),
(24, 8, 30, 1, 8000),
-- Order Details for Transaction 7 (Table Capacity: 4)
(25, 9, 16, 2, 20000),
(26, 9, 13, 2, 32000),
(27, 9, 22, 2, 15000),
(28, 9, 24, 2, 12000),
-- Order Details for Transaction 8 (Table Capacity: 6)
(29, 10, 17, 3, 18000),
(30, 10, 19, 3, 15000),
(31, 10, 26, 3, 18000),
(32, 10, 29, 3, 20000),
-- Order Details for Transaction 9 (Table Capacity: 4)
(33, 12, 4, 3, 32000),
(34, 12, 8, 3, 33000),
(35, 12, 23, 3, 12000),
(36, 13, 6, 2, 30000),
(37, 13, 10, 2, 40000),
(38, 13, 24, 2, 12000),
(39, 13, 27, 2, 18000);
-- Order Details for Transaction 10
-- No Order Details because it status is Cancelled
SET IDENTITY_INSERT OrderDetails OFF;