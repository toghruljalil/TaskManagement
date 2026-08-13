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
    public IActionResult GetAllProjects()
    {
        var projects = _context.Projects.ToList();
        
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
}