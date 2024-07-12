using Microsoft.Data.SqlClient;
using Dapper;
using ShoppingBackstage.ViewModel;
using Shopping.lib.Helpers;
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
            // 加密後驗證
            if (!string.IsNullOrEmpty(password))
            {
                password = HashHelper.Sha512(password);
            }

            var sql = @"SELECT *
                        FROM a0001_adminAccount
                        WHERE account_ = @Account AND password_ = @Password AND delete_ = 0";
            var result = _con.QueryFirstOrDefault<a0001_adminAccount>(sql, new
            {
                Account = account,
                Password = password,
            });
            
            return result;
        }
    }

}
