using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class HealthJourney
{
    public int JourneyId { get; set; }

    public int UserId { get; set; }

    public int TreatmentId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<ConnectionMedical> ConnectionMedicals { get; set; } = new List<ConnectionMedical>();

    public virtual ICollection<MonitoringForm> MonitoringForms { get; set; } = new List<MonitoringForm>();

    public virtual ICollection<PeriodicHealth> PeriodicHealths { get; set; } = new List<PeriodicHealth>();

    public virtual ICollection<PsychologyDiary> PsychologyDiaries { get; set; } = new List<PsychologyDiary>();

    public virtual Treatment Treatment { get; set; } = null!;

    public virtual UserProfile User { get; set; } = null!;
}
