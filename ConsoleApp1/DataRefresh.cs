using Microsoft.Data.Sqlite;

public class DataRefresh
{
    private readonly string _csvPath;
    private readonly string _connectionString;

    public DataRefresh(string csvPath, string connectionString)
    {
        _csvPath = csvPath;
        _connectionString = connectionString;
    }

    public void RefreshData()
    {
        try
        {
            Console.WriteLine("Data refresh started");

            CsvLoader.Import(_csvPath, _connectionString);

            Console.WriteLine("Data refresh completed successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Data refresh failed: " + ex.Message);
        }
    }
}