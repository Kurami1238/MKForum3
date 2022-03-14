using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class MemberModerator
    {
        public Guid MemberID { get; set; }
        public int CboardID { get; set; }
    }
}