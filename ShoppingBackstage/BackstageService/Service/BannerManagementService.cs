using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using Shopping.lib.Enums;
using Shopping.lib.Helpers;
using Shopping.lib.ViewModels;
using ShoppingBackstage.BackstageService.Interface;

namespace ShoppingBackstage.BackstageService.Service
{
    public class BannerManagementService : IBannerManagementService
    {
        private readonly SqlConnection _con;

        // Constructor
        public BannerManagementService(SqlConnection connection)
        {
            _con = connection;
        }

        /// <summary>
        /// 取得 列表
        /// </summary>
        /// <returns></returns>
        public List<BannerManagementViewModel> GetList(bool isPreview)
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
    ,b.[enabled_]
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

where b.[delete_] = 0 and b.[start_date_] <= GetDate() and ( b.[end_date_] is null or GetDate() <= b.[end_date_] )";

            if (isPreview)
            {
                sql += " AND b.[enabled_] = 1";
            }

            sql += " ORDER BY [order_] ASC, [modify_time_] DESC, [create_time_] DESC";

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
        /// 取得 單一
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BannerManagementViewModel? Get(Guid id)
        {
            var sql = @"SELECT * FROM i0001_banner WHERE delete_ = 0 AND id_ = @id";
            var result = _con.QueryFirstOrDefault<BannerManagementViewModel>(sql, new { id });

            if (result != null)
            {
                // 查詢附件
                var attachsql = @"SELECT * FROM s0001_serverFiles WHERE delete_ = 0 AND id_ = @id";
                var attachRes = _con.QueryFirstOrDefault<ServerFileViewModel>(attachsql, new { id = result.s0001_id_ });

                // 檢查是否有附件資料
                if (attachRes != null)
                {
                    // 將附件資料列表賦值給事件模型的 ServerFile 屬性
                    result.ServerFile = attachRes;
                }
            }
            return result;
        }
        
        /// <summary>
        /// 新增、更新
        /// </summary>
        /// <param name="model"></param>
        public void Save(BannerManagementViewModel model)
        {
            // 如果目標表中存在具有相同 id_ 的記錄，則更新該記錄；如果不存在，則插入新記錄
            var sql = @"MERGE i0001_banner AS target
                        USING (SELECT 
                                @id_ AS id_, 
                                @title_ AS title_, 
                                @caption_ AS caption_, 
                                @url_ AS url_, 
                                @start_date_ AS start_date_, 
                                @end_date_ AS end_date_, 
                                @enabled_ AS enabled_, 
                                @offset_ AS offset_, 
                                @ServerFileID AS ServerFileID,
                                @delete_ AS delete_,
                                @create_time_ AS create_time_,
                                @modify_time_ AS modify_time_) AS source
                        ON (target.id_ = source.id_)
                        WHEN MATCHED THEN
                            UPDATE SET 
                                title_ = source.title_,
                                caption_ = source.caption_,
                                url_ = source.url_,
                                start_date_ = source.start_date_,
                                end_date_ = source.end_date_,
                                enabled_ = source.enabled_,
                                offset_ = source.offset_,
                                s0001_id_ = source.ServerFileID,
                                modify_time_ = source.modify_time_
                        WHEN NOT MATCHED THEN
                            INSERT (id_, title_, caption_, url_, start_date_, end_date_, enabled_, offset_, s0001_id_, delete_, create_time_, modify_time_)
                            VALUES (source.id_, source.title_, source.caption_, source.url_, source.start_date_, source.end_date_, source.enabled_, source.offset_, source.ServerFileID ,source.delete_, source.create_time_, source.modify_time_);
                        ";
            
            _con.Execute(sql, new
            {
                model.id_,
                model.title_,
                model.caption_,
                model.url_,
                model.start_date_,
                model.end_date_,
                model.enabled_,
                model.offset_,
                ServerFileID = model.ServerFile?.id_,
                delete_ = 0,
                create_time_ = DateTime.Now,
                modify_time_ = DateTime.Now,
            });
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id)
        {
            var sql = @"UPDATE i0001_banner SET delete_ = 1 WHERE id_ = @id";
            _con.Execute(sql, new { id });
        }
        
        /// <summary>
        /// 排序輪播圖
        /// </summary>
        /// <param name="order"></param>
        public void UpdateBannerOrder(List<string> order)
        {
            var sql = new StringBuilder();
            for (int i = 0; i < order.Count; i++)
            {
                sql.AppendLine($"UPDATE i0001_banner SET order_ = @order_{i} WHERE id_ = @id_{i};");
            }
            
            // 建立參數物件來存儲所有的參數
            var parameters = new DynamicParameters();
            for (int i = 0; i < order.Count; i++)
            {
                parameters.Add($"@order_{i}", i + 1); // 或者其他排序邏輯
                parameters.Add($"@id_{i}", order[i]);
            }
            
            _con.Execute(sql.ToString(), parameters);
        }

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<BannerManagementViewModel> GetSearchedList(SearchViewModel model)
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
    ,b.[enabled_]
    ,b.[create_time_]
    ,b.[modify_time_]
    ,[file].[id_]
    ,[file].[file_name_] 
    ,[file].[file_path_]
    ,[file].[display_name_]
    ,[file].[extension_]
FROM [i0001_banner] b
LEFT JOIN s0001_serverFiles [file] ON b.[s0001_id_] = [file].[id_]
WHERE b.[delete_] = 0
AND b.[start_date_] <= GetDate()
AND (b.[end_date_] IS NULL OR GetDate() <= b.[end_date_])
";

            if (!string.IsNullOrEmpty(model.selected))
            {
                sql += " AND b.[enabled_] = @Selected ";
            }

            if (!string.IsNullOrEmpty(model.text))
            {
                sql += " AND (b.[title_] LIKE @Text OR b.[caption_] LIKE @Text) ";
            }

            sql += " ORDER BY [order_] ASC, [modify_time_] DESC, [create_time_] DESC ";

            var result = _con.Query<BannerManagementViewModel, ServerFileViewModel, BannerManagementViewModel>(sql,
                    (banner, frontCover) =>
                    {
                        banner.ServerFile = frontCover;
                        return banner;
                    },
                    new { Selected = model.selected, Text = $"%{model.text}%" },
                    splitOn: "id_")
                .ToList();

            return result;
        }
    }

}
