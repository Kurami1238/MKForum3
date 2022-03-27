using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MKForum.Managers
{
    public class ModeratorManager
    {
        private AccountManager _amgr = new AccountManager();
        private Member _currentmember;
        /// <summary>
        /// 判斷會員是否是當前版主(session版測試正常)
        /// </summary>
        /// <returns>回傳值為bool</returns>
        public bool IsCurrentModerator(string currentCboard)
        {
            bool isCurrentModerator=false;

            //如果有登入
            if (this._amgr.IsLogined()) 
            {
                //取得登入帳號
                string currentmember = HttpContext.Current.Session["MemberID"].ToString();

                string connStr = ConfigHelper.GetConnectionString();
                string commandText = $@"
                SELECT MemberID
                FROM  [MKForum].[dbo].[MemberModerators]
                WHERE [CboardID] = @CboardID
                ";
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        using (SqlCommand command = new SqlCommand(commandText, conn))
                        {
                            conn.Open();

                            command.Parameters.AddWithValue(@"CboardID", currentCboard);
                            command.ExecuteNonQuery();
                            SqlDataReader reader = command.ExecuteReader();

                            while (reader.Read())
                            {
                                MemberModerator MM = new MemberModerator()
                                {
                                    MemberID = (Guid)reader["MemberID"],
                                };
                                string MemberID =MM.MemberID.ToString();
                                if (MemberID == currentmember)
                                    isCurrentModerator = true;
                            }
                            return isCurrentModerator;
                        }
                    }
                }

                catch (Exception ex)
                {
                    Logger.WriteLog("ModeratorManager.AddModeratorsList", ex);
                    throw;
                }
            }
            return isCurrentModerator;
        }

        /// <summary>
        /// 增加版主
        /// </summary>
        /// <param name="strModeratorAcc">(string)由版主輸入的會員資料</param>
        /// <param name="cboardid">(string)當前子板塊</param>
        public void AddModeratorsList(string strModeratorAcc, string cboardid)
        {

            //寫入 當前板塊 會員ID  (SQL已測試OK)
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                INSERT INTO [MKForum].[dbo].[MemberModerators]
                (CboardID,MemberID)
                VALUES  ('@cboardid','@strModeratorAcc')
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@strModeratorAcc", strModeratorAcc);
                        command.Parameters.AddWithValue("@cboardid", cboardid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ModeratorManager.AddModeratorsList", ex);
                throw;
            }
        }

        /// <summary>
        /// 刪除版主
        /// </summary>
        /// <param name="strModeratorAcc">(string)欲刪的版主ID</param>
        /// <param name="cboardid">(string)當前子板塊</param>
        public void DeleteModeratorsList(string strModeratorAcc, string cboardid)
        {
            //刪除一筆資料 依照 會員ID (SQL已測試OK)
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                DELETE FROM [MKForum].[dbo].[MemberModerators]
                WHERE MemberID=@strModeratorAcc
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@strModeratorAcc", strModeratorAcc);
                        command.Parameters.AddWithValue("@cboardid", cboardid);
                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ModeratorManager.DeleteModeratorsList", ex);
                throw;
            }
        }

        /// <summary>
        /// 顯示版主清單
        /// </summary>
        /// <param name="cboardid">(string)session當前子板塊</param>
        /// <returns>回傳值為DataTable</returns>
        public DataTable getModerators(string currentCboard)
        {

            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                SELECT [Members].[Account]
                FROM  [MKForum].[dbo].[MemberModerators]
				INNER JOIN [Members]
				ON [Members].[MemberID] = [MemberModerators].[MemberID]
                WHERE CboardID= @currentCboard ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        conn.Open();
                        command.Parameters.AddWithValue("@currentCboard", currentCboard);
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
                Logger.WriteLog("ModeratorManager.getModeratorsList", ex);
                throw;
            }
        }


    }
}