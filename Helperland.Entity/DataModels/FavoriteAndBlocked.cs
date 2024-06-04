﻿using System;
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

    public bool IsFavorite { get; set; }

    public bool IsBlocked { get; set; }

    [ForeignKey("TargetUserId")]
    [InverseProperty("FavoriteAndBlockedTargetUsers")]
    public virtual User TargetUser { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("FavoriteAndBlockedUsers")]
    public virtual User User { get; set; } = null!;
}
