using Newtonsoft.Json;
using System.Text;

namespace VehiclePermit.web.Extensions
{
    /// <summary>
    /// Session 擴充
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// 設定儲存 Session
        /// </summary>
        /// <param name="session">session 本體不用特別設定</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetObjectAsJson( this ISession session, string key, object? value )
        {
            value ??= string.Empty;
            session.Set( key, Encoding.UTF8.GetBytes( JsonConvert.SerializeObject( value ) ) );
        }

        /// <summary>
        /// 取得 Session
        /// </summary>
        /// <typeparam name="T">取得出來的模型</typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T? GetObjectFromJson<T>( this ISession session, string key )
        {
            if( session.TryGetValue(key, out byte[] bytes ) )
            {
                return JsonConvert.DeserializeObject<T>( Encoding.UTF8.GetString( bytes ) );
            }
            return default;
        }
    }
}
