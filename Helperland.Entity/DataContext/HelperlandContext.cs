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

    public virtual DbSet<ZipCode> ZipCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID = postgres;Password=Krishn@1303;Server=localhost;Port=5432;Database=Helperland;Integrated Security=true;Pooling=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("City_pkey");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("City_StateId_fkey");
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.ContactUsiD).HasName("ContactUs_pkey");
        });

        modelBuilder.Entity<ContactUsAttachment>(entity =>
        {
            entity.HasKey(e => e.ContactUsAttachmentId).HasName("ContactUsAttachment_pkey");
        });

        modelBuilder.Entity<FavoriteAndBlocked>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FavoriteAndBlocked_pkey");

            entity.HasOne(d => d.TargetUser).WithMany(p => p.FavoriteAndBlockedTargetUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FavoriteAndBlocked_TargetUserId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteAndBlockedUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FavoriteAndBlocked_UserId_fkey");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("Rating_pkey");

            entity.HasOne(d => d.RatingFromNavigation).WithMany(p => p.RatingRatingFromNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rating_RatingFrom_fkey");

            entity.HasOne(d => d.RatingToNavigation).WithMany(p => p.RatingRatingToNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rating_RatingTo_fkey");

            entity.HasOne(d => d.ServiceRequest).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rating_ServiceRequestId_fkey");
        });

        modelBuilder.Entity<ServiceRequest>(entity =>
        {
            entity.HasKey(e => e.ServiceRequestId).HasName("ServiceRequest_pkey");

            entity.HasOne(d => d.ServiceProvider).WithMany(p => p.ServiceRequestServiceProviders).HasConstraintName("ServiceRequest_ServiceProviderId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ServiceRequestUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ServiceRequest_UserId_fkey");
        });

        modelBuilder.Entity<ServiceRequestAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ServiceRequestAddress_pkey");

            entity.Property(e => e.Type).HasDefaultValueSql("1");

            entity.HasOne(d => d.ServiceRequest).WithMany(p => p.ServiceRequestAddresses).HasConstraintName("ServiceRequestAddress_ServiceRequestId_fkey");
        });

        modelBuilder.Entity<ServiceRequestExtra>(entity =>
        {
            entity.HasKey(e => e.ServiceRequestExtraId).HasName("ServiceRequestExtra_pkey");

            entity.HasOne(d => d.ServiceRequest).WithMany(p => p.ServiceRequestExtras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ServiceRequestExtra_ServiceRequestId_fkey");
        });

        modelBuilder.Entity<ServiceSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ServiceSetting_pkey");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("State_pkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pkey");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("UserAddress_pkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserAddress_UserId_fkey");
        });

        modelBuilder.Entity<ZipCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ZipCode_pkey");

            entity.HasOne(d => d.City).WithMany(p => p.ZipCodes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ZipCode_CityId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
