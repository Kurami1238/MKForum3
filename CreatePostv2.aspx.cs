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
    public partial class CreatePostv2 : System.Web.UI.Page
    {
        PostManager _pmgr = new PostManager();
        // 從Session取得登錄者ID
        Member member = new Member()
        {
            Account = "a123234",
            Password = "12345678"
        };
        // 先測試 直接輸入

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            string TitleText = this.txtTitle.Text.Trim();
            string PostCotentText = this.txtPostCotent.Text.Trim();
            
            //Guid memberid = this.Session["MemberID"] as Guid;

            // 從QS取得當前子板塊ID
            string cboardsText = this.Request.QueryString["Cboard"];
            int cboardid = (string.IsNullOrWhiteSpace(cboardsText))
                            ? 2 : Convert.ToInt32(cboardsText);
            // 先測試 假設有cboardid
            //int cboardid = this.Session["CboradID"] as int;

            //檢查必填欄位及關鍵字

            if (this._pmgr.CheckInput(TitleText, PostCotentText))
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
                string fileName = "C" +DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFFFF") + "_" + random.Next(100000).ToString("00000") + Path.GetExtension(this.fuCoverImage.FileName);
                
                folderPath = HostingEnvironment.MapPath(folderPath);
                if (!Directory.Exists(folderPath)) // 假如資料夾不存在，先建立
                    Directory.CreateDirectory(folderPath);
                string newFilePath = Path.Combine(folderPath, fileName);
                this.fuPostImage.SaveAs(newFilePath);
                post.CoverImage = "/FileDownload/MapContent/" + fileName;
            }
            
            // 新建一筆Post
            
            Guid postid;
            this._pmgr.CreatePost(member.MemberID, cboardid, post, out postid);
           
            //提示使用者成功
            this.lblMsg.Text = "新增成功！";

        }

        protected void btnPostImage_Click(object sender, EventArgs e)
        {
            string imgpath = string.Empty;
            if (this.fuPostImage.HasFile)
            {
                System.Threading.Thread.Sleep(3);
                Random random = new Random((int)DateTime.Now.Ticks);

                string folderPath = "~/FileDownload/PostContent/";
                string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFFFF") + "_" + random.Next(100000).ToString("00000") + Path.GetExtension(this.fuPostImage.FileName);

                folderPath = HostingEnvironment.MapPath(folderPath);
                if (!Directory.Exists(folderPath)) // 假如資料夾不存在，先建立
                    Directory.CreateDirectory(folderPath);
                string newFilePath = Path.Combine(folderPath, fileName);
                this.fuPostImage.SaveAs(newFilePath);
                imgpath = "/FileDownload/MapContent/" + fileName;

                // 儲存圖片路徑
                if (!string.IsNullOrWhiteSpace(imgpath))
                    this._pmgr.CreatePostImageList(member.MemberID, imgpath);
            }
        }
    }
}