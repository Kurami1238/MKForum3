using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class Members
    {
        public Guid MemberID { get; set; }
        public int MemberStatus { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public DateTime Birthday { get; set; }
        public int Sex { get; set; }


    }
}