using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MKForum.Managers
{
    public class CboardManager
    {
        public void CreateCboard(string pboardid, string cname, string cboardcotent)
        {

            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO Cboard
                    (PboardID, Cname, CboardCotent)
                    VALUES
                    (@pboardID, @cname, @cboardCotent)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue(@"pboardID", pboardid);
                        command.Parameters.AddWithValue(@"cname", cname);
                        command.Parameters.AddWithValue(@"cboardCotent", cboardcotent);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CboardManager,CreateCboard", ex);
                throw;
            }
        }
        public static Cboard GetCboard(int cboardid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM Cboards
                    WHERE CboardID = @cboardID";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText,conn))
                    {
                        command.Parameters.AddWithValue("@cboardID", cboardid);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            Cboard cboard = new Cboard()
                            {
                                CboardID = (int)reader["CboardID"],
                                PboardID = (int)reader["PboardID"],
                                Cname = (string)reader["Cname"],
                                CboardDate = (DateTime)reader["CboardDate"],
                                CboardCotent = (string)reader["CboardCotent"],
                            };
                            return cboard;
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CboardManager.GetCboard", ex);
                throw;
            }
        }
        public void UpdateCboard(int cboardid, string cname, string cboardcotent)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  UPDATE Cboards
                    SET 
                        Cname = @cname,
                        CboardCotent = @cboardcotent,
                    WHERE CboardID = @cboardid ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue(@"cboardID", cboardid);
                        command.Parameters.AddWithValue(@"cname", cname);
                        command.Parameters.AddWithValue(@"cboardcotent", cboardcotent);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CboardManager,UpdateCboard", ex);
                throw;
            }
        }
        public void DeleteCboard(int cboardid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM Cboards
                    WHERE CboardID = @cboardid ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue(@"cboardID", cboardid);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("cboardManager,DeleteCboard", ex);
                throw;
            }
        }


        //======================================================
        public static List<Cboard> GetCPboardtoCboard(int PboardID)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT 
	                Cboards.CboardID,
	                Cboards.PboardID,
	                Cboards.Cname,
	                Cboards.CboardDate,
	                Cboards.CboardCotent,
	                Pboards.Pname
                    FROM Cboards 
                    JOIN Pboards
                    ON Cboards.PboardID = Pboards.PboardID
                    WHERE Cboards.PboardID = @PboardID;
                ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@PboardID", PboardID);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<Cboard> cboards = new List<Cboard>();
                        while (reader.Read())
                        {
                            Cboard cboard = new Cboard()
                            {
                                CboardID = (int)reader["CboardID"],
                                PboardID = (int)reader["PboardID"],
                                Cname = (string)reader["Cname"],
                                CboardDate = (DateTime)reader["CboardDate"],
                                CboardCotent = (string)reader["CboardCotent"],
                                Pname = (string)reader["Pname"],
                            };
                            cboards.Add(cboard);
                        }
                        return cboards;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CboardManager.GetCPboardtoCboard", ex);
                throw;
            }
        }

        /// <summary>
        /// (API用)以母版ID取得子版List的方法
        /// </summary>
        /// <param name="PboardID">(int)輸入值為母版ID</param>
        /// <returns>回傳值為List</returns>
        public List<Cboard> GetCbFromPB(string PboardID)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT 
	                CboardID,
	                PboardID,
	                Cname,
	                CboardDate,
	                CboardCotent
                    FROM  [MKForum].[dbo].[Cboards]
                    WHERE PboardID = @PboardID;
                ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@PboardID", PboardID);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<Cboard> cboardList = new List<Cboard>();
                        while (reader.Read())
                        {
                            Cboard cboard = new Cboard()
                            {
                                CboardID = (int)reader["CboardID"],
                                Cname = (string)reader["Cname"],
                                CboardCotent = (string)reader["CboardCotent"],
                            };
                            cboardList.Add(cboard);
                        }
                        return cboardList;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CboardManager.GetCPboardtoCboard", ex);
                throw;
            }
        }


        /// <summary>
        /// (API用)依母版ID重新命名子板的方法
        /// </summary>
        /// <param name="cboardModel">傳入值為母版的Model</param>
        public void ReNameCB(Cboard cboardModel)
        {
            string strCboardID = cboardModel.CboardID.ToString();
            string strName = cboardModel.Cname.ToString();
            string strContent = cboardModel.CboardCotent.ToString();
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                UPDATE [MKForum].[dbo].[CBoards]
                SET  [Cname]=@cName ,[CboardCotent]=@cBoardCotent
                WHERE [CboardID]=@cBoardID
                    ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue(@"cBoardID", strCboardID);
                        command.Parameters.AddWithValue(@"cName", strName);
                        command.Parameters.AddWithValue(@"cBoardCotent", strContent);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ParentBoardManager.ReNameCB", ex);
                throw;
            }
        }

    }
}