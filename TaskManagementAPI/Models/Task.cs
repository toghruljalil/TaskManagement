using System;
using System.Collections.Generic;

namespace TaskManagementAPI.Models;

public partial class Task
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public int ProjectId { get; set; }

    public int? AssignedToUserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? AssignedToUser { get; set; }

    public virtual Project Project { get; set; } = null!;
}
