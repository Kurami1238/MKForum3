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
        public void AddBlackedList(string strBlackedAcc, string strRealseDate, string cboardid)
        {

            //寫入 當前板塊 會員ID 當前日期 解黑日期 (SQL已測試OK)
            string connStr = ConfigHelper.GetConnectionString();
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
        /// 判斷會員是否已存在於黑名單的方法(包含已經解黑的會員)
        /// </summary>
        /// <param name="strBlackedAcc">欲比對的會員帳號</param>
        /// <param name="cboardid"></param>
        /// <returns></returns>
        public bool IsBlacked(string account, string currontCB)
        {
            bool isBlacked = false;
            string dbAccount;
            string connStr = ConfigHelper.GetConnectionString();
            string commandText = $@"
                SELECT [Account]
                FROM [MKForum].[dbo].[MemberBlacks]
                INNER JOIN [Members]
                ON [MKForum].[dbo].[Members].[MemberID] = [MKForum].[dbo].[MemberBlacks].[MemberID]
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
                            dbAccount = reader["Account"] as string;
                            if (account == dbAccount)
                                return isBlacked = true;
                        }
                    }
                }
                    return isBlacked=false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("BlackManager.IsBlacked", ex);
                throw;
            }
        }

        /// <summary>
        /// 修改黑名單
        /// </summary>
        /// <param name="strBlackedAcc">由版主輸入的會員ID</param>
        /// <param name="strRealseDate">懲處期限</param>
        /// <param name="cboardid">當前子板塊</param>
        public void UpdateBlackedList(string strBlackedAcc, string strRealseDate, string cboardid)
        {
            //修改當前日期.解黑日期，依照當前板塊.會員ID (SQL未測試)
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                UPDATE [MKForum].[dbo].[MemberBlacks]
                SET [CreateDate]=' GETDATE()',[ReleaseDate]=@strRealseDate
                WHERE [CboardID]=@cboardid AND [MemberID]=@strBlackedAcc
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
        public DataTable getBlacked(int cboardid)
        {
            string strcboardid = cboardid.ToString();

            //更新 取得當前子板塊尚未解除懲處的名單 (SQL已測試OK)
            string connStr = ConfigHelper.GetConnectionString();
            string commandText = $@"
                SELECT  [ReleaseDate],[Account]
                FROM [MKForum].[dbo].[MemberBlacks]
                INNER JOIN [Members]
                ON [MKForum].[dbo].[Members].[MemberID] = [MKForum].[dbo].[MemberBlacks].[MemberID]
                WHERE CboardID= @currontCB AND getdate() < [ReleaseDate];";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        List<MemberBlack> DisplayBlcked = new List<MemberBlack>();
                        conn.Open();
                        command.Parameters.AddWithValue("@currontCB", strcboardid);
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
                Logger.WriteLog("BlackManager.getBlackedList", ex);
                throw;
            }

        }



    }
}