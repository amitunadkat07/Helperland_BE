using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("State")]
public partial class State
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string StateName { get; set; } = null!;

    [InverseProperty("State")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
