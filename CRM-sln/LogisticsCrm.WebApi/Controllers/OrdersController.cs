using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Application.Services;
using LogisticsCrm.Domain.Entities;
using LogisticsCrm.WebApi.Dtos.Orders;
using Microsoft.AspNetCore.Mvc;
using LogisticsCrm.Domain.Enums;


namespace LogisticsCrm.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ITrackingNumberGenerator _trackingNumberGenerator;

        public OrdersController(
            IOrderRepository orderRepository,
            IClientRepository clientRepository,
            ITrackingNumberGenerator trackingNumberGenerator)
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _trackingNumberGenerator = trackingNumberGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllAsync(cancellationToken);
            return Ok(orders.Select(o => o.ToDto()).ToList());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
            if (order == null)
                return NotFound();

            return Ok(order.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> Create(
            [FromBody] CreateOrderRequest request,
            CancellationToken cancellationToken)
        {
            // перевіряємо, що Client існує
            var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
            if (client == null)
                return BadRequest($"ClientId '{request.ClientId}' not found.");

            var tracking = _trackingNumberGenerator.Generate();

            var order = new Order(
                request.ClientId,
                tracking,
                request.PickupAddress,
                request.DeliveryAddress,
                request.RecipientName,
                request.RecipientPhone,
                request.Price);

            await _orderRepository.AddAsync(order, cancellationToken);
            await _orderRepository.SaveChangesAsync(cancellationToken);

            var dto = order.ToDto();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }
        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<OrderResponseDto>> UpdateStatus(
         Guid id,
        [FromBody] UpdateOrderStatusRequest request,
         CancellationToken cancellationToken)
        {
            // тут потрібно взяти order з трекінгом (без AsNoTracking)
            // тому або додамо метод GetByIdForUpdateAsync, або тимчасово зробимо так:
            var order = await _orderRepository.GetByIdForUpdateAsync(id, cancellationToken);
            if (order == null)
                return NotFound();

            var newStatus = (OrderStatus)request.Status;

            switch (newStatus)
            {
                case OrderStatus.Assigned: order.Assign(); break;
                case OrderStatus.PickedUp: order.MarkPickedUp(); break;
                case OrderStatus.InTransit: order.MarkInTransit(); break;
                case OrderStatus.Delivered: order.MarkDelivered(); break;
                case OrderStatus.Canceled: order.Cancel(); break;
                case OrderStatus.Created:
                default:
                    return BadRequest("Invalid status transition.");
            }

            await _orderRepository.SaveChangesAsync(cancellationToken);
            return Ok(order.ToDto());
        }

    }
}
