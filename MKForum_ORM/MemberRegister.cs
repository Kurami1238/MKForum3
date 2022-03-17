namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MemberRegister
    {
        [Key]
        public Guid MemberID { get; set; }

        public DateTime SendTime { get; set; }

        [Required]
        [StringLength(100)]
        public string Captcha { get; set; }

        public virtual MemberRegister MemberRegisters1 { get; set; }

        public virtual MemberRegister MemberRegister1 { get; set; }
    }
}
