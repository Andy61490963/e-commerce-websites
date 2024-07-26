using Shopping.lib.ViewModels;

namespace Shopping.Service.Interface;

public interface IDashBoardService
{
    List<BannerManagementViewModel> GetBanner();

    AboutManagementViewModel GetAbout();
}