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
        /// 確認搜尋的關鍵字不可為屏蔽字
        /// </summary>
        /// <param name="checkString">輸入參數為string</param>
        /// <returns>回傳值為Boolean，true為包含禁字，false為不包含禁字</returns>
        public bool IncludeBanWord(string inpText)
        {
            string inpString = inpText.Trim();   //使用者輸入的字串
            bool isBanWord = false;             //是否含有禁字，預設為false(沒有禁字)

            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT [禁字欄位]
                    FROM [MKForum].[dbo].[禁字表]
                    ";  //逐一取得資料庫的禁字(不建List)

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<SearchResult> srchList = new List<SearchResult>();

                        //逐一比對資料庫的禁字
                        while (reader.Read())
                        {
                            string BanWord = reader["禁字欄位"] as string;

                            isBanWord = inpString.Contains(BanWord);  //checkString：輸入字串，BanWord：禁字
                        }

                        return isBanWord;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SearchManager.IncludeBanWord", ex);
                throw;
            }


            //errorMsgList = new List<string>();

            //if (string.IsNullOrWhiteSpace(this.txtTitle.Text))
            //    errorMsgList.Add("標題為必填。");

            //if (string.IsNullOrWhiteSpace(this.txtBody.Text))
            //    errorMsgList.Add("內文為必填。");

            //if (string.IsNullOrWhiteSpace(this.txtLongitude.Text))
            //    errorMsgList.Add("經度為必填。");

            //if (string.IsNullOrWhiteSpace(this.txtLatitude.Text))
            //    errorMsgList.Add("緯度為必填。");

            //if (!this._isEditMode)  // 只有新增模式，才做封面圖的必填
            //{
            //    if (!this.fuCoverImage.HasFile)
            //        errorMsgList.Add("封面圖為必填。");
            //}

            //double temp;
            //if (!double.TryParse(this.txtLongitude.Text.Trim(), out temp))
            //    errorMsgList.Add("經度須介於 -180~180 度，精度允許六位小數。");
            //else if (temp < -180 || temp > 180)
            //    errorMsgList.Add("經度須介於 -180~180 度，精度允許六位小數。");

            //if (!double.TryParse(this.txtLatitude.Text.Trim(), out temp))
            //    errorMsgList.Add("緯度須介於 -90~90 度，精度允許六位小數。");
            //else if (temp < -90 || temp > 90)
            //    errorMsgList.Add("緯度須介於 -90~90 度，精度允許六位小數。");

            //if (errorMsgList.Count > 0)
            //    return false;
            //else
            //    return true;
        }

        /// <summary>
        /// 檢查搜尋輸入的參數
        /// </summary>
        /// <param name="errorMsgList">回傳值為字串List</param>
        /// <returns></returns>

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