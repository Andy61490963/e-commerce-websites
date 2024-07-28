using Microsoft.AspNetCore.Mvc;
using Shopping.Service.Interface;

namespace Shopping.ViewComponents
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
            var menu = _service.GetMenu().ToList();
            return View(menu);
        }
    }
}
