namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            MemberFollows = new HashSet<MemberFollow>();
            PostImgLists = new HashSet<PostImgList>();
            Posts = new HashSet<Post>();
            Cboards = new HashSet<Cboard>();
        }

        public Guid MemberID { get; set; }

        public int MemberStatus { get; set; }

        [Required]
        [StringLength(20)]
        public string Account { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string NickName { get; set; }

        public DateTime Birthday { get; set; }

        public int Sex { get; set; }

        [StringLength(100)]
        public string PicPath { get; set; }

        public virtual MemberBlack MemberBlack { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberFollow> MemberFollows { get; set; }

        public virtual MemberIP MemberIP { get; set; }

        public virtual MemberRecord MemberRecord { get; set; }

        public virtual MemberScan MemberScan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostImgList> PostImgLists { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cboard> Cboards { get; set; }
    }
}
