using System.Security.Claims;

namespace ShoppingBackstage.Models;

public class ClaimsPrincipalExtension : ClaimsPrincipal
{
    public ClaimsPrincipalExtension( ClaimsPrincipal principal ) : base( principal )
    {
    }

    /// <summary>
    /// 使用者 id
    /// </summary>
    public int Id => int.Parse( FindFirst( ClaimTypes.NameIdentifier )?.Value ?? "0" );

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string Name => FindFirst( ClaimTypes.Name )?.Value ?? string.Empty; 
    
    /// <summary>
    /// 帳號
    /// </summary>
    public string Ac => FindFirst("account")?.Value ?? string.Empty;
}