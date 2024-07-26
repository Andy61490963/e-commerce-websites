using System.ComponentModel.DataAnnotations;
using Shopping.lib.ViewModels;

namespace Shopping.ViewModel
{
    public class HomeViewModel
    {
        public List<BannerManagementViewModel>? BannerViewModel { get; set; }
        
        public AboutManagementViewModel? AboutViewModel { get; set; }
    }
}
