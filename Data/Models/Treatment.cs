using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class Treatment
{
    public int TreatmentId { get; set; }

    public string TreatmentName { get; set; } = null!;

    public virtual ICollection<HealthJourney> HealthJourneys { get; set; } = new List<HealthJourney>();
}
