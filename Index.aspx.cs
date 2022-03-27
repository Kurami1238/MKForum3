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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)       //因為asp.net只能認識同一個頁面上的東西，而資料送出後，如果重新建立資料繫節就不是同一個按鈕了，所以這邊不重新跑一次
            {
                List<RankingDATA> rankingList = this._rnkmgr.GetScansList();
                List<RankingDATA> rankingTitleList = this._rnkmgr.GetScansTitleList();

                var newranking =
                    from r in rankingList
                    join rt in rankingTitleList on r.CboardID equals rt.CboardID
                    select new
                    {
                        Account = r.Account,
                        Title = r.Title,
                        PostDate = r.PostDate,
                        CoverImage = r.CoverImage,
                        PostCotent = r.PostCotent,
                        PostID = r.PostID,
                        ViewCount = r.ViewCount,
                        CboardID = r.CboardID,
                        Cname = rt.Cname,
                        Pname = rt.Pname
                    };

                this.rptHotPosts.DataSource = newranking;
                this.rptHotPosts.DataBind();

                List<RankingDATA> rankingHottag = this._rnkmgr.GetHashtags();
                this.rptHotTags.DataSource = rankingHottag;
                this.rptHotTags.DataBind();

            }

        }

    }
}