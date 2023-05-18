using AutoMapper;
using CQRS.Domain.Contracts;
using CQRS.Domain.Core;
using CQRS.Domain.Domain;

namespace CQRS.Domain.Commands.CreatePerson
{
    public class CreatePersonCommandHandler : BaseHandler
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        public CreatePersonCommandHandler(IPersonRepository personRepository, IMapper mapper) 
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<Guid> HandleAsync(CreatePersonCommand command, CancellationToken cancellationToken)
        {           
 
            var entity = _mapper.Map<Person>(command);

            await _personRepository.InsertAsync(entity, cancellationToken);
            return entity.Id;
        }

    }
}
