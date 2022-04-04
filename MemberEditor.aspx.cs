using MKForum.Helpers;
using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum.BackAdmin
{
    public partial class MemberEditor : System.Web.UI.Page
    {
        private MemberManager _mmgr = new MemberManager();
        private AccountManager _amgr = new AccountManager();
        private EncryptionHelper _encryption = new EncryptionHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Member account = _amgr.GetCurrentUser();
                string memberID = account.MemberID.ToString();
                Models.Member memberInfo = _mmgr.GetMember(memberID);

                if (memberInfo == null)
                {
                    this.txtMember_name.Visible = false;
                    this.rdbtnMember_SexList.Visible = false;
                    this.txtMember_Birthday.Visible = false;
                    this.lblMember_Status.Visible = false;
                    this.txtMember_Account.Visible = false;
                    this.txtMember_Mail.Visible = false;
                    this.phEmpty.Visible = true;
                }
                else
                {
                    this.txtMember_name.Text = memberInfo.NickName;

                    if (memberInfo.Sex == 1)
                        rdbtnMember_SexList.Items[0].Selected = true;
                    else if (memberInfo.Sex == 2)
                        rdbtnMember_SexList.Items[1].Selected = true;

                    if (memberInfo.MemberStatus == 1)
                        this.lblMember_Status.Text = "一般會員";
                    else if (memberInfo.MemberStatus == 2)
                        this.lblMember_Status.Text = "版主";
                    else if (memberInfo.MemberStatus == 3)
                        this.lblMember_Status.Text = "管理員";

                    this.txtMember_Birthday.Text = Convert.ToDateTime(memberInfo.Birthday).ToString("yyyyy-MM-dd");

                    this.txtMember_Account.Text = memberInfo.Account;

                    this.txtMember_Mail.Text = memberInfo.Email;

                    this.txtMember_PassWord.Text = memberInfo.Password;
                    this.txtMember_PassWord_Check.Text = memberInfo.Password;

                    this.phEmpty.Visible = false;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Index.aspx");
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            Member account = _amgr.GetCurrentUser();
            string memberID = account.MemberID.ToString();

            int ? SexValue = null;
            if (rdbtnMember_SexList.Items[0].Selected == true)
                SexValue = 1;
            else if (rdbtnMember_SexList.Items[1].Selected == true)
                SexValue = 2;

            int? StatusValue = null;
            if (this.lblMember_Status.Text == "一般會員")
                StatusValue = 1;
            else if (this.lblMember_Status.Text == "版主")
                StatusValue = 2;
            else if (this.lblMember_Status.Text == "管理員")
                StatusValue = 3;

            //雜湊
            string Password = this.txtMember_PassWord.Text.Trim();
            string key = _encryption.HashPasswordkey();
            byte[] salt = _encryption.BuildNewSalt();

            byte[] securitBytes = _encryption.GetHashPassword(Password, key, salt);
            string HashPassword = Convert.ToBase64String(securitBytes);

            if (SaveCheck(SexValue, StatusValue))
            {
                Models.Member memberSet = new Models.Member()
                {
                    MemberID = Guid.Parse(memberID.Trim()),
                    NickName = this.txtMember_name.Text.Trim(),
                    Sex = (int)SexValue,
                    Birthday = Convert.ToDateTime(this.txtMember_Birthday.Text.Trim()),
                    MemberStatus = (int)StatusValue,
                    Account = this.txtMember_Account.Text.Trim(),
                    Password = HashPassword,
                    Salt = salt,
                    Email = this.txtMember_Mail.Text.Trim(),
                };
                _mmgr.UpdateMember(memberSet);
                this.lblSave_notice.Text = "存檔成功";
            }
        }

        protected void btnMember_Password_Click(object sender, EventArgs e)
        {
            txtMember_PassWord_Check.Text = "";
            txtMember_PassWord.Text = "";
            this.txtMember_PassWord.Visible = true;
            this.txtMember_PassWord_Check.Visible = true;
            this.btnMember_Password.Visible = false;
        }

        public bool SaveCheck(int? SexValue, int? StatusValue)
        {
            Member account = _amgr.GetCurrentUser();
            string memberID = account.MemberID.ToString();
            List<string> msgList = new List<string>();

            string MemberIDValue = memberID;
            string NickNameValue = this.txtMember_name.Text.Trim();
            string BirthdayValue = this.txtMember_Birthday.Text.Trim();
            string AccountValue = this.txtMember_Account.Text.Trim();
            string EmailValue = this.txtMember_Mail.Text.Trim();
            string PassWordValue = this.txtMember_PassWord.Text.Trim();
            string PassWordValueCheck = this.txtMember_PassWord_Check.Text.Trim();

            char[] special_symbols = { '\\' ,':', '*', '"', '<', '>', '|', ','};
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


            if (NickNameValue.Count() == 0)
                msgList.Add("請輸入姓名");

            if (PassWordValue.Count() < 8)
                msgList.Add("密碼請輸入8個字以上");
            if (PassWordValue != PassWordValueCheck)
                msgList.Add("密碼輸入不一致");

            Regex a_zPattern = new Regex("[a-z]");
            Regex a_zPattern2 = new Regex("[A-Z]");
            var a_zPattern01 = a_zPattern.Matches(PassWordValue);
            var a_zPattern02 = a_zPattern2.Matches(PassWordValue);
            if (a_zPattern01.Count == 0)
                if (a_zPattern02.Count == 0)
                    msgList.Add("密碼請用英文及數字");

            if (SexValue == null)
                msgList.Add("性別輸入錯誤");
            if (StatusValue == null)
                msgList.Add("會員輸入錯誤");

            if (msgList.Count > 0)
            {
                string ErrorMessage = string.Join("<br />", msgList);
                this.lblSave_notice.Text = ErrorMessage;
                return false;
            }
              
            return true;
        }
    }
}