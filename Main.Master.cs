using MKForum.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum
{

    public partial class Main : System.Web.UI.MasterPage
    {
        //引用SearchManager
        private SearchManager _srchMgr = new SearchManager();
        private CheckInputManager _chkInpMgr = new CheckInputManager();

        //顯示母列表 (!=IsPostBack)
        //顯示個人資料 (!=IsPostBack)
        //如果網址列為子板塊，則依會員權限 顯示黑名單 (!=IsPostBack)
        //如果網址列為子板塊，則依會員權限 顯示板主名單 (!=IsPostBack)
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        //搜尋按鈕( 搜尋結果頁面還沒有連結主板頁面 )
        protected void btnSearh_Click(object sender, EventArgs e)
        {
            string srchText = this.txtSearch.Text.Trim();   //使用者輸入的關鍵字
            string drowValue = this.srchDrow.SelectedValue.Trim();      //搜尋範圍

            //確認搜尋的關鍵字不可為屏蔽字
            bool IsBanWord = _chkInpMgr.IncludeBanWord(srchText);

            //如果搜尋關鍵字不為空，則藉由url導向搜尋結果頁面
            if (!string.IsNullOrWhiteSpace(this.txtSearch.Text) && !IsBanWord)
            {
                //組成搜尋的url
                string url = this.Request.Url.LocalPath + "SearchEngine.aspx" + "?keyword=" + srchText + "&&" + drowValue;
                this.Response.Redirect(url);    //導向目標網址
            }
        }

    }
}