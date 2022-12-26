using AuthorsService.Data;
using AutoMapper;
using Grpc.Core;

namespace AuthorsService.SyncDataServices.Grpc
{
    public class GrpcAuthorService: GrpcAuthor.GrpcAuthorBase
    {
        private readonly IAuthorRepo _repository;
        private readonly IMapper _mapper;

        public GrpcAuthorService(IAuthorRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<AuthorResponse> GetAllAuthors(GetAllRequest request, ServerCallContext context)
        {
            var response = new AuthorResponse();
            var authors = _repository.GetAllAuthors();

            foreach (var auth in authors)
            {
                response.Author.Add(_mapper.Map<GrpcAuthorModel>(auth));
            }

            return Task.FromResult(response);
        }
    }
}