namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MemberBlack
    {
        [Key]
        public Guid MemberID { get; set; }

        public int CboardID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ReleaseDate { get; set; }

        public virtual Cboard Cboard { get; set; }

        public virtual Member Member { get; set; }
    }
}
