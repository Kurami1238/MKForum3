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

            // 取得子版文章類型按鈕
            //List<PostStamp> 
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void DisplayPost(int cboard)
        {
            List<Post> postList = this._pmgr.GetPostListmoto(cboard);
            this.rptcBtoP.DataSource = postList;
            this.rptcBtoP.DataBind();
        }

        protected void btnCreatePost_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["CboardID"] = _cboardid;
            Response.Redirect("CreatePost.aspx",true);
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
                    Response.Redirect($"editpost.aspx?Cboard={cboardid}&PostID={editpostid}", true);
                    break;
            }
        }

        protected void rptStampbutton_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}