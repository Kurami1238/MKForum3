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
            string pboarqs = this.Request.QueryString["PboardID"];
            if (int.TryParse(pboarqs, out int a))
            {
                int intpboarqs = Convert.ToInt32(pboarqs);

                List<Cboard> cboards = CboardManager.GetCPboardtoCboard(intpboarqs);
                this.rptpbtocb.DataSource = cboards;
                this.rptpbtocb.DataBind();
            }



        }
    }
}