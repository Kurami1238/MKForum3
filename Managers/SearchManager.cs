using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace MKForum.Managers
{
    public class SearchManager
    {

        /// <summary>
        /// 建立一個搜尋清單
        /// </summary>
        public List<SearchResult> getAllSrchList(string srchText, int cboardid)
        {

            //找 文章標題 或 文章內容 或 作者名稱 或 子板塊名稱 (SQL已測試OK)
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";            //連線字串
            string commandText = @"
                        SELECT [Cboards].[CboardID],[Cboards].[Cname],[Posts].[PostID],[Posts].Title,[Posts].PostCotent, [Members].[NickName]
                        FROM [MKForum].[dbo].[Posts]
                        INNER JOIN [Members]
                        ON [MKForum].[dbo].[Posts].[MemberID] = [MKForum].[dbo].[Members].[MemberID]
			            INNER JOIN [Cboards]
			            ON [MKForum].[dbo].[Cboards].[CboardID] = [MKForum].[dbo].[Posts].[CboardID]
			            WHERE[PostCotent] LIKE '%@srchText%' OR [Title] LIKE '%@srchText%' OR [NickName] LIKE '%@srchText%' OR [Cname] LIKE '%@srchText%';
                        ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@srchText", srchText);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<SearchResult> srchList = new List<SearchResult>();

                        //把取得的資料放進陣列(不知道為什麼讀不到)
                        while (reader.Read())
                        {
                            SearchResult MatchData = new SearchResult()
                            {
                                PostID = (Guid)reader["PostID"],
                                NickName = reader["NickName"] as string,
                                Title = reader["Title"] as string,
                                PostCotent = reader["PostCotent"] as string,
                                Cname = reader["Title"] as string,
                                CboardID = (Guid)reader["CboardID"],
                                PostDate = (DateTime)reader["PostDate"],
                            };
                            srchList.Add(MatchData);

                        }
                        return srchList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SearchManager.getAllSrchList", ex);
                throw;
            }
        }
        /// <summary>
        /// 取得當前子版搜尋的List
        /// </summary>
        /// <param name="srchText"></param>
        /// <param name="cboardid"></param>
        /// <returns></returns>
        public List<SearchResult> getCboardSrchList(string srchText, int cboardid)
        {
            //從Session取得當前子板塊ID
            //int cboardid = this.Session["CboradID"] as int;

            //找當前子板塊內的 文章標題 或 文章內容 或 作者名稱 (SQL已測試OK)
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";            //連線字串
            string commandText = @"
                        SELECT [Cboards].[Cname],[Posts].[PostID],[Posts].Title,[Posts].PostCotent, [Members].[NickName]
                        FROM [MKForum].[dbo].[Posts]
                        INNER JOIN [Members]
                        ON [Posts].[MemberID] = [Members].[MemberID]
			            INNER JOIN [Cboards]
			            ON [Cboards].[CboardID] = [Posts].[CboardID]
			            WHERE ([PostCotent] LIKE '%@srchText%' OR [Title] LIKE '%@srchText%' OR [NickName] LIKE '%@srchText%') AND [Cname] = '@CboradID';
                        ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@srchText", srchText);
                        command.Parameters.AddWithValue("@cboardid", cboardid);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<SearchResult> srchList = new List<SearchResult>();

                        //把取得的資料放進陣列
                        while (reader.Read())
                        {
                            SearchResult MatchData = new SearchResult()
                            {
                                PostID = (Guid)reader["PostID"],
                                NickName = reader["NickName"] as string,
                                Title = reader["Title"] as string,
                                PostCotent = reader["PostCotent"] as string,
                                Cname = reader["Title"] as string,
                                CboardID = (Guid)reader["CboardID"],
                                PostDate = (DateTime)reader["PostDate"],
                            };
                            srchList.Add(MatchData);
                        }
                        return srchList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SearchManager.getCboardSrchList", ex);
                throw;
            }
        }
        /// <summary>
        /// 取得作者搜尋的List
        /// </summary>
        /// <param name="srchText"></param>
        /// <param name="cboardid"></param>
        /// <returns></returns>
        public List<SearchResult> getWriterSrchList(string srchText, int cboardid)
        {

            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";            //連線字串
            string commandText = @"
                        SELECT [Cboards].[CboardID],[Cboards].[Cname],[Posts].[PostID],[Posts].Title,[Posts].PostCotent, [Members].[NickName]
                        FROM [MKForum].[dbo].[Posts]
                        INNER JOIN [Members]
                        ON [MKForum].[dbo].[Posts].[MemberID] = [MKForum].[dbo].[Members].[MemberID]
			            INNER JOIN [Cboards]
			            ON [MKForum].[dbo].[Cboards].[CboardID] = [MKForum].[dbo].[Posts].[CboardID]
			            WHERE[NickName] LIKE '%@srchText%';
                        ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@srchText", srchText);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<SearchResult> srchList = new List<SearchResult>();

                        //把取得的資料放進陣列
                        while (reader.Read())
                        {
                            SearchResult MatchData = new SearchResult()
                            {
                                PostID = (Guid)reader["PostID"],
                                NickName = reader["NickName"] as string,
                                Title = reader["Title"] as string,
                                PostCotent = reader["PostCotent"] as string,
                                Cname = reader["Title"] as string,
                                CboardID = (Guid)reader["CboardID"],
                                PostDate = (DateTime)reader["PostDate"],
                            };
                            srchList.Add(MatchData);
                        }
                        return srchList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SearchManager.getWriterSrchList", ex);
                throw;
            }
        }

    }
}