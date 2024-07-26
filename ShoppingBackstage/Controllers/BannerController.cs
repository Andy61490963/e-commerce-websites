using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBackstage.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shopping.lib.Enums;
using ShoppingBackstage.BackstageService.Interface;
using Shopping.lib.ViewModels;
using X.PagedList;

namespace ShoppingBackstage.Controllers
{
    [Authorize]
    public class BannerController : BaseController
    {
        private readonly IBannerManagementService _bannerManagementService;

        
        public BannerController(IBannerManagementService bannerManagementService)
        {
            _bannerManagementService = bannerManagementService;
        }
            
        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            var model = new BannerViewModel();
            model.Banners = _bannerManagementService.GetList(false).ToPagedList(page, PageSize);
            
            return View( model );
        }

        /// <summary>
        /// 取得 列表
        /// </summary>
        /// <returns></returns>
        public IActionResult List()
        {
            IPagedList<BannerManagementViewModel> result = _bannerManagementService.GetList(false).ToPagedList(page, PageSize);
            
            return PartialView( "_list", result );
        }
        
        /// <summary>
        /// 取得 單一
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Input( Guid? id )
        {
            var result  = _bannerManagementService.Get( id ?? Guid.Empty );
            if ( !id.HasValue )
            {
                var model = new BannerManagementViewModel
                {
                    id_ = Guid.NewGuid(),
                    start_date_ = DateTime.Now,
                    end_date_ = DateTime.Now.Date.AddMonths( 3 ),
                    ActionType = ActionType.Add.ToInt()
                };
                
                var previousFiles = new List<string>();
                ViewData[ "PreviousFiles" ] = JsonConvert.SerializeObject( previousFiles );
                return PartialView( "_inputmodal", model );
            }
            else
            {
                if( result == null )
                {
                    return RedirectToAction( "Index" );
                }
                result.ActionType = ActionType.Edit.ToInt();
                var previousFiles = new List<object>();
                if( result.ServerFile != null )
                {
                    previousFiles.Add( new
                    {
                        source = result.ServerFile.id_.ToString(),
                        options = new
                        {
                            type = "local",
                        }
                    } );
                }
                
                ViewData[ "PreviousFiles" ] = JsonConvert.SerializeObject( previousFiles );
                return PartialView( "_inputmodal", result );
            }
        }
        
        /// <summary>
        /// 儲存、更新 結果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult Save(BannerManagementViewModel model)
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
                // 重新設置 ViewData["PreviousFiles"]
                ViewData["PreviousFiles"] = model.previousFilesAttach;
                
                return PartialView("_inputmodal", model);
            }

            ProcessFileObject(model);

            _bannerManagementService.Save(model);
            
            return Json(new { success = true });
        }
        
        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete( Guid id )
        {
            _bannerManagementService.Delete( id );
        
            return Json(new { success = true });
        }
        
        /// <summary>
        /// 排序輪播圖
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
            
            _bannerManagementService.UpdateBannerOrder(order);

            return Json(new { success = true });
        }

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult Search( SearchViewModel model )
        {
            IPagedList<BannerManagementViewModel> result  = _bannerManagementService.GetSearchedList( model ).ToPagedList(page, PageSize);
            return PartialView( "_list", result );
        }

        /// <summary>
        /// 預覽
        /// </summary>
        /// <returns></returns>
        public IActionResult Preview()
        {
            List<BannerManagementViewModel> result = _bannerManagementService.GetList(true);
            
            return View("_Banner", result);
        }
        
        /// <summary>
        /// 處理 FileObject
        /// </summary>
        /// <param name="model"></param>
        private void ProcessFileObject(BannerManagementViewModel model)
        {
            if (string.IsNullOrEmpty(model.FileObject))
            {
                return;
            }

            model.ServerFile = new ServerFileViewModel();

            if (model.FileObject.Contains("file_name_")) // 新增，把json物件轉換成ServerFileViewModel
            {
                model.ServerFile = JsonConvert.DeserializeObject<ServerFileViewModel>(model.FileObject);
            }
            else if (Guid.TryParse(model.FileObject, out Guid id)) // 編輯，只會有id
            {
                model.ServerFile.id_ = id;
            }
        }

    }
}
