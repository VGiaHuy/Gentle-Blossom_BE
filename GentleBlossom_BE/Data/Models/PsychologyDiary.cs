using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class PsychologyDiary
{
    public int DiaryId { get; set; }

    public int JourneyId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public string Mood { get; set; } = null!;

    public string Content { get; set; } = null!;

    public virtual HealthJourney Journey { get; set; } = null!;
}
