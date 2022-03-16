using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum
{

    public partial class Main : System.Web.UI.MasterPage
    {
        //引用hManager
        private SearchManager _srchMng = new SearchManager();
        private CheckInputManager _chkInpMgr = new CheckInputManager();
        private ParentBoardManager _pBrdMgr = new ParentBoardManager();

        //未做:
        //HomePage.aspx
        //顯示母列表 (!=IsPostBack)
        //顯示個人資料 (!=IsPostBack)
        //如果網址列為子板塊，則依會員權限 顯示黑名單 (!=IsPostBack)
        //如果網址列為子板塊，則依會員權限 顯示板主名單 (!=IsPostBack)
        protected void Page_Load(object sender, EventArgs e)
        {
            //搜尋功能
            string currentPboard = this.Request.QueryString["PboardID"];           //從URL取得當前CboardID
            string currentCboard = this.Request.QueryString["CboardID"];           //從URL取得當前CboardID


            /*當前位於母版塊或子板塊內，第二個選項變為可使用
            if (currentPboard != null || currentCboard != null)
            {
            var aaa = this.srchDrop.Items[1].Attributes;
            aaa.Remove("disable");
            }*/

            if (!this.IsPostBack)
            {
                DataTable dt = _pBrdMgr.GetPBoardStatus();
                this.Repeater1.DataSource = dt;
                this.Repeater1.DataBind();
                this.Repeater2.DataSource = dt;
                this.Repeater2.DataBind();
            }


        }


        //搜尋按鈕( 負責組成URL並導向搜尋葉面 )
        protected void btnSearh_Click(object sender, EventArgs e)
        {
            string srchText = this.txtSearch.Text.Trim();                   //使用者輸入的關鍵字
            string drowValue = this.srchDrop.SelectedValue.Trim();          //搜尋範圍下拉選單
            bool IsBanWord = _chkInpMgr.IncludeBanWord(srchText);   //確認搜尋的關鍵字是否包含屏蔽字




            //驗證關鍵字不為空，且不含禁字，導向搜尋頁面
            if (!string.IsNullOrWhiteSpace(this.txtSearch.Text)/* && !IsBanWord*/)
            {
                if (drowValue != "srchCurrent")  //如果使用者不是選擇搜尋當前頁面
                    this.Response.Redirect("SearchPage.aspx" + "?keyword=" + srchText + "&searcharea=" + drowValue);
                else
                {
                    string srchCboardID = this.Request.QueryString["CboardID"];
                    string srchPboardID = this.Request.QueryString["PboardID"];
                    string srchCurrent = "";

                    if (srchPboardID != null)
                        srchCurrent = "&srchPboardID=" + srchPboardID + "&srchCboardID=" + srchCboardID;
                    else
                    {
                        srchCurrent = "&srchCboardID=" + srchCboardID;
                    }

                    this.Response.Redirect("SearchPage.aspx" + "?keyword=" + srchText + srchCurrent + " & searcharea=" + drowValue);
                }
            }
            else
            {
                //跳出警告訊息
            }
        }


        //<%--<ul class="list-unstyled">--%>

        //<%--                <a href = "#" > 音樂討論區 < i class="fab fa-youtube"></i> </a>
        //    </li>
        //    <li>
        //        <a href = "#sublist" data-bs-toggle="collapse" id="dropdown">遊戲討論區<i class="far fa-file-video"></i></a>
        //        <!-- 子連結列表/下拉式選單  -->
        //        <ul id = "sublist" class="list-unstyled collapse">
        //            <li><a href = "#" > PC </ a ></ li >
        //            < li >< a href="#">PlayStation</a></li>
        //            <li><a href = "#" > Switch </ a ></ li >
        //        </ ul >
        //    </ li >
        //    < li >< a href="#">生活討論區<i class="fas fa-briefcase"></i></a></li>--%>
        //<%--</ul>--%>


    }
}