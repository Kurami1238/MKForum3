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
    public partial class MemberList : System.Web.UI.Page
    {
        private MemberManager _mmgr = new MemberManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            List<Member> memberslist = this._mmgr.GetMembers();

            if (memberslist.Count() == 0)
            {
                this.GVMembers.Visible = false;
                this.phEmpty.Visible = true;
            }
            else
            {
                this.phEmpty.Visible = false;
                this.GVMembers.DataSource = memberslist;
                this.GVMembers.DataBind();
            }
        }
    }
}