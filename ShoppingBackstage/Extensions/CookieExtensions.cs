using Shopping.lib.Helpers;
using Newtonsoft.Json;

namespace ShoppingBackstage.Extensions
{
    /// <summary>
    /// Cookie 擴充
    /// </summary>
    public static class CookieExtensions
    {
        /// <summary>
        /// 設定儲存 Cookie
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireDay"></param>
        public static void SetObjectAsJson( this IResponseCookies cookies, string key, object? value, DateTime? expireDay = null )
        {
            value ??= string.Empty;
            expireDay ??= DateTime.Now.AddHours( 1 );
            var json = JsonConvert.SerializeObject( value );
            cookies.Append( key, json.AesEncrypt(), new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                Expires = expireDay,
                HttpOnly = true,
                Path = "/"
            } );
        }

        /// <summary>
        /// 取得 Cookie
        /// </summary>
        /// <typeparam name="T">取得出來的模型</typeparam>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T? GetObjectFromJson<T>( this IRequestCookieCollection cookies, string key )
        {
            if( cookies.TryGetValue( key, out string? encrypt ) )
            {
                if(!string.IsNullOrWhiteSpace( encrypt ) )
                {
                    return JsonConvert.DeserializeObject<T>( encrypt.AesDecrypt() );
                }
            }
            return default;
        }
    }
}
