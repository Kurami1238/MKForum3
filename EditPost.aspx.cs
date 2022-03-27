using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum
{
    public partial class EditPost1 : System.Web.UI.Page
    {
        private PostManager _pmgr = new PostManager();
        private AccountManager _amgr = new AccountManager();
        private Member _member;
        protected void Page_Init(object sender, EventArgs e)
        {
            // 從Session取得登錄者ID
            if (this._amgr.IsLogined())
            {
                Member account = this._amgr.GetCurrentUser();
                _member = account;
            }
            // 如果登錄者ID與EditPostID不符則回列表頁
            if (this._member != HttpContext.Current.Session["EditPostMember"])
                this.BackToListPage();
            // 從Session取得當前子板塊ID
            string cboard = this.Request.QueryString["CboardID"];
            int cboardid = 0;
            int.TryParse(cboard, out cboardid);
            List<PostStamp> psList = this._pmgr.GetPostStampList(cboardid);
            // 繫結PostStamp

            this.dpdlPostStamp.DataSource = psList;
            this.dpdlPostStamp.DataTextField = "PostSort";
            this.dpdlPostStamp.DataValueField = "SortID";
            this.dpdlPostStamp.DataBind();

            // 取得文章資訊
            Guid postid;
            Post post = new Post();

            string postidtext = this.Request.QueryString["PostID"];
            if (Guid.TryParse(postidtext, out postid))
                post = this._pmgr.GetPost(postid);
            this.DisplayPost(post);
        }
        private void BackToListPage()
        {
            // 從QS取得當前子板塊ID
            string CboardidText = this.Request.QueryString["CboardID"];
            int cboardid;
            if (int.TryParse(CboardidText, out cboardid))
                Response.Redirect($"CbtoPost.aspx?CboardID={CboardidText}", true);
        }
        private void DisplayPost(Post post)
        {
            int sort = 0;
            if (post.SortID != null)
            sort = (int)post.SortID;
            this.txtTitle.Text = post.Title;
            this.content.InnerText = post.PostCotent;
            // 讀取Post裡的sortID  沒辦法塞回DropDownList
            //this.ddlPostStamp.;
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            string TitleText = this.txtTitle.Text.Trim();
            string PostCotentText = this.content.InnerText.Trim();

            //檢查必填欄位及關鍵字

            if (!this._pmgr.CheckInput(TitleText, PostCotentText))
            {
                this.lblMsg.Text = this._pmgr.GetmsgText();
                return;
            }
            // 處理類型

            // 儲存圖片
            Post post = new Post()
            {
                Title = TitleText,
                PostCotent = PostCotentText
            };
            if (this.fuCoverImage.HasFile)
            {

                System.Threading.Thread.Sleep(3);
                Random random = new Random((int)DateTime.Now.Ticks);

                string folderPath = "~/FileDownload/PostContent/";
                string fileName = "C" + DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFFFF") + "_" + random.Next(100000).ToString("00000") + Path.GetExtension(this.fuCoverImage.FileName);

                folderPath = HostingEnvironment.MapPath(folderPath);
                if (!Directory.Exists(folderPath)) // 假如資料夾不存在，先建立
                    Directory.CreateDirectory(folderPath);
                string newFilePath = Path.Combine(folderPath, fileName);
                this.fuCoverImage.SaveAs(newFilePath);
                post.CoverImage = "/FileDownload/MapContent/" + fileName;
            }

            // 更新Post

            this._pmgr.UpdatePost(post);

            //提示使用者成功
            this.lblMsg.Text = "更新成功！";

        }

        protected void btnPostImage_Click(object sender, EventArgs e)
        {
            string imgpath = string.Empty;
            if (this.fuPostImage.HasFile)
            {
                System.Threading.Thread.Sleep(3);
                Random random = new Random((int)DateTime.Now.Ticks);

                string folderPath = "~/FileDownload/PostContent/";
                string fileName = "P" + DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFFFF") + "_" + this._member.Account + "_" + random.Next(100000).ToString("00000") + Path.GetExtension(this.fuPostImage.FileName);

                folderPath = HostingEnvironment.MapPath(folderPath);
                if (!Directory.Exists(folderPath)) // 假如資料夾不存在，先建立
                    Directory.CreateDirectory(folderPath);
                string newFilePath = Path.Combine(folderPath, fileName);
                this.fuPostImage.SaveAs(newFilePath);
                imgpath = "/FileDownload/PostContent/" + fileName;

                // 儲存圖片路徑
                this._pmgr.CreatePostImageList(this._member.MemberID, imgpath);
                string imagelink = this._pmgr.GetImage(this._member.MemberID);
                this.content.InnerText += $" ![]({imagelink})";
            }
        }
        protected void btnback_Click(object sender, EventArgs e)
        {
            this.BackToListPage();
        }
        
    }
}