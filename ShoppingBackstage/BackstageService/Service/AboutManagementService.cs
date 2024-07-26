using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using Shopping.lib.Enums;
using Shopping.lib.Helpers;
using Shopping.lib.ViewModels;
using ShoppingBackstage.BackstageService.Interface;

namespace ShoppingBackstage.BackstageService.Service
{
    public class AboutManagementService : IAboutManagementService
    {
        private readonly SqlConnection _con;

        // Constructor
        public AboutManagementService(SqlConnection connection)
        {
            _con = connection;
        }

        /// <summary>
        /// 取得 列表
        /// </summary>
        /// <returns></returns>
        public List<AboutManagementViewModel> GetList()
        {
            var sql = @"SELECT * FROM i0002_about WHERE delete_ = 0 ORDER BY enabled_ desc";

            var result = _con.Query<AboutManagementViewModel>(sql).ToList();
            return result;
        }

        /// <summary>
        /// 取得 單一
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AboutManagementViewModel? Get(Guid id)
        {
            var sql = @"SELECT * FROM i0002_about WHERE delete_ = 0 AND id_ = @id";
            var result = _con.QueryFirstOrDefault<AboutManagementViewModel>(sql, new { id });
            
            return result;
        }
        
        /// <summary>
        /// 新增、更新
        /// </summary>
        /// <param name="model"></param>
        public void Save(AboutManagementViewModel model, Guid id)
        {
            var sql = @"MERGE i0002_about AS target
                        USING (SELECT 
                                @id_ AS id_, 
                                @title_ AS title_, 
                                @content_ AS content_,
                                @footer_ AS footer_,
                                @enabled_ AS enabled_,                              
                                @delete_ AS delete_,
                                @create_time_ AS create_time_,
                                @modify_time_ AS modify_time_,
                                @a0001_id_create_ AS a0001_id_create_,
                                @a0001_id_modify_ AS a0001_id_modify_) AS source
                        ON (target.id_ = source.id_)
                        WHEN MATCHED THEN
                            UPDATE SET 
                                title_ = source.title_,
                                content_ = source.content_,
                                footer_ = source.footer_,
                                delete_ = source.delete_,
                                modify_time_ = source.modify_time_,
                                a0001_id_modify_ = source.a0001_id_modify_
                        WHEN NOT MATCHED THEN
                            INSERT (id_, title_, content_, footer_, enabled_, delete_, create_time_, modify_time_, a0001_id_create_, a0001_id_modify_)
                            VALUES (source.id_, source.title_, source.content_, source.footer_, source.enabled_, source.delete_, source.create_time_, source.modify_time_, source.a0001_id_create_, source.a0001_id_modify_);
                        ";

            _con.Execute(sql, new
            {
                model.id_,
                model.title_,
                model.content_,
                model.footer_,
                model.enabled_,
                delete_ = 0,
                create_time_ = DateTime.Now,
                modify_time_ = DateTime.Now,
                a0001_id_create_ = id,
                a0001_id_modify_ = id
            });
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id)
        {
            var sql = @"UPDATE i0002_about SET delete_ = 1 WHERE id_ = @id";
            _con.Execute(sql, new { id });
        }

        /// <summary>
        /// 更新第一位為開啟
        /// </summary>
        /// <param name="order"></param>
        public void UpdateBannerOrder(List<string> order)
        {
            if (order == null || order.Count == 0)
                return;

            // 將所有的 enabled_ 設置為 false
            var sqlDisableAll = "UPDATE i0002_about SET enabled_ = 0;";
            _con.Execute(sqlDisableAll);

            // 將第一個項目的 enabled_ 設置為 true
            var sqlEnableFirst = "UPDATE i0002_about SET enabled_ = 1 WHERE id_ = @id;";
            _con.Execute(sqlEnableFirst, new { id = order[0] });
        }

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<AboutManagementViewModel> GetSearchedList(SearchViewModel model)
        {
            var sql = @"
SELECT * FROM i0002_about WHERE delete_ = 0
";

            if (!string.IsNullOrEmpty(model.selected))
            {
                sql += " AND [id_] = @Selected ";
            }

            if (!string.IsNullOrEmpty(model.text))
            {
                sql += " AND [title_] LIKE @Text ";
            }

            sql += " ORDER BY enabled_ desc ";

            var result = _con.Query<AboutManagementViewModel>(sql, new { Selected = model.selected, Text = $"%{model.text}%" }).ToList();

            return result;
        }
    }

}
