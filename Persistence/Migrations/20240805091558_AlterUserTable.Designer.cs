﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240805091558_AlterUserTable")]
    partial class AlterUserTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Leagues.League", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("MatchFormat")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("Domain.Leagues.LeagueInvitation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("InvitationToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LeagueId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("LeagueInvitations");
                });

            modelBuilder.Entity("Domain.Leagues.LeaguePlayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("LeagueId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LeaguePlayerLevel")
                        .HasColumnType("int");

                    b.Property<Guid?>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.HasIndex("PlayerId");

                    b.ToTable("LeaguePlayers");
                });

            modelBuilder.Entity("Domain.Matches.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ScoreAway")
                        .HasColumnType("int");

                    b.Property<int?>("ScoreHome")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Domain.Matches.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AwayId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("HomeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("bit");

                    b.Property<int?>("RoundNumber")
                        .HasColumnType("int");

                    b.Property<int?>("ScoreAway")
                        .HasColumnType("int");

                    b.Property<int?>("ScoreHome")
                        .HasColumnType("int");

                    b.Property<Guid?>("SeasonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AwayId");

                    b.HasIndex("HomeId");

                    b.HasIndex("SeasonId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Domain.Notifications.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("RecipientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("nvarchar(34)");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Notifications");

                    b.HasDiscriminator<string>("Type").HasValue("Notification");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Seasons.Season", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BestOf")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("GameThreshold")
                        .HasColumnType("int");

                    b.Property<Guid?>("LeagueId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NumberOfRounds")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Users.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("RoleType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");

                    b.HasDiscriminator<string>("RoleType").HasValue("UserRole");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Notifications.LeagueInvitationNotification", b =>
                {
                    b.HasBaseType("Domain.Notifications.Notification");

                    b.Property<Guid?>("LeagueInvitationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("LeagueInvitationId");

                    b.HasDiscriminator().HasValue("LeagueInvitationNotification");
                });

            modelBuilder.Entity("Domain.Users.Player", b =>
                {
                    b.HasBaseType("Domain.Users.UserRole");

                    b.HasDiscriminator().HasValue("Player");
                });

            modelBuilder.Entity("Domain.Leagues.LeagueInvitation", b =>
                {
                    b.HasOne("Domain.Leagues.League", "League")
                        .WithMany("LeagueInvitations")
                        .HasForeignKey("LeagueId");

                    b.Navigation("League");
                });

            modelBuilder.Entity("Domain.Leagues.LeaguePlayer", b =>
                {
                    b.HasOne("Domain.Leagues.League", "League")
                        .WithMany("LeaguePlayers")
                        .HasForeignKey("LeagueId");

                    b.HasOne("Domain.Users.Player", "Player")
                        .WithMany("LeaguePlayers")
                        .HasForeignKey("PlayerId");

                    b.Navigation("League");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Domain.Matches.Game", b =>
                {
                    b.HasOne("Domain.Matches.Match", "Match")
                        .WithMany("Sets")
                        .HasForeignKey("MatchId");

                    b.Navigation("Match");
                });

            modelBuilder.Entity("Domain.Matches.Match", b =>
                {
                    b.HasOne("Domain.Leagues.LeaguePlayer", "Away")
                        .WithMany()
                        .HasForeignKey("AwayId");

                    b.HasOne("Domain.Leagues.LeaguePlayer", "Home")
                        .WithMany()
                        .HasForeignKey("HomeId");

                    b.HasOne("Domain.Seasons.Season", "Season")
                        .WithMany("Matches")
                        .HasForeignKey("SeasonId");

                    b.Navigation("Away");

                    b.Navigation("Home");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("Domain.Notifications.Notification", b =>
                {
                    b.HasOne("Domain.Users.Player", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId");

                    b.HasOne("Domain.Users.Player", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Domain.Seasons.Season", b =>
                {
                    b.HasOne("Domain.Leagues.League", "League")
                        .WithMany("Seasons")
                        .HasForeignKey("LeagueId");

                    b.Navigation("League");
                });

            modelBuilder.Entity("Domain.Users.UserRole", b =>
                {
                    b.HasOne("Domain.Users.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Notifications.LeagueInvitationNotification", b =>
                {
                    b.HasOne("Domain.Leagues.LeagueInvitation", "LeagueInvitation")
                        .WithMany()
                        .HasForeignKey("LeagueInvitationId");

                    b.Navigation("LeagueInvitation");
                });

            modelBuilder.Entity("Domain.Leagues.League", b =>
                {
                    b.Navigation("LeagueInvitations");

                    b.Navigation("LeaguePlayers");

                    b.Navigation("Seasons");
                });

            modelBuilder.Entity("Domain.Matches.Match", b =>
                {
                    b.Navigation("Sets");
                });

            modelBuilder.Entity("Domain.Seasons.Season", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Domain.Users.Player", b =>
                {
                    b.Navigation("LeaguePlayers");
                });
#pragma warning restore 612, 618
        }
    }
}
