using MKForum.Helpers;
using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum
{

    public partial class Main : System.Web.UI.MasterPage
    {
        private SearchManager _srchMng = new SearchManager();           //搜尋
        private CheckInputManager _chkInpMgr = new CheckInputManager(); //確認輸入值
        private ParentBoardManager _pBrdMgr = new ParentBoardManager(); //母版塊
        private AccountManager _amgr = new AccountManager();            //帳號
        private LoginHelper _lgihp = new LoginHelper();
        private AccountManager _Amgr = new AccountManager();


        //未做:
        //HomePage.aspx
        //顯示母列表 (!=IsPostBack)
        //顯示個人資料 (!=IsPostBack)
        //如果網址列為子板塊，則依會員權限 顯示黑名單 (!=IsPostBack)
        //如果網址列為子板塊，則依會員權限 顯示板主名單 (!=IsPostBack)
        protected void Page_Load(object sender, EventArgs e)
        {

#if(DEBUG)
            this.ckbskip.Visible = true;
#endif


            if (!_amgr.IsLogined())
            {
                this.plhLogin.Visible = false;
                this.plhLogined.Visible = true;
            }

            else if (_amgr.IsLogined())
            {
                this.plhLogin.Visible = false;
                this.plhLogined.Visible = true;
                int memberStatus = 1;//用來測試不同身分別的
                #region//搜尋區

                string currentPboard = this.Request.QueryString["PboardID"];           //從URL取得當前CboardID
                string currentCboard = this.Request.QueryString["CboardID"];           //從URL取得當前CboardID


                /*當前位於母版塊或子板塊內，第二個選項變為可使用
                if (currentPboard != null || currentCboard != null)
                {
                var aaa = this.srchDrop.Items[1].Attributes;
                aaa.Remove("disable");
                }*/
                #endregion

                #region//母版塊區
                if (_amgr.IsLogined())
                {
                    memberStatus = _pBrdMgr.GetMemberStatus();
                }

                if (!this.IsPostBack)
                {
                    DataTable dt = _pBrdMgr.GetPBoardStatus();
                    this.Repeater1.DataSource = dt;
                    this.Repeater1.DataBind();
                    this.Repeater2.DataSource = dt;
                    this.Repeater2.DataBind();

                    if (memberStatus == 3)
                    {
                        btnPBMode1.Visible = true;
                        btnPBMode2.Visible = true;
                    }
                }

                #endregion

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string account = this.txtAccount.Text.Trim();
            string pwd = this.txtPassword.Text.Trim();
            if (this.ckbskip.Checked)
            {
                if (this._Amgr.TryLogin("a123234", "12345678"))
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
            else if (this._Amgr.TryLogin(account, pwd))
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                this.ltlMessage.Text = "登錄失敗，請檢察帳號密碼。";
            }
            

        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            LoginHelper.Logout();
            Response.Redirect(Request.RawUrl);
            
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
            this.plhPBMode1.Visible = false;
            this.plhPBMode2.Visible = false;
            this.plhPBEdit1.Visible = true;
            this.plhPBEdit2.Visible = true;
        }

        //儲存編輯後的母版塊按鈕事件
        protected void btnPBSave_Click(object sender, EventArgs e)
        {
            List<Pboard> newPBData = new List<Pboard>();
            //取得新的母版塊資料(待補)

            //寫入資料庫
            _pBrdMgr.SetPBoardStatus(newPBData);

        }
        //取消編輯母版塊按鈕事件
        protected void btnPBCancel_Click(object sender, EventArgs e)
        {
            this.plhPBMode1.Visible = true;
            this.plhPBMode2.Visible = true;
            this.plhPBEdit1.Visible = false;
            this.plhPBEdit2.Visible = false;
        }
    }
}