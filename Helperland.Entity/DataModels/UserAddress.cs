using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("UserAddress")]
public partial class UserAddress
{
    [Key]
    public int AddressId { get; set; }

    public int UserId { get; set; }

    [StringLength(200)]
    public string AddressLine1 { get; set; } = null!;

    [StringLength(200)]
    public string? AddressLine2 { get; set; }

    [StringLength(50)]
    public string City { get; set; } = null!;

    [StringLength(50)]
    public string? State { get; set; }

    [StringLength(20)]
    public string PostalCode { get; set; } = null!;

    [Column(TypeName = "bit(1)")]
    public BitArray IsDefault { get; set; } = null!;

    [Column(TypeName = "bit(1)")]
    public BitArray IsDeleted { get; set; } = null!;

    [StringLength(20)]
    public string? Mobile { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    public int? Type { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserAddresses")]
    public virtual User User { get; set; } = null!;
}
