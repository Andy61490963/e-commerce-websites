using Dapper;
using Microsoft.Data.SqlClient;
using Shopping.lib.ViewModels;
using ShoppingBackstage.Service.Interface;

namespace ShoppingBackstage.Service.Service;

public class ServerFileService : IServerFileService
{
    private readonly SqlConnection _con;

    public ServerFileService( SqlConnection con )
    {
        _con = con;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    public void Save(ServerFileViewModel file)
    {
        var sql = @"
INSERT INTO [dbo].[s0001_serverFiles]
( [id_],[file_name_],[file_path_],[display_name_],[extension_],[file_size_],[delete_],[create_time_] )
VALUES ( @id_ , @file_name_ , @file_path_ , @display_name_, @extension_, @file_size_, 0, GETDATE() )
";
        _con.Execute(sql, file);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ServerFileViewModel? Get(Guid id)
    {
        var sql = @"
SELECT [seqno_]
      ,[id_]
      ,[file_name_]
      ,[file_path_]
      ,[display_name_]
      ,[file_size_]
      ,[extension_]
      ,[delete_]
      ,[create_time_]
FROM [dbo].[s0001_serverFiles]
WHERE [id_] = @id
";
        return _con.QueryFirstOrDefault<ServerFileViewModel?>(sql, new { id });
    }
}