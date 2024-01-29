DROP TABLE Orderrows, Orders, Categories, PriceLists, Products



CREATE TABLE [Products] (
  [ArticleNumber] nvarchar(450) PRIMARY KEY NOT NULL,
  [Title] nvarchar(50) NOT NULL,
  [Ingress] nvarchar(200),
  [Description] nvarchar(max),
  [PriceId] int NOT NULL,
  [Unit] nvarchar(30) NOT NULL,
  [Stock] int NOT NULL,
  [CategoryName] nvarchar(100) NOT NULL
)
GO

CREATE TABLE [PriceLists] (
  [Id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [Price] money NOT NULL,
  [DiscountPrice] money,
  [UnitType] nvarchar(20) NOT NULL
)
GO

CREATE TABLE [Categories] (
  [Id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [CategoryName] nvarchar(100) UNIQUE NOT NULL
)
GO

CREATE TABLE [Orders] (
  [Id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [OrderDate] datetime2,
  [OrderPrice] money NOT NULL,
  [CustomerId] nvarchar(450) NOT NULL,
)
GO

CREATE TABLE [OrderRows] (
  [Id] int NOT NULL IDENTITY(1, 1),
  [Quantity] int NOT NULL,
  [OrderRowPrice] money NOT NULL,
  [ArticleNumber] nvarchar(450) UNIQUE NOT NULL,
  [OrderId] int NOT NULL,
  PRIMARY KEY ([Id], [OrderId])
)
GO


ALTER TABLE [Products] ADD FOREIGN KEY ([PriceId]) REFERENCES [PriceLists] ([Id])
GO

ALTER TABLE [Products] ADD FOREIGN KEY ([CategoryName]) REFERENCES [Categories] ([CategoryName])
GO

ALTER TABLE [OrderRows] ADD FOREIGN KEY ([ArticleNumber]) REFERENCES [Products] ([ArticleNumber]) ON DELETE CASCADE
GO

ALTER TABLE [OrderRows] ADD FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
GO

--SELECT name, compatibility_level
--FROM sys.databases
--WHERE name LIKE 'D:\%NEW_DATASTORAGEAPP_DB%' OR name LIKE 'D:\%NEW_ORDERINGSYSTEM_DB%';



