using MKForum.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MKForum.Managers
{
    public class StampManager
    {

        /// <summary>
        /// 依照CboardID取得該子版的文章類型的方法
        /// </summary>
        /// <param name="currentCboard">(string)傳入值為session當前子板塊</param>
        /// <returns>回傳值為DataTable</returns>
        public DataTable getStamp(string currentCboard)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText = $@"
                SELECT  [PostSort]
                FROM [PostStamps]
                WHERE CboardID= @CboardID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        conn.Open();
                        command.Parameters.AddWithValue("@CboardID", currentCboard);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        DataTable dt = new DataTable();

                        dt.Load(reader);

                        return dt;

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("BlackManager.getStamp", ex);
                throw;
            }

        }

        /// <summary>
        /// 判斷是否包含文章類型的方法
        /// </summary>
        /// <param name="strIinpStp">(string)欲比對的文章類型名稱</param>
        /// <param name="currontCB">(string)當前子板塊</param>
        /// <returns>回傳值為boolean</returns>
        public bool IncludeStp(string strIinpStp, string currontCB)
        {
            bool includeStp = false;
            string dbPostSort;
            string connStr = ConfigHelper.GetConnectionString();
            string commandText = $@"
                SELECT [PostSort]
                FROM [PostStamps]
                WHERE CboardID= @currontCB ;
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        conn.Open();
                        command.Parameters.AddWithValue("@currontCB", currontCB);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            dbPostSort = reader["PostSort"] as string;
                            if (strIinpStp == dbPostSort)
                                return includeStp = true;
                        }
                    }
                }
                return includeStp = false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("BlackManager.IsBlacked", ex);
                throw;
            }
        }


        /// <summary>
        /// 增加文章類型
        /// </summary>
        /// <param name="strBlackedAcc">(string)由版主輸入的文章類型</param>
        /// <param name="cboardid">(string)從session取得當前子板塊</param>
        public void AddStmp(string postSort, string cboardid)
        {

            string connStr = ConfigHelper.GetConnectionString();
            string commandText = @"
                INSERT INTO [PostStamps]
                ([CboardID],[PostSort])
                VALUES  (@cboardID,@postSort)
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@cboardID", cboardid);
                        command.Parameters.AddWithValue("@postSort", postSort);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("BlackManager.AddStmp", ex);
                throw;
            }


        }


        /// <summary>
        /// 刪除文章類型的方法
        /// </summary>
        /// <param name="postSort">(string)由版主輸入的文章類型</param>
        /// <param name="cboardid">(string)從session取得當前子板塊</param>
        public void DeleteStmp(string postSort, string cboardid)
        {
            //刪除一筆資料 依照 會員ID (SQL已測試OK)
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                DELETE FROM [PostStamps]
                WHERE cboardID=@cboardid AND PostSort=@postSort
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@postSort", postSort);
                        command.Parameters.AddWithValue("@cboardid", cboardid);
                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ModeratorManager.DeleteStmp", ex);
                throw;
            }
        }

    }
}