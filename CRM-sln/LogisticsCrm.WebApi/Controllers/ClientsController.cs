using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Domain.Entities;
using LogisticsCrm.WebApi.Dtos.Clients;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCrm.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClientResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            var clients = await _clientRepository.GetAllAsync(cancellationToken);
            return Ok(clients.Select(c => c.ToDto()).ToList());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClientResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(id, cancellationToken);
            if (client == null)
                return NotFound();

            return Ok(client.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<ClientResponseDto>> Create(
            [FromBody] CreateClientRequest request,
            CancellationToken cancellationToken)
        {
            var client = new Client(
                request.Name,
                request.ContactPerson,
                request.Phone,
                request.Email);

            await _clientRepository.AddAsync(client, cancellationToken);
            await _clientRepository.SaveChangesAsync(cancellationToken);

            var dto = client.ToDto();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ClientResponseDto>> Update(
            Guid id,
            [FromBody] UpdateClientRequest request,
            CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdForUpdateAsync(id, cancellationToken);
            if (client == null)
                return NotFound();

            client.UpdateDetails(request.Name, request.ContactPerson, request.Phone, request.Email);
            await _clientRepository.SaveChangesAsync(cancellationToken);

            return Ok(client.ToDto());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdForUpdateAsync(id, cancellationToken);
            if (client == null)
                return NotFound();

            await _clientRepository.DeleteAsync(client, cancellationToken);
            await _clientRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
