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
    
    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, [FromBody] TaskCreateDto request)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound("Tapşırıq tapılmadı.");
        }
        
        _mapper.Map(request, task);
        
        _context.SaveChanges();

        return Ok("Tapşırıq uğurla yeniləndi.");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound("Tapşırıq tapılmadı.");
        }
        
        _context.Tasks.Remove(task);
        _context.SaveChanges();

        return Ok("Tapşırıq uğurla silindi.");
    }
    
    [HttpGet]
    public IActionResult GetAllTasks(
        [FromQuery] string? status, 
        [FromQuery] string? sortBy, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Tasks.AsQueryable();
        
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(t => t.Status != null && t.Status.ToLower() == status.ToLower());
        }
        
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "title" => query.OrderBy(t => t.Title),
                "title_desc" => query.OrderByDescending(t => t.Title),
                _ => query.OrderBy(t => t.Id)
            };
        }
        else
        {
            query = query.OrderBy(t => t.Id);
        }
        
        var skip = (page - 1) * pageSize;
        
        var tasks = query.Skip(skip).Take(pageSize).ToList();
        
        var response = _mapper.Map<List<TaskResponseDto>>(tasks);

        return Ok(response);
    }
}