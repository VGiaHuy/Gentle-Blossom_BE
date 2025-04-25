using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Models;

public partial class ConnectionMedical
{
    public int ConnectId { get; set; }

    public int ExpertId { get; set; }

    public int UserId { get; set; }

    public int JourneyId { get; set; }

    public byte Status { get; set; }

    public virtual Expert Expert { get; set; } = null!;

    public virtual HealthJourney Journey { get; set; } = null!;

    public virtual UserProfile User { get; set; } = null!;
}
