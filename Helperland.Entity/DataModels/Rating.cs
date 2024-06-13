using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("Rating")]
public partial class Rating
{
    [Key]
    public int RatingId { get; set; }

    public int ServiceRequestId { get; set; }

    public int RatingFrom { get; set; }

    public int RatingTo { get; set; }

    [Precision(2, 1)]
    public decimal Ratings { get; set; }

    [StringLength(2000)]
    public string? Comments { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime RatingDate { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsApproved { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray VisibleOnHomeScreen { get; set; } = null!;

    [Precision(2, 1)]
    public decimal OnTimeArrival { get; set; }

    [Precision(2, 1)]
    public decimal Friendly { get; set; }

    [Precision(2, 1)]
    public decimal QualityOfService { get; set; }

    [ForeignKey("RatingFrom")]
    [InverseProperty("RatingRatingFromNavigations")]
    public virtual User RatingFromNavigation { get; set; } = null!;

    [ForeignKey("RatingTo")]
    [InverseProperty("RatingRatingToNavigations")]
    public virtual User RatingToNavigation { get; set; } = null!;

    [ForeignKey("ServiceRequestId")]
    [InverseProperty("Ratings")]
    public virtual ServiceRequest ServiceRequest { get; set; } = null!;
}
