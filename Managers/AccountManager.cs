using MKForum.Helpers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MKForum.Managers
{
    public class AccountManager
    {
        public Member GetAccount(string account)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM Members
                    WHERE Account = @account ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@account", account);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Member member = new Member()
                            {
                                MemberID = (Guid)reader["MemberID"],
                                Account = reader["Account"] as string,
                                Password = reader["Password"] as string,
                                Email = reader["Email"] as string,
                                Salt = reader["Salt"] as byte[]

                            };

                            return member;
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountMember.GetAccount(string account)", ex);
                throw;
            }
        }
        public Member GetAccount(Guid id)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM Members
                    WHERE MemberID = @id ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Member member = new Member()
                            {
                                MemberID = (Guid)reader["MemberID"],
                                Account = reader["Account"] as string,
                                Password = reader["Password"] as string
                            };

                            return member;
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountMember.GetAccount(Guid id)", ex);
                throw;
            }
        }
        
        public void CreateAccount(Member member)
        {

            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  
                    INSERT INTO Members
                        (MemberStatus, Account, Password, Email, NickName, Birthday, Sex, PicPath, Salt)
                    VALUES
                        (@MemberStatus, @Account, @Password, @Email, @NickName, @Birthday, @Sex, @PicPath, @Salt)
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@MemberStatus", member.MemberStatus);
                        command.Parameters.AddWithValue("@Account", member.Account);
                        command.Parameters.AddWithValue("@Password", member.Password);
                        command.Parameters.AddWithValue("@Email", member.Email);
                        command.Parameters.AddWithValue("@NickName", member.NickName);
                        command.Parameters.AddWithValue("@Birthday", member.Birthday);
                        command.Parameters.AddWithValue("@Sex", member.Sex);
                        command.Parameters.AddWithValue("@PicPath", member.PicPath);
                        command.Parameters.AddWithValue("@Salt", member.Salt);

                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountMember.CreateAccount", ex);
                throw;
            }
        }
        public void UpdateAccount(Member member)
        {
            // 1. 判斷資料庫是否有相同的 Account
            if (this.GetAccount(member.Account) == null)
                throw new Exception("帳號不存在：" + member.Account);

            // 2. 編輯資料
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  UPDATE Members
                    SET 
                        PWD = @pwd
                    WHERE
                        ID = @id ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@id", member.MemberID);
                        command.Parameters.AddWithValue("@pwd", member.Password);

                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountMember.UpdateAccount(Member member)", ex);
                throw;
            }
        }

        public void DeleteAccounts(List<Guid> ids)
        {
            // 1. 判斷是否有傳入 id
            if (ids == null || ids.Count == 0)
                throw new Exception("需指定 id");

            List<string> param = new List<string>();
            for (var i = 0; i < ids.Count; i++)
            {
                param.Add("@id" + i);
            }
            string inSql = string.Join(", ", param);    // @id1, @id2, @id3, etc...

            // 2. 刪除資料
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@" DELETE Members
                    WHERE MemberID IN ({inSql}) ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        for (var i = 0; i < ids.Count; i++)
                        {
                            command.Parameters.AddWithValue("@id" + i, ids[i]);
                        }

                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("MapContentManager.GetMapList", ex);
                throw;
            }
        }

        public List<Member> GetAccountList(string keyword)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM Members ";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                commandText += " WHERE Account LIKE '%'+@keyword+'%'";
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(keyword))
                        {
                            command.Parameters.AddWithValue("@keyword", keyword);
                        }

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<Member> list = new List<Member>();

                        while (reader.Read())
                        {
                            Member member = new Member()
                            {
                                MemberID = (Guid)reader["MemberID"],
                                Account = reader["Account"] as string
                            };

                            list.Add(member);
                        }

                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountMember.List<Member> GetAccountList(string keyword)", ex);
                throw;
            }
        }


        private LoginHelper _lgihp = new LoginHelper();
        private EncryptionHelper _encryption = new EncryptionHelper();

        public bool TryLogin(string account, string password)
        {
            bool isAccountRight = false;
            bool isPasswordRight = false;

            Member member = this.GetAccount(account);

            //雜湊
            string key = _encryption.HashPasswordkey();
            byte[] salt = member.Salt;

            byte[] securitBytes = _encryption.GetHashPassword(password, key, salt);
            string HashPassword = Convert.ToBase64String(securitBytes);


            if (member == null) // 找不到就代表登錄失敗
                return false;
            if (string.Compare(member.Account, account, true) == 0)
            {
                isAccountRight = true;
            }
            if (member.Password == HashPassword)
            {
                isPasswordRight = true;
            }
            // 檢查帳號密碼是否正確
            bool result = (isAccountRight && isPasswordRight);

            // 帳密正確，把值寫入 Session
            if (result)
            {
                // 為避免任何BUG，導致SESSION流出，先把密碼清除
                member.Password = null;
                HttpContext.Current.Session["Member"] = member;
                HttpContext.Current.Session["MemberID"] = member.MemberID;
                _lgihp.Login(member.Account, member.MemberID.ToString());

            }
            return result;
        }
        public Member GetCurrentUser()
        {
            Member account = HttpContext.Current.Session["Member"] as Member;
            return account;
        }

        public bool IsLogined()
        {
            Member account = GetCurrentUser();
            bool dentitylogined = _lgihp.dentityLogined();
            return (account != null && dentitylogined);
        }

    }
}