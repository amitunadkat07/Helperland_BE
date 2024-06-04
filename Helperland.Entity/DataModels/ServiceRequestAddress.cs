using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("ServiceRequestAddress")]
public partial class ServiceRequestAddress
{
    [Key]
    public int Id { get; set; }

    public int? ServiceRequestId { get; set; }

    [StringLength(200)]
    public string? AddressLine1 { get; set; }

    [StringLength(200)]
    public string? AddressLine2 { get; set; }

    [StringLength(50)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? State { get; set; }

    [StringLength(20)]
    public string? PostalCode { get; set; }

    [StringLength(20)]
    public string? Mobile { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    public int? Type { get; set; }

    [ForeignKey("ServiceRequestId")]
    [InverseProperty("ServiceRequestAddresses")]
    public virtual ServiceRequest? ServiceRequest { get; set; }
}
