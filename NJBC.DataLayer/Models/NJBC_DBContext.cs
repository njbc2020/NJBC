using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace NJBC.DataLayer.Models
{
    public partial class NJBC_DBContext : DbContext
    {
        
        public NJBC_DBContext(DbContextOptions<NJBC_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OrgQuestion> OrgQuestion { get; set; }
        public virtual DbSet<RelComment> RelComment { get; set; }
        public virtual DbSet<RelQuestion> RelQuestion { get; set; }
        public virtual DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //        optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=NJBC_DB;Integrated Security=True;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrgQuestion>(entity =>
            {
                entity.HasKey(e => e.OrgqId);

                entity.Property(e => e.OrgqId).HasColumnName("ORGQ_ID");

                entity.Property(e => e.OrgQbody).HasColumnName("OrgQBody");

                entity.Property(e => e.OrgQsubject).HasColumnName("OrgQSubject");

                entity.Property(e => e.OrgqIdName)
                    .HasColumnName("ORGQ_ID_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RelComment>(entity =>
            {
                entity.HasKey(e => e.RelcId);

                entity.Property(e => e.RelcId).HasColumnName("RELC_ID");

                entity.Property(e => e.RelCtext).HasColumnName("RelCText");

                entity.Property(e => e.RelcDate)
                    .HasColumnName("RELC_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.RelcIdName)
                    .HasColumnName("RELC_ID_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.RelcRelevance2orgq)
                    .HasColumnName("RELC_RELEVANCE2ORGQ")
                    .HasMaxLength(50);

                entity.Property(e => e.RelcRelevance2relq)
                    .HasColumnName("RELC_RELEVANCE2RELQ")
                    .HasMaxLength(50);

                entity.Property(e => e.RelcUserid)
                    .HasColumnName("RELC_USERID")
                    .HasMaxLength(50);

                entity.Property(e => e.RelcUsername)
                    .HasColumnName("RELC_USERNAME")
                    .HasMaxLength(200);

                entity.Property(e => e.RelqId).HasColumnName("RELQ_ID");

                entity.HasOne(d => d.Relq)
                    .WithMany(p => p.RelComment)
                    .HasForeignKey(d => d.RelqId)
                    .HasConstraintName("FK__RelCommen__RELQ___3C69FB99");
            });

            modelBuilder.Entity<RelQuestion>(entity =>
            {
                entity.HasKey(e => e.RelqId);

                entity.Property(e => e.RelqId).HasColumnName("RELQ_ID");

                entity.Property(e => e.OrgqId).HasColumnName("ORGQ_ID");

                entity.Property(e => e.RelQbody).HasColumnName("RelQBody");

                entity.Property(e => e.RelQsubject).HasColumnName("RelQSubject");

                entity.Property(e => e.RelqCategory)
                    .HasColumnName("RELQ_CATEGORY")
                    .HasMaxLength(250);

                entity.Property(e => e.RelqDate)
                    .HasColumnName("RELQ_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.RelqIdName)
                    .HasColumnName("RELQ_ID_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.RelqRankingOrder)
                    .HasColumnName("RELQ_RANKING_ORDER")
                    .HasMaxLength(50);

                entity.Property(e => e.RelqRelevance2orgq)
                    .HasColumnName("RELQ_RELEVANCE2ORGQ")
                    .HasMaxLength(150);

                entity.Property(e => e.RelqUserid)
                    .HasColumnName("RELQ_USERID")
                    .HasMaxLength(150);

                entity.Property(e => e.RelqUsername)
                    .HasColumnName("RELQ_USERNAME")
                    .HasMaxLength(150);

                entity.HasOne(d => d.Orgq)
                    .WithMany(p => p.RelQuestion)
                    .HasForeignKey(d => d.OrgqId)
                    .HasConstraintName("FK__RelQuesti__ORGQ___398D8EEE");
            });
        }
    }
}
