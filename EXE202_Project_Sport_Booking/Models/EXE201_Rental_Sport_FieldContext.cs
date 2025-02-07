﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EXE202_Project_Sport_Booking.Models
{
    public partial class EXE201_Rental_Sport_FieldContext : DbContext
    {
        public EXE201_Rental_Sport_FieldContext()
        {
        }

        public EXE201_Rental_Sport_FieldContext(DbContextOptions<EXE201_Rental_Sport_FieldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Court> Courts { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<SubCourt> SubCourts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-2N5GN50\n;Initial Catalog=EXE201_Rental_Sport_Field;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(200);

                entity.Property(e => e.SubCourtId).HasColumnName("SubCourtID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.SubCourt)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.SubCourtId)
                    .HasConstraintName("FK__Bookings__SubCou__45F365D3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Bookings__UserID__44FF419A");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(200);

                entity.Property(e => e.Description).HasMaxLength(200);
            });

            modelBuilder.Entity<Court>(entity =>
            {
                entity.Property(e => e.CourtId).HasColumnName("CourtID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CourtDescription).HasMaxLength(255);

                entity.Property(e => e.CourtName).HasMaxLength(200);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Courts)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Courts__Category__3E52440B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Courts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Courts__UserID__3F466844");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentMethod).HasMaxLength(220);

                entity.Property(e => e.PaymentStatus).HasMaxLength(220);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__Payments__Bookin__48CFD27E");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");

                entity.Property(e => e.Comment).HasColumnType("text");

                entity.Property(e => e.ReviewDate).HasColumnType("datetime");

                entity.Property(e => e.SubCourtId).HasColumnName("SubCourtID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.SubCourt)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.SubCourtId)
                    .HasConstraintName("FK__Reviews__SubCour__4CA06362");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Reviews__ReviewD__4BAC3F29");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(155);
            });

            modelBuilder.Entity<SubCourt>(entity =>
            {
                entity.ToTable("Sub_Courts");

                entity.Property(e => e.SubCourtId).HasColumnName("SubCourtID");

                entity.Property(e => e.CourtId).HasColumnName("CourtID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.SubCourtName).HasMaxLength(200);

                entity.HasOne(d => d.Court)
                    .WithMany(p => p.SubCourts)
                    .HasForeignKey(d => d.CourtId)
                    .HasConstraintName("FK__Sub_Court__Court__4222D4EF");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.AvatarUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.PasswordResetSentAt).HasColumnType("datetime");

                entity.Property(e => e.PasswordResetToken).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(150);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__RoleID__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
