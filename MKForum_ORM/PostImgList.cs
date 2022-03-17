namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PostImgList
    {
        public int ID { get; set; }

        public Guid MemberID { get; set; }

        [Required]
        [StringLength(300)]
        public string ImagePath { get; set; }

        public virtual Member Member { get; set; }
    }
}
