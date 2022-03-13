using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MKForum.Managers
{
    public class BlackManager
    {
        /// <summary>
        /// 增加黑名單
        /// </summary>
        /// <param name="strBlackedAcc">由版主輸入的會員資料</param>
        /// <param name="strRealseDate">懲處期限</param>
        /// <param name="cboardid">當前子板塊</param>
        public void AddBlackedList(string strBlackedAcc, string strRealseDate, int cboardid)
        {
            try
            {
                //寫入 當前板塊 會員ID 當前日期 解黑日期 (SQL已測試OK)
                string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
                string commandText = $@"
                INSERT INTO [MKForum].[dbo].[MemberBlacks]
                (CboardID,MemberID, CreateDate, ReleaseDate)
                VALUES  ('@cboardid','@strBlackedAcc', GETDATE(),'@strRealseDate')
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
            catch (Exception ex)
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
        public List<MemberBlack> getBlackedList(int cboardid)
        {

            //取得現在時間(為了跟解黑時間比對)
            DateTime dtNow = DateTime.Now;

            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                SELECT [MemberID], [ReleaseDate]
                FROM [MKForum].[dbo].[MemberBlacks]
                WHERE CboardID= @currountCB AND ReleaseDate < {dtNow}";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@currountCB", cboardid);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<MemberBlack> DisplayBlcked = new List<MemberBlack>();

                        //把取得的資料放進陣列
                        while (reader.Read())
                        {
                            MemberBlack BlackedData = new MemberBlack()
                            {
                                MemberID = (Guid)reader["MemberID"],
                                ReleaseDate = (DateTime)reader["ReleaseDate"]
                            };
                            DisplayBlcked.Add(BlackedData);

                        }
                        return DisplayBlcked;
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