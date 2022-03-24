using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum.BackAdmin
{
    public partial class MemberEditor : System.Web.UI.Page
    {
        private MemberManager _mmgr = new MemberManager();

        private string SearchMemberID = "c8142d85-68c2-4483-ab51-e7d3fc366b89";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Models.Member memberInfo = _mmgr.GetMember(SearchMemberID);

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            

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

            if (SaveCheck(SexValue, StatusValue))
            {
                Models.Member memberSet = new Models.Member()
                {
                    MemberID = Guid.Parse(SearchMemberID.Trim()),
                    NickName = this.txtMember_name.Text.Trim(),
                    Sex = (int)SexValue,
                    Birthday = Convert.ToDateTime(this.txtMember_Birthday.Text.Trim()),
                    MemberStatus = (int)StatusValue,
                    Account = this.txtMember_Account.Text.Trim(),
                    Password = this.txtMember_PassWord.Text.Trim(),
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
            List<string> msgList = new List<string>();

            string MemberIDValue = SearchMemberID;
            string NickNameValue = this.txtMember_name.Text.Trim();
            string BirthdayValue = this.txtMember_Birthday.Text.Trim();
            string AccountValue = this.txtMember_Account.Text.Trim();
            string EmailValue = this.txtMember_Mail.Text.Trim();
            string PassWordValue = this.txtMember_PassWord.Text.Trim();
            string PassWordValueCheck = this.txtMember_PassWord_Check.Text.Trim();

            char[] special_symbols = { '\\', '/' ,':', '*', '"', '<', '>', '|', ','};
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