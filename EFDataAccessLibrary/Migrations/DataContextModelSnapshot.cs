﻿// <auto-generated />
using System;
using EFDataAccessLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFDataAccessLibrary.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EFDataAccessLibrary.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Dollars")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<double>("Gold")
                        .HasColumnType("float");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("OwningTeamId")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("OwningTeamId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TicketID")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Event", b =>
                {
                    b.Property<int>("EventID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PrizePool")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("EventID");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.EventTeam", b =>
                {
                    b.Property<int>("EventID")
                        .HasColumnType("int");

                    b.Property<int>("TeamID")
                        .HasColumnType("int");

                    b.HasKey("EventID", "TeamID");

                    b.HasIndex("TeamID");

                    b.ToTable("EventTeam");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Faq", b =>
                {
                    b.Property<int>("FaqID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifierID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifyDate")
                        .HasColumnType("datetime2");

                    b.HasKey("FaqID");

                    b.HasIndex("FaqID", "IsActive")
                        .IsUnique()
                        .HasFilter("[IsActive] != 0");

                    b.ToTable("Faq");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.FaqQuestion", b =>
                {
                    b.Property<int>("FaqID")
                        .HasColumnType("int");

                    b.Property<int>("QuestionID")
                        .HasColumnType("int");

                    b.HasKey("FaqID", "QuestionID");

                    b.HasIndex("QuestionID");

                    b.ToTable("FaqQuestion");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Map", b =>
                {
                    b.Property<int>("MapID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tag")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("MapID");

                    b.ToTable("Map");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.MarketPlayer", b =>
                {
                    b.Property<int>("PlayerID")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsBought")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.HasKey("PlayerID", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("MarketPlayers");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Match", b =>
                {
                    b.Property<int>("MatchID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EventID")
                        .HasColumnType("int");

                    b.Property<int?>("MapID")
                        .HasColumnType("int");

                    b.Property<int?>("ScoreTeam1")
                        .HasColumnType("int");

                    b.Property<int?>("ScoreTeam2")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Team1TeamID")
                        .HasColumnType("int");

                    b.Property<int?>("Team2TeamID")
                        .HasColumnType("int");

                    b.HasKey("MatchID");

                    b.HasIndex("EventID");

                    b.HasIndex("MapID");

                    b.HasIndex("Team1TeamID");

                    b.HasIndex("Team2TeamID");

                    b.ToTable("Match");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.NewsContent", b =>
                {
                    b.Property<int>("NewsContentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NewsContentID");

                    b.ToTable("NewsContent");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.NewsHeader", b =>
                {
                    b.Property<int>("NewsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<int>("NewsContentID")
                        .HasColumnType("int");

                    b.Property<DateTime>("NewsCreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NewsDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NewsPublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NewsTitle")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NewsId");

                    b.HasIndex("NewsContentID");

                    b.ToTable("NewsHeader");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Player", b =>
                {
                    b.Property<int>("PlayerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Aim")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool>("IsForSale")
                        .HasColumnType("bit");

                    b.Property<int>("Knowledge")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("NickName")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("PlayerLevel")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerPositionPositionID")
                        .HasColumnType("int");

                    b.Property<int>("PositionID")
                        .HasColumnType("int");

                    b.Property<int>("Potencial")
                        .HasColumnType("int");

                    b.Property<double>("Prize")
                        .HasColumnType("float");

                    b.Property<int>("Quality")
                        .HasColumnType("int");

                    b.HasKey("PlayerID");

                    b.HasIndex("PlayerPositionPositionID");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.PlayerPosition", b =>
                {
                    b.Property<int>("PositionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("PositionID");

                    b.ToTable("PlayerPosition");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.PlayerTeam", b =>
                {
                    b.Property<int>("PlayerID")
                        .HasColumnType("int");

                    b.Property<int>("TeamID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExitDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isOnTransfer")
                        .HasColumnType("bit");

                    b.HasKey("PlayerID", "TeamID");

                    b.HasIndex("TeamID");

                    b.ToTable("PlayerTeam");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Question", b =>
                {
                    b.Property<int>("QuestionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuestionID");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Team", b =>
                {
                    b.Property<int>("TeamID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("OwnerID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tag")
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("TeamName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeamID");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModify")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserID");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.TicketComment", b =>
                {
                    b.Property<int>("TicketID")
                        .HasColumnType("int");

                    b.Property<int>("CommentID")
                        .HasColumnType("int");

                    b.HasKey("TicketID", "CommentID");

                    b.HasIndex("CommentID");

                    b.ToTable("TicketComments");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.TicketUser", b =>
                {
                    b.Property<int>("TicketID")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TicketID", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("TicketUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.ApplicationUser", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Team", "OwningTeam")
                        .WithMany()
                        .HasForeignKey("OwningTeamId");

                    b.Navigation("OwningTeam");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Comment", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.EventTeam", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Event", "Event")
                        .WithMany("EventTeams")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFDataAccessLibrary.Models.Team", "Team")
                        .WithMany("EventTeams")
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.FaqQuestion", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Faq", "Faq")
                        .WithMany("FaqQuestions")
                        .HasForeignKey("FaqID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFDataAccessLibrary.Models.Question", "Question")
                        .WithMany("FaqQuestions")
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faq");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.MarketPlayer", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFDataAccessLibrary.Models.ApplicationUser", "User")
                        .WithMany("MarketPlayer")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Match", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Event", "Event")
                        .WithMany("Matches")
                        .HasForeignKey("EventID");

                    b.HasOne("EFDataAccessLibrary.Models.Map", "Map")
                        .WithMany()
                        .HasForeignKey("MapID");

                    b.HasOne("EFDataAccessLibrary.Models.Team", "Team1")
                        .WithMany()
                        .HasForeignKey("Team1TeamID");

                    b.HasOne("EFDataAccessLibrary.Models.Team", "Team2")
                        .WithMany()
                        .HasForeignKey("Team2TeamID");

                    b.Navigation("Event");

                    b.Navigation("Map");

                    b.Navigation("Team1");

                    b.Navigation("Team2");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.NewsHeader", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.NewsContent", "NewsContent")
                        .WithMany()
                        .HasForeignKey("NewsContentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NewsContent");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Player", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.PlayerPosition", "PlayerPosition")
                        .WithMany()
                        .HasForeignKey("PlayerPositionPositionID");

                    b.Navigation("PlayerPosition");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.PlayerTeam", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Player", "Player")
                        .WithMany("PlayerTeam")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFDataAccessLibrary.Models.Team", "Team")
                        .WithMany("PlayerTeam")
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Ticket", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.TicketComment", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Comment", "Comment")
                        .WithMany()
                        .HasForeignKey("CommentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFDataAccessLibrary.Models.Ticket", "Ticket")
                        .WithMany("TicketComments")
                        .HasForeignKey("TicketID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.TicketUser", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Ticket", "Ticket")
                        .WithMany()
                        .HasForeignKey("TicketID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFDataAccessLibrary.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFDataAccessLibrary.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.ApplicationUser", b =>
                {
                    b.Navigation("MarketPlayer");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Event", b =>
                {
                    b.Navigation("EventTeams");

                    b.Navigation("Matches");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Faq", b =>
                {
                    b.Navigation("FaqQuestions");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Player", b =>
                {
                    b.Navigation("PlayerTeam");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Question", b =>
                {
                    b.Navigation("FaqQuestions");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Team", b =>
                {
                    b.Navigation("EventTeams");

                    b.Navigation("PlayerTeam");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Ticket", b =>
                {
                    b.Navigation("TicketComments");
                });
#pragma warning restore 612, 618
        }
    }
}
