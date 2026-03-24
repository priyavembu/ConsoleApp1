using CsvHelper;
using Microsoft.Data.Sqlite;
using System.Globalization;

public static class CsvLoader
{
    public static void Import(string csvPath, string connectionString)
    {
        var records = ReadCsv(csvPath);

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        foreach (var r in records)
        {
            if (string.IsNullOrEmpty(r.ProductId) || r.UnitPrice < 0)
                continue;

            InsertProduct(connection, transaction, r);
            InsertCustomer(connection, transaction, r);
            InsertOrder(connection, transaction, r);
            InsertOrderItem(connection, transaction, r);
        }

        transaction.Commit();
        Console.WriteLine("CSV data imported successfully.");
    }

    private static List<ProductRecord> ReadCsv(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<ProductRecord>().ToList();
    }

    private static void InsertProduct(SqliteConnection conn, SqliteTransaction tran, ProductRecord r)
    {
        var cmd = new SqliteCommand(
        @"INSERT INTO Products(ProductId,ProductName,Category,UnitPrice)
          VALUES(@id,@name,@cat,@price)", conn, tran);

        cmd.Parameters.AddWithValue("@id", r.ProductId);
        cmd.Parameters.AddWithValue("@name", r.ProductName);
        cmd.Parameters.AddWithValue("@cat", r.Category);
        cmd.Parameters.AddWithValue("@price", r.UnitPrice);

        cmd.ExecuteNonQuery();
    }

    private static void InsertCustomer(SqliteConnection conn, SqliteTransaction tran, ProductRecord r)
    {
        var cmd = new SqliteCommand(
        @"INSERT OR IGNORE INTO Customers(CustomerID,CustomerName,CustomerEmail,CustomerAddress,Region)
          VALUES(@id,@name,@email,@addr,@region)", conn, tran);

        cmd.Parameters.AddWithValue("@id", r.CustomerID);
        cmd.Parameters.AddWithValue("@name", r.CustomerName);
        cmd.Parameters.AddWithValue("@email", r.CustomerEmail);
        cmd.Parameters.AddWithValue("@addr", r.CustomerAddress);
        cmd.Parameters.AddWithValue("@region", r.Region);

        cmd.ExecuteNonQuery();
    }

    private static void InsertOrder(SqliteConnection conn, SqliteTransaction tran, ProductRecord r)
    {
        var cmd = new SqliteCommand(
        @"INSERT OR IGNORE INTO Orders(OrderID,CustomerID,DateOfSale,PaymentMethod)
          VALUES(@oid,@cid,@date,@pay)", conn, tran);

        cmd.Parameters.AddWithValue("@oid", r.OrderID);
        cmd.Parameters.AddWithValue("@cid", r.CustomerID);
        cmd.Parameters.AddWithValue("@date", r.DateofSale);
        cmd.Parameters.AddWithValue("@pay", r.PaymentMethod);

        cmd.ExecuteNonQuery();
    }

    private static void InsertOrderItem(SqliteConnection conn, SqliteTransaction tran, ProductRecord r)
    {
        var cmd = new SqliteCommand(
        @"INSERT INTO OrderItems(OrderID,ProductId,QuantitySold,Discount,ShippingCost)
          VALUES(@oid,@pid,@qty,@disc,@ship)", conn, tran);

        cmd.Parameters.AddWithValue("@oid", r.OrderID);
        cmd.Parameters.AddWithValue("@pid", r.ProductId);
        cmd.Parameters.AddWithValue("@qty", r.QuantitySold);
        cmd.Parameters.AddWithValue("@disc", r.Discount);
        cmd.Parameters.AddWithValue("@ship", r.ShippingCost);

        cmd.ExecuteNonQuery();
    }
}