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
        private MemberManager _mmgr = new MemberManager();
        private BlackManager _blkmgr = new BlackManager();
        private ModeratorManager _Mmgr = new ModeratorManager();
        private int memberStatus = 0;//預設會員等級為0


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
                        btnPBMode1.Visible = true;
                        //btnPBMode2.Visible = true;
                    }
                }
                #endregion
            }
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


        protected void btnwebLogin_Click(object sender, EventArgs e)
        {
            this.plhLogin.Visible = true;
            this.plhLogined.Visible = false;
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string account = this.txtAccount.Text.Trim();
            string pwd = this.txtPassword.Text.Trim();
            if (this.ckbskip.Checked)
            {
                if (this._amgr.TryLogin("Text05", "12345678"))
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

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MemberRegister.aspx");
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
            //bool IsBanWord = _chkInpMgr.IncludeBanWord(srchText);   //確認搜尋的關鍵字是否包含屏蔽字


            //驗證關鍵字不為空，且不含禁字，導向搜尋頁面
            if (!string.IsNullOrWhiteSpace(this.txtSearch.Text)/* && !IsBanWord*/)
            {
                if (drowValue != "srchCurrent")  //如果使用者不是選擇搜尋當前頁面
                    this.Response.Redirect("SearchKekka.aspx" + "?keyword=" + srchText + "&searcharea=" + drowValue);
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

                    this.Response.Redirect("SearchKekka.aspx" + "?keyword=" + srchText + srchCurrent + " & searcharea=" + drowValue);
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
            List<Pboard> newPBData = new List<Pboard>();
            //取得新的母版塊資料(待補)

            //寫入資料庫
            //_pBrdMgr.SetPBoardStatus(newPBData);

        }
        //取消編輯母版塊按鈕事件
        protected void btnPBCancel_Click(object sender, EventArgs e)
        {
            //this.plhPBMode1.Visible = true;
            //this.plhPBMode2.Visible = true;
            //this.plhPBEdit1.Visible = false;
            //this.plhPBEdit2.Visible = false;
        }

        protected void lblMember_Change_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MemberEditor.aspx");
        }

        //黑名單儲存按鈕
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
            if (!_chkInpMgr.IsNumAndEG(inpAccount, out outAccount))
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



        //儲存板主
        protected void btnMMSave_Click(object sender, EventArgs e)
        {
            string currentCboard = this.Request.QueryString["CboardID"];    //當前子板塊
            string inpModerator = this.txtBlkAcc.Text.Trim();   //輸入的版主帳號，並去掉空白字元

            if (inpModerator == "")
            {
                string msg = "請輸入新增的版主帳號";
                Response.Write($"<script>alert('{msg}')</script>");
                return;
            }

            string outModerator = "";
            if (!_chkInpMgr.IsNumAndEG(inpModerator, out outModerator))
            {
                string msg = "輸入的帳號不得有英文及數字以外的字元";
                Response.Write($"<script>alert('{msg}')</script>");
                return;

            }
            if (!_blkmgr.HasMember(outModerator))
            {
                string msg = "輸入的帳號不存在";
                Response.Write($"<script>alert('{msg}')</script>");
                return;
            }


            if (this._blkmgr.IsCurrentModerator(currentCboard, outModerator) == true || this._pBrdMgr.CheckMemberStatus(outModerator) == 3)
            {
                string msg = $"輸入帳號為管理員，或已經是該板板主。";
                Response.Write($"<script>alert('{msg}')</script>");
                return;

            }
            //如果已經在黑名單表，不能被提升為板主
            if (_blkmgr.IsBlacked(outModerator, currentCboard))
            {
                string msg = $"黑名單中的帳號不能被提升為板主，請先解除黑名單。";
                Response.Write($"<script>alert('{msg}')</script>");
                return;
            }
            else
            {
                this._Mmgr.AddModeratorsList(outModerator, currentCboard);
                string msg = $"已加入{outModerator}至版主。";
                Response.Write($"<script>alert('{msg}')</script>");
                return;

            }
        }
        protected void btnMMDelete_Click(object sender, EventArgs e)
        {

        }

    }
}