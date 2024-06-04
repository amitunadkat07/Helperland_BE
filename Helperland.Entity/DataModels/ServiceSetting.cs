using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("ServiceSetting")]
public partial class ServiceSetting
{
    [Key]
    public int Id { get; set; }

    public int ActionType { get; set; }

    public int Interval { get; set; }

    public TimeSpan ScheduleTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LastPoll { get; set; }
}
