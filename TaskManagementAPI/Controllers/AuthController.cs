using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AuthController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserRegisterDto request)
    {
        var newUser = _mapper.Map<User>(request);
        
        newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok("İstifadəçi uğurla qeydiyyatdan keçdi.");
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDto request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        
        if (user == null)
        {
            return BadRequest("İstifadəçi tapılmadı və ya email yanlışdır.");
        }
        
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return BadRequest("Şifrə yanlışdır.");
        }
        
        return Ok(new { message = $"Xoş gəldin, {user.Username}! Giriş uğurludur." });
    }
    
    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        var users = _context.Users.ToList();

        var usersResponse = _mapper.Map<List<UserResponseDto>>(users);
        
        return Ok(usersResponse);
    }
}