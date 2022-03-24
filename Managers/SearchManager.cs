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
        /// <summary>去除URL內的.aspx副檔名</summary>
        /// <param name="currentPage">輸入型別為string，請輸入網址列的querystring</param>
        /// <returns>回傳值為string</returns>
        public string ReplacFileExtension(string currentPage)
        {
            string current = ""; //避免回傳null
            if (currentPage.Replace(".aspx", "") != null)
                current += currentPage.Replace(".aspx", "");
            return current;
        }

        /// <summary>取得全站搜尋的搜尋結果List</summary>
        /// <param name="srchText">使用者輸入的關鍵字</param>
        /// <returns>回傳值為SearchResult的List</returns>
        //public List<SearchResult> GetAllSrchList(string srchText)
        //{

        //    //找 文章標題 或 文章內容 或 作者名稱 或 子板塊名稱 (SQL已測試OK)
        //    string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";            //連線字串
        //    string commandText = @"
        //                SELECT [Cboards].[CboardID],[Cboards].[Cname],[Posts].[PostID],[Posts].Title,[Posts].PostCotent, [Members].[NickName]
        //                FROM [MKForum].[dbo].[Posts]
        //                INNER JOIN [Members]
        //                ON [MKForum].[dbo].[Posts].[MemberID] = [MKForum].[dbo].[Members].[MemberID]
        //       INNER JOIN [Cboards]
        //       ON [MKForum].[dbo].[Cboards].[CboardID] = [MKForum].[dbo].[Posts].[CboardID]
        //       WHERE[PostCotent] LIKE '%@srchText%' OR [Title] LIKE '%@srchText%' OR [NickName] LIKE '%@srchText%' OR [Cname] LIKE '%@srchText%';
        //                ";

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            using (SqlCommand command = new SqlCommand(commandText, conn))
        //            {
        //                command.Parameters.AddWithValue("@srchText", srchText);

        //                conn.Open();
        //                SqlDataReader reader = command.ExecuteReader();

        //                List<SearchResult> srchList = new List<SearchResult>();

        //                //把取得的資料放進陣列(不知道為什麼讀不到)
        //                while (reader.Read())
        //                {
        //                    SearchResult MatchData = new SearchResult()
        //                    {
        //                        PostID = (Guid)reader["PostID"],
        //                        NickName = reader["NickName"] as string,
        //                        Title = reader["Title"] as string,
        //                        PostCotent = reader["PostCotent"] as string,
        //                        Cname = reader["Title"] as string,
        //                        CboardID = (Guid)reader["CboardID"],
        //                        PostDate = (DateTime)reader["PostDate"],
        //                    };
        //                    srchList.Add(MatchData);

        //                }
        //                return srchList;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog("SearchManager.getAllSrchList", ex);
        //        throw;
        //    }
        //}


        ///// <summary>取得當前板塊搜尋的搜尋結果List</summary>
        ///// <param name="srchText">使用者輸入的關鍵字</param>
        ///// <param name="currentBoard">當前所在的母/子板塊</param>
        ///// <returns>回傳值為SearchResult的List</returns>
        //public List<SearchResult> GetboardSrchList(string srchText, string srchCurrent, string PorCBoard)
        //{

        //    //找當前母板塊內的 文章標題 或 文章內容 或 作者名稱 需修改)
        //    string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";            //連線字串
        //    string commandText = "";
        //    if (PorCBoard == "c")   //搜子板塊的SQL語法
        //    {
        //        commandText = @"
        //                SELECT [Cboards].[Cname],[Posts].[PostID],[Posts].Title,[Posts].PostCotent, [Members].[NickName]
        //                FROM [MKForum].[dbo].[Posts]
        //                INNER JOIN [Members]
        //                ON [Posts].[MemberID] = [Members].[MemberID]
        //       INNER JOIN [Cboards]
        //       ON [Cboards].[CboardID] = [Posts].[CboardID]
        //       WHERE ([PostCotent] LIKE '%@srchText%' OR [Title] LIKE '%@srchText%' OR [NickName] LIKE '%@srchText%') AND [Cname] = '@CboradID';
        //                ";
        //    }
        //    else   //搜母板塊的SQL語法
        //    {
        //        commandText = @"
        //            SELECT [Posts].[CboardID],[Cname],[Posts].[PostID],[Posts].Title,[Posts].PostCotent, [Members].[NickName],[Cboards].[PboardID]
        //            FROM [MKForum].[dbo].[Posts]
        //            INNER JOIN [MKForum].[dbo].[Members]
        //            ON [Posts].[MemberID] = [Members].[MemberID]
        //            INNER JOIN [MKForum].[dbo].[Cboards]
        //            ON [Cboards].[CboardID] = [Posts].[CboardID]
        //            WHERE ([PostCotent] LIKE '%srchText%' OR [Title] LIKE '%@srchText%' OR [NickName] LIKE '%@srchText%')
        //            AND [PboardID] = '%@boardid%'";
        //    }
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            using (SqlCommand command = new SqlCommand(commandText, conn))
        //            {
        //                command.Parameters.AddWithValue("@srchText", srchText);
        //                command.Parameters.AddWithValue("@boardid", srchCurrent);

        //                conn.Open();
        //                SqlDataReader reader = command.ExecuteReader();

        //                List<SearchResult> srchList = new List<SearchResult>();

        //                //把取得的資料放進陣列
        //                while (reader.Read())
        //                {
        //                    SearchResult MatchData = new SearchResult()
        //                    {
        //                        PostID = (Guid)reader["PostID"],
        //                        NickName = reader["NickName"] as string,
        //                        Title = reader["Title"] as string,
        //                        PostCotent = reader["PostCotent"] as string,
        //                        Cname = reader["Title"] as string,
        //                        CboardID = (Guid)reader["CboardID"],
        //                        PostDate = (DateTime)reader["PostDate"],
        //                    };
        //                    srchList.Add(MatchData);
        //                }
        //                return srchList;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog("SearchManager.getCboardSrchList", ex);
        //        throw;
        //    }
        //}



        ///// <summary>取得作者搜尋的搜尋結果List</summary>
        ///// <param name="srchText">使用者輸入的關鍵字</param>
        ///// <returns>回傳值為SearchResult的List</returns>
        //public List<SearchResult> GetWriterSrchList(string srchText)
        //{

        //    string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";            //連線字串
        //    string commandText = @"
        //                SELECT [Cboards].[CboardID],[Cboards].[Cname],[Posts].[PostID],[Posts].Title,[Posts].PostCotent, [Members].[NickName]
        //                FROM [MKForum].[dbo].[Posts]
        //                INNER JOIN [Members]
        //                ON [MKForum].[dbo].[Posts].[MemberID] = [MKForum].[dbo].[Members].[MemberID]
        //       INNER JOIN [Cboards]
        //       ON [MKForum].[dbo].[Cboards].[CboardID] = [MKForum].[dbo].[Posts].[CboardID]
        //       WHERE[NickName] LIKE '%@srchText%';
        //                ";

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            using (SqlCommand command = new SqlCommand(commandText, conn))
        //            {
        //                command.Parameters.AddWithValue("@srchText", srchText);

        //                conn.Open();
        //                SqlDataReader reader = command.ExecuteReader();

        //                List<SearchResult> srchList = new List<SearchResult>();

        //                //把取得的資料放進陣列
        //                while (reader.Read())
        //                {
        //                    SearchResult MatchData = new SearchResult()
        //                    {
        //                        PostID = (Guid)reader["PostID"],
        //                        NickName = reader["NickName"] as string,
        //                        Title = reader["Title"] as string,
        //                        PostCotent = reader["PostCotent"] as string,
        //                        Cname = reader["Title"] as string,
        //                        CboardID = (Guid)reader["CboardID"],
        //                        PostDate = (DateTime)reader["PostDate"],
        //                    };
        //                    srchList.Add(MatchData);
        //                }
        //                return srchList;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog("SearchManager.getWriterSrchList", ex);
        //        throw;
        //    }
        //}

        // -----------------------------New---------------------------------------
        public List<SearchResult> GetAllSearchKekka(List<string> hosii)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT PostID,Title,PostCotent,MemberID,CboardID,
                            LastEditTime,PostView,Members.Account
                    FROM Posts
                    INNER JOIN Members
                    ON Posts.MemberID = Members.MemberID
                    WHERE PostCotent 
                ";
            for (int i = 0; i < hosii.Count; i++)
            {
                if (i != hosii.Count - 1)
                    commandText += $"( LIKE '%'+@{hosii[i]}+'%' OR";
                else
                    commandText += $"( LIKE '%'+@{hosii[i]}+'%')";
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<SearchResult> srl = new List<SearchResult>();
                        connection.Open();
                        for (int i = 0; i < hosii.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@{hosii[i]}", hosii[i]);
                        }
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            SearchResult sr = new SearchResult()
                            {
                                PostID = (Guid)reader["PostID"],
                                Title = (string)reader["Title"],
                                PostCotent = (string)reader["PostCotent"],
                                MemberID = (Guid)reader["MemberID"],
                                CboardID = (int)reader["CboardID"],
                                Account = (string)reader["Account"],
                                LastEditTime = (DateTime)reader["LastEditTime"],
                                PostView = (int)reader["PostView"],
                            };
                            srl.Add(sr);
                        }
                        return srl;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetMemberFollowsMemberID", ex);
                throw;
            }
        }

    }
}