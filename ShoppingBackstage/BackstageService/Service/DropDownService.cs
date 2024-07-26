using Microsoft.Data.SqlClient;
using Dapper;
using ShoppingBackstage.ViewModel;
using Shopping.lib.Helpers;
using ShoppingBackstage.BackstageService.Interface;

namespace ShoppingBackstage.BackstageService.Service
{
    public class DropDownService : IDropDownService
    {
        private readonly SqlConnection _con;

        // Constructor
        public DropDownService(SqlConnection connection)
        {
            _con = connection;
        }

        public List<GeneralViewModel> GetAboutTitle()
        {
            var sql = @"SELECT id_ as Id, title_ as Text FROM i0002_about WHERE delete_ = 0";

            return _con.Query<GeneralViewModel>(sql).ToList();
        }
    }

}
