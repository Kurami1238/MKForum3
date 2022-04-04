using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Linq;

namespace MKForum.Managers
{

    public class PostManager
    {
        private static List<string> _msgList = new List<string>();
        public void CreatePost(Guid member, int cboard, Post post, out Guid postid)
        {

            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    INSERT INTO Posts
                    (PostID, MemberID, PostView, CboardID, Title, PostCotent, Floor, CoverImage, SortID, LastEditTime, PostDate)
                    VALUES
                    (@postID, @memberID, @postView, @cboardID, @title, @postCotent, @floor, @coverimage, @sortID, @lastedittime, @postdate)
                    ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        post.PostID = Guid.NewGuid();
                        DateTime time = DateTime.Now;
                        connection.Open();
                        command.Parameters.AddWithValue(@"postID", post.PostID);
                        command.Parameters.AddWithValue(@"memberID", member);
                        command.Parameters.AddWithValue(@"postView", 0);
                        command.Parameters.AddWithValue(@"cboardID", cboard);
                        command.Parameters.AddWithValue(@"title", post.Title);
                        command.Parameters.AddWithValue(@"postCotent", post.PostCotent);
                        command.Parameters.AddWithValue(@"floor", 1);
                        command.Parameters.AddWithValue(@"coverimage", post.CoverImage);
                        command.Parameters.AddWithValue(@"sortID", post.SortID);
                        command.Parameters.AddWithValue(@"lastedittime", time);
                        command.Parameters.AddWithValue(@"postdate", time);
                        command.ExecuteNonQuery();
                        postid = post.PostID;

                        this.CreateInMemberFollows(member, postid);  // 增加至會員的追蹤表
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.CreatePost", ex);
                throw;
            }
        }
        public void CreatePost(Guid member, Guid pointtid, Post post, out Guid postid)
        {
            Post pointpost = this.GetPost(pointtid);   // 取發文的標題
            List<Post> pointpostlist = this.GetPostpointNowFloor(pointtid); // 搜那篇回文數有多少
            int floor;
            if (pointpostlist.Count > 0)
                floor = pointpostlist.Count + 2;
            else
                floor = 2;
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    INSERT INTO Posts
                    (PostID, MemberID, PointID, CboardID, PostView, Title, PostCotent, Floor, LastEditTime, PostDate)
                    VALUES
                    (@postID, @memberID, @pointID, @cboardID, @postView, @title, @postCotent, @floor, @lastedittime, @postdate)
                    ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        post.PostID = Guid.NewGuid();
                        DateTime time = DateTime.Now;
                        connection.Open();
                        command.Parameters.AddWithValue(@"postID", post.PostID);
                        command.Parameters.AddWithValue(@"memberID", member);
                        command.Parameters.AddWithValue(@"pointID", pointtid);
                        command.Parameters.AddWithValue(@"cboardID", post.CboardID);
                        command.Parameters.AddWithValue(@"postView", 0);
                        command.Parameters.AddWithValue(@"title", pointpost.Title);
                        command.Parameters.AddWithValue(@"postCotent", post.PostCotent);
                        command.Parameters.AddWithValue(@"floor", floor);
                        command.Parameters.AddWithValue(@"postdate", time);
                        command.Parameters.AddWithValue(@"lastedittime", time);


                        command.ExecuteNonQuery();
                        postid = post.PostID;
                    }
                }
                // 查追蹤表有無追蹤，沒有才增加
                MemberFollow mf = this.GetMemberFollowThisPost(member, pointtid);
                if (mf == null)
                    this.CreateInMemberFollows(member, pointtid);  // 增加至會員的追蹤表
                // 取追蹤該文章的會員
                List<MemberFollow> followlist = this.GetMemberFollowsMemberID(pointtid);
                // 如果追蹤者有發文者自己則刪去
                for (int i = 0; i < followlist.Count; i++)
                {
                    if (string.Compare(member.ToString(), followlist[i].MemberID.ToString()) == 0)
                    {
                        followlist.Remove(followlist[i]);
                    }
                }
                //var fl = followlist.Select((x, index) => { return new { Index = index }; });
                if (followlist.Count() > 0)
                    this.RepliedtoNO(followlist, pointtid); // 讓追蹤原文的會員狀態都改為未讀
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.CreatePost", ex);
                throw;
            }
        }
        public void CreatePostImageList(Guid memberid, string imagepath)
        {

            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    INSERT INTO  PostImgLists
                    (MemberID, ImagePath)
                    VALUES
                    (@memberID, @imagepath)
                    ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue(@"memberID", memberid);
                        command.Parameters.AddWithValue(@"imagepath", imagepath);
                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.CreatePostImageList", ex);
                throw;
            }
        }
        public string GetImage(Guid memberid)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT * FROM PostImgLists
                    WHERE MemberID = @memberID
                    ORDER BY ID DESC
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        string imagepath = string.Empty;
                        connection.Open();
                        command.Parameters.AddWithValue("@memberID", memberid);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            imagepath = reader["ImagePath"] as string;
                        }
                        if (!string.IsNullOrEmpty(imagepath))
                            return imagepath;
                        else
                            return string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetImage", ex);
                throw;
            }
        }
        public List<Post> GetPostpointNowFloor(Guid pointid)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT * FROM Posts
                    WHERE PointID = @pointID
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Post> pointList = new List<Post>();
                        connection.Open();
                        command.Parameters.AddWithValue("@pointID", pointid);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Post po = new Post()
                            {
                                MemberID = (Guid)reader["MemberID"],
                                PostID = (Guid)reader["PostID"],
                                PostDate = (DateTime)reader["PostDate"],
                            };
                            pointList.Add(po);
                        }

                        return pointList;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPostNowFloor", ex);
                throw;
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
        public void RepliedtoNO(List<MemberFollow> member, Guid postid)
        {
            string Zyouken = " WHERE  ";
            string connectionString = ConfigHelper.GetConnectionString();

            for (int i = 0; i < member.Count; i++)
            {
                if (i != member.Count - 1)
                    Zyouken += $"(MemberID = @{i} AND PostID = @postid) OR ";
                else
                    Zyouken += $"(MemberID = @{i} AND PostID = @postid)";
            }
            string commandText =
                $@"UPDATE MemberFollows
                  SET Replied = 0
                  {Zyouken} ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        for (int i = 0; i < member.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@{i}", member[i].MemberID);
                        }
                        command.Parameters.AddWithValue("@postid", postid);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.RepliedtoNO", ex);
                throw;
            }
        }
        public void CreateInMemberFollows(Guid member, Guid postid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    INSERT INTO MemberFollows
                    (MemberID, PostID, FollowStatus, Replied)
                    VALUES
                    (@memberID, @postID, @followStatus, @replied)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue(@"memberID", member);
                        command.Parameters.AddWithValue(@"postID", postid);
                        command.Parameters.AddWithValue(@"followStatus", 1);
                        command.Parameters.AddWithValue(@"replied", 1);
                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.CreateInMemberFollows", ex);
                throw;
            }
        }
        public List<MemberFollow> GetMemberFollowsMemberID(Guid postid)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT * FROM MemberFollows
                    WHERE PostID = @postID
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<MemberFollow> Follows = new List<MemberFollow>();
                        connection.Open();

                        command.Parameters.AddWithValue("@postID", postid);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {

                            MemberFollow Follow = new MemberFollow()
                            {
                                MemberID = (Guid)reader["MemberID"],
                                //PostID = (Guid)reader["PostID"],
                                //FollowStatus = (bool)reader["FollowStatus"],
                                //ReadedDate = (DateTime)reader["ReadedDate"],
                                //Replied = (bool)reader["Replied"],
                            };
                            for (int i = 0; i < Follows.Count; i++)
                            {
                                if (string.Compare(Follow.MemberID.ToString(), Follows[i].MemberID.ToString()) == 0)
                                {
                                    Follows.Remove(Follows[i]);
                                }
                            }
                            Follows.Add(Follow);
                        }
                        return Follows;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetMemberFollowsMemberID", ex);
                throw;
            }
        }
        public List<Post> GetPostList(int cboardid, int pageSize, int pageIndex, out int totalRows)
        {
            int skip = pageSize * (pageIndex - 1); // 計算跳頁數
            if (skip < 0)
                skip = 0;
            string whereCondition = "AND CboardID = @cboardID";
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT TOP {pageSize} * 
                    FROM Posts
                    WHERE 
                        PointID IS NULL AND 
                        PostID NOT IN 
                            ( 
                            SELECT TOP {skip} PostID
                            FROM Posts
                            WHERE PointID IS NULL AND CboardID = @cboardID
                            ORDER BY LastEditTime DESC
                            )
                        {whereCondition}
                        ORDER BY LastEditTime DESC
                ";
            string commandCountText =
                $@"  SELECT COUNT(PostID)
                    FROM Posts
                    WHERE PointID IS NULL 
                    {whereCondition}";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Post> postList = new List<Post>();
                        connection.Open();
                        command.Parameters.AddWithValue("@cboardID", cboardid);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Post po = this.BuildPostContent(reader);
                            postList.Add(po);
                        }
                        reader.Close();

                        // 取得總筆數
                        // 因為使用同一個command，不同的查詢，必須使用不同的參數集合
                        command.Parameters.Clear();
                        command.CommandText = commandCountText;
                        command.Parameters.AddWithValue("@cboardID", cboardid);

                        totalRows = (int)command.ExecuteScalar();
                        // command.ExecuteScalar 只會回傳一個資料 為Object
                        return postList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPostList", ex);
                throw;
            }
        }
        public List<Post> GetPostList(int cboardid, int pageSize, int pageIndex, int sortid, out int totalRows)
        {
            int skip = pageSize * (pageIndex - 1); // 計算跳頁數
            if (skip < 0)
                skip = 0;
            string whereCondition = "AND CboardID = @cboardID AND SortID = @sortID";
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT TOP {pageSize} * 
                    FROM Posts
                    WHERE 
                        PointID IS NULL AND 
                        PostID NOT IN 
                            ( 
                            SELECT TOP {skip} PostID
                            FROM Posts
                            WHERE PointID IS NULL AND ( CboardID = @cboardID AND SortID = @sortID )
                            ORDER BY LastEditTime DESC
                            )
                        {whereCondition}
                        ORDER BY LastEditTime DESC
                ";
            string commandCountText =
                $@"  SELECT COUNT(PostID)
                    FROM Posts
                    WHERE PointID IS NULL 
                    {whereCondition}";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Post> postList = new List<Post>();
                        connection.Open();
                        command.Parameters.AddWithValue("@cboardID", cboardid);
                        command.Parameters.AddWithValue("@sortID", sortid);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Post po = this.BuildPostContent(reader);
                            postList.Add(po);
                        }
                        reader.Close();

                        // 取得總筆數
                        // 因為使用同一個command，不同的查詢，必須使用不同的參數集合
                        command.Parameters.Clear();
                        command.CommandText = commandCountText;
                        command.Parameters.AddWithValue("@cboardID", cboardid);
                        command.Parameters.AddWithValue("@sortID", sortid);

                        totalRows = (int)command.ExecuteScalar();
                        // command.ExecuteScalar 只會回傳一個資料 為Object
                        return postList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPostList", ex);
                throw;
            }
        }
        public List<Post> GetPostListmoto(int cboardid)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT * 
                    FROM Posts
                    WHERE  PointID IS NULL AND CboardID = @cboardID 
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<Post> postList = new List<Post>();
                        connection.Open();
                        command.Parameters.AddWithValue("@cboardID", cboardid);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Post po = this.BuildPostContent(reader);
                            postList.Add(po);
                        }
                        return postList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPostList", ex);
                throw;
            }
        }
        public Post GetPost(Guid postid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM Posts
                    WHERE PostID = @postID";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@postID", postid);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            Post post = this.BuildPostContent(reader);
                            this.UpdatePostView(post.PostID, post.PostView + 1);
                            return post;
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPost", ex);
                throw;
            }
        }
        public Post GetPost(Guid postid, Guid memberid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM Posts
                    WHERE PostID = @postID";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@postID", postid);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            Post post = this.BuildPostContent(reader);
                            this.UpdatePostView(post.PostID, post.PostView + 1);
                            MemberScan ms = this.GetMemberScanwithAdd1day(postid, memberid);
                            if (ms == null)
                                this.CreateMemberScan(postid, memberid);
                            else
                                this.UpdateMemberScan(ms.ScanID);
                            return post;
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPost", ex);
                throw;
            }
        }
        public List<Post> GetPostListwithPoint(Guid pointID)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM Posts
                    WHERE PointID = @pointID
                    ORDER BY Floor
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@pointID", pointID);
                        conn.Open();
                        List<Post> pointList = new List<Post>();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Post po = this.BuildPostContent(reader);
                            pointList.Add(po);
                        }
                        return pointList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPostwithPoint", ex);
                throw;
            }
        }
        private Post BuildPostContent(SqlDataReader reader)
        {
            return new Post()
            {
                PostID = (Guid)reader["PostID"],
                MemberID = (Guid)reader["MemberID"],
                CboardID = (int)reader["CboardID"],
                PointID = reader["PointID"] as Guid?,
                PostDate = (DateTime)reader["PostDate"],
                PostView = (int)reader["PostView"],
                Title = (string)reader["Title"],
                PostCotent = (string)reader["PostCotent"],
                LastEditTime = (DateTime)reader["LastEditTime"],
                Floor = (int)reader["Floor"],
                CoverImage = reader["CoverImage"] as string,
                SortID = reader["SortID"] as int?
            };
        }
        public void UpdatePostView(Guid postid, int postview)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  UPDATE Posts
                    SET 
                        PostView = @postview
                    WHERE PostID = @postid ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue(@"postID", postid);
                        command.Parameters.AddWithValue(@"postview", postview);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.UpdatePostView", ex);
                throw;
            }
        }
        public void UpdatePost(Post post)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  UPDATE Posts
                    SET 
                        Title = @title,
                        PostCotent = @postcotent,
                        LastEditTime = @lastedittime,
                        CoverImage = @coverimage,
                        SortID = @sortid
                    WHERE PostID = @postID ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue(@"postID", post.PostID);
                        command.Parameters.AddWithValue(@"title", post.Title);
                        command.Parameters.AddWithValue(@"postcotent", post.PostCotent);
                        command.Parameters.AddWithValue(@"lastedittime", DateTime.Now);
                        command.Parameters.AddWithValue(@"coverimage", post.CoverImage);
                        command.Parameters.AddWithValue(@"sortid", post.SortID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.UpdatePost", ex);
                throw;
            }
        }
        public void DeleteMemberScan(List<MemberScan> msl, Guid postid)
        {
            string Zyouken = " WHERE  ";
            for (int i = 0; i < msl.Count; i++)
            {
                //  @{i}是因為 Guid一堆毛
                if (i != msl.Count - 1)
                    Zyouken += $" (MemberID = @{i} AND PostID = @postid) OR ";
                else
                    Zyouken += $" (MemberID = @{i} AND PostID = @postid) ";
            }
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                $@" DELETE FROM MemberScans
                    {Zyouken} ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        for (int i = 0; i < msl.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@{i}", msl[i].MemberID);
                        }
                        command.Parameters.AddWithValue(@"postid", postid);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.DeleteMemberFollows", ex);
                throw;
            }
        }
        public void DeleteMemberFollows(List<MemberFollow> followlist, Guid postid)
        {
            string Zyouken = " WHERE  ";
            for (int i = 0; i < followlist.Count; i++)
            {
                //  @{i}是因為 Guid一堆毛
                if (i != followlist.Count - 1)
                    Zyouken += $" (MemberID = @{i} AND PostID = @postid) OR ";
                else
                    Zyouken += $" (MemberID = @{i} AND PostID = @postid) ";
            }
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                $@" DELETE FROM MemberFollows
                    {Zyouken} ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        for (int i = 0; i < followlist.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@{i}", followlist[i].MemberID);
                        }
                        command.Parameters.AddWithValue(@"postid", postid);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.DeleteMemberFollows", ex);
                throw;
            }
        }
        public void DeletePost(Guid postid)
        {
            // 透過postid找 有追蹤的memberid 
            List<MemberFollow> followlist = this.GetMemberFollowsMemberID(postid);
            // 比對postid與memberid 兩者皆符合者 刪除追蹤狀態
            if (followlist.Count > 0)
                this.DeleteMemberFollows(followlist, postid);
            // 透過postid找 這篇文的會員瀏覽紀錄
            List<MemberScan> msl = this.GetMemberScanList(postid);
            // 比對postid與memberid 兩者皆符合者 刪除會員瀏覽紀錄
            if (msl.Count > 0)
                this.DeleteMemberScan(msl, postid);
            // 刪有關連性資料後才刪除文章本體
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM Posts
                    WHERE PostID = @postid ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue(@"postid", postid);
                        command.ExecuteNonQuery();
                        // 之後刪除此文的回文
                        this.DeletePoint(postid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.DeletePost", ex);
                throw;
            }
        }
        public void DeletePoint(Guid postid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM Posts
                    WHERE PointID = @postid ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue(@"postid", postid);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.DeletePoint", ex);
                throw;
            }
        }
        public bool CheckInput(string titletext, string postcotenttext)
        {
            _msgList = new List<string>();
            List<string> msgList = new List<string>();

            if (titletext.Length < 1)
                msgList.Add("請輸入標題。");
            if (postcotenttext.Length < 1)
                msgList.Add("請輸入內文。");
            if (titletext.Length > 100)
                msgList.Add("標題字數請小於一百中文字。");
            if (postcotenttext.Length > 4096)
                msgList.Add("內文字數請小於兩千中文字。");
            //if (KinkiNoKotoba(titletext, postcotenttext) == true)
            //    _msgList = msgList;
            //else
            //    // 失敗就額外加禁字提示 還沒完成
            //    _msgList = msgList;
            if (msgList.Count > 0)
            {
                _msgList = msgList;
                return false;
            }
            return true;
        }
        public List<string> GetmsgList()
        {
            return _msgList;
        }
        public string GetmsgText()
        {
            List<string> errlist = this.GetmsgList();
            string allError = string.Join("<br/>", errlist);
            return allError;
        }
        static bool KinkiNoKotoba(string titletext, string postcotenttext)
        {
            // 比對標題及內文與禁字表是否有重疊
            return true;
        }
        public List<MemberBlack> GetCboardBlackList(int cboardid)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT * 
                    FROM MemberBlacks
                    WHERE ReleaseDate > GETDATE() AND CboardID = @cboardid
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<MemberBlack> blackList = new List<MemberBlack>();
                        connection.Open();
                        command.Parameters.AddWithValue("@cboardID", cboardid);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            MemberBlack bl = new MemberBlack()
                            {
                                Account = (string)reader["Account"],
                            };
                            blackList.Add(bl);
                        }
                        return blackList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetCboardBlackList", ex);
                throw;
            }
        }
        public List<MemberModerator> GetModeratorList(int cboardid)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT * 
                    FROM MemberModerators
                    WHERE CboardID = @cboardid
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<MemberModerator> MrList = new List<MemberModerator>();
                        connection.Open();
                        command.Parameters.AddWithValue("@cboardID", cboardid);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            MemberModerator mr = new MemberModerator()
                            {
                                MemberID = (Guid)reader["MemberID"],
                            };
                            MrList.Add(mr);
                        }
                        return MrList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetModeratorList", ex);
                throw;
            }
        }


        //----------------Htag--------------------
        public void CreateHashtag(Guid postid, string htag)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    INSERT INTO PostHashtags
                    (PostID, Naiyo)
                    VALUES
                    (@postID, @naiyo)
                    ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue(@"postID", postid);
                        command.Parameters.AddWithValue(@"naiyo", htag);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.CreateHashtag", ex);
                throw;
            }
        }
        public List<PostHashtag> GetPostHashtagList(Guid postid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM PostHashtags
                    WHERE PostID = @postID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@postID", postid);
                        conn.Open();
                        List<PostHashtag> phtl = new List<PostHashtag>();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            PostHashtag pht = new PostHashtag
                            {
                                Naiyo = reader["Naiyo"] as string,
                            };
                            phtl.Add(pht);
                        }
                        return phtl;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPostHashtagList", ex);
                throw;
            }
        }
        //----------------Stamp-------------------
        public List<PostStamp> GetPostStampList(int cboardid)
        {
            string connectionStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT * FROM PostStamps
                    WHERE CboardID = @cboardID
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<PostStamp> stampList = new List<PostStamp>();
                        connection.Open();
                        command.Parameters.AddWithValue("@cboardID", cboardid);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            PostStamp ps = new PostStamp()
                            {
                                SortID = (int)reader["SortID"],
                                CboardID = (int)reader["CboardID"],
                                PostSort = reader["PostSort"] as string
                            };
                            stampList.Add(ps);
                        }
                        return stampList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPostStampList", ex);
                throw;
            }
        }
        public List<Post> GetPostListwithStamp(int sortid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM Posts
                    WHERE SortID = @sortID
                    ORDER BY LastEditTime DESC
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@sortID", sortid);
                        conn.Open();
                        List<Post> pointList = new List<Post>();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Post po = this.BuildPostContent(reader);
                            pointList.Add(po);
                        }
                        return pointList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetPostListwithStamp", ex);
                throw;
            }
        }
        //----------------Scans-------------------
        public void CreateMemberScan(Guid postid, Guid memberid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    INSERT INTO MemberScans
                    (MemberID, PostID, ScanDate)
                    VALUES
                    (@memberID, @postID, @scandate)
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        DateTime time = DateTime.Now;
                        connection.Open();
                        command.Parameters.AddWithValue(@"memberID", memberid);
                        command.Parameters.AddWithValue(@"postID", postid);
                        command.Parameters.AddWithValue(@"scandate", time);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.CreateMemberScan", ex);
                throw;
            }
        }
        public MemberScan GetMemberScanwithAdd1day(Guid postid, Guid memberid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT *
                    FROM MemberScans
                    WHERE PostID = @postID AND MemberID = @memberid 
                            AND DATEADD(day,1,ScanDate) > GETDATE()
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue(@"memberID", memberid);
                        command.Parameters.AddWithValue(@"postID", postid);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            MemberScan ms = new MemberScan()
                            {
                                ScanID = (int)reader["ScanID"],

                            };
                            return ms;
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetMemberScan", ex);
                throw;
            }
        }
        public List<MemberScan> GetMemberScanList(Guid postid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"
                    SELECT *
                    FROM MemberScans
                    WHERE PostID = @postID 
                ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        List<MemberScan> msl = new List<MemberScan>();
                        connection.Open();
                        command.Parameters.AddWithValue(@"postID", postid);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            MemberScan ms = new MemberScan()
                            {
                                MemberID = (Guid)reader["MemberID"],
                            };
                            msl.Add(ms);
                        }
                        return msl;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.GetMemberScan", ex);
                throw;
            }
        }
        public void UpdateMemberScan(int scanid)
        {
            string connectionString = ConfigHelper.GetConnectionString();
            string commandText =
                @"  UPDATE MemberScans
                    SET 
                        ScanDate = @scandate
                    WHERE ScanID = @scanID ";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        DateTime time = DateTime.Now;
                        connection.Open();

                        command.Parameters.AddWithValue(@"scanID", scanid);
                        command.Parameters.AddWithValue(@"scandate", time);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostManager.UpdateMemberScan", ex);
                throw;
            }
        }

    }
}