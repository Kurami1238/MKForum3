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
    //目前整理到有bug的地方:
    //版主Manager裡面的IsCurrentModerator有錯誤

    //未做:
    //HomePage.aspx
    //母列表 (ajax有錯)
    //顯示會員個人資料 (未做記得要處理!=IsPostBack)
    //如果網址列為子板塊，則依會員權限 顯示板主名單 (版主名單還沒處理按鈕事件)
    public partial class Main : System.Web.UI.MasterPage
    {
        private SearchManager _srchMng = new SearchManager();           //搜尋
        private CheckInputManager _chkInpMgr = new CheckInputManager(); //確認輸入值
        private ParentBoardManager _pBrdMgr = new ParentBoardManager(); //母版塊
        private AccountManager _amgr = new AccountManager();            //帳號
        private BlackManager _blkmgr = new BlackManager();              //黑名單
        private ModeratorManager _MMmgr = new ModeratorManager();      //版主

        //string currentPboard = this.Request.QueryString["PboardID"];           //從URL取得當前PboardID
        string currentPboard = "2";           //用來測試

        //string currentCboard = this.Request.QueryString["CboardID"];   //從URL取得當前CboardID
        private string currentCboard = "2";           //用來測試

        private int memberStatus = 3;//用來測試不同身分別的

        protected void Page_Load(object sender, EventArgs e)
        {

            #region//母版塊區的功能
            if (_amgr.IsLogined())
            {
                memberStatus = _pBrdMgr.GetMemberStatus();
            }

            if (!this.IsPostBack)
            {
                //顯示母版塊列表( 左拉選單及固定選單各一 )
                if (!this.IsPostBack)
                {
                    var list = this._pBrdMgr.GetList();
                    //this.gdvList.DataSource = list;
                    //this.gdvList.DataBind();
                }

                DataTable dt = _pBrdMgr.GetPBoardStatus();
                this.Repeater1.DataSource = dt;
                this.Repeater1.DataBind();
                //this.Repeater2.DataSource = dt;
                //this.Repeater2.DataBind();

                if (memberStatus == 3)
                {
                    //btnPBMode1.Visible = true;
                    //btnPBMode2.Visible = true;
                }
            }
            #endregion
            int intcurrentCboard;
            DataTable BlckMbrDT;
            #region//顯示黑名單的功能
            //如果當前在子板塊內，且為該子版版主，則顯示黑名單
            if (currentCboard != null || memberStatus != 1)
            {
                if (this._MMmgr.IsCurrentModerator())
                {
                    this.plhBlk.Visible = true;

                    intcurrentCboard = int.Parse(currentCboard);
                    BlckMbrDT = _blkmgr.getBlacked(intcurrentCboard);
                    this.RptrBlk.DataSource = BlckMbrDT;
                    this.RptrBlk.DataBind();
                }
            }
            else

                #endregion
            #region//顯示版主名單的功能(資料庫沒資料無法測試)
            //如果當前在子板塊內，且為後台人員(身分別為3)，則顯示板主名單
            if (currentCboard != null)
            {
                if (this._pBrdMgr.GetMemberStatus() == 3)
                {
                    this.plhMM.Visible = true;
                    intcurrentCboard = int.Parse(currentCboard);
                    DataTable MMDT = _blkmgr.getBlacked(intcurrentCboard);
                    this.RptrMM.DataSource = MMDT;
                    this.RptrMM.DataBind();

                }

            }
#endregion

        }

        protected void Page_Prerender(object sender, EventArgs e)
        {
            #region//搜尋區的功能

            //當前位於母版塊或子板塊內，第二個選項變為可使用
            if (currentPboard != null || currentCboard != null)
            {
                this.srchDrop.Items[1].Attributes.Remove("disabled");

            }
            #endregion

        }

        //搜尋按鈕( 負責組成URL並導向搜尋頁面 )
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

        //母版塊編輯模式切換按鈕事件
        protected void btnPBMode_Click(object sender, EventArgs e)
        {
            //this.plhPBMode1.Visible = false;
            //this.plhPBMode2.Visible = false;
            //this.plhPBEdit1.Visible = true;
            //this.plhPBEdit2.Visible = true;
        }

        //儲存編輯後的母版塊按鈕事件
        protected void btnPBSave_Click(object sender, EventArgs e)
        {
            //取得編輯過後的母版塊資料(待補)


            //寫入資料庫
            List<Pboard> newPBData = new List<Pboard>();
            //_pBrdMgr.UpdatePBoardStatus(newPBData);

        }

        //取消編輯母版塊按鈕事件
        protected void btnPBCancel_Click(object sender, EventArgs e)
        {
            //this.plhPBMode1.Visible = true;
            //this.plhPBMode2.Visible = true;
            //this.plhPBEdit1.Visible = false;
            //this.plhPBEdit2.Visible = false;
        }


        protected void btnBlk_Click(object sender, EventArgs e)
        {
            string account = this.txtBlkAcc.Text;   //輸入的帳號
            string dt = this.txtBlkDate.Text;       //輸入的日期

            //去掉輸入的空白及沒有輸入的值

            //檢查輸入的帳號是否正在被黑名單
            bool isBlacked = _blkmgr.IsBlacked(account, currentCboard);

            //如果已經在黑名單中，就更新他的資訊，否則就新增一筆資料
            if (isBlacked)
                this._blkmgr.UpdateBlackedList(account, dt, currentCboard);
            else
                this._blkmgr.AddBlackedList(account, dt, currentCboard);

        }
    }
}