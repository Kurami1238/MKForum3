using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MKForum.MKForum_ORM
{
    public partial class ContextModel : DbContext
    {
        public ContextModel()
            : base("name=MKFourm")
        {
        }

        public virtual DbSet<Cboard> Cboards { get; set; }
        public virtual DbSet<MemberBlack> MemberBlacks { get; set; }
        public virtual DbSet<MemberFollow> MemberFollows { get; set; }
        public virtual DbSet<MemberIP> MemberIPs { get; set; }
        public virtual DbSet<MemberRecord> MemberRecords { get; set; }
        public virtual DbSet<MemberRegister> MemberRegisters { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberScan> MemberScans { get; set; }
        public virtual DbSet<Pboard> Pboards { get; set; }
        public virtual DbSet<PostImgList> PostImgLists { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostStamp> PostStamps { get; set; }
        public virtual DbSet<PostHashtag> PostHashtags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cboard>()
                .Property(e => e.Cname)
                .IsUnicode(false);

            modelBuilder.Entity<Cboard>()
                .Property(e => e.CboardCotent)
                .IsUnicode(false);

            modelBuilder.Entity<Cboard>()
                .HasOptional(e => e.Cboards1)
                .WithRequired(e => e.Cboard1);

            modelBuilder.Entity<Cboard>()
                .HasMany(e => e.PostStamps)
                .WithRequired(e => e.Cboard)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cboard>()
                .HasMany(e => e.MemberBlacks)
                .WithRequired(e => e.Cboard)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cboard>()
                .HasMany(e => e.Posts)
                .WithRequired(e => e.Cboard)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MemberIP>()
                .Property(e => e.IPLocation)
                .IsUnicode(false);

            modelBuilder.Entity<MemberRegister>()
                .Property(e => e.Captcha)
                .IsUnicode(false);

            modelBuilder.Entity<MemberRegister>()
                .HasOptional(e => e.MemberRegisters1)
                .WithRequired(e => e.MemberRegister1);

            modelBuilder.Entity<Member>()
                .Property(e => e.Account)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.NickName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.PicPath)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .HasOptional(e => e.MemberBlack)
                .WithRequired(e => e.Member);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.MemberFollows)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasOptional(e => e.MemberIP)
                .WithRequired(e => e.Member);

            modelBuilder.Entity<Member>()
                .HasOptional(e => e.MemberRecord)
                .WithRequired(e => e.Member);

            modelBuilder.Entity<Member>()
                .HasOptional(e => e.MemberScan)
                .WithRequired(e => e.Member);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.PostImgLists)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Posts)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pboard>()
                .Property(e => e.Pname)
                .IsUnicode(false);

            modelBuilder.Entity<Pboard>()
                .HasMany(e => e.Cboards)
                .WithRequired(e => e.Pboard)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostImgList>()
                .Property(e => e.ImagePath)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.PostCotent)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.CoverImage)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.MemberFollows)
                .WithRequired(e => e.Post)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.MemberScans)
                .WithRequired(e => e.Post)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostStamp>()
                .Property(e => e.PostSort)
                .IsUnicode(false);

            modelBuilder.Entity<PostHashtag>()
                .Property(e => e.Naiyo)
                .IsUnicode(false);
        }
    }
}
