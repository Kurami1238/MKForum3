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
        /// <param name="currentUrl">輸入型別為string，請輸入網址列的querystring</param>
        /// <returns>回傳值為string</returns>
        public string GetCurrentPage(string currentUrl)
        {
            string current ="";
            if (currentUrl.Replace(".aspx", "") != null)
                current += currentUrl.Replace(".aspx", "");
            return current;
        }

        /// <summary>(SQL未完成/還需確認網址列原則)判斷當前是否位於母版塊</summary>
        /// <param name="currentPage">輸入的當前板塊值</param>
        /// <returns></returns>
        public bool IsInPBoard(string currentPage)
        {
            bool isInPBoard = false;
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";            //連線字串
            string commandText = @"
                                    把網址丟進SQL裡驗證是否有這個屬性
                        ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        //逐一比對是否有符合關鍵字的資料
                        while (reader.Read())
                        {
                            if (currentPage == reader["Pname"] as string)
                                isInPBoard = true;
                        }
                        return isInPBoard;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SearchManager.IsInPBoard", ex);
                throw;
            }


        }

        /// <summary>(SQL未完成/還需確認網址列原則)判斷當前是否位於子版塊</summary>
        /// <param name="currentPage">輸入的當前板塊值</param>
        /// <returns></returns>
        public bool IsInCBoard(string currentPage)
        {
            bool isInCBoard = false;
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";            //連線字串
            string commandText = @"
                                    把網址丟進SQL裡驗證是否有這個屬性
                        ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        //逐一比對是否有符合關鍵字的資料
                        while (reader.Read())
                        {
                            if (currentPage == reader["Pname"] as string)
                                isInCBoard = true;
                        }
                        return isInCBoard;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SearchManager.IsInCBoard", ex);
                throw;
            }
        }


        /// <summary>取得全站搜尋的搜尋結果List</summary>
        /// <param name="srchText">使用者輸入的關鍵字</param>
        /// <returns>回傳值為SearchResult的List</returns>
        public List<SearchResult> GetAllSrchList(string srchText)
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


        /// <summary>取得當前母版搜尋的搜尋結果List(SQL需修改)</summary>
        /// <param name="srchText">使用者輸入的關鍵字</param>
        /// <param name="currentBoard">當前所在的母板塊</param>
        /// <returns>回傳值為SearchResult的List</returns>
        public List<SearchResult> GetPboardSrchList(string srchText, string currentBoard)
        {
            //找當前母板塊內的 文章標題 或 文章內容 或 作者名稱 需修改)
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
                        command.Parameters.AddWithValue("@cboardid", currentBoard);

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

        /// <summary>取得當前子版搜尋的搜尋結果List</summary>
        /// <param name="srchText">使用者輸入的關鍵字</param>
        /// <param name="currentBoard">當前所在的子板塊</param>
        /// <returns>回傳值為SearchResult的List</returns>
        public List<SearchResult> GetCboardSrchList(string srchText, string currentBoard)
        {
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
                        command.Parameters.AddWithValue("@cboardid", currentBoard);

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

        /// <summary>取得作者搜尋的搜尋結果List</summary>
        /// <param name="srchText">使用者輸入的關鍵字</param>
        /// <returns>回傳值為SearchResult的List</returns>
        public List<SearchResult> GetWriterSrchList(string srchText)
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