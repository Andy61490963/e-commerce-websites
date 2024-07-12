using ShoppingBackstage.Areas.Account.ViewModels;

namespace ShoppingBackstage.Areas.Account.Services.Interface
{
    public interface IAccountManagementService  
    {
        /// <summary>
        /// 取得 列表
        /// </summary>
        /// <returns></returns>
        public List<AdminAccountViewModels> GetList();

        /// <summary>
        /// 取得 單一資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdminAccountViewModels Get( Guid id );
        
        /// <summary>
        /// 檢查 使用者帳號是否重複
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsAccountExists( Guid id, string account);

        /// <summary>
        /// 新增、更新
        /// </summary>
        /// <param name="model"></param>
        public void Save( AdminAccountViewModels model );

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id);
    }
}
