using Microsoft.AspNetCore.Mvc;
using NET_MVC_Environment.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace NET_MVC_Environment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private Database _database;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _database = new Database("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"E:\\NOT_WINDOWS\\GITHUB\\DEVELOPERAPPLICATIONTESTS\\SWAARM GRADUATE FULL STACK DEVELOPER TEST\\NET MVC ENVIRONMENT\\SWAARMTESTDB.MDF\";" +
                "Integrated Security=True;" +
                "Connect Timeout=30;" +
                "Encrypt=False;" +
                "TrustServerCertificate=False;" +
                "ApplicationIntent=ReadWrite;" +
                "MultiSubnetFailover=False");
        }

        public IActionResult Index()
        {
            TemporaryTable();

            return View();
        }

        public void TemporaryTable()
        {
            _database.OpenConnection();

            string tableName = "TestTable";

            if (_database.TableExists(tableName))
            {
                _database.RemoveTable(tableName);
            }

            string[] columnNames = { "Id INT PRIMARY KEY", "Name VARCHAR(50)", "Email VARCHAR(80)" };
            _database.AddTable(tableName, columnNames);

            string[] rowValues = { "1", "'John Doe'", "'john.doe@example.com'" };
            _database.AddData(tableName, rowValues);

            DataRow row = _database.SelectRow(tableName, "Id = 1");
            Debug.WriteLine("\n\n" + row["Name"] + " " + row["Email"] + "\n\n");

            _database.RemoveData(tableName, "Id = 1");

            _database.RemoveTable(tableName);
            _database.CloseConnection();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}