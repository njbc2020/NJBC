﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NJBC.DataLayer.Models;

namespace NJBC.DataLayer.Migrations
{
    [DbContext(typeof(NJBC_DBContext))]
    [Migration("20200313212214_User_ForeignKey")]
    partial class User_ForeignKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NJBC.DataLayer.Models.OrgQuestion", b =>
                {
                    b.Property<int>("OrgqId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ORGQ_ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("OrgQbody")
                        .HasColumnName("OrgQBody");

                    b.Property<string>("OrgQsubject")
                        .HasColumnName("OrgQSubject");

                    b.Property<string>("OrgqIdName")
                        .HasColumnName("ORGQ_ID_Name")
                        .HasMaxLength(50);

                    b.Property<int>("UserId");

                    b.HasKey("OrgqId");

                    b.HasIndex("UserId");

                    b.ToTable("OrgQuestion");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.RelComment", b =>
                {
                    b.Property<int>("RelcId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RELC_ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RelCtext")
                        .HasColumnName("RelCText");

                    b.Property<DateTime?>("RelcDate")
                        .HasColumnName("RELC_DATE")
                        .HasColumnType("datetime");

                    b.Property<string>("RelcIdName")
                        .HasColumnName("RELC_ID_Name")
                        .HasMaxLength(50);

                    b.Property<string>("RelcRelevance2orgq")
                        .HasColumnName("RELC_RELEVANCE2ORGQ")
                        .HasMaxLength(50);

                    b.Property<string>("RelcRelevance2relq")
                        .HasColumnName("RELC_RELEVANCE2RELQ")
                        .HasMaxLength(50);

                    b.Property<string>("RelcUserid")
                        .HasColumnName("RELC_USERID")
                        .HasMaxLength(50);

                    b.Property<string>("RelcUsername")
                        .HasColumnName("RELC_USERNAME")
                        .HasMaxLength(200);

                    b.Property<int?>("RelqId")
                        .HasColumnName("RELQ_ID");

                    b.HasKey("RelcId");

                    b.HasIndex("RelqId");

                    b.ToTable("RelComment");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.RelQuestion", b =>
                {
                    b.Property<int>("RelqId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RELQ_ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("OrgqId")
                        .HasColumnName("ORGQ_ID");

                    b.Property<string>("RelQbody")
                        .HasColumnName("RelQBody");

                    b.Property<string>("RelQsubject")
                        .HasColumnName("RelQSubject");

                    b.Property<string>("RelqCategory")
                        .HasColumnName("RELQ_CATEGORY")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("RelqDate")
                        .HasColumnName("RELQ_DATE")
                        .HasColumnType("datetime");

                    b.Property<string>("RelqIdName")
                        .HasColumnName("RELQ_ID_Name")
                        .HasMaxLength(50);

                    b.Property<string>("RelqRankingOrder")
                        .HasColumnName("RELQ_RANKING_ORDER")
                        .HasMaxLength(50);

                    b.Property<string>("RelqRelevance2orgq")
                        .HasColumnName("RELQ_RELEVANCE2ORGQ")
                        .HasMaxLength(150);

                    b.Property<string>("RelqUserid")
                        .HasColumnName("RELQ_USERID")
                        .HasMaxLength(150);

                    b.Property<string>("RelqUsername")
                        .HasColumnName("RELQ_USERNAME")
                        .HasMaxLength(150);

                    b.HasKey("RelqId");

                    b.HasIndex("OrgqId");

                    b.ToTable("RelQuestion");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("LastDatetime");

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.OrgQuestion", b =>
                {
                    b.HasOne("NJBC.DataLayer.Models.User", "User")
                        .WithMany("OrgQuestions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.RelComment", b =>
                {
                    b.HasOne("NJBC.DataLayer.Models.RelQuestion", "Relq")
                        .WithMany("RelComment")
                        .HasForeignKey("RelqId")
                        .HasConstraintName("FK__RelCommen__RELQ___3C69FB99");
                });

            modelBuilder.Entity("NJBC.DataLayer.Models.RelQuestion", b =>
                {
                    b.HasOne("NJBC.DataLayer.Models.OrgQuestion", "Orgq")
                        .WithMany("RelQuestion")
                        .HasForeignKey("OrgqId")
                        .HasConstraintName("FK__RelQuesti__ORGQ___398D8EEE");
                });
#pragma warning restore 612, 618
        }
    }
}
