Select Customers.*, Addresss.StreetName,PostalCode,City,Country
From Customers
INNER JOIN Customer_Addresses ON Customer_Addresses.CustomerId = Customers.Id
INNER JOIN Addresss ON Customer_Addresses.AddressId = Addresss.Id

Select * From Addresss

Select * From Customers


