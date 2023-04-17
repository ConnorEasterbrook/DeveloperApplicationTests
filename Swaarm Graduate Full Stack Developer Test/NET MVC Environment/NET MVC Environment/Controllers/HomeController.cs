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
        public List<Data> dataClass = new List<Data>();

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
                string[] columnNames = { "Id INT IDENTITY PRIMARY KEY", "Name VARCHAR(50)", "CreatedDate DATETIME DEFAULT (GETDATE())", "LastUpdatedDate DATETIME DEFAULT (GETDATE())" };
                _database.AddTable(tableName, columnNames);
            }

            dataClass = _database.GetDataForList(tableName);

            _database.CloseConnection();
        }

        public IActionResult SetSort(bool descending, int elementID)
        {
            _database.SetVariables(descending, elementID);
            return Json(new { success = true });
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
        public ActionResult CreateData(string name, string date, string tableName = "TestTable")
        {
            _database.OpenConnection();
            _database.AddData(tableName, name, date);
            _database.CloseConnection();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult UpdateData(string oldName, string name, string date, string tableName = "TestTable")
        {
            _database.OpenConnection();
            _database.UpdateData(tableName, oldName, name, date);
            _database.CloseConnection();
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult DeleteData(string name, string tableName = "TestTable")
        {
            _database.OpenConnection();
            _database.RemoveData(tableName, name);
            _database.CloseConnection();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult ClearTable()
        {
            _database.OpenConnection();

            string tableName = "TestTable";
            _database.RemoveData(tableName);

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