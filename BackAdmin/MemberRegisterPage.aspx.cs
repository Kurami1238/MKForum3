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
    public partial class MemberRegisterPage : System.Web.UI.Page
    {
        private bool _isEditMode = true;
        private AccountManager _mgr = new AccountManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request.QueryString["ID"]))
                _isEditMode = true;
            else
                _isEditMode = false;


            if (_isEditMode)
            {
                string idText = this.Request.QueryString["ID"];
                Guid id;
                if (!Guid.TryParse(idText, out id))
                    this.lblMsg.Text = "id 錯誤";
                else
                    this.InitEditMode(id);
            }
            else
            {
                this.InitCreateMode();
            }
        }

        private void InitCreateMode()
        {
            this.ltlAccount.Visible = false;
            this.txtAccount.Visible = true;

            this.ltlID.Text = "尚待新增";
        }

        private void InitEditMode(Guid id)
        {
            this.ltlAccount.Visible = true;
            this.txtAccount.Visible = false;

            Member member = this._mgr.GetAccount(id);

            if (member == null)
            {
                this.lblMsg.Text = "查無此 id ";
                return;
            }

            this.ltlAccount.Text = member.Account;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string account = this.txtAccount.Text.Trim();
            string pwd = this.txtPassword.Text.Trim();
            string mail=this.txtEmail.Text.Trim();
            string capt = this.txtCapt.Text.Trim();

            if (!_isEditMode)
            {
                Member member = new Member();
                member.Account = account;
                member.Password = pwd;
                member.Email = mail;

                MemberRegister memberRegister = new MemberRegister();
                memberRegister.Captcha = capt;

                this._mgr.CreateAccount(member, memberRegister);
                Response.Redirect("MemberRegisterPage.aspx?ID=" + member.MemberID);
            }
            else
            {
                string idText = this.Request.QueryString["ID"];
                Guid id;
                if (!Guid.TryParse(idText, out id))
                {
                    this.lblMsg.Text = "id 錯誤";
                    return;
                }

                // 從資料庫查出來更新
                Member member = this._mgr.GetAccount(id);
                member.Password = pwd;
                this._mgr.UpdateAccount(member);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("MemberRegisterList.aspx");
        }
    }
}