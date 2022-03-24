using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class SearchResult
    {
        public Guid PostID { get; set; }
        public string Title { get; set; }
        public string PostCotent { get; set; }
        public Guid MemberID { get; set; }
        public int CboardID { get; set; }
        public string MemberAccount { get; set; }
        public DateTime LastEditTime { get; set; }
        public int PostView { get; set; }
        public string CoverImage { get; set; }
    }
}