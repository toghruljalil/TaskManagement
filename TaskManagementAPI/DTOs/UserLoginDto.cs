using System.ComponentModel.DataAnnotations;
namespace TaskManagementAPI.DTOs;

public class UserLoginDto
{
    [Required(ErrorMessage = "Email mütləq daxil edilməlidir.")]
    [EmailAddress(ErrorMessage = "Düzgün email formatı yazın.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifrə mütləqdir.")]
    [MinLength(6, ErrorMessage = "Şifrə ən azı 6 simvol olmalıdır.")]
    public string Password { get; set; } = string.Empty;
}