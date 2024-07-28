using ShoppingBackstage.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingBackstage.BackstageService.Interface;

namespace ShoppingBackstage.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {

        private readonly IMenuService _service;

        public MenuViewComponent(IMenuService service)
        {
            _service = service;
        }

        public IViewComponentResult Invoke()
        {
            var menu = _service.GetMenu();
            return View(menu);
        }
    }
}
