using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace NET_MVC_Environment.Controllers
{
    public class IndexController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddContact(string name, string email)
        {
            // TODO: Add code to store the name and email address
            Debug.WriteLine("\n\n" + name + " " + email + "\n\n");

            return Json(new { success = true });
        }
    }
}
