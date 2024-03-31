using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBackstage.Models;

namespace ShoppingBackstage.Controllers
{
    public class BaseController : Controller
    {

        protected const int PageSize = 15;

        protected ClaimsPrincipalExtension CurrentUser => new ClaimsPrincipalExtension(User);

        protected string ClientIp
        {
            get
            {
                if (Request.HttpContext.Connection.RemoteIpAddress == null)
                {
                    return !string.IsNullOrEmpty(Request.Headers["HTTP_X_FORWARDED_FOR"])
                            ? Request.Headers["HTTP_X_FORWARDED_FOR"]
                            : Request.Headers["REMOTE_ADDR"];
                }

                return Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }
    }
}