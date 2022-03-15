using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MKForum.Managers;
using MKForum.Models;

namespace MKForum
{
    public partial class Index : System.Web.UI.Page
    {
        //引用RankingManager & 宣告List變數
        private RankingManager _rnkmgr = new RankingManager();
        private List<Post> _rankingList = new List<Post>();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)       //因為asp.net只能認識同一個頁面上的東西，而資料送出後，如果重新建立資料繫節就不是同一個按鈕了，所以這邊不重新跑一次
            {

                _rankingList = this._rnkmgr.GetRankingList();

                Repeater1.DataSource = _rankingList;
                Repeater1.DataBind();

            }

        }

    }
}