using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

public partial class ContactU
{
    [Key]
    public int ContactUsiD { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string Email { get; set; } = null!;

    [StringLength(100)]
    public string SubjectType { get; set; } = null!;

    [StringLength(500)]
    public string? Subject { get; set; }

    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string Message { get; set; } = null!;

    [StringLength(100)]
    public string? UploadFileName { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? Status { get; set; }

    public int? Priority { get; set; }

    public int? AssignedToUser { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsDeleted { get; set; }
}
