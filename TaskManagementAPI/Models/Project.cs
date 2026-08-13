using System;
using System.Collections.Generic;

namespace TaskManagementAPI.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int CreatedById { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
