using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class PostHashtag
    {
        public int HashtagID { get; set; }
        public Guid PostID { get; set; }
        public string Naiyo { get; set; }
    }
}