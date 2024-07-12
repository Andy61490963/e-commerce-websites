using Shopping.lib.Helpers;
using Microsoft.AspNetCore.Mvc;
using VehiclePermit.web.Extensions;

namespace ShoppingBackstage.Extensions
{
    /// <summary>
    /// controller 擴充
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// 設定 Session
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public static void SetSession( this Controller controller, string key, object? data )
        {
            controller.HttpContext.Session.SetObjectAsJson( key, data );
        }

        /// <summary>
        /// 讀取 Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T? GetSession<T>( this Controller controller, string key )
        {
            return controller.HttpContext.Session.GetObjectFromJson<T>( key );
        }

        /// <summary>
        /// 移除 Session
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        public static void RemoveSession( this Controller controller, string key )
        {
            controller.HttpContext.Session.Remove( key );
        }

        /// <summary>
        /// 重設 Session
        /// </summary>
        /// <param name="controller"></param>
        public static void ResetSession( this Controller controller )
        {
            controller.HttpContext.Session.Clear();
        }

        /// <summary>
        /// 設定 Cookie
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expireDay"></param>
        public static void SetCookie( this Controller controller, string key, object? data, DateTime? expireDay = null )
        {
            controller.HttpContext.Response.Cookies.SetObjectAsJson( key, data, expireDay );
        }

        /// <summary>
        /// 讀取 Cookie
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T? GetCookie<T>( this Controller controller, string key )
        {
            return controller.HttpContext.Request.Cookies.GetObjectFromJson<T>( key );
        }

        /// <summary>
        /// 移除 Cookie
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        public static void RemoveCookie( this Controller controller, string key )
        {
            controller.HttpContext.Response.Cookies.Delete( key );
        }

        /// <summary>
        /// 生成驗證碼
        /// </summary>
        public static void GenerateCaptcha( this Controller controller )
        {
            var captcha = CaptchaHelper.Generate( noiseType: CaptchaHelper.NoiseType.Point );
            controller.ViewData[ CaptchaHelper.Key ] = captcha;
            controller.SetCookie( CaptchaHelper.Key, captcha, DateTime.Now.AddMinutes( 5 ) );
        }
    }
}
