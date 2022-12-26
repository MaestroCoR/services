using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using AuthorsService.Dtos;

namespace AuthorsService.SyncDataServices.Http
{
    public class HttpBookDataClient : IBookDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpBookDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendAuthorToBook(AuthorReadDto auth)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(auth),
                Encoding.UTF8,
                 "application/json");
            var response = await _httpClient.PostAsync($"{_configuration["BooksService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("----> Sync Post to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("----> Sync Post to CommandService was NOT OK!");
            }
        }
    }
}