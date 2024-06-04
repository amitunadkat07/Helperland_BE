using System;
using System.Collections.Generic;
using Helperland.Entity.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataContext;

public partial class HelperlandContext : DbContext
{
    public HelperlandContext()
    {
    }

    public HelperlandContext(DbContextOptions<HelperlandContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<ContactUsAttachment> ContactUsAttachments { get; set; }

    public virtual DbSet<FavoriteAndBlocked> FavoriteAndBlockeds { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }

    public virtual DbSet<ServiceRequestAddress> ServiceRequestAddresses { get; set; }

    public virtual DbSet<ServiceRequestExtra> ServiceRequestExtras { get; set; }

    public virtual DbSet<ServiceSetting> ServiceSettings { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<Zipcode> Zipcodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=192.168.0.5;Database=krishnthakrar_db;User=krishn;Password=nf9Y5XNT;Encrypt=False;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_State");
        });

        modelBuilder.Entity<FavoriteAndBlocked>(entity =>
        {
            entity.HasOne(d => d.TargetUser).WithMany(p => p.FavoriteAndBlockedTargetUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteAndBlocked_User");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteAndBlockedUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteAndBlocked_FavoriteAndBlocked");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.Property(e => e.IsApproved).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.RatingFromNavigation).WithMany(p => p.RatingRatingFromNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_User");

            entity.HasOne(d => d.RatingToNavigation).WithMany(p => p.RatingRatingToNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_User1");

            entity.HasOne(d => d.ServiceRequest).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_ServiceRequest");
        });

        modelBuilder.Entity<ServiceRequest>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ServiceProvider).WithMany(p => p.ServiceRequestServiceProviders).HasConstraintName("FK_ServiceRequest_User1");

            entity.HasOne(d => d.User).WithMany(p => p.ServiceRequestUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceRequest_User");
        });

        modelBuilder.Entity<ServiceRequestAddress>(entity =>
        {
            entity.Property(e => e.Type).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.ServiceRequest).WithMany(p => p.ServiceRequestAddresses).HasConstraintName("FK_ServiceRequestAddress_ServiceRequest");
        });

        modelBuilder.Entity<ServiceRequestExtra>(entity =>
        {
            entity.HasOne(d => d.ServiceRequest).WithMany(p => p.ServiceRequestExtras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceRequestExtra_ServiceRequest");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK_UserAddresses");

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAddresses_User");
        });

        modelBuilder.Entity<Zipcode>(entity =>
        {
            entity.HasOne(d => d.City).WithMany(p => p.Zipcodes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zipcode_City");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
