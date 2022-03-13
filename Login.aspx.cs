using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum
{
    public partial class Login : System.Web.UI.Page
    {
        private AccountManager _Amgr = new AccountManager();
        protected void Page_Load(object sender, EventArgs e)
        {
#if(DEBUG)
            this.ckbskip.Visible = true;
#endif
            if (this._Amgr.IsLogined())
            {
                this.plcUserInfo.Visible = true;
                this.plcLogin.Visible = false;

                Member account = this._Amgr.GetCurrentUser();
                this.ltlAccount.Text = account.Account;
            }
            else
            {
                this.plcLogin.Visible = true;
                this.plcUserInfo.Visible = false;
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string account = this.txtAccount.Text.Trim();
            string pwd = this.txtPassword.Text.Trim();
            if (this.ckbskip.Checked)
            {
                if (this._Amgr.TryLogin("a123234", "12345678"))
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
            else if (this._Amgr.TryLogin(account, pwd))
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                this.ltlMessage.Text = "登錄失敗，請檢察帳號密碼。";
            }
        }
    }
}
