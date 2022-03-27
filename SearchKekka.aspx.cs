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
            if (!IsPostBack)
            {
                string keyword = this.Request.QueryString["keyword"];
                string searchArea = this.Request.QueryString["searcharea"];
                if ((string.Compare(searchArea, "srchWriter") == 0) || (string.Compare(searchArea, "srchCurrent") == 0) || (string.Compare(searchArea, "srchAll") == 0)|| (string.Compare(searchArea, "srchTag") == 0))
                {
                    // 切分關鍵字，透過空白，有重複的會排除
                    List<string> kwlist = this.Setudan(keyword);
                    // 根據選擇的模式分流
                    List<SearchResult> srl = this.ModeErabi(searchArea, kwlist, out int kazu);
                    // 文章若長度大於二十個字，後面則隱藏
                    List<SearchResult> seripost = PostContentSeri(srl);
                    // 合併把文章內容大於二十字的替換掉
                    var pLMLS = from p in srl
                                join s in seripost on p.PostID equals s.PostID
                                into tempPS
                                select new
                                {
                                    PostID = p.PostID,
                                    Title = p.Title,
                                    PostCotent = (tempPS.FirstOrDefault() != null) ? tempPS.FirstOrDefault().PostCotent : p.PostCotent,
                                    MemberID = p.MemberID,
                                    CboardID = p.CboardID,
                                    MemberAccount = p.MemberAccount,
                                    LastEditTime = p.LastEditTime,
                                    PostView = p.PostView,
                                    CoverImage = p.CoverImage,
                                    Floor = p.Floor,
                                    PointID = p.PointID,
                                };
                    this.ltlmsg.Text = $"搜尋結果： 共 {kazu} 筆";
                    this.rptcBtoP.DataSource = pLMLS;
                    this.rptcBtoP.DataBind();
                }
                else
                    Response.Redirect("Index.aspx", true);
                //Request.Form = Formcollection
            }
        }

        private List<SearchResult> ModeErabi(string searchArea, List<string> kwlist, out int totalrow)
        {
            int kazu = 0;
            List<SearchResult> srl = new List<SearchResult>();
            switch (searchArea)
            {
                case "srchWriter":
                    srl = this._srmgr.GetMemberSearchKekka(kwlist, out kazu);
                    break;
                case "srchCurrent":
                    if (this.Request.QueryString["srchCboardID"] != null)
                        srl = this._srmgr.GetboardSearchKekka(kwlist, this.Request.QueryString["srchCboardID"], "c", out kazu);
                    else
                        srl = this._srmgr.GetboardSearchKekka(kwlist, this.Request.QueryString["srchPboardID"], "p", out kazu);
                    break;
                case "srchAll":
                    srl = this._srmgr.GetAllSearchKekka(kwlist, out kazu);
                    break;
                case "srchTag":
                    srl = this._srmgr.GetTagSearchKekka(kwlist, out kazu);
                    break;
            }
            totalrow = kazu;
            return srl;
        }

        private List<string> Setudan(string keyword)
        {
            var kwlist = new List<string>();
            string[] kwarr = keyword.Split(' ');
            foreach (var x in kwarr)
            {
                for (int i = 0; i < kwlist.Count; i++)
                {
                    if (string.Compare(x, kwlist[i]) == 0)
                    {
                        kwlist.Remove(x);
                    }
                }
                kwlist.Add(x);
            }

            return kwlist;
        }

        private static List<SearchResult> PostContentSeri(List<SearchResult> postList)
        {
            var postcontent = postList.Select((x, index) => { return new { PostCotent = x.PostCotent, PostID = x.PostID }; });
            List<SearchResult> seripost = new List<SearchResult>();
            foreach (var x in postcontent)
            {
                char[] pccarr = x.PostCotent.ToCharArray();
                if (pccarr.Length > 20)
                {
                    string newpc = string.Empty;
                    for (int i = 0; i < 20; i++)
                    {
                        newpc += pccarr[i];
                    }
                    newpc += "...(點擊後觀看)";
                    seripost.Add(new SearchResult() { PostID = x.PostID, PostCotent = newpc });
                }
            }

            return seripost;
        }
    }
}