using CQRS.Domain.Commands.CreatePerson;
using CQRS.Domain.Core;
using CQRS.Domain.Queries.GetPerson;
using CQRS.Domain.Queries.ListPerson;
using Microsoft.AspNetCore.Mvc;

namespace CqrsPattern.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly  CreatePersonCommandHandler _createPersonCommandHandler;
        private readonly ListPersonQueryHandler _listPersonQueryHandler;
        private readonly GetPersonQueryHandler _getPersonQueryHandler;

        public PeopleController(CreatePersonCommandHandler createPersonCommandHandler,
            ListPersonQueryHandler listPersonQueryHandler,
            GetPersonQueryHandler getPersonQueryHandler)
        {
            _createPersonCommandHandler = createPersonCommandHandler;
            _listPersonQueryHandler = listPersonQueryHandler;
            _getPersonQueryHandler = getPersonQueryHandler;
        }

        [HttpGet("{id:guid}", Name = "Get Person By Id")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var response = await _getPersonQueryHandler.HandleAsync(new GetPersonQuery(id), cancellationToken);
            return GetResponse(_getPersonQueryHandler, response);
        }

        [HttpPost(Name = "Insert Person")]
        public async Task<Guid> InsertPeopleAsync(CreatePersonCommand createPersonCommand, CancellationToken cancellationToken)
        {
            return await _createPersonCommandHandler.HandleAsync(createPersonCommand, cancellationToken);
        }

        [HttpGet(Name = "List People")]
        public async Task<IEnumerable<ListPersonQueryResponse>> GetAsync([FromQuery] string? name,
            [FromQuery] string? cpf, CancellationToken cancellationToken)
        {
            return await _listPersonQueryHandler.HandleAsync(new ListPersonQuery(name, cpf), cancellationToken);
        }

        private IActionResult GetResponse<THandler, TResponse>(THandler handler, TResponse response) where THandler : BaseHandler
        {
            return StatusCode((int)handler.GetStatusCode(), new { Data = response, Notifications = handler.GetNotifications()});
        }

        private IActionResult GetResponse<THandler>(THandler handler) where THandler : BaseHandler
        {
            return StatusCode((int)handler.GetStatusCode(), new {Notifications = handler.GetNotifications()});
        }
    }
}
