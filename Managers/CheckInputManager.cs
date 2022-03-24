using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MKForum.Managers
{
    public class CheckInputManager
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
                Logger.WriteLog("CheckInputManager.IncludeBanWord", ex);
                throw;
            }
        }

        /// <summary>
        /// 判斷字串文字是否皆為英數
        /// </summary>
        /// <param name="str">(string)檢測的字串</param>
        /// <param name="outAccount">(string)可用於輸出alert的字串，若boolean結果為false，輸出值將為空字串</param>
        /// <returns>回傳值為boolean</returns>
        public bool IsNumAndEG(string str, out string outAccount)
        {
            Regex NumandEG = new Regex("[^A-Za-z0-9]");
            bool result = !NumandEG.IsMatch(str);

            outAccount = "";
            if (result)
                outAccount = str;
            return result;
        }
    }
}