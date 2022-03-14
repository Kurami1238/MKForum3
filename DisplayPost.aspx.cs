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
    public partial class WebForm1 : System.Web.UI.Page
    {
        private PostManager _pmgr = new PostManager();
        private AccountManager _amgr = new AccountManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            // 從QS取得文章id 不能就回子版
            string postidText = this.Request.QueryString["Post"];
            if (string.IsNullOrWhiteSpace(postidText))
                this.BackToListPage();
            Guid postid;
            if(!Guid.TryParse(postidText, out postid))
                this.BackToListPage();
            // 取得文章資訊
            Post post = this._pmgr.GetPost(postid);
            if (post == null)
                this.BackToListPage();
            this.DisplayPost(post);
        }
        private void DisplayPost(Post post)
        {
            this.lblTitle.Text = post.Title;
            Member member = this._amgr.GetAccount(post.MemberID);
            this.lblMember.Text = member.Account;
            this.lblFloor.Text = post.Floor.ToString();
            this.lblCotent.Text = post.PostCotent;


        }
        protected void btnEditPost_Click(object sender, EventArgs e)
        {
            string postidText = this.Request.QueryString["Post"];
            Guid postid;
            Guid.TryParse(postidText, out postid);
            HttpContext.Current.Session["PostID"] = postid;
        }

        protected void btnDeletePost_Click(object sender, EventArgs e)
        {

        }
        private void BackToListPage()
        {
            // 從Session取得當前子板塊ID
            int cboardid = (int)HttpContext.Current.Session["CboardID"];
            // (須改 還沒寫)返回本文章子板塊
            this.Response.Redirect("CbtoPost.aspx");
        }
    }
}