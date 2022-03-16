using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Helpers
{
    public class SessionHelper
    {
        public Member GetSessionMember()
        {
            Member member = HttpContext.Current.Session["Member"] as Member;
            return member;
        }
        public string GetSessionMemberID()
        {
            string member = HttpContext.Current.Session["MemberID"].ToString();
            return member;
        }
    }
   
}