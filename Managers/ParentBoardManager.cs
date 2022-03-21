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
    //新增母版塊(驗證母版塊名稱不能重複)

    //頁面內容要顯示的是要從local db(SQL)取得排版順序

    //後台人員更改順序，傳回順序
    //------------------------


    public class ParentBoardManager
    {
        public DataTable GetDataTable()
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM [MKForum].[dbo].[PBoards]
                    
                    ";//取得目前有哪些母板塊
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Pname", typeof(string)));

            DataRow dataRow = dt.NewRow();
            dataRow["Pname"] = "Pboard";
            dataRow["PboardID"] = "PboardID";

            return dt;
        }

        /// <summary>
        /// 藉由Session的會員id導入local db(SQL)判別會員身分的方法，1是一般會員，2是版主，3是後台人員
        /// </summary>
        /// <returns>回傳身份別(int:MemberStatus)</returns>
        public int GetMemberStatus()
        {
            int memberStatus = 0;
            Member mmmember = HttpContext.Current.Session["Member"] as Member;
            Guid memberID = mmmember.MemberID;
            //string memberID = mmmember.MemberID.ToString();
            //Guid _memberID = (Guid)HttpContext.Current.Session["MemberAccount"];
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT [MemberStatus]
                    FROM [MKForum].[dbo].[Members]
                    WHERE MemberID= @memberID
                    ";//取得SQL會員id
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@memberID", memberID);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Member member = new Member();

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


        /// <summary>
        /// 寫入板塊名稱(string)，編號(int)，順序(List index)方法
        /// </summary>
        /// <param name="PboardList">帶入母版塊list</param>
        public void UpdatePBoardStatus(Pboard PboardModel)
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
                        //for(int i = 0;i< PboardList.Count;i++)
                        //{
                        //    command.Parameters.AddWithValue(@"PboardID", PboardModel[i].PboardID);
                        //    command.Parameters.AddWithValue(@"Pname", PboardModel[i].Pname);
                        //    command.Parameters.AddWithValue(@"PboardDate", PboardModel[i].PboardDate);
                        //    command.Parameters.AddWithValue(@"Porder", PboardModel[i].Porder);
                        //    command.Parameters.AddWithValue(@"Pshow", PboardModel[i].Pshow);
                        //    command.ExecuteNonQuery();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ParentBoardManager.SetPBoardStatus", ex);
                throw;
            }
        }

        /// <summary>
        /// 依母版ID填入母版順位的方法
        /// </summary>
        /// <param name="pboardModel">傳入值為母版的Model</param>
        public void UpdatePBoardOrder(Pboard pboardModel)
        {
            int order = pboardModel.Porder;
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                UPDATE [MKForum].[dbo].[PBoards]
                SET [PboardDate]=GETDATE(),
                    [Porder]='@Porder'
                WHERE [PboardID]='@PboardID'
                    ";//使用UPDATE更新母板塊
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                            command.Parameters.AddWithValue(@"PboardID", pboardModel.PboardID);
                            command.Parameters.AddWithValue(@"Porder", pboardModel.Porder);
                            command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ParentBoardManager.MoveUpPBoard", ex);
                throw;
            }
        }

        /// <summary>
        /// 依母版順位取得母版ID的方法
        /// </summary>
        /// <param name="pboardModel">傳入值為母版的Model</param>
        /// <returns>回傳母版ID(int)</returns>
        public int GetnPBoardOrder(Pboard pboardModel)
        {
            int pboardID=0;
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                SELECT [PboardID]
                FROM [MKForum].[dbo].[PBoards]
                WHERE [Porder]='@Porder'
                    ";//使用UPDATE更新母板塊
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
                            command.Parameters.AddWithValue(@"Porder", pboardModel.Porder);
                            pboardID = (int)reader["PboardID"];
                        }
                        return pboardID;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ParentBoardManager.GetMoveDownPBoard", ex);
                throw;
            }
        }

        /// <summary>
        /// 取得目前全部有哪些母板塊的DataTable(改成API之後應該會作廢)
        /// </summary>
        /// <returns>回傳值為DataTable</returns>
        public DataTable GetPBoardStatus()
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT [Pname],PboardID
                    FROM [MKForum].[dbo].[Pboards]
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

                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ParentBoardManager.GetPBoardStatus", ex);
                throw;
            }
        }

        public List<Pboard> GetListToAPI()
        {
            List<Pboard> PList =  GetPBoardList();
            return PList;
        }

        #region
        /// <summary>
        /// 取得板塊名稱(string)，編號(int)，順序(int)方法
        /// </summary>
        /// <returns>回傳list</returns>
        public static List<Pboard> GetPBoardList()
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT PboardID,Pname,Porder,Pshow
                    FROM [MKForum].[dbo].[Pboards]
                    ";//取得目前有哪些母板塊
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        List<Pboard> boardPropertyList = new List<Pboard>();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Pboard pb = new Pboard()
                            {
                                PboardID = (int)reader["PboardID"],
                                Pname = (string)reader["Pname"],
                                Porder = (int)reader["Porder"],
                                Pshow = (bool)reader["Pshow"]
                            };
                            boardPropertyList.Add(pb);
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

        /// <summary>
        /// 逐筆跑過List的資料
        /// </summary>
        /// <param name="Pname">傳入值為母版塊名稱</param>
        /// <returns></returns>
        public static Pboard GetPBoard(string Pname)//呼叫本頁的GetPBoardList方法,傳入ajax
        {
            foreach (Pboard pb in GetPBoardList())
            {
                if (string.Compare(pb.Pname, Pname, true) == 0) 
                return pb;
            }
            return null;
        }
        #endregion
        public Pboard Get(int id)
        {
            Pboard dbEntity = ParentBoardManager.GetPBoardList().Where(obj => obj.PboardID == id).FirstOrDefault();
            return dbEntity;
        }
        public List<Pboard> GetList()
        {
            return GetPBoardList();
        }
    }
}