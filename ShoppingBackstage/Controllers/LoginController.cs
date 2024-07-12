using Microsoft.AspNetCore.Mvc;
using ShoppingBackstage.ViewModel;
using ShoppingBackstage.BackstageService.Interface;

// 登入
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Shopping.lib.Helpers;
using ShoppingBackstage.Extensions;


namespace ShoppingBackstage.Controllers
{

    public class LoginController : BaseController
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet]
        public IActionResult Index()
        { 
            var model = new LoginViewModel();
            this.GenerateCaptcha();
            return View( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model)
        {
            var captcha = this.GetCookie<CaptchaHelper.CaptchaResult>( CaptchaHelper.Key );
            if( captcha is null )
            {
                ViewData[ "ErrorMessage" ] = "請重新輸入驗證碼";
            }
            if( captcha != null && !string.IsNullOrWhiteSpace( model.Captcha ) && !captcha.Captcha.Equals( model.Captcha ) )
            {
                ModelState.AddModelError( nameof( model.Captcha ), "驗證碼錯誤" );
                // 重新生成驗證碼
                this.GenerateCaptcha();
                return PartialView(model);
            }
            
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
            // 重新生成驗證碼
            this.GenerateCaptcha();
            return PartialView(model);
        }
        
        public async Task<ActionResult> LoginAction(a0001_adminAccount model)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim( ClaimTypes.NameIdentifier, model.id_.ToString() ),
                    new Claim( ClaimTypes.Name, model.username_ ),
                    new Claim( "account", model.account_ ),
                    new Claim( ClaimTypes.MobilePhone, model.phone_.ToString() ),
                    new Claim( ClaimTypes.Email, model.email_ ),

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

        /// <summary>
        /// 重置驗證碼
        /// </summary>
        /// <returns></returns>
        public IActionResult ResetCaptcha()
        {
            this.GenerateCaptcha();
            return PartialView( "_Captcha" );
        }
        
    }
}
