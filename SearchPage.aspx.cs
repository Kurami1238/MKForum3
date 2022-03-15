using MKForum.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum
{
    public partial class SearchPage : System.Web.UI.Page
    {
        SearchManager _srchMng = new SearchManager();
        protected void Page_Load(object sender, EventArgs e)
        {

            //從網址列取得URL的querystring
            string keyword = this.Request.QueryString["keyword"];           //搜尋關鍵字
            string searchArea = this.Request.QueryString["searcharea"];     //下拉選項
            string srchCurrent = this.Request.QueryString["srchCurrent"];   //當前板塊
            string currentUrl = this._srchMng.GetCurrentPage(srchCurrent);

            //若關鍵字為空則跳回首頁
            if (string.IsNullOrWhiteSpace(keyword))
            {
                this.Response.Redirect("HomePge.aspx"); //跳轉回首頁
                return;
            }

            //依指定的搜尋範圍導向function
            switch (searchArea)
            {
                case "srchAll":
                    _srchMng.GetAllSrchList(keyword);   //全站搜尋
                    break;
                case "srchCBoard":
                    _srchMng.GetPboardSrchList(keyword, currentUrl);   //當前板塊搜尋(可能需要視情況合併方法)
                    break;
                default:
                    _srchMng.GetWriterSrchList(keyword);   //全站搜尋
                    break;
            }




            //int totalRows = 0;
            //var list = this._mgr.GetMapList(keyword, _pageSize, pageIndex, out totalRows);
            //this.ProcessPager(keyword, pageIndex, totalRows);

            //this.ucPager.TotalRows = totalRows;
            //this.ucPager.PageIndex = pageIndex;
            //this.ucPager.Bind("keyword", keyword);

            //if (list.Count == 0)
            //{
            //    this.plcEmpty.Visible = true;
            //    this.rptList.Visible = false;
            //}
            //else
            //{
            //    this.plcEmpty.Visible = false;
            //    this.rptList.Visible = true;

            //    this.rptList.DataSource = list;
            //    this.rptList.DataBind();
            //}





        }
    }
}