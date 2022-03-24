using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace MKForum.Managers
{
    //還需要一個討論區的排行
    public class RankingManager
    {
        public List<RankingDATA> GetScansList()
        {
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";            //連線字串
            string commandText = @"
                                    SELECT 
	                                    Members.Account,
	                                    Posts.Title,
	                                    Posts.PostDate,
	                                    Posts.CoverImage,
	                                    Posts.PostCotent,
	                                    Posts.PostID,
	                                    TempT.ViewCount
                                    FROM Posts
                                    JOIN members
                                    ON Members.MemberID = Posts.MemberID

                                    RIGHT JOIN
	                                    (SELECT
		                                    MemberScans.PostID,
		                                    COUNT(MemberScans.PostID) AS 'ViewCount'
	                                    From MemberScans
	                                    where DateDiff(day,MemberScans.ScanDate,getdate()) <=7
	                                    GROUP BY MemberScans.PostID
	                                    )AS TempT

                                    ON Posts.PostID = TempT.PostID
                                    WHERE PointID IS NULL
                                    ORDER BY ViewCount DESC
                                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<RankingDATA> RankingList = new List<RankingDATA>();

                        //把取得的資料放進陣列(資料庫無資料無法測試)
                        while (reader.Read())
                        {
                            RankingDATA MatchData = new RankingDATA()
                            {
                                Account = (string)reader["Account"],
                                Title = (string)reader["Title"],
                                PostDate = (DateTime)reader["PostDate"],
                                CoverImage = reader["CoverImage"] as string,
                                PostCotent = (string)reader["PostCotent"],
                                PostID = (Guid)reader["PostID"],
                                ViewCount = (int)reader["ViewCount"]
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




        /// <summary>
        /// 排行榜：合併瀏覽紀錄和文章的表，並取得7天內的瀏覽量前5筆
        /// </summary>
        /// <returns>回傳值為List</returns>
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