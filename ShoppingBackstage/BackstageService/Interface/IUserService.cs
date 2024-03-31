

using SA.admin.ViewModel;

namespace ShoppingBackstage.BackstageService.Interface
{
    public interface IUserService
    {

        a0001_adminAccount GetUser(string account, string password);
    }
}
