using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class MessageAttachment
{
    public int AttachmentId { get; set; }

    public int MessageId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public string? FileType { get; set; }

    public int? FileSize { get; set; }

    public virtual Message Message { get; set; } = null!;
}
