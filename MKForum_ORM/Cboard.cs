namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cboard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cboard()
        {
            PostStamps = new HashSet<PostStamp>();
            MemberBlacks = new HashSet<MemberBlack>();
            Posts = new HashSet<Post>();
        }

        public int CboardID { get; set; }

        public int PboardID { get; set; }

        [Required]
        [StringLength(200)]
        public string Cname { get; set; }

        public DateTime CboardDate { get; set; }

        [Required]
        [StringLength(50)]
        public string CboardCotent { get; set; }

        public virtual Cboard Cboards1 { get; set; }

        public virtual Cboard Cboard1 { get; set; }

        public virtual Pboard Pboard { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostStamp> PostStamps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberBlack> MemberBlacks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }

        public virtual Member Member { get; set; }
    }
}
