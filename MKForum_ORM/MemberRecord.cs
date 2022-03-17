namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MemberRecord
    {
        [Key]
        public Guid MemberID { get; set; }

        public DateTime EditDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual Member Member { get; set; }
    }
}
