using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;  //*** Web Service 會用到 ***
using System.Web.Configuration;
using MKForum.Managers;
using MKForum.Models;

namespace MKForum
{
    public partial class test : System.Web.UI.Page
    {
        private PostManager _pmgr = new PostManager();
        private AccountManager _amgr = new AccountManager();
        private Member _member;
        private int _cboardid;
        private const int _pageSize = 10;

        protected void Page_Init(object sender, EventArgs e)
        {
            // 從Session取得登錄者ID
            if (this._amgr.IsLogined())
            {
                Member account = this._amgr.GetCurrentUser();
                _member = account;
            }
            // 從QS取得 子版ID 先測試 假設有cboardid
            string cboardsText = this.Request.QueryString["Cboard"];
            int cboard = (string.IsNullOrWhiteSpace(cboardsText))
                            ? 2 : Convert.ToInt32(cboardsText);
            this.DisplayPost(cboard);
            _cboardid = cboard;


        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void DisplayPost(int cboard)
        {
            List<Post> postList = new List<Post>();
            // 如果有點文章類型按鈕
            string stamp = this.Request.QueryString["Sort"];
            if (int.TryParse(stamp, out int sortid))
                postList = this._pmgr.GetPostListwithStamp(sortid);
            else
                postList = this._pmgr.GetPostListmoto(cboard);
            // 取得子版文章類型按鈕
            this.rptStamp.DataSource = this._pmgr.GetPostStampList(cboard);
            this.rptStamp.DataBind();
            this.rptcBtoP.DataSource = postList;
            this.rptcBtoP.DataBind();
        }

        protected void btnCreatePost_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["CboardID"] = _cboardid;
            Response.Redirect("CreatePost.aspx", true);
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
    }
}