using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.Models
{
    public class PostImgList
    {
        public int ID { get; set; }
        public Guid MemberID { get; set; }
        public string ImagePath { get; set; }
    }
}