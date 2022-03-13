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
    public partial class MemberRegisterList : System.Web.UI.Page
    {
        private AccountManager _mgr = new AccountManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // 取得 keyword 的 querystring ，以及還原至 textbox
                string keyword = this.Request.QueryString["keyword"];
                this.txtKeyword.Text = keyword;

                List<Member> list = this._mgr.GetAccountList(keyword);

                if (list.Count > 0)
                {
                    this.gvList.DataSource = list;
                    this.gvList.DataBind();

                    this.plcEmpty.Visible = false;
                    this.gvList.Visible = true;
                }
                else
                {
                    this.plcEmpty.Visible = true;
                    this.gvList.Visible = false;
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("MemberRegisterPage.aspx");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = this.txtKeyword.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
                Response.Redirect("MemberRegisterList.aspx");
            else
                Response.Redirect("MemberRegisterList.aspx?keyword=" + keyword);
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<Guid> idList = new List<Guid>();
            foreach (GridViewRow gRow in this.gvList.Rows)
            {
                CheckBox ckbDel = gRow.FindControl("ckbDel") as CheckBox;
                HiddenField hfID = gRow.FindControl("hfID") as HiddenField;

                if (ckbDel != null && hfID != null)
                {
                    if (ckbDel.Checked)
                    {
                        Guid id;
                        if (Guid.TryParse(hfID.Value, out id))
                            idList.Add(id);
                    }
                }
            }
            if (idList.Count > 0)
            {
                this._mgr.DeleteAccounts(idList);
                this.Response.Redirect(this.Request.RawUrl);
            }
        }


    }
}