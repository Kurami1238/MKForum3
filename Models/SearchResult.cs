using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class SearchResult
    {
        public Guid PostID { get; set; }
        public string NickName { get; set; }
        public string Title { get; set; }
        public string PostCotent { get; set; }
        public string Cname { get; set; }
        public Guid CboardID { get; set; }
        public DateTime PostDate { get; set; }

    }
}