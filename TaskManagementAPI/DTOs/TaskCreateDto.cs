using System.ComponentModel.DataAnnotations;
namespace TaskManagementAPI.DTOs;

public class TaskCreateDto
{
    [Required(ErrorMessage = "Başlıq mütləq daxil edilməlidir.")]
    [MinLength(3, ErrorMessage = "Başlıq ən azı 3 hərf olmalıdır.")]
    [MaxLength(100, ErrorMessage = "Başlıq ən çox 100 hərf ola bilər.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Təsvir ən çox 500 hərf ola bilər.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Status mütləq seçilməlidir.")]
    public TaskStatusEnum Status { get; set; }

    [Required(ErrorMessage = "Layihə ID-si mütləqdir.")]
    public int ProjectId { get; set; }

    [Required(ErrorMessage = "Təhkim olunan istifadəçi ID-si mütləqdir.")]
    public int AssignedToUserId { get; set; } 
}