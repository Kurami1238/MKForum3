using MKForum.Helpers;
using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum
{
    public partial class MemberRegister : System.Web.UI.Page
    {
        private AccountManager _mgr = new AccountManager();
        private EncryptionHelper _encryption = new EncryptionHelper();


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            string picpath = "PicPath/線條人.png";
            // 處理並儲存圖片
            if (this.fuCoverImage.HasFile)
            {
                System.Threading.Thread.Sleep(3);
                Random random = new Random((int)DateTime.Now.Ticks);

                string folderPath = "~/PicPath/";
                string fileName = "C" + DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFFFF") + "_" + random.Next(100000).ToString("00000") + Path.GetExtension(this.fuCoverImage.FileName);

                folderPath = HostingEnvironment.MapPath(folderPath);
                if (!Directory.Exists(folderPath)) // 假如資料夾不存在，先建立
                    Directory.CreateDirectory(folderPath);
                string newFilePath = Path.Combine(folderPath, fileName);
                this.fuCoverImage.SaveAs(newFilePath);
                picpath = "PicPath/" + fileName;
            }


            int? SexValue = null;
            if (rdbtnMember_SexList.Items[0].Selected == true)
                SexValue = 1;
            else if (rdbtnMember_SexList.Items[1].Selected == true)
                SexValue = 2;

            if (SaveCheck(SexValue))
            {
                string account = this.txtAccount.Text.Trim();
                string pwd = this.txtPassword.Text.Trim();
                string mail = this.txtEmail.Text.Trim();
                string nickname = this.txtMember_name.Text.Trim();
                string bir = this.txtMember_Birthday.Text.Trim();

                //雜湊
                string key = _encryption.HashPasswordkey();
                byte[] salt = _encryption.BuildNewSalt();

                byte[] securitBytes = _encryption.GetHashPassword(pwd, key, salt);
                string HashPassword = Convert.ToBase64String(securitBytes);

                Member member = new Member()
                {
                    MemberStatus = 1,
                    Account = account,
                    Password = HashPassword,
                    Email = mail,
                    NickName = nickname,
                    Birthday = Convert.ToDateTime(bir),
                    Sex = (int)SexValue,
                    PicPath = picpath,
                    Salt = salt
                };

                _mgr.CreateAccount(member);
                this.ltlAccountcheckmsg.Text = "存檔成功";

                Response.Redirect("~/Index.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Index.aspx");
        }

        protected void btnAccouncheck_Click(object sender, EventArgs e)
        {
            if (accountcheck())
            {
                this.ltlAccountcheckmsg.Text = "* 此帳號可使用";
            }
            else
            {
                this.ltlAccountcheckmsg.Text = "* 此帳號已存在";
            }
        }

        private bool accountcheck()
        {
            bool Accountcheck = false;
            string account = this.txtAccount.Text;
            Member member = _mgr.GetAccount(account);

            if (member == null)
                return true;
            else
                return false;
        }

        public bool SaveCheck(int? SexValue)
        {

            List<string> msgList = new List<string>();

            string NickNameValue = this.txtMember_name.Text.Trim();
            string BirthdayValue = this.txtMember_Birthday.Text.Trim();
            string AccountValue = this.txtAccount.Text.Trim();
            string EmailValue = this.txtEmail.Text.Trim();
            string PassWordValue = this.txtPassword.Text.Trim();
            string PassWordValueCheck = this.txtPassword_Check.Text.Trim();

            char[] special_symbols = { '\\', '/', ':', '*', '"', '<', '>', '|', ',' };
            foreach (char i in special_symbols)
            {
                if (NickNameValue.Contains(i))
                    msgList.Add("姓名不能包含特殊符號");
                if (BirthdayValue.Contains(i))
                    msgList.Add("生日不能包含特殊符號");
                if (AccountValue.Contains(i))
                    msgList.Add("帳號不能包含特殊符號");
                if (EmailValue.Contains(i))
                    msgList.Add("信箱不能包含特殊符號");
                if (PassWordValue.Contains(i))
                    msgList.Add("密碼不能包含特殊符號");
            }

            if (!accountcheck())
                msgList.Add("此帳號已存在");

            if (NickNameValue.Count() == 0)
                msgList.Add("請輸入姓名");

            if (PassWordValue.Count() < 8)
                msgList.Add("密碼請輸入8個字以上");
            if (PassWordValue != PassWordValueCheck)
                msgList.Add("密碼輸入不一致");

            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(PassWordValue))
                msgList.Add("密碼請用英文及數字");

            if (SexValue == null)
                msgList.Add("性別輸入錯誤");

            if (BirthdayValue.Length == 0)
                msgList.Add("生日不得為空白");

            if (msgList.Count > 0)
            {
                string ErrorMessage = string.Join("<br />", msgList);
                this.lblMsg.Text = ErrorMessage;
                return false;
            }

            return true;
        }
    }
}