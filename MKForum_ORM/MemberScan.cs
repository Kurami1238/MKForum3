namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MemberScan
    {
        [Key]
        public Guid MemberID { get; set; }

        public Guid PostID { get; set; }

        public DateTime ScanDate { get; set; }

        public virtual Member Member { get; set; }

        public virtual Post Post { get; set; }
    }
}
