﻿using MKForum.Helpers;
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
        public void CreateCboard(Pboard pboardid, string cname, string cboardcotent)
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

    }
}