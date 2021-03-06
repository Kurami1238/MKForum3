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
        private MemberFollowManager _mfmgr = new MemberFollowManager();
        private StampManager _stpmgr = new StampManager();               //文章類型
        private CboardManager _cBrdMgr = new CboardManager(); //母版塊
        private int memberStatus = 0;//預設會員等級為0


        //未做:
        //HomePage.aspx

        //顯示個人資料 (!=IsPostBack)
        protected void Page_Load(object sender, EventArgs e)
        {

#if(DEBUG)
            this.ckbskip.Visible = true;
#endif

            if ((Session["NeedTouroku"] as int? == 1 || Session["NeedTouroku"] as int? == 2) && Session["JumpPage"] as int? != 1)
            {
                HttpContext.Current.Session["JumpPage"] = 1;
                this.plhLogin.Visible = true;
                this.plhLogined.Visible = false;
            }
            else if (!_amgr.IsLogined())
            {
                this.plhLogin.Visible = false;
                this.plhLogined.Visible = true;
                this.plgMemberStatus.Visible = false;
            }
            else if (_amgr.IsLogined())
            {
                //this.lblMember_Change.Text = "資料" + Environment.NewLine + "變更";
                string MemberID = HttpContext.Current.Session["MemberID"].ToString();

                List<Post> MemberFollows = _mfmgr.GetReplied_POSTMemberFollows(MemberID);

                int? RepliedCount = _mfmgr.GetReplied_count(MemberID);
                if (_mfmgr.GetReplied_POSTMemberFollows(MemberID) != null)
                {
                    if (!IsPostBack)
                    {
                        this.rptMemberFollows.DataSource = MemberFollows;
                        this.rptMemberFollows.DataBind();
                    }
                }
                if (RepliedCount != null)
                    this.Notifycount.Text = RepliedCount.ToString();
                else
                    this.Notifycount.Text = "";

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




            #region//母版塊區的功能

            string currentPboard = this.Request.QueryString["PboardID"];           //從URL取得當前CboardID
            string currentCboard = this.Request.QueryString["CboardID"];           //從URL取得當前CboardID


            //如果是管理員，顯示母子版塊編輯按鈕
            if (_pBrdMgr.GetMemberStatus() == 3)
            {
                this.plhPBEdit1.Visible = true;
                this.plhPBEdit2.Visible = true;

                //管理員在母版進子版的頁面才能看到子版增刪修
                if (currentPboard != null && currentCboard == null && plhLogin.Visible ==false)
                {
                    this.plhCBAPI_normal2.Visible = true;
                    this.plhCBEdit2.Visible = true;
                }
            }

            #endregion

            #region//顯示黑名單及子板文章類型的功能
            List<MemberBlack> blckMbrList;
            DataTable stampDT;
            //如果當前在子板塊內，且為該子版版主，則顯示黑名單及子板文章類型
            if (currentCboard != null)
            {
                if (this._Mmgr.IsCurrentModerator(currentCboard) || _pBrdMgr.GetMemberStatus() == 3)
                {
                    this.plhBlk.Visible = true;
                    blckMbrList = _blkmgr.getBlackedList(currentCboard);
                    if (blckMbrList.Count != 0)
                    {
                        this.ltlBlk.Text = "帳號：(解黑日期)";
                        this.RptrBlk.DataSource = blckMbrList;
                        this.RptrBlk.DataBind();
                    }
                    stampDT = this._stpmgr.getStamp(currentCboard);
                    if (stampDT.Rows.Count != 0)
                    {
                        this.ltlStp.Text = "現有的文章類型：";
                        this.RptrStp.DataSource = stampDT;
                        this.RptrStp.DataBind();
                    }
                }
            }
            #endregion

            #region//顯示版主名單的功能
            //如果當前在子板塊內，且為後台人員(身分別為3)，則顯示板主名單
            if (currentCboard != null)
            {
                if (this._pBrdMgr.GetMemberStatus() == 3)
                {
                    this.plhMM.Visible = true;
                    DataTable MMDT = _Mmgr.getModerators(currentCboard);
                    if (MMDT.Rows.Count != 0)
                    {
                        this.ltlMM.Text = "版主名單列表：";
                        this.RptrMM.DataSource = MMDT;
                        this.RptrMM.DataBind();
                    }
                }
            }
            #endregion

        }
        public string Replacereply(string str)
        {
            if (str.Length > 5)
            {
                str = str.Substring(0, 6) + "...";
            }
            return str;
        }
        public string ReplaceArticle(string str)
        {
            if (str.Length > 10)
            {
                str = str.Substring(0, 10) + "...";
            }
            return str;
        }

        protected void Page_Prerender(object sender, EventArgs e)
        {

            #region//搜尋區的功能
            string currentPboard = this.Request.QueryString["PboardID"];    //當前母板塊
            string currentCboard = this.Request.QueryString["CboardID"];    //當前子板塊

            //當前位於母版塊或子板塊內，第二個選項變為可使用
            if (currentPboard != null || currentCboard != null)
            {
                this.srchDrop.Items[1].Attributes.Remove("disabled");
            }
            #endregion

            #region//子母板塊顯示隱藏功能
            //母板編輯按鈕顯示：再次確認如果處於編輯中的狀態，則隱藏"編輯母板"按鈕
            if (this.plhAPI_admin1.Visible || this.plhAPI_admin2.Visible)
            {
                this.plhPBEdit1.Visible = false;    //隱藏母板編輯按鈕
                this.plhPBEdit2.Visible = false;    //隱藏母板編輯按鈕
                this.plhAPI_normal1.Visible = false;    //隱藏顯示用的母板列表
                this.plhAPI_normal2.Visible = false;    //隱藏顯示用的母板列表
            }

            //子板編輯按鈕顯示：再次確認如果處於編輯中的狀態，則隱藏"編輯子板"按鈕
            if (this.plhCBAPI_admin2.Visible || this.plhCBAPI_admin1.Visible)
            {
                this.plhCBEdit1.Visible = false;    //隱藏子板編輯按鈕
                this.plhCBEdit2.Visible = false;    //隱藏子板編輯按鈕
                this.plhCBAPI_normal1.Visible = false;    //隱藏顯示用的子板列表
                this.plhCBAPI_normal2.Visible = false;    //隱藏顯示用的子板列表

            }
            //把當前母板的值傳到HTML再傳到API
            this.currentPB.Value = currentPboard;
            #endregion

            //藉由預存session跳出視窗 的字串
            if (Session["mainMsg"] != null)
            {
                this.mainMsg.Value = Session["mainMsg"] as string;
                Session.Remove("mainMsg");
            }

        }

        protected void rptMemberFollows_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<MemberFollow> allMemberFollow = _mfmgr.GetALLMemberFollow();
            foreach (MemberFollow MFPostID in allMemberFollow)
            {
                if (e.CommandName == MFPostID.PostID.ToString())
                {
                    string MemberID = HttpContext.Current.Session["MemberID"].ToString();
                    string postID = e.CommandArgument.ToString();
                    string[] postIDs = postID.Split(' ');
                    this._mfmgr.UpdateReplied(MemberID, postIDs[2], 1);

                    string url = string.Format("DisplayPost.aspx?CboardID={0}&PostID={1}#{2}", postIDs[0], postIDs[3], postIDs[1]);
                    if (Convert.ToInt32(postIDs[1]) == 1)
                    {
                        url = string.Format("DisplayPost.aspx?CboardID={0}&PostID={1}#{2}", postIDs[0], postIDs[2], postIDs[1]);
                    }

                    Response.Redirect(url);
                }
            }
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
                if (this._amgr.TryLogin("666", "a11111111"))
                {
                    Session["NeedTouroku"] = null;
                    Response.Redirect(Request.RawUrl);
                }
            }
            else if (this._amgr.TryLogin(account, pwd))
            {
                Session["NeedTouroku"] = null;
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

                    if (srchCboardID != null)
                        srchCurrent = "&srchCboardID=" + srchCboardID;
                    else
                    {
                        srchCurrent = "&srchPboardID=" + srchPboardID;
                    }

                    this.Response.Redirect("SearchKekka.aspx" + "?keyword=" + srchText + srchCurrent + "&searcharea=" + drowValue);
                }
            }
            else
            {
                //跳出警告訊息
            }
        }

        //母版塊編輯按鈕
        protected void btnPBEditMode_Click(object sender, EventArgs e)
        {
            #region//第一組的顯示隱藏
            this.plhPBEdit1.Visible = false;    //隱藏編輯按鈕
            this.plhAPI_normal1.Visible = false;    //關閉顯示模式的ajax
            this.plhAPI_admin1.Visible = true;    //換成有按鈕的ajax
            this.plhPBDsplMode1.Visible = true;    //顯示儲存按鈕
            #endregion

            #region//第二組的顯示隱藏
            this.plhPBEdit2.Visible = false;    //隱藏編輯按鈕
            this.plhAPI_normal2.Visible = false;    //關閉顯示模式的ajax
            this.plhAPI_admin2.Visible = true;    //換成有按鈕的ajax
            this.plhPBDsplMode2.Visible = true;    //顯示儲存按鈕
            #endregion
        }

        //儲存母版塊按鈕
        protected void btnPBSave_Click(object sender, EventArgs e)
        {
            this.plhPBDsplMode2.Visible = false;    //隱藏儲存按鈕
            this.plhAPI_admin2.Visible = false;    //關閉有按鈕的ajax
            this.plhAPI_normal2.Visible = true;    //換成顯示模式的ajax
            this.plhPBEdit2.Visible = true;    //顯示編輯按鈕
            this.plhPBDsplMode1.Visible = false;    //隱藏儲存按鈕
            this.plhAPI_admin1.Visible = false;    //關閉有按鈕的ajax
            this.plhAPI_normal1.Visible = true;    //換成顯示模式的ajax
            this.plhPBEdit1.Visible = true;    //顯示編輯按鈕
        }

        //子版塊編輯按鈕
        protected void btnCBEdit_Click(object sender, EventArgs e)
        {
            #region//第一組的顯示隱藏
            this.plhCBEdit1.Visible = false;    //隱藏編輯按鈕
            this.plhCBAPI_normal1.Visible = false;    //關閉顯示模式的ajax子列表
            this.plhCBAPI_admin1.Visible = true;    //換成有按鈕的ajax子列表
            this.plhCBDsplMode1.Visible = true;    //顯示儲存按鈕
            #endregion

            #region//第二組的顯示隱藏
            this.plhCBEdit2.Visible = false;    //隱藏編輯按鈕
            this.plhCBAPI_normal2.Visible = false;    //關閉顯示模式的ajax子列表
            this.plhCBAPI_admin2.Visible = true;    //換成有按鈕的ajax子列表
            this.plhCBDsplMode2.Visible = true;    //顯示儲存按鈕
            #endregion
        }

        //儲存子版塊按鈕
        protected void btnCBSave_Click(object sender, EventArgs e)
        {
            #region//第一組的顯示隱藏
            this.plhCBEdit1.Visible = true;    //顯示編輯按鈕
            this.plhCBAPI_normal1.Visible = true;    //顯示顯示模式的ajax子列表
            this.plhCBAPI_admin1.Visible = false;    //關閉有按鈕的ajax子列表
            this.plhCBDsplMode1.Visible = false;    //關閉儲存按鈕
            #endregion

            #region//第二組的顯示隱藏
            this.plhCBEdit2.Visible = true;    //顯示編輯按鈕
            this.plhCBAPI_normal2.Visible = true;    //顯示顯示模式的ajax子列表
            this.plhCBAPI_admin2.Visible = false;    //關閉有按鈕的ajax子列表
            this.plhCBDsplMode2.Visible = false;    //關閉儲存按鈕
            #endregion
        }

        protected void lblMember_Change_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MemberEditor.aspx");
        }

        //黑名單儲存按鈕
        protected void btnBlk_Click(object sender, EventArgs e)
        {
            try
            {
                string currentCboard = this.Request.QueryString["CboardID"];    //當前子板塊
                string inpAccount = this.txtBlkAcc.Text.Trim();   //輸入的黑名單帳號，並去掉空白字元
                string dt = this.txtBlkDate.Text.Trim();       //輸入的黑名單日期

                if (inpAccount == "" || dt == null)
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的帳號或日期為空。";
                    return;
                }

                string outAccount = "";
                if (!_chkInpMgr.IsNumAndEG(inpAccount, out outAccount))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的帳號不得有英文及數字以外的字元。";
                    return;

                }
                if (!_blkmgr.HasMember(inpAccount))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的帳號不存在。";
                    return;
                }
                DateTime dtDate;
                if (!DateTime.TryParse(dt, out dtDate))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的日期格式有誤。";
                    return;
                }

                string outDate = dtDate.ToString("yyyy-MM-dd");

                if (this._blkmgr.IsCurrentModerator(currentCboard, inpAccount) == true || this._pBrdMgr.CheckMemberStatus(inpAccount) == 3)
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入帳號不能為該板板主或管理員。";
                    return;
                }
                else
                {
                    //如果已經在黑名單表
                    if (_blkmgr.InBlacked(inpAccount, currentCboard))
                    {
                        //如果正在被黑名單
                        if (_blkmgr.IsBlacked(inpAccount, currentCboard))
                        {
                            this._blkmgr.UpdateBlackedList(inpAccount, dt, currentCboard);
                            HttpContext.Current.Session["mainMsg"] = $"已更新{outAccount}懲處期限為{outDate} 的 00時00分。";
                        }

                        else//曾被黑單，但已經解黑，要再度加入黑名單
                        {
                            this._blkmgr.UpdateBlackedList(inpAccount, dt, currentCboard);
                            HttpContext.Current.Session["mainMsg"] = $"已加入{outAccount}至黑單，懲處期限為{outDate} 的 00時00分。";
                        }
                    }
                    //如果還未被黑單過
                    else
                    {
                        this._blkmgr.AddBlackedList(inpAccount, dt, currentCboard);
                        HttpContext.Current.Session["mainMsg"] = $"已加入{outAccount}至黑單，懲處期限為{outDate} 的 00時00分。";
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["mainMsg"] = $"編輯黑名單失敗，請聯絡管理員。";
            }
            //不論判斷結果為何，都重整頁面
            Response.Redirect(Request.RawUrl);
        }

        //儲存板主
        protected void btnMMSave_Click(object sender, EventArgs e)
        {
            try
            {
                string currentCboard = this.Request.QueryString["CboardID"];    //當前子板塊
                string inpModerator = this.txtMMAcc.Text.Trim();   //輸入的版主帳號，並去掉空白字元

                if (inpModerator == "")
                {
                    HttpContext.Current.Session["mainMsg"] = $"請輸入新增的版主帳號。";
                    return;
                }
                string outModerator = "";
                if (!_chkInpMgr.IsNumAndEG(inpModerator, out outModerator))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的帳號不得有英文及數字以外的字元。";
                    return;
                }
                if (!_blkmgr.HasMember(outModerator))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的帳號不存在。";
                    return;
                }
                if (this._blkmgr.IsCurrentModerator(currentCboard, outModerator) == true || this._pBrdMgr.CheckMemberStatus(outModerator) == 3)
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入帳號為管理員，或已經是該板板主。";
                    return;
                }
                //如果已經在黑名單表，不能被提升為板主
                if (_blkmgr.IsBlacked(outModerator, currentCboard))
                {
                    HttpContext.Current.Session["mainMsg"] = $"黑名單中的帳號不能被提升為板主，請先解除黑名單。";
                    return;
                }
                else
                {
                    this._Mmgr.AddModeratorsList(outModerator, currentCboard);
                    HttpContext.Current.Session["mainMsg"] = $"已加入{outModerator}至版主。";
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["mainMsg"] = $"新增板主失敗，請聯絡管理員。";
            }
            //不論判斷結果為何，都重整頁面
            Response.Redirect(Request.RawUrl);
        }

        //刪除板主
        protected void btnMMDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string currentCboard = this.Request.QueryString["CboardID"];    //當前子板塊
                string inpModerator = this.txtMMAcc.Text.Trim();   //輸入的版主帳號，並去掉空白字元

                if (inpModerator == "")
                {
                    HttpContext.Current.Session["mainMsg"] = $"請輸入刪除的版主帳號。";
                    return;
                }
                string outModerator = "";
                if (!_chkInpMgr.IsNumAndEG(inpModerator, out outModerator))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的帳號不得有英文及數字以外的字元。";
                    return;
                }
                if (!this._blkmgr.IsCurrentModerator(currentCboard, outModerator))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的帳號不是本板板主。";
                }
                else
                {

                    this._Mmgr.DeleteModeratorsList(outModerator, currentCboard);
                    HttpContext.Current.Session["mainMsg"] = $"已從板主名單移除{outModerator}。";
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["mainMsg"] = $"刪除板主失敗，請聯絡管理員。";
            }
            //不論判斷結果為何，都重整頁面
            Response.Redirect(Request.RawUrl);
        }

        //新增母板
        protected void btnAddPB_Click(object sender, EventArgs e)
        {
            try
            {
                //取得輸入的新母板名稱，並去掉空白
                string inpPBName = this.addPBoard.Text.Trim();
                //如果輸入的母板名稱是空的，提示使用者
                string msg = "";

                if (inpPBName == null)
                {
                    HttpContext.Current.Session["mainMsg"] = $"未輸入母板名稱。";
                    return;
                }
                //如果輸入的母板塊包含禁字
                if (this._chkInpMgr.IncludeBanWord(inpPBName))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的文字不可包含: 幹尛、你媽超胖 等字詞。";
                    return;
                }

                this._pBrdMgr.AddPBoard(inpPBName);
                HttpContext.Current.Session["mainMsg"] = $"母板塊新增成功。";
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["mainMsg"] = $"母板塊新增失敗，請聯絡管理員。";
            }
        }

        //新增子板
        protected void btnAddCB_Click(object sender, EventArgs e)
        {
            try
            {
                string currentPboard = this.Request.QueryString["PboardID"];           //從URL取得當前CboardID
                //取得輸入的新子板名稱，並去掉空白
                string inpCBName = this.addCBoard.Text.Trim();
                string cboardcontent = this.addCBoardcontent.Text.Trim();

                //如果輸入的子板名稱是空的，提示使用者
                string msg = "";

                if (inpCBName == null)
                {
                    HttpContext.Current.Session["mainMsg"] = $"未輸入子板名稱。";
                    return;
                }
                //如果輸入的子板塊包含禁字
                if (this._chkInpMgr.IncludeBanWord(inpCBName))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的文字不可包含: 幹尛、你媽超胖 等字詞。";
                    return;
                }

                this._cBrdMgr.CreateCboard(currentPboard, inpCBName, cboardcontent);
                HttpContext.Current.Session["mainMsg"] = $"子板塊新增成功。";
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["mainMsg"] = $"子板塊新增失敗，請聯絡管理員。";
            }
        }

        //新增文章類型
        protected void btnStpAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string currentCboard = this.Request.QueryString["CboardID"];    //當前子板塊
                //取得輸入的新類型名稱，並去掉空白
                string strIinpStp = this.inpStp.Text.Trim();

                if (strIinpStp == null)
                {
                    HttpContext.Current.Session["mainMsg"] = $"未輸入文章類型名稱。";
                    return;
                }
                //如果輸入的新類型包含禁字
                if (this._chkInpMgr.IncludeBanWord(strIinpStp))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的文字不可包含: 幹尛、你媽超胖 等字詞。";
                    return;
                }
                //如果輸入的新類型已經存在
                if (this._stpmgr.IncludeStp(strIinpStp, currentCboard))
                {
                    HttpContext.Current.Session["mainMsg"] = $"文章類型已經存在。";
                }
                else
                {
                    this._stpmgr.AddStmp(strIinpStp, currentCboard);
                    HttpContext.Current.Session["mainMsg"] = $"已新增文章類型：{strIinpStp}。";
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["mainMsg"] = $"文章類型新增失敗，請聯絡管理員。";
            }
            //不論判斷結果為何，都重整頁面
            Response.Redirect(Request.RawUrl);
        }

        //刪除文章類型
        protected void btnStpDelect_Click(object sender, EventArgs e)
        {
            try
            {
                string currentCboard = this.Request.QueryString["CboardID"];    //當前子板塊
                string strIinpStp = this.inpStp.Text.Trim();   //輸入的文章類型，並去掉空白字元

                if (strIinpStp == "")
                {
                    HttpContext.Current.Session["mainMsg"] = $"未輸入欲刪除的文章類型。";
                    return;
                }

                if (this._chkInpMgr.IncludeBanWord(strIinpStp))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的文字不可包含: 幹尛、你媽超胖 等字詞。";
                    return;

                }
                if (!this._stpmgr.IncludeStp(strIinpStp, currentCboard))
                {
                    HttpContext.Current.Session["mainMsg"] = $"輸入的文章類型不存在。";
                }
                else
                {
                    this._stpmgr.DeleteStmp(strIinpStp, currentCboard);
                    HttpContext.Current.Session["mainMsg"] = $"已從文章類型移除{strIinpStp}。";
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["mainMsg"] = $"文章類型刪除失敗，請聯絡管理員。";
            }
            //不論判斷結果為何，都重整頁面
            Response.Redirect(Request.RawUrl);
        }

    }
}