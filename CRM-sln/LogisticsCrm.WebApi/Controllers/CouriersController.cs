using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Domain.Entities;
using LogisticsCrm.WebApi.Dtos.Couriers;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCrm.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouriersController : ControllerBase
    {
        private readonly ICourierRepository _courierRepository;

        public CouriersController(ICourierRepository courierRepository)
        {
            _courierRepository = courierRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourierResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            var couriers = await _courierRepository.GetAllAsync(cancellationToken);
            return Ok(couriers.Select(c => c.ToDto()).ToList());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourierResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var courier = await _courierRepository.GetByIdAsync(id, cancellationToken);
            if (courier == null)
                return NotFound();

            return Ok(courier.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<CourierResponseDto>> Create(
            [FromBody] CreateCourierRequest request,
            CancellationToken cancellationToken)
        {
            var courier = new Courier(request.FullName, request.Phone);

            await _courierRepository.AddAsync(courier, cancellationToken);
            await _courierRepository.SaveChangesAsync(cancellationToken);

            var dto = courier.ToDto();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }
    }
}
