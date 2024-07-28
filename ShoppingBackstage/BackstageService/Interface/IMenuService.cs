using Shopping.lib.ViewModels;
    
namespace ShoppingBackstage.BackstageService.Interface
{
    public interface IMenuService
    {
        List<MenuViewModel> GetMenu();
    }
}
