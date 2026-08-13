namespace TaskManagementAPI.DTOs;

public class TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public int AssignedToUserId { get; set; }
}