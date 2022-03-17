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
        //搜尋頁面負責從RUL抓搜尋的內容
        SearchManager _srchMng = new SearchManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            //從網址列取得URL的querystring
            string keyword = this.Request.QueryString["keyword"];           //搜尋關鍵字
            string searchArea = this.Request.QueryString["searcharea"];     //下拉選項
            string srchCurrent = "";
            string PorCBoard = "";

            //若關鍵字為空則跳出警告訊息
            if (string.IsNullOrWhiteSpace(keyword))
            {
                //跳出警告訊息
                return;
            }

            //如果沒有指定子板塊，就搜尋母版塊
            if (string.IsNullOrWhiteSpace(this.Request.QueryString["srchCboardID"]))
            { 
                srchCurrent = this.Request.QueryString["srchPboardID"];   //當前母板塊
                PorCBoard = "p";
            }
            else
            {
                srchCurrent = this.Request.QueryString["srchCboardID"];   //當前子板塊
                PorCBoard = "c";
            }

            //依指定的搜尋範圍導向function
            switch (searchArea)
            {
                case "srchAll":
                    _srchMng.GetAllSrchList(keyword);   //全站搜尋
                    break;
                case "srchCBoard":
                    _srchMng.GetboardSrchList(keyword, srchCurrent, PorCBoard);   //當前板塊搜尋(可能需要視情況合併方法)
                    break;
                default:
                    _srchMng.GetWriterSrchList(keyword);   //作者搜尋
                    break;
            }
        }
    }
}