using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto request)
    {
        var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId);
        if (!projectExists)
        {
            return BadRequest("Göstərilən Layihə mövcud deyil.");
        }

        var userExists = await _context.Users.AnyAsync(u => u.Id == request.AssignedToUserId);
        if (!userExists)
        {
            return BadRequest("Təhkim edilən İstifadəçi mövcud deyil.");
        }

        var newTask = _mapper.Map<Models.Task>(request);
        
        await _context.Tasks.AddAsync(newTask);
        await _context.SaveChangesAsync();

        var response = _mapper.Map<TaskResponseDto>(newTask);
        return Ok(response);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskCreateDto request)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound("Tapşırıq tapılmadı.");
        }

        var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId);
        if (!projectExists)
        {
            return BadRequest("Göstərilən Layihə mövcud deyil.");
        }

        var userExists = await _context.Users.AnyAsync(u => u.Id == request.AssignedToUserId);
        if (!userExists)
        {
            return BadRequest("Təhkim edilən İstifadəçi mövcud deyil.");
        }
        
        _mapper.Map(request, task);
        await _context.SaveChangesAsync();

        return Ok("Tapşırıq uğurla yeniləndi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound("Tapşırıq tapılmadı.");
        }
        
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return Ok("Tapşırıq uğurla silindi.");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllTasks(
        [FromQuery] TaskStatusEnum? status, 
        [FromQuery] string? sortBy, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Tasks.AsQueryable();
        
        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
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
        var tasks = await query.Skip(skip).Take(pageSize).ToListAsync();
        
        var response = _mapper.Map<List<TaskResponseDto>>(tasks);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound("Tapşırıq tapılmadı.");
        }

        var response = _mapper.Map<TaskResponseDto>(task);
        return Ok(response);
    }
}