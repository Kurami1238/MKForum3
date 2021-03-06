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
	                    WHERE MemberID = @MemberID
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
                                LastEditTime = (DateTime)reader["LastEditTime"],
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
                return null;
            }
        }
        public int? GetReplied_count(string MemberID)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT COUNT(Replied) as RepliedCount, MemberID FROM MemberFollows
                    WHERE MemberID = @MemberID AND Replied = 0
                    GROUP BY MemberID
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@MemberID", MemberID);
                        SqlDataReader reader = command.ExecuteReader();

                        reader.Read();
                        //Post RepliedCount = new Post()
                        //{
                        //    RepliedCount = reader["RepliedCount"] as int?,
                        //    MemberID = (Guid)reader["MemberID"],
                        //};
                        //return RepliedCount;
                        int? RepliedCount = reader["RepliedCount"] as int?;
                        return RepliedCount;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("MemberFollowManager.GetReplied_POSTMemberFollows", ex);
                return null;
            }
        }

        public MemberFollow GetMemberFollowThisPost(Guid MemberID, Guid PostID)
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
                return null;
            }
        }

        public List<MemberFollow> GetALLMemberFollow()
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT * FROM MemberFollows
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<MemberFollow> allmemberfollow = new List<MemberFollow>();
                        while (reader.Read())
                        {
                            MemberFollow Follow = new MemberFollow()
                            {
                                MemberID = (Guid)reader["MemberID"],
                                PostID = (Guid)reader["PostID"],
                                FollowStatus = (bool)reader["FollowStatus"],
                                ReadedDate = (DateTime)reader["ReadedDate"],
                                Replied = (bool)reader["Replied"],
                            };
                            allmemberfollow.Add(Follow);
                        }

                        
                        return allmemberfollow;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("MemberFollowManager.GetMemberFollows", ex);
                throw;
            }
        }


        public void Updatetrack(Guid MemberID, Guid PostID, int FollowStatus)
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


        public void Createtrack(Guid MemberID, Guid PostID, int FollowStatus)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    INSERT INTO MemberFollows
                        (MemberID, PostID, FollowStatus, Replied)
                    VALUES
                        (@MemberID, @PostID, @FollowStatus, 0)

                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@MemberID", MemberID);
                        command.Parameters.AddWithValue("@PostID", PostID);
                        command.Parameters.AddWithValue("@FollowStatus", FollowStatus);
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

        public void UpdateReplied(string MemberID, string PostID, int Replied)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    UPDATE MemberFollows
                    SET Replied = @Replied
                    WHERE MemberID = @MemberID AND PostID = @PostID;
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@Replied", Replied);
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