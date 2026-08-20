using System.ComponentModel.DataAnnotations;
namespace TaskManagementAPI.DTOs;

public class ProjectCreateDto
{
    [Required(ErrorMessage = "Layihə adı mütləq daxil edilməlidir.")]
    [MinLength(3, ErrorMessage = "Layihə adı ən azı 3 simvoldan ibarət olmalıdır.")]
    [MaxLength(100, ErrorMessage = "Layihə adı ən çox 100 simvol ola bilər.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Təsvir ən çox 500 simvol ola bilər.")]
    public string Description { get; set; } = string.Empty;
}