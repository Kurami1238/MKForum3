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
    public partial class MemberStatus : System.Web.UI.Page
    {
        MemberManager MemberManager = new MemberManager();

        protected void Page_Load(object sender, EventArgs e)
        {
             Member memberInfo = MemberManager.GetMember("c8142d85-68c2-4483-ab51-e7d3fc366b89");

            this.imgMember_PicPath.ImageUrl = memberInfo.PicPath;
            this.lblMember_NickName.Text = memberInfo.NickName;
            this.lblMember_Account.Text = memberInfo.Account;

            if (memberInfo.MemberStatus == 1)
                this.lblMember_MemberStatus.Text = "一般會員";
            else if (memberInfo.MemberStatus == 2)
                this.lblMember_MemberStatus.Text = "版主";
            else if (memberInfo.MemberStatus == 3)
                this.lblMember_MemberStatus.Text = "管理員";
        }
    }
}