using Microsoft.Data.SqlClient;
using Dapper;
using Shopping.Service.Interface;
using Shopping.lib.ViewModels;

namespace Shopping.Service.Service;

public class MenuService : IMenuService
{
    private readonly SqlConnection _con;

    public MenuService( SqlConnection con )
    {
        _con = con;
    }

    public List<MenuViewModel> GetMenu()
    {
        var sql = " SELECT * FROM s0002_menu where delete_ = 0 AND is_backstage_ = 0 ORDER BY sort_ ";

        var result = _con.Query<MenuViewModel>( sql ).ToList();
        return result;
    }
}