using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MKForum.Managers
{
    //接判斷使用者 id(NUMBER) 會被記錄在瀏覽器Session內 是什麼身份別 

    //頁面內容要顯示的是要從local db(SQL)取得排版順序
    //如果身份別是3，顯示管理頁面，否則不顯示(顯示內容由asp.net去實作)

    //後台人員更改順序，傳回順序
    //------------------------
    public class BlackManager
    {
        /// <summary>
        /// 增加黑名單
        /// </summary>
        /// <param name="strBlackedAcc">由版主輸入的會員資料</param>
        /// <param name="strRealseDate">懲處期限</param>
        /// <param name="cboardid">從session取得當前子板塊</param>
        public void AddBlackedList(string strBlackedAcc, string strRealseDate, int cboardid)
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

        /// <summary>
        /// 修改黑名單
        /// </summary>
        /// <param name="strBlackedAcc">由版主輸入的會員ID</param>
        /// <param name="strRealseDate">懲處期限</param>
        /// <param name="cboardid">當前子板塊</param>
        public void UpdateBlackedList(string strBlackedAcc, string strRealseDate, int cboardid)
        {
            //修改當前日期.解黑日期，依照當前板塊.會員ID (SQL未測試)
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

        /// <summary>
        /// 顯示黑名單
        /// </summary>
        /// <param name="cboardid">session當前子板塊</param>
        /// <returns></returns>
        public List<MemberBlack> getBlackedList(int cboardid)
        {
            List<MemberBlack> DisplayBlcked = new List<MemberBlack>();

            //更新 取得當前子板塊尚未解除懲處的名單 (SQL已測試OK)
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                SELECT [MemberID], [ReleaseDate]
                FROM [MKForum].[dbo].[MemberBlacks]
                WHERE CboardID= '@currountCB' AND getdate() < [ReleaseDate];";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@currountCB", cboardid);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

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
                Logger.WriteLog("BlackManager.getBlackedList", ex);
                throw;
            }

        }

    }
}