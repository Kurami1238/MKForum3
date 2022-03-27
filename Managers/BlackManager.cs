using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// 判斷是否有該帳號存在
        /// </summary>
        /// <param name="inpAccount">(string)輸入的帳號</param>
        /// <returns>回傳值為boolean</returns>
        public bool HasMember(string inpAccount)
        {
            bool hasMember = false;

            string connStr = ConfigHelper.GetConnectionString();
            string commandText = $@"
                SELECT Account
                FROM  [MKForum].[dbo].[Members]
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();

                        command.ExecuteNonQuery();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Member MM = new Member()
                            {
                                Account = reader["Account"] as string,
                            };
                            if (MM.Account == inpAccount)
                                hasMember = true;
                        }
                        return hasMember;
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.WriteLog("BlackManager.IsCurrentModerator", ex);
                throw;
            }
        }

        /// <summary>
        /// 判斷輸入的帳號是否為當前板主(輸入黑名單帳號時使用)
        /// </summary>
        /// <param name="currentCboard">(string)當前板塊</param>
        /// <param name="inpAccount">(string)輸入的帳號</param>
        /// <returns>回傳值為boolean</returns>
        public bool IsCurrentModerator(string currentCboard, string inpAccount)
        {
            bool isCurrentModerator = false;

            string connStr = ConfigHelper.GetConnectionString();
            string commandText = $@"
                SELECT Account
                FROM  [MKForum].[dbo].[MemberModerators]
				INNER JOIN [Members]
                ON [MKForum].[dbo].[Members].[MemberID] = [MKForum].[dbo].[MemberModerators].[MemberID]
                WHERE [CboardID] = @CboardID
                ";
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

                        while (reader.Read())
                        {
                            Member MM = new Member()
                            {
                                Account = reader["Account"] as string,
                            };
                            if (MM.Account == inpAccount)
                                isCurrentModerator = true;
                        }
                        return isCurrentModerator;
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.WriteLog("BlackManager.IsCurrentModerator", ex);
                throw;
            }
        }

        /// <summary>
        /// 增加黑名單
        /// </summary>
        /// <param name="strBlackedAcc">(string)由版主輸入的會員資料</param>
        /// <param name="strRealseDate">(string)懲處期限</param>
        /// <param name="cboardid">(string)從session取得當前子板塊</param>
        public void AddBlackedList(string strBlackedAcc, string strRealseDate, string cboardid)
        {

            string connStr = ConfigHelper.GetConnectionString();
            string commandText = @"
                INSERT INTO [MKForum].[dbo].[MemberBlacks]
                ([CboardID],[Account], [CreateDate], [ReleaseDate])
                VALUES  (@cboardid,@strBlackedAcc, GETDATE(),@strRealseDate)
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
                        command.ExecuteNonQuery();
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
        /// 判斷會員是否已存在於黑名單的方法(不包含已經解黑的會員)
        /// </summary>
        /// <param name="account">(string)欲比對的會員帳號</param>
        /// <param name="currontCB">(string)當前子板塊</param>
        /// <returns>回傳值為boolean</returns>
        public bool IsBlacked(string account, string currontCB)
        {
            bool isBlacked = false;
            string dbAccount;
            string connStr = ConfigHelper.GetConnectionString();
            string commandText = $@"
                SELECT [Account]
                FROM [MKForum].[dbo].[MemberBlacks]

                WHERE CboardID= @currontCB AND getdate() < [ReleaseDate] ;
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
                return isBlacked = false;
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
        /// <param name="strBlackedAcc">(string)由版主輸入的會員ID</param>
        /// <param name="strRealseDate">(string)懲處期限</param>
        /// <param name="cboardid">(string)當前子板塊</param>
        public void UpdateBlackedList(string strBlackedAcc, string strRealseDate, string cboardid)
        {
            string connStr = "Server=localhost;Database=MKForum;Integrated Security=True;";
            string commandText = $@"
                UPDATE [MKForum].[dbo].[MemberBlacks]
                SET [CreateDate]= GETDATE(),[ReleaseDate]=@strRealseDate
                WHERE [CboardID]=@cboardid AND Account=@strBlackedAcc
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
                        command.ExecuteNonQuery();

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
        /// 顯示黑名單測試list
        /// </summary>
        /// <param name="cboardid">(string)session當前子板塊</param>
        /// <returns>回傳值為DataTable</returns>
        public List<MemberBlack> getBlackedList(string currentCboard)
        {
            List<MemberBlack> blackedList = new List<MemberBlack>();
            string connStr = ConfigHelper.GetConnectionString();
            string commandText = $@"
                SELECT  [ReleaseDate],[Account]
                FROM [MKForum].[dbo].[MemberBlacks]
                WHERE CboardID= @currontCB AND getdate() < [ReleaseDate];";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        conn.Open();
                        command.Parameters.AddWithValue("@currontCB", currentCboard);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            DateTime dbRleaseDate = (DateTime)reader["ReleaseDate"];
                            string strRleaseDate = dbRleaseDate.ToString("D");
                            DateTime releaseDate = Convert.ToDateTime(strRleaseDate);
                            MemberBlack memberBlacked = new MemberBlack()
                            {
                                Account = reader["Account"] as string,
                                ReleaseDate = releaseDate
                            };

                            blackedList.Add(memberBlacked);
                        }

                        //DataTable dt = new DataTable();

                        //dt.Load(reader);

                        return blackedList;

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("BlackManager.getBlackedList", ex);
                throw;
            }

        }



        /// <summary>
        /// 顯示黑名單 (改成List作廢)
        /// </summary>
        /// <param name="cboardid">(string)session當前子板塊</param>
        /// <returns>回傳值為DataTable</returns>
        public DataTable getBlacked(string currentCboard)//改成list作廢
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText = $@"
                SELECT  [ReleaseDate],[Account]
                FROM [MKForum].[dbo].[MemberBlacks]
                WHERE CboardID= @currontCB AND getdate() < [ReleaseDate];";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        conn.Open();
                        command.Parameters.AddWithValue("@currontCB", currentCboard);
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
                Logger.WriteLog("BlackManager.getBlacked", ex);
                throw;
            }

        }



    }
}