namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PostStamp
    {
        [Key]
        public int SortID { get; set; }

        public int CboardID { get; set; }

        public Guid MemberID { get; set; }

        [Required]
        [StringLength(50)]
        public string PostSort { get; set; }

        public DateTime PostStampsDate { get; set; }

        public virtual Cboard Cboard { get; set; }
    }
}
