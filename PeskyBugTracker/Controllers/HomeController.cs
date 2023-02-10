using Microsoft.AspNetCore.Mvc;
using PeskyBugTracker.Data;
using PeskyBugTracker.Models;
using System.Diagnostics;

namespace PeskyBugTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PeskyBugTrackerContext _context;

        public HomeController(ILogger<HomeController> logger, PeskyBugTrackerContext context)
        {
            _logger = logger;
            _context = context;
        }

        

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AuthenticateModel objUser)
        {

            if(ModelState.IsValid)
            {
                var objAuthUser= _context.Agents.Where(u => u.UserName == objUser.Username && u.Password == objUser.Password).FirstOrDefault();
                if(objAuthUser != null)
                {
                    HttpContext.Session.SetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID, objAuthUser.Id.ToString());

                    return this.RedirectToAction("Index","PeskyBugs");

                }
            }
            return View(objUser);
        }

        [HttpGet]
        public IActionResult Logout()
        {

            if (ModelState.IsValid)
            {
                
                
                    HttpContext.Session.Clear();
                
            }
            return View("Login");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ApiTest()
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