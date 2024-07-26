using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingBackstage.ViewModel;

namespace ShoppingBackstage.BackstageService.Interface
{
    public interface IDropDownService
    {
        List<GeneralViewModel> GetAboutTitle();
    }
}
