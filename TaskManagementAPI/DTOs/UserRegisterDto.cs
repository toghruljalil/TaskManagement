using System.ComponentModel.DataAnnotations;
namespace TaskManagementAPI.DTOs;

public class UserRegisterDto
{
    [Required(ErrorMessage = "İstifadəçi adı mütləqdir.")]
    [MinLength(3, ErrorMessage = "İstifadəçi adı ən azı 3 hərf olmalıdır.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email mütləqdir.")]
    [EmailAddress(ErrorMessage = "Düzgün email formatı daxil edin.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifrə mütləqdir.")]
    [MinLength(6, ErrorMessage = "Şifrə ən azı 6 simvol olmalıdır.")]
    public string Password { get; set; } = string.Empty;
}