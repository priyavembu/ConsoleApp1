using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace SalesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AnalysisController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("totalrevenue")]
        public IActionResult GetTotalRevenue(string startDate, string endDate)
        {
            double revenue = 0;

            string connectionString = _config.GetConnectionString("DefaultConnection");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT SUM(p.UnitPrice * oi.QuantitySold)
                FROM Products p
                JOIN OrderItems oi ON p.ProductId = oi.ProductId
                JOIN Orders o ON o.OrderID = oi.OrderID
                WHERE o.DateOfSale BETWEEN @start AND @end";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@start", startDate);
                    command.Parameters.AddWithValue("@end", endDate);

                    var result = command.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        revenue = Convert.ToDouble(result);
                    }
                }
            }

            return Ok(new
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = revenue
            });
        }
        [HttpGet("totalrevenuebyproduct")]
        public IActionResult GetTotalRevenueByproduct(string startDate, string endDate)
        {
            double revenue = 0;

            string connectionString = _config.GetConnectionString("DefaultConnection");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = @"
                            SELECT p.ProductName,
            SUM(p.UnitPrice * oi.QuantitySold) AS Revenue
            FROM Products p
            JOIN OrderItems oi ON p.ProductId = oi.ProductId
            JOIN Orders o ON o.OrderID = oi.OrderID
            WHERE o.DateOfSale BETWEEN @start AND @end
            GROUP BY p.ProductName;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@start", startDate);
                    command.Parameters.AddWithValue("@end", endDate);

                    var result = command.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        revenue = Convert.ToDouble(result);
                    }
                }
            }

            return Ok(new
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = revenue
            });
        }
        [HttpGet("totalrevenuebycategory")]
        public IActionResult GetTotalRevenueByCategory(string startDate, string endDate)
        {
            double revenue = 0;

            string connectionString = _config.GetConnectionString("DefaultConnection");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = @"
        SELECT p.Category,
SUM(p.UnitPrice * oi.QuantitySold) AS Revenue
FROM Products p
JOIN OrderItems oi ON p.ProductId = oi.ProductId
JOIN Orders o ON o.OrderID = oi.OrderID
WHERE o.DateOfSale BETWEEN @start AND @end
GROUP BY p.Category;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@start", startDate);
                    command.Parameters.AddWithValue("@end", endDate);

                    var result = command.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        revenue = Convert.ToDouble(result);
                    }
                }
            }

            return Ok(new
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = revenue
            });
        }
    }
}