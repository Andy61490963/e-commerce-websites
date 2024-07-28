using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shopping.lib.Enums;
using Shopping.lib.ViewModels;
using ShoppingBackstage.Controllers;
using ShoppingBackstage.Areas.Categories.Services.Interface;
using ShoppingBackstage.Areas.Categories.ViewModels;
using X.PagedList;
namespace ShoppingBackstage.Areas.Categories.Controllers;

[Area("Categories")]
public class CategoriesManagementController : BaseController
{
    private readonly ICategoriesManagementService _categoriesManagementService;

    public CategoriesManagementController(ICategoriesManagementService categoriesManagementService)
    {
        _categoriesManagementService = categoriesManagementService;
    }
    
    public IActionResult Index()
    {
        CategoriesViewModels model = new CategoriesViewModels();
        model.Categories  = _categoriesManagementService.GetList();
        
        return View( model );
    }
    
    /// <summary>
    /// 取得 功能
    /// </summary>
    /// <returns></returns>
    public IActionResult Function()
    {
        return PartialView( "_function" );
    }
    
    /// <summary>
    /// 取得 左列表
    /// </summary>
    /// <returns></returns>
    public IActionResult LeftList()
    {
        List<CategoriesManagementViewModels> model  = _categoriesManagementService.GetList().Where(x => x.enabled_ == 1).OrderBy(x => x.dropdown_order_).ToList();

        return PartialView( "_leftlist", model );
    }
    
    /// <summary>
    /// 取得 右列表
    /// </summary>
    /// <returns></returns>
    public IActionResult RightList()
    {
        List<CategoriesManagementViewModels> model  = _categoriesManagementService.GetList().OrderBy(x => x.layout_order_).ToList();

        return PartialView( "_rightlist", model );
    }
    
    /// <summary>
    /// 取得 單一
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Input( Guid? id )
    {
        var result  = _categoriesManagementService.Get( id ?? Guid.Empty );
        if ( !id.HasValue )
        {
            result.id_ = Guid.NewGuid();
            result.actionType_ = ActionType.Add.ToInt();
            
            var layoutFileAttach = new List<string>();
            ViewData[ "LayoutFileAttach" ] = JsonConvert.SerializeObject( layoutFileAttach );
            var bannerFileAttach = new List<string>();
            ViewData[ "BannerFileAttach" ] = JsonConvert.SerializeObject( bannerFileAttach );
        }
        else
        {
            result.actionType_ = ActionType.Edit.ToInt();
            var layoutFileAttach = new List<object>(); // LayoutFile
            if( result.LayoutServerFile != null )
            {
                layoutFileAttach.Add( new
                {
                    source = result.LayoutServerFile.id_.ToString(),
                    options = new
                    {
                        type = "local",
                    }
                } );
            }
            var bannerFileAttach = new List<object>(); // BannerFile
            if( result.BannerServerFile != null )
            {
                bannerFileAttach.Add( new
                {
                    source = result.BannerServerFile.id_.ToString(),
                    options = new
                    {
                        type = "local",
                    }
                } );
            }
            
            ViewData[ "LayoutFileAttach" ] = JsonConvert.SerializeObject( layoutFileAttach );
            ViewData[ "BannerFileAttach" ] = JsonConvert.SerializeObject( bannerFileAttach );
        }
        
        return PartialView( "_inputmodal", result );
    }
    
    /// <summary>
    /// 儲存、更新 結果
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public IActionResult Save( CategoriesManagementViewModels model )
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
        
        // 檢查是否重複
        if (_categoriesManagementService.IsExists( model.id_ ,model.name_))
        {
            ModelState.AddModelError("name_", "名稱重複");
            return PartialView( "_inputmodal", model );
        }
        
        ProcessFileObject(model);
        
        _categoriesManagementService.Save( model, CurrentUser.Id );

        return Json(new { success = true });
    }

    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IActionResult Delete( Guid id )
    {
        _categoriesManagementService.Delete( id );
        
        return Json(new { success = true });
    }
    
    /// <summary>
    /// 更新排序
    /// </summary>
    /// <param name="request">包含排序類型和順序的請求對象</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult UpdateOrder([FromBody] UpdateOrderRequest request)
    {
        if (request == null)
        {
            return BadRequest("Invalid request data.");
        }
        
        _categoriesManagementService.UpdateOrder(request);

        return Json(new { success = true });
    }
    
    /// <summary>
    /// 處理 FileObject
    /// </summary>
    /// <param name="model"></param>
    private void ProcessFileObject(CategoriesManagementViewModels model)
    {
        // 處理 LayoutFileObject
        if (!string.IsNullOrEmpty(model.LayoutFileObject))
        {
            model.LayoutServerFile = new ServerFileViewModel();

            if (model.LayoutFileObject.Contains("file_name_")) // 新增，把json物件轉換成ServerFileViewModel
            {
                model.LayoutServerFile = JsonConvert.DeserializeObject<ServerFileViewModel>(model.LayoutFileObject);
            }
            else if (Guid.TryParse(model.LayoutFileObject, out Guid id)) // 編輯，只會有id
            {
                model.LayoutServerFile.id_ = id;
            }
        }

        // 處理 BannerFileObject
        if (!string.IsNullOrEmpty(model.BannerFileObject))
        {
            model.BannerServerFile = new ServerFileViewModel();

            if (model.BannerFileObject.Contains("file_name_")) // 新增，把json物件轉換成ServerFileViewModel
            {
                model.BannerServerFile = JsonConvert.DeserializeObject<ServerFileViewModel>(model.BannerFileObject);
            }
            else if (Guid.TryParse(model.BannerFileObject, out Guid id)) // 編輯，只會有id
            {
                model.BannerServerFile.id_ = id;
            }
        }
    }
}