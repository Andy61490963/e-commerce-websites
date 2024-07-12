using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Shopping.lib.ViewModels;

namespace Shopping.lib.Enums;

public static class EnumHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj">列舉物件</param>
    /// <returns></returns>s
    public static byte ToByte( this Enum obj )
    {
        return Convert.ToByte( obj );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj">列舉物件</param>
    /// <returns></returns>
    public static int ToInt( this Enum obj )
    {
        return Convert.ToInt32( obj );
    }

    /// <summary>
    /// 取得列舉名稱
    /// </summary>
    /// <param name="obj">列舉物件</param>
    /// <returns></returns>
    public static string GetName( this Enum obj )
    {
        var objName = obj.ToString();

        var t = obj.GetType();

        var fi = t.GetField( objName );

        var arrayDisplayAttribute = ( DisplayAttribute[] ) fi.GetCustomAttributes( typeof( DisplayAttribute ), false );

        return arrayDisplayAttribute[ 0 ].Name;
    }

    /// <summary>
    /// 取得列舉描述
    /// </summary>
    /// <typeparam name="T">列舉類型</typeparam>
    /// <param name="obj">列舉數字</param>
    /// <returns></returns>
    public static string GetName<T>( this object obj )
    {
        if ( !int.TryParse( obj.ToString(), out var value ) )
        {
            value = 0;
        }

        var e = Enum.Parse( typeof( T ), Convert.ToString( value ) );

        return ( ( Enum ) e ).GetName();
    }

    /// <summary>
    /// 取得列舉描述
    /// </summary>
    /// <param name="obj">列舉物件</param>
    /// <returns></returns>
    public static string GetDescription( this Enum obj )
    {
        var objName = obj.ToString();

        var t = obj.GetType();

        var fi = t.GetField( objName );

        var arrayDescription = ( DisplayAttribute[] ) fi.GetCustomAttributes( typeof( DisplayAttribute ), false );

        return arrayDescription[ 0 ].Description;
    }

    /// <summary>
    /// 取得列舉描述
    /// </summary>
    /// <typeparam name="T">列舉類型</typeparam>
    /// <param name="obj">列舉數字</param>
    /// <returns></returns>
    public static string GetDescription<T>( this object obj )
    {
        if ( !int.TryParse( obj.ToString(), out var value ) )
        {
            value = 0;
        }

        var e = Enum.Parse( typeof( T ), Convert.ToString( value ) );

        return ( ( Enum ) e ).GetDescription();
    }
    
    public static List<SelectListViewModel> GetEnumSelectListByName<T> ( this Type type ) where T : struct
    {
        var list = new List<SelectListViewModel>();
        foreach ( var item in Enum.GetValues( type ) )
        {
            var value = ( int ) item;
            list.Add(new SelectListViewModel
                     {
                         Text = value.GetName<T>(),
                         Value = value.ToString()
                     });
        }

        return list;
    }
    
    public static string GetEnumDescription<T>(this T enumValue) where T : Enum
    {
        FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());
        DisplayAttribute[] attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);

        return attributes != null && attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();
    }
}