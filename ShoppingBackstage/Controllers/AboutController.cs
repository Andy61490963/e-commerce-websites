using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shopping.lib.Enums;
using ShoppingBackstage.BackstageService.Interface;
using Shopping.lib.ViewModels;
using X.PagedList;

namespace ShoppingBackstage.Controllers
{
    [Authorize]
    public class AboutController : BaseController
    {
        private readonly IAboutManagementService _aboutManagementService;
        private readonly IDropDownService _dropDownService;
        
        public AboutController(IAboutManagementService bannerManagementService, IDropDownService dropDownService)
        {
            _aboutManagementService = bannerManagementService;
            _dropDownService = dropDownService;
        }
        
        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            var model = new AboutViewModel();
            model.Abouts = _aboutManagementService.GetList().ToPagedList(page, PageSize);
            ViewData["title_"] = _dropDownService.GetAboutTitle();
            return View( model );
        }
        
        /// <summary>
        /// 取得 篩選
        /// </summary>
        /// <returns></returns>
        public IActionResult Function()
        {
            ViewData["title_"] = _dropDownService.GetAboutTitle();
            return PartialView( "_function" );
        }
        
        /// <summary>
        /// 取得 列表
        /// </summary>
        /// <returns></returns>
        public IActionResult List()
        {
            IPagedList<AboutManagementViewModel> result = _aboutManagementService.GetList().ToPagedList(page, PageSize);
            
            return PartialView( "_list", result );
        }
        
        /// <summary>
        /// 取得 單一
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Input( Guid? id )
        {
            var result  = _aboutManagementService.Get( id ?? Guid.Empty );
            if ( !id.HasValue )
            {
                var model = new AboutManagementViewModel
                {
                    id_ = Guid.NewGuid(),
                    ActionType = ActionType.Add.ToInt()
                };
                
                return PartialView( "_inputmodal", model );
            }
            else
            {
                if( result == null )
                {
                    return RedirectToAction( "Index" );
                }
                result.ActionType = ActionType.Edit.ToInt();
            }
            
            return PartialView( "_inputmodal", result );
        }
        
        /// <summary>
        /// 儲存、更新 結果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult Save(AboutManagementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                
                return PartialView("_inputmodal", model);
            }
 
            _aboutManagementService.Save(model, CurrentUser.Id);
            
            return Json(new { success = true });
        }
        
        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete( Guid id )
        {
            _aboutManagementService.Delete( id );
        
            return Json(new { success = true });
        }
        
        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult Search( SearchViewModel model )
        {
            IPagedList<AboutManagementViewModel> result  = _aboutManagementService.GetSearchedList( model ).ToPagedList(page, PageSize);
            return PartialView( "_list", result );
        }
        
        /// <summary>
        /// 更新第一位為開啟
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateOrder([FromBody] List<string>? order)
        {
            if (order == null)
            {
                return Json(new { success = false, message = "無效的數據" });
            }
            
            _aboutManagementService.UpdateBannerOrder(order);

            return Json(new { success = true });
        }
    }
}