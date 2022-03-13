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
    public partial class EditPost : System.Web.UI.Page
    {
        private PostManager _pmgr = new PostManager();
        protected void Page_Init(object sender, EventArgs e)
        {
            // 從Session取得當前文章ID
            Post postid = new Post();
            //int cboardid = this.Session["PostID"] as Guid;

            // 顯示文章
            Post Displaypost = this._pmgr.GetPost(postid.PostID);

            Title.Text = Displaypost.Title;
            PostCotent.InnerText = Displaypost.PostCotent;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           

        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            string TitleText = Title.Text.Trim();
            string PostCotentText = PostCotent.InnerText;

            // 檢查必填欄位及關鍵字

            if (this._pmgr.CheckInput(TitleText, PostCotentText) == false)
            {
                ltlmsg.Text = this._pmgr.GetmsgText();
                return;
            }
            // 更新Post

            // 從Session取得當前文章ID
            Post postid = new Post();
            //int cboardid = this.Session["PostID"] as Guid;
            this._pmgr.UpdatePost(postid.PostID,TitleText,PostCotentText);

            // 提示使用者成功


        }
    }
}