using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class MemberRegister
    {
        public int MemberID { get; set; }
        public DateTime SendTime { get; set; }
        public string Captcha { get; set; }

    }
}