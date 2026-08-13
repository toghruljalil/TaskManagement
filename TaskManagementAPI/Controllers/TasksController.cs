using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models; // Sənin Entity-lərin

namespace TaskManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TasksController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult CreateTask([FromBody] TaskCreateDto request)
    {
        var newTask = _mapper.Map<Models.Task>(request);
        
        _context.Tasks.Add(newTask);
        _context.SaveChanges();

        var response = _mapper.Map<TaskResponseDto>(newTask);
        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetAllTasks()
    {
        var tasks = _context.Tasks.ToList();
        var response = _mapper.Map<List<TaskResponseDto>>(tasks);
        return Ok(response);
    }
}