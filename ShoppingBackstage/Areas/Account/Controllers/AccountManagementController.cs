using Microsoft.AspNetCore.Mvc;
using Shopping.lib.Enums;
using ShoppingBackstage.Controllers;
using ShoppingBackstage.Areas.Account.Services.Interface;
using ShoppingBackstage.Areas.Account.ViewModels;
using Shopping.lib.Enums;
using X.PagedList;
namespace ShoppingBackstage.Areas.Account.Controllers;

[Area("Account")]
public class AccountManagementController : BaseController
{
    private readonly IAccountManagementService _accountManagementService;

    public AccountManagementController(IAccountManagementService accountManagementService)
    {
        _accountManagementService = accountManagementService;
    }
    
    public IActionResult Index(int page = 1)
    {
        IPagedList<AdminAccountViewModels> model  = _accountManagementService.GetList().ToPagedList(page, PageSize);
        return View( model );
    }
    
    public IActionResult Function()
    {
        return PartialView( "_function" );
    }
    
    /// <summary>
    /// 取得 列表
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public IActionResult List(int page = 1)
    {
        IPagedList<AdminAccountViewModels> model  = _accountManagementService.GetList().ToPagedList(page , PageSize);

        return PartialView( "_list", model );
    }
    
    /// <summary>
    /// 取得 單一
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IActionResult Input( Guid? id )
    {
        var result  = _accountManagementService.Get( id ?? Guid.Empty );
        if ( !id.HasValue )
        {
            result.actionType_ = ActionType.Add.ToInt();
        }
        else
        {
            result.actionType_ = ActionType.Edit.ToInt();
        }
        result.modify_id_ = CurrentUser.Id; // 修改者 id
        
        return PartialView( "_inputmodal", result );
    }
    
    /// <summary>
    /// 儲存、更新 結果
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public IActionResult Save( AdminAccountViewModels model )
    {
        if ( !ModelState.IsValid )
        {
            foreach (var state in ModelState.Values)
            {
                foreach (var error in state.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return PartialView( "_inputmodal", model );
        }
        
        // 檢查使用者帳號是否重複
        if (_accountManagementService.IsAccountExists( model.id_ ,model.account_))
        {
            ModelState.AddModelError("account_", "使用者帳號已存在");
            return PartialView( "_inputmodal", model );
        }

        _accountManagementService.Save( model );

        return Json(new { success = true });
    }

    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IActionResult Delete( Guid id )
    {
        _accountManagementService.Delete( id );
        
        return Json(new { success = true });
    }
}