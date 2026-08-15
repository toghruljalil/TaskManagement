using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    public IActionResult CreateProject([FromBody] ProjectCreateDto request)
    {
        var newProject = _mapper.Map<Project>(request);
        
        _context.Projects.Add(newProject);
        _context.SaveChanges();
        
        var response = _mapper.Map<ProjectResponseDto>(newProject);
        return Ok(response);
    }
    
    [HttpGet]
    public IActionResult GetAllProjects(
        [FromQuery] string? search,
        [FromQuery] string? sortBy,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Projects.AsQueryable();

        if (!string.IsNullOrEmpty(search))
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

        var projects = query.Skip(skip).Take(pageSize).ToList();

        var response = _mapper.Map<List<ProjectResponseDto>>(projects);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult GetProjectById(int id)
    {
        var project = _context.Projects.Find(id);
        
        if (project == null)
        {
            return NotFound("Layihə tapılmadı.");
        }
        
        var response = _mapper.Map<ProjectResponseDto>(project);
        
        return Ok(response);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProject(int id, [FromBody] ProjectCreateDto request)
    {
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound("Layihə tapılmadı.");
        }

        _mapper.Map(request, project);

        _context.SaveChanges();

        return Ok("Layihə uğurla yeniləndi.");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProject(int id)
    {
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound("Layihə tapılmadı.");
        }

        _context.Projects.Remove(project);
        _context.SaveChanges();

        return Ok("Layihə uğurla silindi.");
    }
}