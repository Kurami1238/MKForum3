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
        private SearchManager _srchMng = new SearchManager();
        private CheckInputManager _chkInpMgr = new CheckInputManager();

        //顯示母列表 (!=IsPostBack)
        //顯示個人資料 (!=IsPostBack)
        //如果網址列為子板塊，則依會員權限 顯示黑名單 (!=IsPostBack)
        //如果網址列為子板塊，則依會員權限 顯示板主名單 (!=IsPostBack)
        protected void Page_Load(object sender, EventArgs e)
        {
            //從網址列取得URL的當前頁面
            string srchCurrent = System.IO.Path.GetFileName(Request.PhysicalPath);  //取得當前頁面含副檔名
            string currentPage = this._srchMng.GetCurrentPage(srchCurrent);

            //bool isInPBoard = _srchMng.IsInPBoard(currentPage);   方法未完成暫時先註解
            //bool isInCBoard = _srchMng.IsInCBoard(currentPage);
            ////當前位於母版塊或子板塊內，第二個選項變為可使用
            //if (isInPBoard || isInCBoard)
                //this.srchDrop.Items[1].disabled = true;
        }


        //搜尋按鈕( 搜尋結果頁面還沒有連結主板頁面 )
        protected void btnSearh_Click(object sender, EventArgs e)
        {
            string srchText = this.txtSearch.Text.Trim();   //使用者輸入的關鍵字
            string drowValue = this.srchDrop.SelectedValue.Trim();      //搜尋範圍搜尋範圍下拉選單
            string srchCurrent = System.IO.Path.GetFileName(Request.PhysicalPath);  //取得當前所在的母版塊/子板塊


            bool IsBanWord = _chkInpMgr.IncludeBanWord(srchText);   //確認搜尋的關鍵字是否包含屏蔽字

            //如果搜尋關鍵字不為空，且不含禁字，則藉由url導向搜尋結果頁面
            if (!string.IsNullOrWhiteSpace(this.txtSearch.Text)/* && !IsBanWord*/)
            {
                if(drowValue != "srchCurrent")
                //導向SearchPage.aspx
                this.Response.Redirect("SearchPage.aspx" + "?keyword=" + srchText + "&searcharea=" + drowValue);
                else
                    this.Response.Redirect("SearchPage.aspx" + "?keyword=" + srchText + "&srchCurrent=" + srchCurrent + "&searcharea=" + drowValue);

            }
        }

    }
}