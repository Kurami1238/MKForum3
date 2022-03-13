using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MKForum.Managers
{
    public class MemberFollowManager
    {
        public List<Post> GetReplied_POSTMemberFollows(string MemberID) //, string DateTime
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT  DISTINCT --
                            Posts.PostID, Posts.MemberID, CboardID, PointID,
		                    PostDate, PostView, Title, Floor,
		                    PostCotent, LastEditTime, MemberFollows.Replied
                     FROM Posts

                     JOIN MemberFollows
	                    ON MemberFollows.PostID = Posts.PostID

                    WHERE MemberFollows.PostID in 
                    (
	                    SELECT PostID FROM MemberFollows
	                    WHERE MemberID = 'c8142d85-68c2-4483-ab51-e7d3fc366b89'
	                    AND Replied = 0
                    )

                    ORDER BY Replied ASC, LastEditTime DESC, PostDate DESC
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@MemberID", MemberID);
                        //command.Parameters.AddWithValue("@DateTime", DateTime);
                        SqlDataReader reader = command.ExecuteReader();
                        List<Post> PostFollows = new List<Post>();

                        while (reader.Read())
                        {
                            Post PostFollow = new Post()
                            {
                                PostID = (Guid)reader["PostID"],
                                MemberID = (Guid)reader["MemberID"],
                                CboardID = (int)reader["CboardID"],
                                PointID = reader["PointID"] as Guid?,
                                PostDate = (DateTime)reader["PostDate"],
                                PostView = (int)reader["PostView"],
                                Title = reader["Title"] as string,
                                PostCotent = reader["PostCotent"]as string,
                                LastEditTime = reader["LastEditTime"] as DateTime?,
                                Replied = (bool)reader["Replied"],
                                Floor = (int)reader["Floor"]
                            };
                            PostFollows.Add(PostFollow);
                        }
                        return PostFollows;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("MemberFollowManager.GetReplied_POSTMemberFollows", ex);
                throw;
            }
        }

        public MemberFollow GetMemberFollowThisPost(string MemberID, string PostID)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT * FROM MemberFollows
                    WHERE MemberID = @MemberID AND PostID = @PostID;
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@MemberID", MemberID);
                        command.Parameters.AddWithValue("@PostID", PostID);
                        SqlDataReader reader = command.ExecuteReader();

                        reader.Read();

                        MemberFollow Follow = new MemberFollow()
                        {
                            MemberID = (Guid)reader["MemberID"],
                            PostID = (Guid)reader["PostID"],
                            FollowStatus = (bool)reader["FollowStatus"],
                            ReadedDate = (DateTime)reader["ReadedDate"],
                            Replied = (bool)reader["Replied"],
                        };
                        return Follow;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("MemberFollowManager.GetMemberFollows", ex);
                throw;
            }
        }

        public void Updatetrack(string MemberID, string PostID, int FollowStatus)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    UPDATE MemberFollows
                    SET FollowStatus = @FollowStatus
                    WHERE MemberID = @MemberID AND PostID = @PostID;
                ";
            try
            {
                using(SqlConnection connection = new SqlConnection(connStr))
                {
                    using(SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@FollowStatus", FollowStatus);
                        command.Parameters.AddWithValue("@MemberID", MemberID);
                        command.Parameters.AddWithValue("@PostID", PostID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Updatetrack", ex);
                throw;
            }
        }
    }
}