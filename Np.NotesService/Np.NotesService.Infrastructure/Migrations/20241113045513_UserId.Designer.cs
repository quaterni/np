﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Np.NotesService.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Np.NotesService.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241113045513_UserId")]
    partial class UserId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Np.NotesService.Application.Abstractions.Outbox.OutboxEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("data");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("event_name");

                    b.Property<DateTime>("RefreshTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refresh_time");

                    b.HasKey("Id")
                        .HasName("pk_outbox_entries");

                    b.ToTable("outbox_entries", (string)null);
                });

            modelBuilder.Entity("Np.NotesService.Domain.Notes.Note", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_time");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update_time");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)")
                        .HasColumnName("title");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id")
                        .HasName("pk_notes");

                    b.ToTable("notes", (string)null);
                });

            modelBuilder.Entity("Np.NotesService.Domain.Notes.Note", b =>
                {
                    b.OwnsOne("Np.NotesService.Domain.Notes.User", "User", b1 =>
                        {
                            b1.Property<Guid>("NoteId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid")
                                .HasColumnName("user_id");

                            b1.HasKey("NoteId");

                            b1.ToTable("notes");

                            b1.WithOwner()
                                .HasForeignKey("NoteId")
                                .HasConstraintName("fk_notes_notes_id");
                        });

                    b.Navigation("User")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
