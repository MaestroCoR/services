using AuthorsService.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace AuthorsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("---> Attempting to apply migrations....");
                try
                {
                context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"---> Could not run migrations: {ex.Message}");
                }
            }
            if(!context.Authors.Any())
            {
                Console.WriteLine("---> Seeding Data...");
                context.Authors.AddRange(
                    new Author() {FirstName="Joanne", LastName="Rowling", Country="England", BirthDate =  new DateTime(1965, 7, 31)},
                    new Author() {FirstName="Joanne", LastName="Rowling", Country="England", BirthDate =  new DateTime(1965, 7, 31)},
                    new Author() {FirstName="Jeam", LastName="Rowling", Country="England", BirthDate =  new DateTime(1965, 7, 31)}
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---> We already have data");
            }
        }
        
    }
}