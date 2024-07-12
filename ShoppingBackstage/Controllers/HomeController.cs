using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBackstage.Models;
using System.Diagnostics;

namespace ShoppingBackstage.Controllers
{
    [Authorize]
    [Route("Backstage/[Action]")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var test = CurrentUser.Name;
            var test1 = CurrentUser.Id;
            var test2 = CurrentUser.account;
            var test3 = CurrentUser.phone;
            var test4 = CurrentUser.email;
            return View();
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
