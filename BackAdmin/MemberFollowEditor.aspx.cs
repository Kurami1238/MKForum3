using MKForum.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MKForum.BackAdmin
{
    public partial class MemberFollowEditor : System.Web.UI.Page
    {
        private MemberFollowManager _mfmsg = new MemberFollowManager();
        private Guid memberID = (Guid)HttpContext.Current.Session["MemberID"];

        //private string memberID = "52524a17-45ab-4c49-8f0a-3a66d4a72e48";
        //private string postID = "3e9d0f09-0634-4bab-bfce-bb104084c3c6";

        protected void Page_Load(object sender, EventArgs e)
        {
            Guid postID = Guid.Parse(Request.QueryString["postID"]);

            if (_mfmsg.GetMemberFollowThisPost(memberID, postID).FollowStatus)
                this.lblMemberFollow_FollowStatus.Text = "追蹤中";
            else
                this.lblMemberFollow_FollowStatus.Text = "未追蹤";
        }

        protected void btnMemberFollow_FollowStatus_Click(object sender, EventArgs e)
        {
            Guid postID = Guid.Parse(Request.QueryString["postID"]);


            if (this.lblMemberFollow_FollowStatus.Text == "追蹤中")
            {
                _mfmsg.Updatetrack(memberID, postID, 0);
                this.lblMemberFollow_FollowStatus.Text = "未追蹤";
            }
            else if (this.lblMemberFollow_FollowStatus.Text == "未追蹤")
            {
                _mfmsg.Updatetrack(memberID, postID, 1);
                    this.lblMemberFollow_FollowStatus.Text = "追蹤中";
            }
            
        }
    }
}