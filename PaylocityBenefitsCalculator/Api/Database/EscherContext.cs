using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public class EscherContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Dependent> Dependents { get; set; }

    public EscherContext (DbContextOptions<EscherContext> options)
        : base(options)
    {
    }
}