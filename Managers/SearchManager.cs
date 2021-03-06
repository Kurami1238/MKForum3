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
        public List<SearchResult> GetAllSearchKekka(List<string> hosii, out int totalRows)
        {
            string Zyouken = " WHERE  ";
            string connectionStr = ConfigHelper.GetConnectionString();
            for (int i = 0; i < hosii.Count; i++)
            {
                if (i != hosii.Count - 1)
                    Zyouken += $" (PostCotent LIKE '%'+@{hosii[i]}+'%' OR Title LIKE '%'+@{hosii[i]}+'%' ) OR";
                else
                    Zyouken += $" (PostCotent LIKE '%'+@{hosii[i]}+'%' OR Title LIKE '%'+@{hosii[i]}+'%' ) ";
            }
            string commandText =
                $@"
                    SELECT PostID,Title,PostCotent,Posts.MemberID,CboardID,
                            LastEditTime,PostView,Members.Account,CoverImage,Floor,PointID
                    FROM Posts
                    INNER JOIN Members
                    ON Posts.MemberID = Members.MemberID
                    {Zyouken}
                    ORDER BY LastEditTime DESC
                ";
            string commandCountText = $@"SELECT COUNT(PostID)
                                        FROM Posts
                                        {Zyouken}";
            
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
                            SearchResult sr = this.BuildSearchKekka(reader);
                            if (sr != null)
                                srl.Add(sr);
                        }
                        reader.Close();
                        command.Parameters.Clear();
                        command.CommandText = commandCountText;
                        for (int i = 0; i < hosii.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@{hosii[i]}", hosii[i]);
                        }
                        totalRows = (int)command.ExecuteScalar();
                        if (srl == null)
                        {
                            return new List<SearchResult>();
                        }
                        return srl;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetAllSearchKekka", ex);
                throw;
            }
        }
        public List<SearchResult> GetMemberSearchKekka(List<string> hosii,out int totalRows)
        {
            string Zyouken = " WHERE  ";
            string connectionStr = ConfigHelper.GetConnectionString();
            for (int i = 0; i < hosii.Count; i++)
            {
                if (i != hosii.Count - 1)
                    Zyouken += $" Members.Account = @{hosii[i]} OR ";
                else
                    Zyouken += $" Members.Account = @{hosii[i]} ";
            }
            string commandText =
                $@"
                    SELECT PostID,Title,PostCotent,Posts.MemberID,CboardID,
                            LastEditTime,PostView,Members.Account,CoverImage,Floor,PointID
                    FROM Posts
                    INNER JOIN Members
                    ON Posts.MemberID = Members.MemberID
                    {Zyouken}
                    ORDER BY LastEditTime DESC
                ";
            string commandCountText = $@"SELECT COUNT(PostID)
                                        FROM Posts
                                        INNER JOIN Members
                                        ON Posts.MemberID = Members.MemberID
                                        {Zyouken}";
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
                            SearchResult sr = this.BuildSearchKekka(reader);
                            if (sr != null)
                                srl.Add(sr);
                        }
                        reader.Close();
                        command.Parameters.Clear();
                        command.CommandText = commandCountText;
                        for (int i = 0; i < hosii.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@{hosii[i]}", hosii[i]);
                        }
                        totalRows = (int)command.ExecuteScalar();
                        if (srl == null)
                        {
                            return new List<SearchResult>();
                        }
                        return srl;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetMemberSearchKekka", ex);
                throw;
            }
        }
        public List<SearchResult> GetTagSearchKekka(List<string> hosii, out int totalRows)
        {
            // 透過關鍵字得到postID清單
            List<Post> pl = this.SearchPostIDwithTag(hosii);
            if (pl.Count == 0)
            {
                totalRows = 0;
                return new List<SearchResult>();
            }
            string Zyouken = " WHERE  ";
            string fkfk = string.Empty;
            string connectionStr = ConfigHelper.GetConnectionString();
            for (int i = 0; i < pl.Count; i++)
            {
                //  @{i}是因為 Guid一堆毛
                if (i != pl.Count - 1)
                    Zyouken += $" PostID = @{i} OR ";
                else
                    Zyouken += $" PostID = @{i} ";
            }
            string commandText =
                $@"
                    SELECT PostID,Title,PostCotent,Posts.MemberID,CboardID,
                            LastEditTime,PostView,Members.Account,CoverImage,Floor,PointID
                    FROM Posts
                    INNER JOIN Members
                    ON Posts.MemberID = Members.MemberID
                    {Zyouken}
                    ORDER BY LastEditTime DESC
                ";
            string commandCountText = $@"SELECT COUNT(PostID)
                                        FROM Posts
                                        INNER JOIN Members
                                        ON Posts.MemberID = Members.MemberID
                                        {Zyouken}";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<SearchResult> srl = new List<SearchResult>();
                        connection.Open();
                        for (int i = 0; i < pl.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@{i}", pl[i].PostID);
                        }

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            SearchResult sr = this.BuildSearchKekka(reader);
                            if (sr != null)
                                srl.Add(sr);
                        }
                        reader.Close();
                        command.Parameters.Clear();
                        command.CommandText = commandCountText;
                        for (int i = 0; i < pl.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@{i}", pl[i].PostID);
                        }
                        totalRows = (int)command.ExecuteScalar();
                        if (srl == null)
                        {
                            return new List<SearchResult>();
                        }
                        return srl;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetMemberSearchKekka", ex);
                throw;
            }
        }
        public List<Post> SearchPostIDwithTag(List<string> tag)
        {
            string Zyouken = " WHERE  ";
            for (int i = 0; i < tag.Count; i++)
            {
                if (i != tag.Count - 1)
                    Zyouken += $" (Naiyo LIKE '%'+@{tag[i]}+'%') OR";
                else
                    Zyouken += $" (Naiyo LIKE '%'+@{tag[i]}+'%') ";
            }
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM PostHashtags
                    {Zyouken}
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Post> pl = new List<Post>();
                        connection.Open();
                        for (int i = 0; i < tag.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@{tag[i]}", tag[i]);
                        }
                        
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Post cb = new Post()
                            {
                                PostID = (Guid)reader["PostID"],
                            };
                            pl.Add(cb);
                        }
                        return pl;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.SearchPostIDwithTag", ex);
                throw;
            }
        }
        public List<SearchResult> GetboardSearchKekka(List<string> hosii, string board, string pORc,out int totalRows)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string ruru = @"(SELECT PostID 
                             FROM Posts 
                             WHERE ";
            for (int i = 0; i < hosii.Count; i++)
            {
                if (i != hosii.Count - 1)
                    ruru += $" (PostCotent LIKE '%'+@{hosii[i]}+'%' OR Title LIKE '%'+@{hosii[i]}+'%' ) OR";
                else
                    ruru += $" (PostCotent LIKE '%'+@{hosii[i]}+'%' OR Title LIKE '%'+@{hosii[i]}+'%' ))";
            }
            string pluszyoukenP = @" WHERE PostID IN (SELECT PostID 
                                     FROM Posts 
                                     WHERE ";
            string pluszyoukenC = $@" WHERE PostID IN {ruru}
                                      AND CboardID = @board ";
            string commandText = string.Empty;
            string commandCountText = string.Empty;
            if (string.Compare(pORc, "p") == 0)
            {
                List<Cboard> cblist = this.GetPbnoCb(board);
                if (cblist.Count == 0)
                {
                    totalRows = 0;
                    return new List<SearchResult>();
                }
                for (int i = 0; i < cblist.Count; i++)
                {
                    if (i != cblist.Count - 1)
                        pluszyoukenP += $" (CboardID = {cblist[i].CboardID} AND PostID IN {ruru} ) OR ";
                    else
                        pluszyoukenP += $" (CboardID = {cblist[i].CboardID} AND PostID IN {ruru} ))";
                }
                commandText = $@"
                                SELECT PostID,Title,PostCotent,Posts.MemberID,CboardID,
                                    LastEditTime,PostView,Members.Account,CoverImage,Floor,PointID
                                FROM Posts
                                INNER JOIN Members
                                ON Posts.MemberID = Members.MemberID
                                 {pluszyoukenP}
                                ORDER BY LastEditTime DESC
                                ";
                commandCountText = $@"SELECT COUNT(PostID)
                                      FROM Posts
                                      {pluszyoukenP}";
            }
            if (string.Compare(pORc, "c") == 0)
            {
                commandText = $@"
                                SELECT PostID,Title,PostCotent,Posts.MemberID,CboardID,
                                    LastEditTime,PostView,Members.Account,CoverImage,Floor,PointID
                                FROM Posts
                                INNER JOIN Members
                                ON Posts.MemberID = Members.MemberID
                                {pluszyoukenC} 
                                ORDER BY LastEditTime DESC
                                ";
                commandCountText = $@"SELECT COUNT(PostID)
                                      FROM Posts
                                      {pluszyoukenC}";
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
                        if (string.Compare(pORc, "c") == 0)
                            command.Parameters.AddWithValue($"@board", board);

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            SearchResult sr = this.BuildSearchKekka(reader);
                            if (sr != null)
                                srl.Add(sr);
                        }
                        reader.Close();
                        command.Parameters.Clear();
                        command.CommandText = commandCountText;
                        for (int i = 0; i < hosii.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@{hosii[i]}", hosii[i]);
                        }
                        if (string.Compare(pORc, "c") == 0)
                            command.Parameters.AddWithValue($"@board", board);
                        totalRows = (int)command.ExecuteScalar();
                        if (srl == null)
                        {
                            return new List<SearchResult>();
                        }
                        return srl;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetboardSearchKekka", ex);
                throw;
            }
        }
        public List<Cboard> GetPbnoCb(string pbid)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT *
                    FROM Cboards
                    WHERE PboardID = @pboardID
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Cboard> cbid = new List<Cboard>();
                        connection.Open();
                        command.Parameters.AddWithValue("@pboardID", pbid);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Cboard cb = new Cboard()
                            {
                                CboardID = (int)reader["CboardID"],
                            };
                            cbid.Add(cb);
                        }
                        return cbid;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPbnoCb", ex);
                throw;
            }
        }
        private SearchResult BuildSearchKekka(SqlDataReader reader)
        {
            return new SearchResult()
            {
                PostID = (Guid)reader["PostID"],
                Title = (string)reader["Title"],
                PostCotent = (string)reader["PostCotent"],
                MemberID = (Guid)reader["MemberID"],
                CboardID = (int)reader["CboardID"],
                MemberAccount = (string)reader["Account"],
                LastEditTime = (DateTime)reader["LastEditTime"],
                PostView = (int)reader["PostView"],
                CoverImage = reader["CoverImage"] as string,
                Floor = (int)reader["Floor"],
                PointID = reader["PointID"] as Guid?,
            };
        }
        public List<SearchResult> GetMemberSearchKekkaUPUP(List<string> hosii, Guid memberid)
        {
            string pluszyouken = @"SELECT PostID 
                                   FROM Posts 
                                   WHERE ";
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT PostID,Title,PostCotent,Posts.MemberID,CboardID,
                            LastEditTime,PostView,Members.Account,CoverImage
                    FROM Posts
                    INNER JOIN Members
                    ON Posts.MemberID = Members.MemberID
                    WHERE PostsID.MemberID = @memberID AND 
                          PostID IN
                          ({pluszyouken}) 
                ";

            for (int i = 0; i < hosii.Count; i++)
            {
                if (i != hosii.Count - 1)
                    pluszyouken += $" PostCotent LIKE '%'+@{hosii[i]}+'%' OR";
                else
                    pluszyouken += $" PostCotent LIKE '%'+@{hosii[i]}+'%' ";
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
                        command.Parameters.AddWithValue($"@memberID", memberid);

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
                                MemberAccount = (string)reader["Account"],
                                LastEditTime = (DateTime)reader["LastEditTime"],
                                PostView = (int)reader["PostView"],
                                CoverImage = reader["CoverImage"] as string,
                            };
                            if (sr != null)
                                srl.Add(sr);
                            else
                                return new List<SearchResult>();
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