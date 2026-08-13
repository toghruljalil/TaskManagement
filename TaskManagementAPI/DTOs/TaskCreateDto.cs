namespace TaskManagementAPI.DTOs;

public class TaskCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public int AssignedToUserId { get; set; } 
}