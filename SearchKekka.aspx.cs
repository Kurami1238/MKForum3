using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum
{
    public partial class SearchKekka : System.Web.UI.Page
    {
        private SearchManager _srmgr = new SearchManager();
        protected void Page_Init(object sender, EventArgs e)
        {
            string keyword = this.Request.QueryString["keyword"];           //搜尋關鍵字
            string searchArea = this.Request.QueryString["searcharea"];

            var kwlist = new List<string>();
            string[] kwarr = keyword.Split(' ');
            foreach (var x in kwarr)
            {
                kwlist.Add(x);
            }
            List<SearchResult> srl = this._srmgr.GetAllSearchKekka(kwlist);
            
        }
    }
}