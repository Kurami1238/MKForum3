using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace MKForum
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private PostManager _pmgr = new PostManager();
        private AccountManager _amgr = new AccountManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            // 從QS取得文章id 不能就回子版
            string postidText = this.Request.QueryString["PostID"];
            if (string.IsNullOrWhiteSpace(postidText))
                this.BackToListPage();
            Guid postid;
            if (!Guid.TryParse(postidText, out postid))
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

            List<Post> pointList = this._pmgr.GetPostListwithPoint(post.PostID);
            var memberListwithpoint = pointList.Select(P => P.MemberID);
            List<Member> memberList = new List<Member>();
            foreach (var x in memberListwithpoint)
            {
                Member me = this._amgr.GetAccount(x);
                memberList.Add(me);
            }
            // 為什麼這裡要加52行才能引用m的屬性
            var pLML = from p in pointList
                       join m in memberList on p.MemberID equals m.MemberID
                       into tempPM
                       from g in tempPM.DefaultIfEmpty()
                       select new { 
                            PostID = p.PostID,
                            Floor = p.Floor,
                            MemberAccount = g.Account,
                            PostCotent = p.PostDate,
                            MemberID = g.MemberID
                       };
            this.rptNmP.DataSource = pLML;
            this.rptNmP.DataBind();

        }
        protected void btnEditPost_Click(object sender, EventArgs e)
        {
            string postidText = this.Request.QueryString["PostID"];
            Guid postid;

            if (Guid.TryParse(postidText, out postid))
            {
                HttpContext.Current.Session["EditPostID"] = postid;
                Response.Redirect($"EditPost.aspx?PostID={postidText}", true);
            }
        }

        protected void btnDeletePost_Click(object sender, EventArgs e)
        {
            string postidText = this.Request.QueryString["PostID"];
            Guid postid = Guid.Empty;
            if (!Guid.TryParse(postidText, out postid))
            {
                this._pmgr.DeletePost(postid);
                // JS Alert 提示刪除成功
                Response.Write("<script language='javascipt'>alert('刪除成功')</script>");
                this.BackToListPage();
            }
        }
        private void BackToListPage()
        {
            // 從QS取得當前子板塊ID
            string CboardidText = this.Request.QueryString["CboardID"];
            int cboardid;
            if (int.TryParse(CboardidText, out cboardid))
                Response.Redirect($"CbtoPost.aspx?CboardID={CboardidText}",true);
        }

        protected void btnDeleteNmPost_Click(object sender, EventArgs e)
        {
            Guid postID = Guid.Empty;
            foreach (RepeaterItem item in this.rptNmP.Items)
            {
                HiddenField hf = item.FindControl("hfNmPID") as HiddenField;
                if (hf.HasControls())
                {
                    if (Guid.TryParse(hf.Value, out postID))
                    {
                        this._pmgr.DeletePost(postID);
                        // Js alert 提示刪除成功
                        Response.Write("<script language='javascipt'>alert('刪除成功')</script>");
                        this.BackToListPage();
                    }
                }
            }
        }

        protected void btnEditNmPost_Click(object sender, EventArgs e)
        {
            string postID = string.Empty;
            foreach (RepeaterItem item in this.rptNmP.Items)
            {
                HiddenField hf = item.FindControl("hfNmPID") as HiddenField;
                if (hf.HasControls())
                    postID = hf.Value.ToString();
            }
            Response.Redirect($"EditPost.aspx?PostID={postID}", true);
        }

        protected void btnCNNmPost_Click(object sender, EventArgs e)
        {
            string cboardid = this.Request.QueryString["CboardID"];
            string pointid = this.Request.QueryString["PostID"];
            Guid point = Guid.Empty;
            Guid.TryParse(pointid, out point);
            string postcotent = this.txtCNNmPost.Text;
            Member member = HttpContext.Current.Session["MemberID"] as Member;
            Post post = new Post()
            {
                PostCotent = postcotent,
                CboardID = Convert.ToInt32(cboardid)
            };
            Guid NmpostID = Guid.Empty;
            this._pmgr.CreatePost(member.MemberID, point, post,out NmpostID);
            Response.Redirect(Request.RawUrl);
            // Js alert 提示回覆成功
            Response.Write("<script language='javascipt'>alert('回覆成功')</script>");
        }
    }
}