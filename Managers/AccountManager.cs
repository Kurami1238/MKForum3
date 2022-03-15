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
                                Email = reader["Email"] as string
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
        
        public void CreateAccount(Member member, MemberRegister memberRegister)
        {
            // 1. 判斷資料庫是否有相同的 Account
            if (this.GetAccount(member.Account) != null)
                throw new Exception("已存在相同的帳號");

            // 2. 新增資料
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  
                INSERT INTO Members(MemberID,Account,PWD,Email)VALUES(@MemberID,@Account,@PWD,@Email)

                INSERT INTO MemberRecords (MemberID)VALUES(@MemberID)

                 INSERT INTO MemberRegisters(MemberID,Captcha)
                  VALUES(@MemberID,@Captcha)
                  ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        member.MemberID = Guid.NewGuid();
                        command.Parameters.AddWithValue("@MemberID", member.MemberID);
                        command.Parameters.AddWithValue("@Account", member.Account);
                        command.Parameters.AddWithValue("@PWD", member.Password);
                        command.Parameters.AddWithValue("@Email", member.Email);
                        command.Parameters.AddWithValue("@Captcha", memberRegister.Captcha);

                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountMember.CreateAccount(Member member)", ex);
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


        
        public bool TryLogin(string account, string password)
        {
            bool isAccountRight = false;
            bool isPasswordRight = false;

            Member member = this.GetAccount(account);

            if (member == null) // 找不到就代表登錄失敗
                return false;
            if (string.Compare(member.Account, account, true) == 0)
            {
                isAccountRight = true;
            }
            if (member.Password == password)
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

            }
            return result;
        }
        public bool IsLogined()
        {
            Member account = GetCurrentUser();
            return (account != null);
        }
        public Member GetCurrentUser()
        {
            Member account = HttpContext.Current.Session["Member"] as Member;
            return account;
        }
    }
}