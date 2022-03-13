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
        private string memberID = "c8142d85-68c2-4483-ab51-e7d3fc366b89";
        private string postID = "b1234463-5433-487f-8a75-a54abaa4017e";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_mfmsg.GetMemberFollowThisPost(memberID, postID).FollowStatus)
                this.lblMemberFollow_FollowStatus.Text = "追蹤中";
            else
                this.lblMemberFollow_FollowStatus.Text = "未追蹤";
        }

        protected void btnMemberFollow_FollowStatus_Click(object sender, EventArgs e)
        {
            
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