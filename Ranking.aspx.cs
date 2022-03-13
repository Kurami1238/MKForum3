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
    public partial class Ranking : System.Web.UI.Page
    {
        private RankingManager _Rmgr = new RankingManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            List<Post> _RankingList = new List<Post>();

            _RankingList = this._Rmgr.GetRankingList();

                this.Repeater1.DataSource = _RankingList;
                this.Repeater1.DataBind();

        }
    }
}