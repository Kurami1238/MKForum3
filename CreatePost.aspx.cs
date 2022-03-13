using MKForum.Helpers;
using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum
{
    public partial class CreatePost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string TitleText = Title.Text.Trim();
            string PostCotentText = PostCotent.InnerText;
            // 從Session取得登錄者ID
            Member memberid = new Member();
            //Guid memberid = this.Session["MemberID"] as Guid;

            // 從Session取得當前子板塊ID
            Cboard cboardid = new Cboard();
             //int cboardid = this.Session["CboradID"] as int;

            // 檢查必填欄位及關鍵字

            //if (PostManager.CheckInput(TitleText, PostCotentText) == false)
            //{
            //    ltlmsg.Text = PostManager.GetmsgText();
            //    return;
            //}
            // // 新建一筆Post

            //PostManager.CreatePost(memberid.MemberID, cboardid.CboardID, TitleText, PostCotentText);

            // 提示使用者成功


        }
    }
}