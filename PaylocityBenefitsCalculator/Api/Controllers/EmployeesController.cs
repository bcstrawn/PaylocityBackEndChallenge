using Api.Database;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EscherContext _context;
    private readonly IMapper _mapper;
    private readonly IPaycheckCalculator _paycheckCalculator;

    public EmployeesController(EscherContext context, IMapper mapper, IPaycheckCalculator paycheckCalculator)
    {
        _context = context;
        _mapper = mapper;
        _paycheckCalculator = paycheckCalculator;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Dependents)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
        {
            return NotFound();
        }

        var employeeDto = _mapper.Map<GetEmployeeDto>(employee);
        
        return new ApiResponse<GetEmployeeDto>
        {
            Data = employeeDto,
            Success = true
        };
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await _context.Employees
            .Include(e => e.Dependents)
            .ToListAsync();
        
        var employeeDtos = _mapper.Map<List<GetEmployeeDto>>(employees);

        return new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employeeDtos,
            Success = true
        };
    }

    // Validation could be done with something like pipes and filters, especially when there's more rules
    [SwaggerOperation(Summary = "Add employee")]
    [HttpPost("")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Post([FromBody] CreateEmployeeDto employee)
    {
        if (employee == null)
        {
            return BadRequest("No employee data provided");
        }

        if (employee.Dependents.Count(d => d.Relationship == Relationship.Spouse || d.Relationship == Relationship.DomesticPartner) > 1)
        {
            return BadRequest("Employee may only have 1 spouse or domestic partner (not both)");
        }

        var employeeModel = _mapper.Map<Employee>(employee);
        _context.Employees.Add(employeeModel);
        await _context.SaveChangesAsync();

        var employeeDto = _mapper.Map<GetEmployeeDto>(employeeModel);

        return new ApiResponse<GetEmployeeDto>
        {
            Data = employeeDto,
            Success = true
        };
    }

    [SwaggerOperation(Summary = "Get paycheck for employee")]
    [HttpGet("paycheck/{id}")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> Paycheck(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Dependents)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
        {
            return NotFound();
        }

        var checkDate = DateTime.Today; // This would come from the request
        var paycheckDto = _paycheckCalculator.CalculatePaycheck(employee, checkDate);

        return new ApiResponse<GetPaycheckDto>
        {
            Data = paycheckDto,
            Success = true
        };
    }
}
