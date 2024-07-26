using Microsoft.AspNetCore.Mvc;
using Shopping.Models;
using System.Diagnostics;
using Shopping.ViewModel;
using Shopping.Service.Interface;

namespace Shopping.Controllers
{
    //[Area("Shopping")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashBoardService _dashBoardService;
        
        public HomeController(ILogger<HomeController> logger, IDashBoardService dashboard)
        {
            _logger = logger;
            _dashBoardService = dashboard;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                BannerViewModel = _dashBoardService.GetBanner(),
                AboutViewModel = _dashBoardService.GetAbout(),
            };

            return View( model );
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
