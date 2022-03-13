using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class MemberFollow
    {
        public Guid MemberID { get; set; }
        public Guid PostID { get; set; }
        public bool FollowStatus { get; set; }
        public DateTime ReadedDate { get; set; }
        public bool Replied { get; set; }
    }
}