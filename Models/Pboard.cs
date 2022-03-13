using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class Pboard
    {
        public int PboardID { get; set; }
        public string Pname { get; set; }
        public DateTime PboardDate { get; set; }
        public int Porder { get; set; }
        public bool Pshow { get; set; }
    }
}