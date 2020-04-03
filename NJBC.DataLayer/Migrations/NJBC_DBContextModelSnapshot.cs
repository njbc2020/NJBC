﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NJBC.DataLayer.Models;

namespace NJBC.DataLayer.Migrations
{
    [DbContext(typeof(NJBC_DBContext))]
    partial class NJBC_DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NJBC.DataLayer.Models.OrgQuestion", b =>
                {
                    b.Property<int>("OrgqId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ORGQ_ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("OrgQbody")
                        .HasColumnName("OrgQBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrgQsubject")
                        .HasColumnName("OrgQSubject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrgqIdName")
                        .HasColumnName("ORGQ_ID_Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrgqId");

                    b.HasIndex("UserId");

                    b.ToTable("OrgQuestion");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.RelComment", b =>
                {
                    b.Property<int>("RelcId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RELC_ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RelCtext")
                        .HasColumnName("RelCText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelCtextClean")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RelcDate")
                        .HasColumnName("RELC_DATE")
                        .HasColumnType("datetime");

                    b.Property<string>("RelcIdName")
                        .HasColumnName("RELC_ID_Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("RelcRelevance2orgq")
                        .HasColumnName("RELC_RELEVANCE2ORGQ")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("RelcRelevance2relq")
                        .HasColumnName("RELC_RELEVANCE2RELQ")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("RelcUserid")
                        .HasColumnName("RELC_USERID")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("RelcUsername")
                        .HasColumnName("RELC_USERNAME")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<int>("RelqId")
                        .HasColumnName("RELQ_ID")
                        .HasColumnType("int");

                    b.HasKey("RelcId");

                    b.HasIndex("RelqId");

                    b.ToTable("RelComment");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.RelQuestion", b =>
                {
                    b.Property<int>("RelqId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RELQ_ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OrgqId")
                        .HasColumnName("ORGQ_ID")
                        .HasColumnType("int");

                    b.Property<string>("RelQbody")
                        .HasColumnName("RelQBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelQsubject")
                        .HasColumnName("RelQSubject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelqCategory")
                        .HasColumnName("RELQ_CATEGORY")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("RelqDate")
                        .HasColumnName("RELQ_DATE")
                        .HasColumnType("datetime");

                    b.Property<string>("RelqIdName")
                        .HasColumnName("RELQ_ID_Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("RelqRankingOrder")
                        .HasColumnName("RELQ_RANKING_ORDER")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("RelqRelevance2orgq")
                        .HasColumnName("RELQ_RELEVANCE2ORGQ")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("RelqUserid")
                        .HasColumnName("RELQ_USERID")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("RelqUsername")
                        .HasColumnName("RELQ_USERNAME")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("RelqId");

                    b.HasIndex("OrgqId");

                    b.ToTable("RelQuestion");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.Semeval2015.Comment", b =>
                {
                    b.Property<long>("CommentId")
                        .HasColumnType("bigint");

                    b.Property<string>("CBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CBodyClean")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CGOLD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CGOLD_YN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CSubject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CUSERID")
                        .HasColumnType("bigint");

                    b.Property<string>("CUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LabelDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ReplayCommentId")
                        .HasColumnType("bigint");

                    b.HasKey("CommentId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.Semeval2015.Question", b =>
                {
                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<bool>("Label")
                        .HasColumnType("bit");

                    b.Property<bool>("LabelComplete")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LabelCompleteDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LabelDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("QBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QCATEGORY")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("QDATE")
                        .HasColumnType("datetime2");

                    b.Property<string>("QGOLD_YN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QSubject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QTYPE")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("QUSERID")
                        .HasColumnType("bigint");

                    b.Property<string>("QUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Reject")
                        .HasColumnType("bit");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastDatetime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.OrgQuestion", b =>
                {
                    b.HasOne("NJBC.DataLayer.Models.User", "User")
                        .WithMany("OrgQuestions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.RelComment", b =>
                {
                    b.HasOne("NJBC.DataLayer.Models.RelQuestion", "Relq")
                        .WithMany("RelComment")
                        .HasForeignKey("RelqId")
                        .HasConstraintName("FK__RelCommen__RELQ___3C69FB99")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.RelQuestion", b =>
                {
                    b.HasOne("NJBC.DataLayer.Models.OrgQuestion", "Orgq")
                        .WithMany("RelQuestion")
                        .HasForeignKey("OrgqId")
                        .HasConstraintName("FK__RelQuesti__ORGQ___398D8EEE")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.Semeval2015.Comment", b =>
                {
                    b.HasOne("NJBC.DataLayer.Models.Semeval2015.Question", "Question")
                        .WithMany("Comments")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.Semeval2015.Question", b =>
                {
                    b.HasOne("NJBC.DataLayer.Models.User", "User")
                        .WithMany("Questions")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
