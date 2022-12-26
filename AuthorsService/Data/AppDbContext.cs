using Microsoft.EntityFrameworkCore;
using AuthorsService.Models;

namespace AuthorsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt): base(opt)
        {
            
        }
        public DbSet<Author> Authors { get; set; }
        
    }
}