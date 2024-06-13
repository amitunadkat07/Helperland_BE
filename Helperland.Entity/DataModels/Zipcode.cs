using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("ZipCode")]
public partial class ZipCode
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ZipcodeValue { get; set; } = null!;

    public int CityId { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("ZipCodes")]
    public virtual City City { get; set; } = null!;
}
