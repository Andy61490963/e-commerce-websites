using ShoppingBackstage.Areas.Categories.ViewModels;

namespace ShoppingBackstage.Areas.Categories.Services.Interface
{
    public interface ICategoriesManagementService  
    {
        /// <summary>
        /// 取得 列表
        /// </summary>
        /// <returns></returns>
        public List<CategoriesManagementViewModels> GetList();

        /// <summary>
        /// 取得 單一資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CategoriesManagementViewModels Get( Guid id );
        
        /// <summary>
        /// 檢查 重複
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExists( Guid id, string name);

        /// <summary>
        /// 新增、更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        public void Save( CategoriesManagementViewModels model, Guid currentUserId );

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id);
        
        /// <summary>
        /// 更新排序
        /// </summary>
        /// <param name="request">包含排序類型和順序的請求對象</param>
        /// <returns></returns>
        void UpdateOrder(UpdateOrderRequest request);
    }
}
