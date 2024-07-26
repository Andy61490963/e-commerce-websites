namespace Shopping.lib.Extensions;

public static class LongExtension
{
    /// <summary>
    /// 顯示幾MB幾KB
    /// </summary>
    /// <param name="contentLength"></param>
    /// <returns></returns>
    public static string ToFileSizeString( this long contentLength )
    {
        if( contentLength / 1024000.0 > 1 )
        {
            return ( contentLength / 1024000.0 ).ToString( "0.0" ) + "M";
        }

        if( contentLength / 1024.0 > 1 )
        {
            return ( contentLength / 1024.0 ).ToString( "0.0" ) + "K";
        }

        return contentLength + "B";
    }
}