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

            if(!_database.TableExists(tableName))
            {
                string[] columnNames = { "Id INT IDENTITY PRIMARY KEY", "Name VARCHAR(50)", "Email VARCHAR(80)" };
                _database.AddTable(tableName, columnNames);
            }

            _database.CloseConnection();
        }

        public IActionResult GetAllData(string tableName = "TestTable")
        {
            try
            {
                _database.OpenConnection();

                var data = _database.GetAllData(tableName);

                _database.CloseConnection();
                return Json(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AddContact(string name, string email, string tableName = "TestTable")
        {
            _database.OpenConnection();
            string[] rowValues = { name, email };
            _database.AddData(tableName, rowValues);

            Debug.WriteLine("\n\n" + name + " " + email + "\n\n");

            _database.CloseConnection();
            return Json(new { success = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}