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
    public partial class WebForm3 : System.Web.UI.Page
    {
        private PostStampsManager _psmar = new PostStampsManager();
        protected void Page_init(object sender, EventArgs e)
        {
            Uri Url = HttpContext.Current.Request.UrlReferrer; 
            Uri MyUrl = Request.UrlReferrer;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                string CboardID = HttpContext.Current.Request.QueryString["CboardID"];
                int numCboardID = 0;
                if (int.TryParse(CboardID, out int a))
                {
                    numCboardID = Convert.ToInt32(CboardID);
                    List<PostStamp> postStamps = _psmar.GetPostStamps(numCboardID);

                    this.lblCboardName.Text = postStamps[0].Cname;
                };
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsCheck())
            {
                string PostSort = this.txtCboardtype.Text;
                int numCboardID = GetQuerycboardID();

                Member MemberIDs = HttpContext.Current.Session["Member"] as Member;
                Guid MemberID = MemberIDs.MemberID;

                _psmar.InsertPostStamps(numCboardID, PostSort, MemberID);
                this.lblmessage.Text = "存檔成功!";
            }
        }

        private static int GetQuerycboardID()
        {
            string CboardID = HttpContext.Current.Request.QueryString["CboardID"];
            int numCboardID = 0;
            if (int.TryParse(CboardID, out int a))
            {
                numCboardID = Convert.ToInt32(CboardID);
            }

            return numCboardID;
        }

        protected void btncCancel_Click(object sender, EventArgs e)
        {
            
            //Response.Redirect(Url.ToString());
        }

        public bool IsCheck()
        {
            List<string> errormsg = new List<string>();
            List<PostStamp> postStamps = _psmar.GetPostStamps(GetQuerycboardID());
            int c = 0;
            foreach (PostStamp postStamp in postStamps)
            {
                if (postStamp.PostSort == this.txtCboardtype.Text)
                    errormsg.Add("此類型在" + postStamp.PostStampsDate + "加過了啦，87喔!");
                c += 1;

            }


            if (errormsg.Count > 0)
            {
                string ErrorMessage = string.Join("<br />", errormsg);
                this.lblmessage.Text = ErrorMessage;
                return false;
            }

            return true;
        }
    }
}