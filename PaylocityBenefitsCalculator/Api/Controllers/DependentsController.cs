using Api.Database;
using Api.Dtos.Dependent;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly EscherContext _context;
    private readonly IMapper _mapper;

    public DependentsController(EscherContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _context.Dependents.FindAsync(id);

        if (dependent == null)
        {
            return NotFound();
        }

        var dependentDto = _mapper.Map<GetDependentDto>(dependent);
        
        return new ApiResponse<GetDependentDto>
        {
            Data = dependentDto,
            Success = true
        };
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _context.Dependents.ToListAsync();

        var dependentDtos = _mapper.Map<List<GetDependentDto>>(dependents);

        return new ApiResponse<List<GetDependentDto>>
        {
            Data = dependentDtos,
            Success = true
        };
    }
}
