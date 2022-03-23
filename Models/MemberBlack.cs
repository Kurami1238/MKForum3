using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class MemberBlack
    {
        public string Account { get; set; }
        public int CboardID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}