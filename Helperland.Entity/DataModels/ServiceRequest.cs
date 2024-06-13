using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("ServiceRequest")]
public partial class ServiceRequest
{
    [Key]
    public int ServiceRequestId { get; set; }

    public int UserId { get; set; }

    public int ServiceId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime ServiceStartDate { get; set; }

    [StringLength(10)]
    public string ZipCode { get; set; } = null!;

    public short? ServiceFrequency { get; set; }

    [Precision(8, 2)]
    public decimal? ServiceHourlyRate { get; set; }

    public decimal ServiceHours { get; set; }

    public decimal? ExtraHours { get; set; }

    [Precision(8, 2)]
    public decimal SubTotal { get; set; }

    [Precision(8, 2)]
    public decimal? Discount { get; set; }

    [Precision(8, 2)]
    public decimal TotalCost { get; set; }

    [StringLength(500)]
    public string? Comments { get; set; }

    [StringLength(50)]
    public string? PaymentTransactionRefNo { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray PaymentDue { get; set; } = null!;

    public short? JobStatus { get; set; }

    public int? ServiceProviderId { get; set; }

    [Column("SPAcceptedDate", TypeName = "timestamp without time zone")]
    public DateTime? SpacceptedDate { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray HasPets { get; set; } = null!;

    public int? Status { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    [Precision(8, 2)]
    public decimal? RefundedAmount { get; set; }

    [Precision(18, 2)]
    public decimal Distance { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? HasIssue { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? PaymentDone { get; set; }

    [InverseProperty("ServiceRequest")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    [ForeignKey("ServiceProviderId")]
    [InverseProperty("ServiceRequestServiceProviders")]
    public virtual User? ServiceProvider { get; set; }

    [InverseProperty("ServiceRequest")]
    public virtual ICollection<ServiceRequestAddress> ServiceRequestAddresses { get; set; } = new List<ServiceRequestAddress>();

    [InverseProperty("ServiceRequest")]
    public virtual ICollection<ServiceRequestExtra> ServiceRequestExtras { get; set; } = new List<ServiceRequestExtra>();

    [ForeignKey("UserId")]
    [InverseProperty("ServiceRequestUsers")]
    public virtual User User { get; set; } = null!;
}
