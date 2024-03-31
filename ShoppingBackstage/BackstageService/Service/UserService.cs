using Microsoft.Data.SqlClient;
using Dapper;
using SA.admin.ViewModel;
using ShoppingBackstage.BackstageService.Interface;

namespace ShoppingBackstage.BackstageService.Service
{
    public class UserService : IUserService
    {
        private readonly SqlConnection _con;

        // Constructor
        public UserService(SqlConnection connection)
        {
            _con = connection;
        }

        public a0001_adminAccount GetUser(string account, string password)
        {
            var sql = @"SELECT *
                        FROM a0001_adminAccount
                        WHERE account = @Account AND password = @Password";
            var result = _con.QueryFirstOrDefault<a0001_adminAccount>(sql, new
            {
                Account = account,
                Password = password,
            });
            
            return result;
        }
    }

}
