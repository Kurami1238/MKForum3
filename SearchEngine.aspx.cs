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
    public partial class SearchEngine : System.Web.UI.Page
    {
        private SearchManager _Srchmgr = new SearchManager();

        // 從Session取得當前子板塊ID，先暫訂是2
        int cboardid = 2;
        //int cboardid = this.Session["CboradID"] as int;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<SearchResult> _srchList = new List<SearchResult>();

            //使用者搜尋的關鍵字(srchText)
            string srchText = this.SearchBox.Text;


            //根據選取的搜尋範圍走不同的方法，取得搜尋結果的List
            if (this.srchAreaList.SelectedValue == "srchAllSite")
            {
                _srchList = this._Srchmgr.getAllSrchList(srchText, cboardid);
            }

            else if (this.srchAreaList.SelectedValue == "srchPosts")
            {
                _srchList = this._Srchmgr.getCboardSrchList(srchText, cboardid);
            }

            else
            {
                _srchList = this._Srchmgr.getWriterSrchList(srchText, cboardid);
            }


            //如果沒有資料，顯示無資料
            if (_srchList.Count != 0)
            {
                this.Repeater1.DataSource = _srchList;
                this.Repeater1.DataBind();
            }
            else
            {
                this.Repeater1.Visible = false;
                this.plcEmpty.Visible = true;
            }
        }
    }
}