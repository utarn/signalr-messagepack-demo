using Microsoft.EntityFrameworkCore;

namespace TestSignalR.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Person> People => Set<Person>();
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
}