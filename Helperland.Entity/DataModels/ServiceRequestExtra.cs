﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("ServiceRequestExtra")]
public partial class ServiceRequestExtra
{
    [Key]
    public int ServiceRequestExtraId { get; set; }

    public int ServiceRequestId { get; set; }

    public int ServiceExtraId { get; set; }

    [ForeignKey("ServiceRequestId")]
    [InverseProperty("ServiceRequestExtras")]
    public virtual ServiceRequest ServiceRequest { get; set; } = null!;
}
