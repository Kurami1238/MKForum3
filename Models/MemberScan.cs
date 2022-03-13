using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class MemberScan
    {
        public int MemberID { get; set; }
        public int PostID { get; set; }
        public DateTime ScanDate { get; set; }
    }
}