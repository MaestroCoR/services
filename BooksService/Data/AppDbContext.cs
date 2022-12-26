using BooksService.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksService.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt): base(opt)
        {
            
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<Author>()
            .HasMany(p => p.Books)
            .WithOne(p => p.Author!)
            .HasForeignKey(p => p.AuthorId);

            modelBuilder
            .Entity<Book>()
            .HasOne(p => p.Author)
            .WithMany(p => p.Books)
            .HasForeignKey(p => p.AuthorId);

        }
    }
}