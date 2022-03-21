using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MKForum.Managers
{
    public class PostStampsManager
    {
        public void InsertPostStamps(int CboardID, string PostSort, Guid MemberID)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    INSERT INTO PostStamps
                        (CboardID, PostSort, MemberID)
                    VALUES
                        (@CboardID, @PostSort, @MemberID)
                    ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue(@"CboardID", CboardID);
                        command.Parameters.AddWithValue(@"PostSort", PostSort);
                        command.Parameters.AddWithValue(@"MemberID", MemberID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.CreatePost", ex);
                throw;
            }
        }


        public List<PostStamp> GetPostStamps(int CboardID)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT PostStamps.CboardID,
                           PostStamps.PostSort,
                           PostStamps.PostStampsDate,
                           PostStamps.MemberID,
	                       Cboards.Cname
                    FROM PostStamps
                    JOIN Cboards
                    ON Cboards.CboardID = PostStamps.CboardID

                    WHERE PostStamps.CboardID = @CboardID;
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue(@"CboardID", CboardID);
                        SqlDataReader reader = command.ExecuteReader();
                        List<PostStamp> postStamps = new List<PostStamp>();
                        while (reader.Read())
                        {
                            PostStamp postStamp = new PostStamp()
                            {
                                CboardID = (int)reader["CboardID"],
                                PostSort = (string)reader["PostSort"],
                                PostStampsDate = (DateTime)reader["PostStampsDate"],
                                MemberID = (Guid)reader["MemberID"],
                                Cname = (string)reader["Cname"],
                            };
                            postStamps.Add(postStamp);
                        }
                        return postStamps;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.CreatePost", ex);
                throw;
            }
        }

    }
}