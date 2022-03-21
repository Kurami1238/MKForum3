using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
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
        /// 判斷會員是否是當前版主
        /// </summary>
        /// <returns>回傳值為bool</returns>
        public bool IsCurrentModerator()
        {
            bool isCurrentModerator=false;
            //string currentCboard = this.Request.QueryString["CboardID"];           //從URL取得當前CboardID
            string currentCboard = "2"; //測試用

            //如果有登入
            if (/*this._amgr.IsLogined()*/ true)        //測試用鮮註解
            {
                //取得登入帳號
                //string currentmember = HttpContext.Current.Session["MemberID"] ;
                string currentmember = "d5f7f8dc-0c02-42d9-8e92-ba6421f29020";

                //寫入 當前板塊 會員ID  (SQL已測試OK)
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
                                string MemberID = reader["MemberID"]as string;  //這粒不知道為什麼取失敗
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
        /// <param name="strModeratorAcc">由版主輸入的會員資料</param>
        /// <param name="cboardid">當前子板塊</param>
        public void AddModeratorsList(string strModeratorAcc, int cboardid)
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
        /// <param name="strModeratorAcc">欲刪的版主ID</param>
        /// <param name="cboardid">當前子板塊</param>
        public void DeleteModeratorsList(string strModeratorAcc, int cboardid)
        {
            //刪除一筆資料 依照 會員ID (SQL已測試OK)
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                DELETE FROM [MKForum].[dbo].[MemberModerators]
                WHERE MemberID='@strModeratorAcc'
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
                Logger.WriteLog("ModeratorManager.DeleteModeratorsList", ex);
                throw;
            }
        }

        /// <summary>
        /// 顯示版主清單
        /// </summary>
        /// <param name="cboardid">取得當前子板塊</param>
        /// <returns></returns>
        public List<MemberModerator> getModeratorsList(int cboardid)
        {

            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                SELECT [MemberID]
                FROM  [MKForum].[dbo].[MemberModerators]
                WHERE CboardID= @currountCB";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@currountCB", cboardid);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<MemberModerator> DisplayModerators = new List<MemberModerator>();


                        //把取得的資料放進陣列
                        while (reader.Read())
                        {
                            MemberModerator moderatorData = new MemberModerator()
                            {
                                MemberID = (Guid)reader["MemberID"],
                            };
                            DisplayModerators.Add(moderatorData);

                        }
                        return DisplayModerators;
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