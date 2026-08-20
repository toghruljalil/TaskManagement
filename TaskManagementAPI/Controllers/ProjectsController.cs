using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProjectsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto request)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized("İstifadəçi sessiyası etibarsızdır.");
        }

        var newProject = _mapper.Map<Project>(request);
        newProject.CreatedById = userId;

        await _context.Projects.AddAsync(newProject);
        await _context.SaveChangesAsync();
        
        var response = _mapper.Map<ProjectResponseDto>(newProject);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProjects(
        [FromQuery] string? search,
        [FromQuery] string? sortBy,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Projects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "name" => query.OrderBy(p => p.Name),
                "name_desc" => query.OrderByDescending(p => p.Name),
                _ => query.OrderBy(p => p.Id)
            };
        }
        else
        {
            query = query.OrderBy(p => p.Id);
        }

        var skip = (page - 1) * pageSize;
        var projects = await query.Skip(skip).Take(pageSize).ToListAsync();

        var response = _mapper.Map<List<ProjectResponseDto>>(projects);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        
        if (project == null)
        {
            return NotFound("Layihə tapılmadı.");
        }
        
        var response = _mapper.Map<ProjectResponseDto>(project);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectCreateDto request)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound("Layihə tapılmadı.");
        }

        _mapper.Map(request, project);
        await _context.SaveChangesAsync();

        return Ok("Layihə uğurla yeniləndi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound("Layihə tapılmadı.");
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return Ok("Layihə uğurla silindi.");
    }
}