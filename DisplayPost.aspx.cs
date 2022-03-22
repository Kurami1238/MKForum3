using MKForum.Helpers;
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
        private Member _member;
        private SessionHelper _shlp = new SessionHelper();
        private MemberFollowManager _mfmsg = new MemberFollowManager();

        protected void Page_Init(object sender, EventArgs e)
        {
            // 從Session取得登錄者ID
            if (this._amgr.IsLogined())
            {
                Member account = this._amgr.GetCurrentUser();
                _member = account;
            }
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
            this.MemberFollowFirst(this._member.MemberID, postid);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void DisplayPost(Post post)
        {
            this.lblTitle.Text = post.Title;
            Member member = this._amgr.GetAccount(post.MemberID);
            this.lblMember.Text = "作者：" +member.Account;
            this.lblFloor.Text = post.Floor.ToString() + "F";
            this.sortid.Value = post.PostCotent;
            this.hfMemberID.Value = post.MemberID.ToString();
            this.phl.DataBind();

            List<Post> pointList = this._pmgr.GetPostListwithPoint(post.PostID);
            // 分離memberID，然後用memberID查出 memberAccount
            var memberListwithpoint = pointList.Select(P => P.MemberID);
            List<Member> memberList = new List<Member>();
            foreach (var x in memberListwithpoint)
            {
                Member me = this._amgr.GetAccount(x);
                memberList.Add(me);
            }
            // 合併兩表連接rpt
            // BUG 重複會員回復的話 會重複顯示回文 已解決
            var pLML = from p in pointList
                       join m in memberList on p.MemberID equals m.MemberID
                       into tempPM
                       //from g in tempPM.DefaultIfEmpty().Distinct()
                       select new
                       {
                           PostID = p.PostID,
                           Floor = p.Floor,
                           // 問問原理
                           MemberAccount = (tempPM.FirstOrDefault() != null) ? tempPM.FirstOrDefault().Account : "無", 
                           PostCotent = p.PostCotent,
                           MemberID = p.MemberID,
                           CboardID = p.CboardID
                       };
            this.rptNmP.DataSource = pLML;
            this.rptNmP.DataBind();

        }
        protected void btnEditPost_Click(object sender, EventArgs e)
        {
            string postidText = this.Request.QueryString["PostID"];
            string CboardidText = this.Request.QueryString["CboardID"];
            Guid postid;

            if (Guid.TryParse(postidText, out postid))
            {
                Member member = this._member;
                HttpContext.Current.Session["EditPostMember"] = member;
                Response.Redirect($"EditPost.aspx?CboardID={CboardidText}&PostID={postidText}", true);
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
                Response.Redirect($"CbtoPost.aspx?CboardID={CboardidText}", true);
        }
        protected void btnCNNmPost_Click(object sender, EventArgs e)
        {
            // 從session 取得member
            Member member = this._member;
            if (string.IsNullOrWhiteSpace(this.txtCNNmPost.Text))
                return;
            if (member != null)
            {
                string cboardid = this.Request.QueryString["CboardID"];
                string pointid = this.Request.QueryString["PostID"];
                Guid point = Guid.Empty;
                Guid.TryParse(pointid, out point);
                string postcotent = this.txtCNNmPost.Text;
                Post post = new Post()
                {
                    PostCotent = postcotent,
                    CboardID = Convert.ToInt32(cboardid)
                };
                Guid NmpostID = Guid.Empty;
                this._pmgr.CreatePost(member.MemberID, point, post, out NmpostID);
                // Js alert 提示回覆成功
                Response.Redirect(Request.RawUrl);
            }
            else
                Response.Redirect("Login.aspx", true);
        }
        protected void rptNmP_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "btnEditNmpost":
                    string cboardid = this.Request.QueryString["CboardID"];
                    string editpostid = e.CommandArgument.ToString();
                    Member member = this._member;
                    HttpContext.Current.Session["EditPostMember"] = member;
                    Response.Redirect($"editpost.aspx?Cboard={cboardid}&PostID={editpostid}", true);
                    break;
                case "btnDeleteNmPost":
                    string deletepostid = e.CommandArgument.ToString();
                    Guid postID = Guid.Empty;
                    if (Guid.TryParse(deletepostid, out postID))
                    {
                        this._pmgr.DeletePost(postID);
                        // Js alert 提示刪除成功
                        this.BackToListPage();
                    }
                    break;
            }
        }
        private void MemberFollowFirst(Guid memberID,Guid postID)
        {
            //if (_mfmsg.GetMemberFollowThisPost(memberID, postID).FollowStatus)
            //    this.lblMemberFollow_FollowStatus.Text = "追蹤中";
            //else
            //    this.lblMemberFollow_FollowStatus.Text = "未追蹤";
        }
        protected void btnMemberFollow_FollowStatus_Click(object sender, EventArgs e)
        {
            //從QS取得文章id 不能就回子版
            //string postidText = this.Request.QueryString["PostID"];
            //Guid postid;
            //if (!Guid.TryParse(postidText, out postid))
            //    this.BackToListPage();
            //if (this.lblMemberFollow_FollowStatus.Text == "追蹤中")
            //{
            //    this._mfmsg.Updatetrack(this._member.MemberID, postid, 0);
            //    this.lblMemberFollow_FollowStatus.Text = "未追蹤";
            //}
            //else if (this.lblMemberFollow_FollowStatus.Text == "未追蹤")
            //{
            //    this._mfmsg.Updatetrack(this._member.MemberID, postid, 1);
            //    this.lblMemberFollow_FollowStatus.Text = "追蹤中";
            //}

        }

        //protected void rptNmP_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{

        //    string postid = string.empty;
        //    foreach (repeateritem item in this.rptnmp.items)
        //    {
        //        //itemcomain
        //        hiddenfield hf = e.item.findcontrol("hfnmpid") as hiddenfield;
        //        if (hf.hascontrols())
        //            postid = hf.value.tostring();

        //    }
        //    response.redirect($"editpost.aspx?postid={postid}", true);
        //}
        //protected void rptNmP_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    List<ProgramData> Programlist = this._pmgr.GetProgramDatas();
        //    int j = Programlist[i].ID;

        //    List<UnitData> unitlist = this._umgr.GetUnitDatas(j);

        //    if (e.Item.ItemType == ListItemType.Item ||
        //        e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        Repeater rptUnit = e.Item.FindControl("rptUnit") as Repeater;

        //        if (rptUnit != null)
        //            rptUnit.DataSource = unitlist;
        //        rptUnit.DataBind();
        //        i += 1;
        //    }
        //}
    }
}