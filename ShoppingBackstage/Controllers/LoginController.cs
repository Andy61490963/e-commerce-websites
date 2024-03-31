using Microsoft.AspNetCore.Mvc;
using ShoppingBackstage.ViewModel;
using ShoppingBackstage.BackstageService.Interface;
using SA.admin.ViewModel;

// 登入
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;



namespace ShoppingBackstage.Controllers
{

    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.account))
                {
                    var user = _userService.GetUser(model.account, model.password);
                    if (user != null)
                    {
                        return await LoginAction(user);
                    }
                    ViewData["errorMessage"] = "帳號或密碼錯誤，請重新輸入";
                }
            }
            else
            {
                ViewData["errorMessage"] = "輸入資料格式錯誤";
            }

            return PartialView(model);

        }

        public async Task<ActionResult> LoginAction(a0001_adminAccount model)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim( ClaimTypes.NameIdentifier, model.id.ToString() ),
                    new Claim( ClaimTypes.Name, model.username ),
                    new Claim( "account", model.account ),
                    new Claim( "phone", model.phone.ToString() ),
                    new Claim( "email", model.email ),

                }, CookieAuthenticationDefaults.AuthenticationScheme);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();
       
            return RedirectToAction("Index", "Login");
        }

    }
}
