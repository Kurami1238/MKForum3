using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MKForum.Managers
{
    //GetMemberStatus:接判斷使用者 id(NUMBER) 會被記錄在瀏覽器Session內，輸出身份別(3:後台人員) 
    //如果身份別是3，顯示管理頁面，否則不顯示(顯示內容由asp.net去實作)


    //頁面內容要顯示的是要從local db(SQL)取得排版順序

    //後台人員更改順序，傳回順序
    //------------------------


    public class ParentBoardManager
    {
        // private static List<string> _memberStatusList = new List<string>();

        //GetMemberStatus()，藉由Session的會員id導入local db(SQL)判斷身分別的方法，回傳身份別(int:MemberStatus)
        public int GetMemberStatus()
        {
            int memberStatus = 0;
            Guid _memberID = (Guid)HttpContext.Current.Session["MemberAccount"];
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT [MemberStatus]
                    FROM [MKForum].[dbo].[Members]
                    WHERE MemberID='@memberID'
                    ";//取得SQL會員id
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Member member = new Member();
                            command.Parameters.AddWithValue("@memberID", _memberID);

                            memberStatus = (int)reader["MemberStatus"]; //身份別由Guid轉型為int
                            
                        }
                        return memberStatus;

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ParentBoardManager.GetMemberStatus", ex);
                throw;
            }
        }

        //GetPBoardStatus()，取得板塊名稱(string)，編號(int)，順序(List index)方法，回傳list class
        public List<Pboard> GetPBoardStatus()//取得目前全部有哪些母板塊的list
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM [MKForum].[dbo].[PBoards]
                    
                    ";//取得目前有哪些母板塊
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Pboard> boardPropertyList = new List<Pboard>();
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Pboard bp = new Pboard()
                            {
                                PboardID = (int)reader["PboardID"],
                                Pname = (string)reader["Pname"],
                                Porder = (int)reader["Porder"],
                                Pshow = (bool)reader["Pshow"],
                            };
                            boardPropertyList.Add(bp);
                        }
                        return boardPropertyList;

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ParentBoardManager.GetPBoardStatus", ex);
                throw;
            }
        }

        //SetPBoardStatus，寫入板塊名稱(string)，編號(int)，順序(List index)方法，帶入list class
        public void SetPBoardStatus(List<Pboard> PboardList)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                UPDATE [MKForum].[dbo].[PBoards]
                SET [Pname]='@Pname',[PboardDate]=GETDATE(),
                    [Porder]='@Porder',[Pshow]='@Pshow',
                WHERE [PboardID]='@PboardID'
                    ";//使用UPDATE更新母板塊
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        for(int i = 0;i< PboardList.Count;i++)
                        {
                            command.Parameters.AddWithValue(@"PboardID", PboardList[i].PboardID);
                            command.Parameters.AddWithValue(@"Pname", PboardList[i].Pname);
                            command.Parameters.AddWithValue(@"PboardDate", PboardList[i].PboardDate);
                            command.Parameters.AddWithValue(@"Porder", PboardList[i].Porder);
                            command.Parameters.AddWithValue(@"Pshow", PboardList[i].Pshow);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ParentBoardManager.SetPBoardStatus", ex);
                throw;
            }
        }
    }
}