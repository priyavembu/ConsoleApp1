using CsvHelper;
using Microsoft.Data.Sqlite;
using Raven.Database.Util;

class Program
{
    static void Main()
    {
        string csvPath = @"C:\Users\priya\OneDrive\Pictures\Documents\testdemo.csv";
        string dbPath = @"C:\Users\priya\Test.db";

        string connectionString = $"Data Source={dbPath}";

        DatabaseHelper.InitializeDatabase(connectionString);

        CsvLoader.Import(csvPath, connectionString);

        Console.WriteLine("Process Completed.");
        string query = @"
SELECT SUM(p.UnitPrice * oi.QuantitySold)
FROM Products p
JOIN OrderItems oi ON p.ProductId = oi.ProductId
JOIN Orders o ON o.OrderID = oi.OrderID
WHERE o.DateOfSale BETWEEN @start AND @end"; 
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var starDate = DateTime.Now.AddDays(-24);
        var endDate = DateTime.Now;
        using (var command = new SqliteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@start", starDate);
            command.Parameters.AddWithValue("@end", endDate);

            var result = command.ExecuteScalar();
            Console.WriteLine("Total Revenue: " + result);
        }

        
    }
    
}