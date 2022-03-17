namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            MemberFollows = new HashSet<MemberFollow>();
            MemberScans = new HashSet<MemberScan>();
        }

        public Guid PostID { get; set; }

        public Guid MemberID { get; set; }

        public int CboardID { get; set; }

        public Guid? PointID { get; set; }

        public DateTime PostDate { get; set; }

        public int PostView { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(4096)]
        public string PostCotent { get; set; }

        public DateTime LastEditTime { get; set; }

        public int Floor { get; set; }

        [StringLength(300)]
        public string CoverImage { get; set; }

        public int? Stamp { get; set; }

        public virtual Cboard Cboard { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberFollow> MemberFollows { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberScan> MemberScans { get; set; }
    }
}
