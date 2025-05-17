using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class PostMedium
{
    public int MediaId { get; set; }

    public int PostId { get; set; }

    public string MediaUrl { get; set; } = null!;

    public string MediaType { get; set; } = null!;

    public string? FileName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Post Post { get; set; } = null!;
}
