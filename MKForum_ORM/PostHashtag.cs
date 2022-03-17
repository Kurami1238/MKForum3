namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PostHashtag
    {
        [Key]
        [Column(Order = 0)]
        public int HashtagID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid PostID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string Naiyo { get; set; }
    }
}
