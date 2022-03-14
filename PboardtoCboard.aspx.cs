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
    public partial class PboardtoCboard : System.Web.UI.Page
    {
        private CboardManager _cmgr = new CboardManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            List<Cboard> cboards = CboardManager.GetCPboardtoCboard(1);
            this.rptpbtocb.DataSource = cboards;
            this.rptpbtocb.DataBind();
        }
    }
}