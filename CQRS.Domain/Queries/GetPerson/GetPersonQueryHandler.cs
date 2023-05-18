using AutoMapper;
using CQRS.Domain.Contracts;
using CQRS.Domain.Core;
using System.Net;

namespace CQRS.Domain.Queries.GetPerson
{
    public class GetPersonQueryHandler : BaseHandler
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public GetPersonQueryHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetPersonQueryResponse?> HandleAsync(GetPersonQuery command, CancellationToken cancellationToken)
        {
            var databaseEntity = await _repository.GetByIdAsync(command.Id, cancellationToken);

            if (!string.IsNullOrWhiteSpace(databaseEntity?.Name))
                return _mapper.Map<GetPersonQueryResponse>(databaseEntity);

            AddNotification($"Person with id = {command.Id} does not exist.");
            SetSatusCode(HttpStatusCode.NotFound);
            return null;
        }
    }
}
