using AuthorsService;
using AutoMapper;
using BooksService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

namespace BooksService.SyncDataServices.Grpc
{
    public class AuthorDataClient : IAuthorDataClient
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthorDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public IEnumerable<Author> ReturnAllAuthors()
        {
            
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcAuthor"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcAuthor"]);
            var client = new GrpcAuthor.GrpcAuthorClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllAuthors(request);
                return _mapper.Map<IEnumerable<Author>>(reply.Author);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--->Could not call GRPC Server{ex.Message}");
                return null;
            }
        }
    }
}