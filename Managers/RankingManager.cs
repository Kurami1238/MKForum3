using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace MKForum.Managers
{

    public class RankingManager
    {
        /// <summary>
        /// 排行榜：合併瀏覽紀錄和文章的表，並取得7天內的瀏覽量前5筆
        /// </summary>
        /// <returns></returns>
        public List<Post> GetRankingList()
        {
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";            //連線字串
            string commandText = @"
                        SELECT TOP 5 * 
                        FROM [MKForum].[dbo].[Posts] 
                        INNER JOIN [MemberScans]
                        ON [Posts].[MemberID] = [MemberScans].[MemberID]
                        where DateDiff(day,[MemberScans].[ScanDate],getdate()) <=7
                        ORDER BY [PostView]  DESC 
                        ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<Post> RankingList = new List<Post>();

                        //把取得的資料放進陣列(資料庫無資料無法測試)
                        while (reader.Read())
                        {
                            Post MatchData = new Post()
                            {
                                PostID = (Guid)reader["PostID"],
                                Title = reader["Title"] as string,
                                PostCotent = reader["PostCotent"] as string,
                                PostView = (int)reader["PostView"],
                                PostDate = (DateTime)reader["PostDate"],
                            };
                            RankingList.Add(MatchData);

                        }
                        return RankingList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("RankingManager.GetRankingList", ex);
                throw;
            }
        }
    }
}