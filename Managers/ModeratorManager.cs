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
        /// <summary>
        /// 增加黑名單
        /// </summary>
        /// <param name="strBlackedAcc">由版主輸入的會員資料</param>
        /// <param name="strRealseDate">懲處期限</param>
        /// <param name="cboardid">當前子板塊</param>
        public void AddModeratorsList(string strModeratorAcc,int cboardid)
        {
            try         //如果能寫入新的，如果不能就進catch刪除舊檔案
            {
                //寫入 當前板塊 會員ID  (SQL已測試OK)
                string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
                string commandText = $@"
                INSERT INTO [MKForum].[dbo].[MemberModerators]
                (CboardID,MemberID)
                VALUES  ('@cboardid','@strModeratorAcc')
                ";

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        using (SqlCommand command = new SqlCommand(commandText, conn))
                        {
                            conn.Open();
                            command.Parameters.AddWithValue("@strBlackedAcc", strModeratorAcc);
                            command.Parameters.AddWithValue("@cboardid", cboardid);
                        }
                    }
            }
            catch 
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
                            command.Parameters.AddWithValue("@cboardid", cboardid);
                        }
                    }
                }
                catch 
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 修改黑名單
        /// </summary>
        /// <param name="strBlackedAcc">由版主輸入的會員資料</param>
        /// <param name="strRealseDate">懲處期限</param>
        /// <param name="cboardid">當前子板塊</param>
        public void UpdateBlackedList(string strBlackedAcc, string strRealseDate, int cboardid)
        {
            //更新 當前日期 解黑日期 依照 當前板塊 會員ID (SQL未測試)
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                UPDATE [MKForum].[dbo].[MemberBlacks]
                SET [CreateDate]=' GETDATE()',[ReleaseDate]='@strRealseDate'
                WHERE [CboardID]='@cboardid' AND [MemberID]='@strBlackedAcc'
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@strBlackedAcc", strBlackedAcc);
                        command.Parameters.AddWithValue("@strRealseDate", strRealseDate);
                        command.Parameters.AddWithValue("@cboardid", cboardid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("BlackManager.AddBlackedList", ex);
                throw;
            }
        }

        //顯示黑名單:從資料庫讀取會員ID.會員暱稱.解黑日期(只顯示未過期的)(還需要重新整理)
        public List<MemberBlack> getModeratorsList(int cboardid)
        {

            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                SELECT [MemberID], [ReleaseDate]
                FROM [MKForum].[dbo].[MemberBlacks]
                WHERE CboardID= @currountCB AND ReleaseDate < GetDate()";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@currountCB", cboardid);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<MemberBlack> DisplayModerators = new List<MemberBlack>();


                        //把取得的資料放進陣列
                        while (reader.Read())
                        {
                            MemberBlack BlackedData = new MemberBlack()
                            {
                                MemberID = (Guid)reader["MemberID"],
                                ReleaseDate = (DateTime)reader["ReleaseDate"]
                            };
                            DisplayModerators.Add(BlackedData);

                        }
                        return DisplayModerators;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            //protected void MKForum_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
            //{

        }

    }
}