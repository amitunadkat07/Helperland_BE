using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("FavoriteAndBlocked")]
public partial class FavoriteAndBlocked
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TargetUserId { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray IsFavorite { get; set; } = null!;

    [Column(TypeName = "bit(1)")]
    public BitArray IsBlocked { get; set; } = null!;

    [ForeignKey("TargetUserId")]
    [InverseProperty("FavoriteAndBlockedTargetUsers")]
    public virtual User TargetUser { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("FavoriteAndBlockedUsers")]
    public virtual User User { get; set; } = null!;
}
