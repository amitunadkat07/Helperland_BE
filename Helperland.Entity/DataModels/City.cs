using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("City")]
public partial class City
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string CityName { get; set; } = null!;

    public int StateId { get; set; }

    [ForeignKey("StateId")]
    [InverseProperty("Cities")]
    public virtual State State { get; set; } = null!;

    [InverseProperty("City")]
    public virtual ICollection<Zipcode> Zipcodes { get; set; } = new List<Zipcode>();
}
