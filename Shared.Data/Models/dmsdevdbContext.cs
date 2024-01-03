using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Shared.Data.Models
{
    public partial class dmsdevdbContext : DbContext
    {
        public dmsdevdbContext()
        {
        }

        public dmsdevdbContext(DbContextOptions<dmsdevdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Unsubscribe> Unsubscribes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserNotification> UserNotifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notifications", "Notifications");

                entity.Property(e => e.EmailTemplateId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventTriggerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PushMessageTemplate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SmsMessageTemplate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Unsubscribe>(entity =>
            {
                entity.ToTable("Unsubscribes", "Notifications");

                entity.Property(e => e.DateUnsubscribed).HasColumnType("datetime");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.Unsubscribes)
                    .HasForeignKey(d => d.NotificationId)
                    .HasConstraintName("FK_Unsubscribes_Notifications");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Unsubscribes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Unsubscribes_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "Notifications");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Roles)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserNotification>(entity =>
            {
                entity.ToTable("UserNotifications", "Notifications");

                entity.Property(e => e.NotificationMessage)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.UserNotifications)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserNotifications_Notifications");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserNotifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserNotifications_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
