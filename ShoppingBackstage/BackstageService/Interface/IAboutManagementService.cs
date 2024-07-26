using Shopping.lib.ViewModels;

namespace ShoppingBackstage.BackstageService.Interface
{
    public interface IAboutManagementService
    {
        /// <summary>
        /// 取得 列表
        /// </summary>
        /// <returns></returns>
        public List<AboutManagementViewModel> GetList();

        /// <summary>
        /// 取得 單一
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AboutManagementViewModel? Get(Guid id);
        
        /// <summary>
        /// 新增、更新
        /// </summary>
        /// <param name="model"></param>
        public void Save( AboutManagementViewModel model, Guid id );

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id);
        
        /// <summary>
        /// 更新第一位為開啟
        /// </summary>
        /// <param name="order"></param>
        void UpdateBannerOrder(List<string> order);

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<AboutManagementViewModel> GetSearchedList(SearchViewModel model);
    }
}
