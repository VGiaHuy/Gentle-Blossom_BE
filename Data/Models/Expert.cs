using System;
using System.Collections.Generic;

namespace GentleBlossom_BE.Data.Models;

public partial class Expert
{
    public int ExpertId { get; set; }

    public int UserId { get; set; }

    public string AcademicTitle { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public string Organization { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ConnectionMedical> ConnectionMedicals { get; set; } = new List<ConnectionMedical>();

    public virtual ICollection<MonitoringForm> MonitoringForms { get; set; } = new List<MonitoringForm>();

    public virtual UserProfile User { get; set; } = null!;
}
