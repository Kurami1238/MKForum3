using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class MemberRecord
    {
        public int MemberID { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}