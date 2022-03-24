using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class MemberScan
    {
        public int ScanID { get; set; }
        public Guid MemberID { get; set; }
        public Guid PostID { get; set; }
        public DateTime ScanDate { get; set; }
    }
}