namespace MKForum.MKForum_ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MemberFollow
    {
        public Guid MemberID { get; set; }

        public Guid PostID { get; set; }

        public bool FollowStatus { get; set; }

        public DateTime? ReadedDate { get; set; }

        public bool Replied { get; set; }

        public int ID { get; set; }

        public virtual Member Member { get; set; }

        public virtual Post Post { get; set; }
    }
}
