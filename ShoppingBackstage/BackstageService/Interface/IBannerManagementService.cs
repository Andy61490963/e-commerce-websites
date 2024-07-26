using Shopping.lib.ViewModels;

namespace ShoppingBackstage.BackstageService.Interface
{
    public interface IBannerManagementService
    {
        /// <summary>
        /// 取得 列表
        /// </summary>
        /// <returns></returns>
        public List<BannerManagementViewModel> GetList(bool isPreview);

        /// <summary>
        /// 取得 單一
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BannerManagementViewModel? Get(Guid id);
        
        /// <summary>
        /// 新增、更新
        /// </summary>
        /// <param name="model"></param>
        public void Save( BannerManagementViewModel model );

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id);
        
        /// <summary>
        /// 排序輪播圖
        /// </summary>
        /// <param name="order"></param>
        void UpdateBannerOrder(List<string> order);

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<BannerManagementViewModel> GetSearchedList(SearchViewModel model);
    }
}
