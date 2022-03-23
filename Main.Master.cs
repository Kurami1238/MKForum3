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
    //目前整理到有bug的地方:
    //版主Manager裡面的IsCurrentModerator有錯誤

    //目前整理到需要改的地方:
    //搜尋功能改成複數關鍵字

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
        private LoginHelper _lgihp = new LoginHelper();
        private BlackManager _blkmgr = new BlackManager();              //黑名單
        private ModeratorManager _MMmgr = new ModeratorManager();      //版主
        private MemberManager _mmgr = new MemberManager();

        private int memberStatus = 0;//預設會員等級為0

        protected void Page_Load(object sender, EventArgs e)
        {
#if (DEBUG)
            this.ckbskip.Visible = true;
#endif


            if (!_amgr.IsLogined())
            {
                this.plhLogin.Visible = false;
                this.plhLogined.Visible = true;
                this.plgMemberStatus.Visible = false;
            }

            else if (_amgr.IsLogined())
            {
                this.btnwebLogin.Visible = false;
                this.plhLogin.Visible = false;
                this.plhLogined.Visible = true;
                Member account = _amgr.GetCurrentUser();
                string memberID = account.MemberID.ToString();
                Member memberInfo = _mmgr.GetMember(memberID);

                this.imgMember_PicPath.ImageUrl = memberInfo.PicPath;
                this.lblMember_NickName.Text = memberInfo.NickName;
                this.lblMember_Account.Text = memberInfo.Account;

                if (memberInfo.MemberStatus == 1)
                    this.lblMember_MemberStatus.Text = "一般會員";
                else if (memberInfo.MemberStatus == 2)
                    this.lblMember_MemberStatus.Text = "版主";
                else if (memberInfo.MemberStatus == 3)
                    this.lblMember_MemberStatus.Text = "管理員";


            }

            string currentCboard = this.Request.QueryString["CboardID"];    //當前子板塊

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
                if (this._MMmgr.IsCurrentModerator(currentCboard) || memberStatus == 3)
                {
                    //this.plhBlk.Visible = true;

                    //BlckMbrDT = _blkmgr.getBlackedList(Convert.ToInt32(currentCboard));
                    //this.RptrBlk.DataSource = BlckMbrDT;
                    //this.RptrBlk.DataBind();
                }
            }
            else
            { }

            #endregion
            #region//顯示版主名單的功能(資料庫沒資料無法測試)
            //如果當前在子板塊內，且為後台人員(身分別為3)，則顯示板主名單
            if (currentCboard != null)
            {
                if (this._pBrdMgr.GetMemberStatus() == 3)
                {
                    this.plhMM.Visible = true;
                    DataTable MMDT = _MMmgr.getModerators(currentCboard);
                    this.RptrMM.DataSource = MMDT;
                    this.RptrMM.DataBind();

                }

            }
            #endregion

        }
        protected void Page_Prerender(object sender, EventArgs e)
        {
            //string currentPboard = this.Request.QueryString["PboardID"];           //從URL取得當前PboardID
            string currentPboard = "2";           //用來測試
            string currentCboard = this.Request.QueryString["CboardID"];    //當前子板塊

            #region//搜尋區的功能

            //當前位於母版塊或子板塊內，第二個選項變為可使用
            if (currentPboard != null || currentCboard != null)
            {
                this.srchDrop.Items[1].Attributes.Remove("disabled");
            }
            #endregion
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string account = this.txtAccount.Text.Trim();
            string pwd = this.txtPassword.Text.Trim();
            if (this.ckbskip.Checked)
            {
                if (this._amgr.TryLogin("a123234", "12345678"))
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
            else if (this._amgr.TryLogin(account, pwd))
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
            _lgihp.Logout();
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
            string currentCboard = this.Request.QueryString["CboardID"];    //當前子板塊
            string inpAccount = this.txtBlkAcc.Text.Trim();   //輸入的黑名單帳號，並去掉空白字元
            string dt = this.txtBlkDate.Text.Trim();       //輸入的黑名單日期

            if (inpAccount == "" || dt == null)
            {
                string msg = "輸入的帳號或日期為空";
                Response.Write($"<script>alert('{msg}')</script>");
                return;
            }

            string outAccount = "";
            if (!_blkmgr.IsNumAndEG(inpAccount, out outAccount))
            {
                string msg = "輸入的帳號不得有英文及數字以外的字元";
                Response.Write($"<script>alert('{msg}')</script>");
                return;

            }
            if (!_blkmgr.HasMember(inpAccount))
            {
                string msg = "輸入的帳號不存在";
                Response.Write($"<script>alert('{msg}')</script>");
                return;
            }
            DateTime dtDate;
            if (!DateTime.TryParse(dt, out dtDate))
            {
                string msg = "輸入的日期格式有誤";
                Response.Write($"<script>alert('{msg}')</script>");
                return;
            }

            string outDate = dtDate.ToString("yyyy-MM-dd");

            if (this._blkmgr.IsCurrentModerator(currentCboard, inpAccount) == true || this._pBrdMgr.CheckMemberStatus(inpAccount) == 3)
            {
                string msg = $"輸入帳號不能為該板板主或管理員。";
                Response.Write($"<script>alert('{msg}')</script>");
                return;

            }
            else
            {
                //如果已經在黑名單表
                if (_blkmgr.IsBlacked(inpAccount, currentCboard))
                {
                    this._blkmgr.UpdateBlackedList(inpAccount, dt, currentCboard);      //無效
                    string msg = $"已更新{outAccount}懲處期限為{outDate} 的 00時00分。";
                    Response.Write($"<script>alert('{msg}')</script>");
                    return;
                }
                //如果還未被黑單過
                else
                {

                    this._blkmgr.AddBlackedList(inpAccount, dt, currentCboard);      //無效
                    string msg = $"已加入{outAccount}至黑單，懲處期限為{outDate} 的 00時00分。";
                    Response.Write($"<script>alert('{msg}')</script>");
                    return;
                }
            }
        }
    }
}