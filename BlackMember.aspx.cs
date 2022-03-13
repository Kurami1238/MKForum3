using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MKForum.Managers;


namespace MKForum
{
    public partial class BlackMember : System.Web.UI.Page
    {
        private BlackManager _Blkmgr = new BlackManager();

        // 從Session取得當前子板塊ID
        int cboardid = 2;
        //int cboardid = this.Session["CboradID"] as int;

        //宣告要顯示在頁面上的黑名單
        private List<MemberBlack> DisplayBlcked = new List<MemberBlack>();


        protected void Page_Load(object sender, EventArgs e)
        {
            DisplayBlcked = this._Blkmgr.getBlackedList(cboardid);

            this.Repeater1.DataSource = DisplayBlcked;
            this.Repeater1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //取得版主輸入的會員資料及懲處時間
            string strBlackedAcc = this.blackedAccount.Text;
            string strRealseDate = this.RealseDate.Text;

           this. _Blkmgr.AddBlackedList( strBlackedAcc,  strRealseDate,  cboardid);        //寫入資料庫目前是用比較暴力的方式判斷新增或覆蓋
        }

    }
}