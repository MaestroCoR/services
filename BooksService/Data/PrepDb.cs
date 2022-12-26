using BooksService.Models;
using BooksService.SyncDataServices.Grpc;

namespace BooksService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope() )
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IAuthorDataClient>();

                var authors = grpcClient.ReturnAllAuthors();

                SeedData(serviceScope.ServiceProvider.GetService<IBookRepo>(), authors);
            }
        
        }

        private static void SeedData(IBookRepo repo, IEnumerable<Author> authors)
        {
            Console.WriteLine("Seeding new platforms...");

            foreach (var auth in authors)
            {
                if (!repo.ExternalAuthorExists(auth.ExternalId))
                {
                    repo.CreateAuthor(auth);
                }
                repo.SaveChanges();
            }
        }
    }
}