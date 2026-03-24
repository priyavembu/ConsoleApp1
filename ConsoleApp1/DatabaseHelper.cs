using Microsoft.Data.Sqlite;

public static class DatabaseHelper
{
    public static void InitializeDatabase(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        string sql = @"

CREATE TABLE IF NOT EXISTS Products(
ProductId TEXT PRIMARY KEY,
ProductName TEXT,
Category TEXT,
UnitPrice REAL
);

CREATE TABLE IF NOT EXISTS Customers(
CustomerID TEXT PRIMARY KEY,
CustomerName TEXT,
CustomerEmail TEXT,
CustomerAddress TEXT,
Region TEXT
);

CREATE TABLE IF NOT EXISTS Orders(
OrderID INTEGER PRIMARY KEY,
CustomerID TEXT,
DateOfSale TEXT,
PaymentMethod TEXT
);

CREATE TABLE IF NOT EXISTS OrderItems(
OrderItemID INTEGER PRIMARY KEY AUTOINCREMENT,
OrderID INTEGER,
ProductId TEXT,
QuantitySold INTEGER,
Discount TEXT,
ShippingCost INTEGER
);
";

        using var cmd = new SqliteCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }
}