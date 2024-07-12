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
    public Guid Id => Guid.Parse(FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
    
    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string Name => FindFirst( ClaimTypes.Name )?.Value ?? string.Empty; 
    
    /// <summary>
    /// 帳號
    /// </summary>
    public string account => FindFirst("account")?.Value ?? string.Empty;
    
    /// <summary>
    /// 手機
    /// </summary>
    public string phone => FindFirst( ClaimTypes.MobilePhone )?.Value ?? string.Empty; 
    
    /// <summary>
    /// 信箱
    /// </summary>
    public string email => FindFirst( ClaimTypes.Email )?.Value ?? string.Empty; 
}