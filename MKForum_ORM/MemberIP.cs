namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MemberIP
    {
        [Key]
        public Guid MemberID { get; set; }

        [Required]
        [StringLength(100)]
        public string IPLocation { get; set; }

        public int MissTotal { get; set; }

        public DateTime LastTime { get; set; }

        public virtual Member Member { get; set; }
    }
}
