using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Models;

public partial class PeriodicHealth
{
    public int HealthId { get; set; }

    public int JourneyId { get; set; }

    public byte WeeksPregnant { get; set; }

    public byte? BloodPressure { get; set; }

    public decimal? WaistCircumference { get; set; }

    public decimal? Weight { get; set; }

    public string? Mood { get; set; }

    public bool? GenderBaby { get; set; }

    public string? Notes { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public virtual HealthJourney Journey { get; set; } = null!;
}
