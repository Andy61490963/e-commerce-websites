using Dapper;
using Microsoft.Data.SqlClient;
using Shopping.lib.ViewModels;
using Shopping.Service.Interface;

namespace Shopping.Service.Service;

public class DashBoardService : IDashBoardService
{
    private readonly SqlConnection _con;

    public DashBoardService( SqlConnection con )
    {
        _con = con;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<BannerManagementViewModel> GetBanner()
    {
        var sql = @"
SELECT
     b.[id_]
    ,b.[url_]
    ,b.[title_]
    ,b.[caption_]
    ,b.[offset_]
    ,b.[start_date_]
    ,b.[end_date_]
    ,b.[s0001_id_]
    ,b.[order_]
    ,b.[create_time_]
    ,b.[modify_time_]
    ,[file].[id_]
    ,[file].[file_name_] 
    ,[file].[file_path_]
    ,[file].[display_name_]
    ,[file].[extension_]
FROM [i0001_banner] b

left join s0001_serverFiles [file]
on b.[s0001_id_] = [file].[id_]

where b.[delete_] = 0 and b.[enabled_] = 1 and b.[start_date_] <= GetDate() and ( b.[end_date_] is null or GetDate() <= b.[end_date_] )

order by [order_] asc, [modify_time_] desc, [create_time_] desc ";

        var result = _con.Query<BannerManagementViewModel, ServerFileViewModel, BannerManagementViewModel>( sql,
                ( banner, frontCover ) =>
                {
                    banner.ServerFile = frontCover;
                    return banner;
                }, splitOn: "id_" )
            .ToList();

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AboutManagementViewModel GetAbout()
    {
        var sql = @"SELECT * FROM i0002_about WHERE delete_ = 0 AND enabled_ = 1 ";

        var result = _con.QueryFirstOrDefault<AboutManagementViewModel>(sql);
        return result;
    }
}