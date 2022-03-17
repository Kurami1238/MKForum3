namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pboard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pboard()
        {
            Cboards = new HashSet<Cboard>();
        }

        public int PboardID { get; set; }

        [Required]
        [StringLength(20)]
        public string Pname { get; set; }

        public DateTime PboardDate { get; set; }

        public int Porder { get; set; }

        public bool Pshow { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cboard> Cboards { get; set; }
    }
}
