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
    public partial class WebForm2 : System.Web.UI.Page
    {
        private PostManager _pmgr = new PostManager();
        private AccountManager _amgr = new AccountManager();
        private Member _member;
        private int _cboardid;
        private const int _pageSize = 5;

        protected void Page_Init(object sender, EventArgs e)
        {
            // 從Session取得登錄者ID
            this.CheckLogin();
            // 從QS取得 子版ID 先測試 假設有cboardid
            int cboard = this.GetCboardID();
            // 假如有文章類型 則取值，沒有則固定為0
            this.CheckSort();
            // 展示文章列表
            this.DisplayPost(cboard);
            // 若被ban則看不到小按鈕
            this.CheckCanCreatePost();
            // 提示使用者訊息
            if (HttpContext.Current.Session["Msg"] != null)
            {
                this.msgmsg.Value = HttpContext.Current.Session["Msg"].ToString();
                HttpContext.Current.Session["Msg"] = null;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void CheckSort()
        {
            string stamp = this.Request.QueryString["Sort"];
            if (!string.IsNullOrWhiteSpace(stamp))
                this.sortid.Value = stamp;
            else
                this.sortid.Value = "0";
        }

        private int GetCboardID()
        {
            string cboardsText = this.Request.QueryString["Cboard"];
            int cboard = (string.IsNullOrWhiteSpace(cboardsText))
                            ? 2 : Convert.ToInt32(cboardsText);
            Cboard cboardd = CboardManager.GetCboard(cboard);
            _cboardid = cboard;
            this.ltlCbn.Text = cboardd.Cname;
            this.hftest.Value = cboard.ToString();
            return cboard;
        }

        private void CheckLogin()
        {
            if (this._amgr.IsLogined())
            {
                Member account = this._amgr.GetCurrentUser();
                _member = account;
            }
        }

        private void DisplayPost(int cboard)
        {
            List<Post> postList = new List<Post>();
            // 如果有點文章類型按鈕
            string stamp = this.Request.QueryString["Sort"];
            if (int.TryParse(stamp, out int sortid))
                postList = this._pmgr.GetPostListwithStamp(sortid);
            else
                postList = this._pmgr.GetPostList(cboard, _pageSize, 1, out int totalrows);
            // 文章若長度大於二十個字，後面則隱藏
            List<Post> seripost = PostContentSeri(postList);
            // 分離memberID，然後用memberID查出 memberAccount
            var memberListwithpoint = postList.Select(P => P.MemberID);
            List<Member> memberList = new List<Member>();
            foreach (var x in memberListwithpoint)
            {
                Member me = this._amgr.GetAccount(x);
                memberList.Add(me);
            }
            // 合併兩表連接rpt
            var pLML = from p in postList
                       join m in memberList on p.MemberID equals m.MemberID
                       into tempPM
                       //from g in tempPM.DefaultIfEmpty().Distinct()
                       select new
                       {
                           PostID = p.PostID,
                           MemberAccount = (tempPM.FirstOrDefault() != null) ? tempPM.FirstOrDefault().Account : "無",
                           PostCotent = p.PostCotent,
                           MemberID = p.MemberID,
                           CboardID = p.CboardID,
                           Title = p.Title,
                           LastEditTime = p.LastEditTime,
                           PostDate = p.PostDate,
                           CoverImage = p.CoverImage,
                           PostView = p.PostView,
                           Floor = p.Floor,
                       };
            // 第二次合併把文章內容大於二十字的替換掉
            var pLMLS = from p in pLML
                        join s in seripost on p.PostID equals s.PostID
                        into tempPS
                        select new
                        {
                            PostID = p.PostID,
                            MemberAccount = p.MemberAccount,
                            PostCotent = (tempPS.FirstOrDefault() != null) ? tempPS.FirstOrDefault().PostCotent : p.PostCotent,
                            MemberID = p.MemberID,
                            CboardID = p.CboardID,
                            Title = p.Title,
                            LastEditTime = p.LastEditTime.ToString("yyyy/MM/dd tt HH:mm:ss"),
                            PostDate = p.PostDate.ToString("yyyy/MM/dd tt HH:mm:ss"),
                            CoverImage = p.CoverImage,
                            PostView = p.PostView,
                            Floor = p.Floor,
                        };
            this.rptcBtoP.DataSource = pLMLS;
            this.rptcBtoP.DataBind();
            // 取得子版文章類型按鈕
            this.rptStamp.DataSource = this._pmgr.GetPostStampList(cboard);
            this.rptStamp.DataBind();
        }

        private static List<Post> PostContentSeri(List<Post> postList)
        {
            var postcontent = postList.Select((x, index) => { return new { PostCotent = x.PostCotent, PostID = x.PostID }; });
            List<Post> seripost = new List<Post>();
            foreach (var x in postcontent)
            {
                char[] pccarr = x.PostCotent.ToCharArray();
                if (pccarr.Length > 20)
                {
                    string newpc = string.Empty;
                    for (int i = 0; i < 20; i++)
                    {
                        newpc += pccarr[i];
                    }
                    newpc += " ..........(點擊後觀看)";
                    seripost.Add(new Post() { PostID = x.PostID, PostCotent = newpc });
                }
            }

            return seripost;
        }

        protected void btnCreatePost_Click(object sender, EventArgs e)
        {
            if (this._member != null)
            {
                HttpContext.Current.Session["CboardID"] = _cboardid;
                Response.Redirect("CreatePost.aspx", true);
            }
            else
            {
                HttpContext.Current.Session["CboardID"] = _cboardid;
                HttpContext.Current.Session["NeedTouroku"] = 1;
                HttpContext.Current.Session["JumpPage"] = null;
                Response.Redirect(Request.RawUrl, true);
            }
        }

        protected void rptcBtoP_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "btnEditNmpost":
                    string cboardid = this.Request.QueryString["CboardID"];
                    string editpostid = e.CommandArgument.ToString();
                    Member member = this._member;
                    HttpContext.Current.Session["EditPostMember"] = member;
                    HttpContext.Current.Session["CboardID"] = cboardid;
                    Response.Redirect($"editpost.aspx?Cboard={cboardid}&PostID={editpostid}", true);
                    break;
            }
        }


        protected void rptStamp_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "btnStamp":
                    string cboardid = this.Request.QueryString["CboardID"];
                    string sortid = e.CommandArgument.ToString();
                    Response.Redirect($"CbtoPost.aspx?Cboard={cboardid}&Sort={sortid}", true);
                    break;
            }
        }
        private void CheckCanCreatePost()
        {
            List<MemberBlack> blist = this._pmgr.GetCboardBlackList(this._cboardid);
            if (blist.Count > 0 && this._member != null)
            {
                var blistAcc = blist.Select(x => x.Account);
                foreach (var x in blistAcc)
                {
                    if (string.Compare(x, this._member.Account) == 0)
                        this.btnCreatePostB.Visible = false;
                }
            }
        }
    }
}