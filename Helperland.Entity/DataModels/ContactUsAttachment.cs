using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Helperland.Entity.DataModels;

[Table("ContactUsAttachment")]
public partial class ContactUsAttachment
{
    [Key]
    public int ContactUsAttachmentId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string FileName { get; set; } = null!;
}
