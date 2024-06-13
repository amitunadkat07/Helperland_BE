using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("User")]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(100)]
    public string? Password { get; set; }

    [StringLength(20)]
    public string Mobile { get; set; } = null!;

    public int UserTypeId { get; set; }

    public int? RoleId { get; set; }

    public int? Gender { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(1000)]
    public string? Website { get; set; }

    [StringLength(200)]
    public string? UserProfilePicture { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray IsRegisteredUser { get; set; } = null!;

    [StringLength(200)]
    public string? PaymentGatewayUserRef { get; set; }

    [StringLength(20)]
    public string? ZipCode { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray WorksWithPets { get; set; } = null!;

    public int? LanguageId { get; set; }

    public int? NationalityId { get; set; }

    [StringLength(200)]
    public string? ResetKey { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    public int ModifiedBy { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray IsApproved { get; set; } = null!;

    [Column(TypeName = "bit(1)")]
    public BitArray IsActive { get; set; } = null!;

    [Column(TypeName = "bit(1)")]
    public BitArray IsDeleted { get; set; } = null!;

    public int? Status { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray IsOnline { get; set; } = null!;

    [StringLength(100)]
    public string? BankTokenId { get; set; }

    [StringLength(50)]
    public string? TaxNo { get; set; }

    [InverseProperty("TargetUser")]
    public virtual ICollection<FavoriteAndBlocked> FavoriteAndBlockedTargetUsers { get; set; } = new List<FavoriteAndBlocked>();

    [InverseProperty("User")]
    public virtual ICollection<FavoriteAndBlocked> FavoriteAndBlockedUsers { get; set; } = new List<FavoriteAndBlocked>();

    [InverseProperty("RatingFromNavigation")]
    public virtual ICollection<Rating> RatingRatingFromNavigations { get; set; } = new List<Rating>();

    [InverseProperty("RatingToNavigation")]
    public virtual ICollection<Rating> RatingRatingToNavigations { get; set; } = new List<Rating>();

    [InverseProperty("ServiceProvider")]
    public virtual ICollection<ServiceRequest> ServiceRequestServiceProviders { get; set; } = new List<ServiceRequest>();

    [InverseProperty("User")]
    public virtual ICollection<ServiceRequest> ServiceRequestUsers { get; set; } = new List<ServiceRequest>();

    [InverseProperty("User")]
    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
}
