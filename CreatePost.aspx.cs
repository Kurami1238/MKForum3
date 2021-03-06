using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum
{
    public partial class CreatePost : System.Web.UI.Page
    {
        private PostManager _pmgr = new PostManager();
        private AccountManager _Amgr = new AccountManager();
        private Member _member;
        protected void Page_Init(object sender, EventArgs e)
        {
            // 從Session取得登錄者ID
            this.CheckLogin();
            // 從Session取得當前子板塊ID
            int cboardid = this.GetCboardID();
            // 繫結PostStamp
            this.BuildPostStamp(cboardid);
            // 提示使用者訊息
            if (HttpContext.Current.Session["Msg"] != null)
            {
                this.msgmsg.Value = HttpContext.Current.Session["Msg"].ToString();
                HttpContext.Current.Session["Msg"] = null;
            }
        }
        private void BuildPostStamp(int cboardid)
        {
            List<PostStamp> psList = this._pmgr.GetPostStampList(cboardid);
            this.dpdlPostStamp.DataSource = psList;
            this.dpdlPostStamp.DataTextField = "PostSort";
            this.dpdlPostStamp.DataValueField = "SortID";
            this.dpdlPostStamp.DataBind();
        }
        private int GetCboardID()
        {
            int cboardid = 0;
            if (HttpContext.Current.Session["CboardID"] != null)
                cboardid = (int)HttpContext.Current.Session["CboardID"];
            else
                Response.Redirect($"Index.aspx", true);
            return cboardid;
        }
        private void CheckLogin()
        {
            if (this._Amgr.IsLogined())
            {
                Member account = this._Amgr.GetCurrentUser();
                _member = account;
                //this.memberid.Value = this._member.MemberID.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            string TitleText = this.txtTitle.Text.Trim();
            string PostCotentText = this.content.Value.Trim();
            // 從Session取得當前子板塊ID
            int cboardid = (int)HttpContext.Current.Session["CboardID"];
            //檢查必填欄位及關鍵字
            if (!this._pmgr.CheckInput(TitleText, PostCotentText))
            {
                this.lblMsg.Text = this._pmgr.GetmsgText();
                return;
            }
            // 處理類型
            string postSorttext = this.dpdlPostStamp.Text;
            int? postSort = Convert.ToInt32(postSorttext);
            // 處理並儲存圖片
            Post post = new Post()
            {
                Title = TitleText,
                PostCotent = PostCotentText,
                SortID = postSort
            };
            if (this.fuCoverImage.HasFile)
            {
                if (this.CheckImgFormat(this.fuCoverImage.FileName) == true)
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
                    post.CoverImage = "/FileDownload/PostContent/" + fileName;
                }
                else
                {
                    HttpContext.Current.Session["Msg"] = "別亂傳檔案，罰你重寫";
                    Response.Redirect(Request.RawUrl);
                    //return;
                }
            }
            if (post.CoverImage == null)
            {
                post.CoverImage = "/FileDownload/PostContent/" + "mokunin.jpg";
            }

            // 新建一筆Post
            Guid postid;
            this._pmgr.CreatePost(this._member.MemberID, cboardid, post, out postid);

            // 處理#tag
            string htagtext = this.txtPostHashtag.Text;
            string[] htagarr = htagtext.Split('/');
            List<string> htaglist = new List<string>();
            foreach (var x in htagarr)
            {
                htaglist.Add(x);
            }
            for (int i = 0; i < htaglist.Count; i++)
            {
                this._pmgr.CreateHashtag(postid, htaglist[i]);
            }
            HttpContext.Current.Session["Msg"] = "新增文章成功";
            Response.Redirect($"CbtoPost.aspx?CboardID={cboardid}", true);
        }
        protected void btnPostImage_Click(object sender, EventArgs e)
        {
            string imgpath = string.Empty;
            if (this.fuPostImage.HasFile)
            {
                System.Threading.Thread.Sleep(3);
                Random random = new Random((int)DateTime.Now.Ticks);
                if (this.CheckImgFormat(this.fuPostImage.FileName) == true)
                {
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
                    this.content.InnerText += $"![]({imagelink})";
                }
                else
                {
                    HttpContext.Current.Session["Msg"] = "別亂傳檔案，罰你重寫";
                    Response.Redirect(Request.RawUrl);
                    //return;
                }

            }
        }
        private Boolean CheckImgFormat(String fileName)
        {
            Boolean flag = false;
            String fileExtension = Path.GetExtension(fileName).ToLower();
            String[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (allowedExtensions[i].ToString().Equals(fileExtension))
                {
                    flag = true;
                }
            }

            return flag;
        }
        protected void btnback_Click(object sender, EventArgs e)
        {
            this.BackToListPage();
        }
        private void BackToListPage()
        {
            // 從Session取得當前子板塊ID
            int cboardid = (int)HttpContext.Current.Session["CboardID"];
            Response.Redirect($"CbtoPost.aspx?CboardID={cboardid}", true);
        }
    }
} 