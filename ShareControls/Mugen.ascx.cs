using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum.ShareControls
{
    public partial class Mugen : System.Web.UI.UserControl
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRow { get; set; } = 0;
        private string _url = null;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}