using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class RankingDATA
    {
        public string Account { get; set; }
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public string CoverImage { get; set; }
        public string PostCotent { get; set; }
        public Guid PostID { get; set; }
        public int ViewCount { get; set; }
        public int CboardID { get; set; }


        public string Cname { get; set; }
        public string Pname { get; set; }


        public string Naiyo { get; set; }
    }
}