using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class Cboard
    {
        public int CboardID { get; set; }
        public int PboardID { get; set; }
        public string Cname { get; set; }
        public DateTime CboardDate { get; set; }
        public string CboardCotent { get; set; }
    }
}